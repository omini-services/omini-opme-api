package api

import (
	"fmt"
	"log"
	"net/http"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/chi/v5/middleware"
	"gorm.io/gorm"

	customMiddlewares "github.com/omini-services/omini-opme-be/cmd/api/middlewares"
)

type Server struct {
	Router chi.Router
	Port   string
	db     *gorm.DB
}

func NewServer(serverPort string) *Server {
	return &Server{
		Router: chi.NewRouter(),
		Port:   serverPort,
	}
}

func (s *Server) UseDb(db *gorm.DB) {
	s.db = db
}

func (s *Server) AddMiddlewares() {
	s.Router.Use(middleware.Logger)
	s.Router.Use(customMiddlewares.Authenticate)
}

func (s *Server) AddHandlers() {
	NewItemHandler(s.Router, s.db)
	NewApiHandler(s.Router)
}

func (s *Server) Start() {
	log.Printf("Listening 👂 on %s 🚪", s.Port)
	fmt.Println("To close connection CTRL+C 🔌 ")

	http.ListenAndServe(s.Port, s.Router)
}
