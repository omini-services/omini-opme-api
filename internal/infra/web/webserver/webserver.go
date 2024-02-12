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
	Router        chi.Router
	Handlers      map[[2]string]http.HandlerFunc
	WebServerPort string
}

func NewWebServer(serverPort string) *WebServer {
	return &WebServer{
		Router:        chi.NewRouter(),
		Handlers:      make(map[[2]string]http.HandlerFunc),
		WebServerPort: serverPort,
	}
}

func (s *WebServer) AddHandler(path string, method string, handler http.HandlerFunc) {
	s.Handlers[[2]string{method, path}] = handler
}

// loop through the handlers and add them to the router
// register middeleware logger
// start the server
func (s *WebServer) Start() {
	s.Router.Use(middleware.Logger)
	s.Router.Use(customMiddleware.Authenticate)

	for path, handler := range s.Handlers {
		s.Router.Method(path[0], path[1], handler)
	}

	log.Printf("Listening 👂 on %s 🚪", s.WebServerPort)
	fmt.Println("To close connection CTRL+C 🔌 ")

	http.ListenAndServe(s.WebServerPort, s.Router)
}
