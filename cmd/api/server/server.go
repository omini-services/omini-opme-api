package main

import (
	"fmt"
	"log"
	"net/http"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/chi/v5/middleware"
	"gorm.io/gorm"

	customMiddleware "github.com/omini-services/omini-opme-be/internal/infra/middleware"
)

type Server struct {
	Router chi.Router
	Port   string
}

func NewServer(serverPort string) *Server {
	return &Server{
		Router: chi.NewRouter(),
		Port:   serverPort,
	}
}

func (s *Server) AddMiddlewares() {
	s.Router.Use(middleware.Logger)
	s.Router.Use(customMiddleware.Authenticate)
}

func (s *Server) AddDb(db *gorm.DB) {
	NewItemHandler(s.Router, db)
}

func (s *Server) AddHandlers(db *gorm.DB) {
	NewItemHandler(s.Router, db)
}

func (s *Server) Start() {
	log.Printf("Listening 👂 on %s 🚪", s.Port)
	fmt.Println("To close connection CTRL+C 🔌 ")

	http.ListenAndServe(s.Port, s.Router)
}
