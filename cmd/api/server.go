package api

import (
	"fmt"
	"log"
	"net/http"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/chi/v5/middleware"
	openApi "github.com/go-openapi/runtime/middleware"
	"gorm.io/gorm"

	customMiddlewares "github.com/omini-services/omini-opme-be/cmd/api/middlewares"
)

type Server struct {
	router chi.Router
	Port   string
	db     *gorm.DB
}

func NewServer(serverPort string) *Server {
	return &Server{
		router: chi.NewRouter(),
		Port:   serverPort,
	}
}

func (s *Server) UseDb(db *gorm.DB) {
	s.db = db
}

func (s *Server) UseSwagger(r chi.Router) {
	r.Handle("/swagger.yaml", http.StripPrefix("/api/", http.FileServer(http.Dir("./"))))
	opts := openApi.SwaggerUIOpts{SpecURL: "/api/swagger.yaml", BasePath: "/api"}
	sh := openApi.SwaggerUI(opts, nil)
	r.Handle("/docs", sh)
}

func (s *Server) AddHandlers(options func(r chi.Router)) {
	s.router.Route("/api", func(r chi.Router) {
		s.addProtectedHandlers(r)
		addPublicHandlers(r, options)
	})
}

func (s *Server) addProtectedHandlers(r chi.Router) {
	r.Group(func(r chi.Router) {
		r.Use(middleware.Logger)
		r.Use(customMiddlewares.Authenticate)

		NewItemHandler(r, s.db)
	})
}

func addPublicHandlers(r chi.Router, options func(r chi.Router)) {
	r.Group(func(r chi.Router) {
		r.Use(middleware.Logger)
		options(r)

		NewApiHandler(r)
	})
}

func (s *Server) Start() {
	log.Printf("Listening 👂 on %s 🚪", s.Port)
	fmt.Println("To close connection CTRL+C 🔌 ")

	http.ListenAndServe(s.Port, s.router)
}
