package webserver

import (
	"fmt"
	"log"
	"net/http"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/chi/v5/middleware"

	customMiddleware "github.com/omini-services/omini-opme-be/internal/infra/web/middleware"
)

type WebServer struct {
	Router           chi.Router
	Handlers         map[string]func(r chi.Router)
	AuthorizedRoutes []string
	WebServerPort    string
}

func NewWebServer(serverPort string) *WebServer {
	return &WebServer{
		Router:        chi.NewRouter(),
		Handlers:      make(map[string]func(r chi.Router)),
		WebServerPort: serverPort,
	}
}

func (s *WebServer) AddRoutes(path string, routes func(r chi.Router)) {
	s.Handlers[path] = routes
}

// loop through the handlers and add them to the router
// register middeleware logger
// start the server
func (s *WebServer) Start() {
	s.Router.Use(middleware.Logger)
	s.Router.Use(customMiddleware.Authenticate)

	for path, handler := range s.Handlers {
		s.Router.Route(path, handler)
	}

	log.Printf("Listening 👂 on %s 🚪", s.WebServerPort)
	fmt.Println("To close connection CTRL+C 🔌 ")

	http.ListenAndServe(s.WebServerPort, s.Router)
}
