package api

import (
	"fmt"
	"net/http"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/cors"
	openApi "github.com/go-openapi/runtime/middleware"
	"go.uber.org/zap"
	"gorm.io/gorm"

	customMiddlewares "github.com/omini-services/omini-opme-be/cmd/api/middlewares"
)

type Server struct {
	router chi.Router
	Port   string
	db     *gorm.DB
}

//var app *newrelic.Application

func init() {
	// var err error
	// app, err = newrelic.NewApplication(
	// 	newrelic.ConfigAppName("opme-be"),
	// 	newrelic.ConfigLicense("b27925bbceb7c1600bd35c74776f65f4FFFFNRAL"),
	// 	newrelic.ConfigAppLogForwardingEnabled(true),
	// )

	// if err != nil {
	// 	panic(fmt.Sprintf("Can`t initiate logging - %s", err))
	// }
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
		r.Use(customMiddlewares.Logging)
		r.Use(cors.Handler(cors.Options{
			// AllowedOrigins:   []string{"https://foo.com"}, // Use this to allow specific origin hosts
			AllowedOrigins: []string{"https://*", "http://*"},
			// AllowOriginFunc:  func(r *http.Request, origin string) bool { return true },
			AllowedMethods:   []string{"GET", "POST", "PUT", "DELETE", "OPTIONS"},
			AllowedHeaders:   []string{"Accept", "Authorization", "Content-Type", "X-CSRF-Token"},
			ExposedHeaders:   []string{"Link"},
			AllowCredentials: true,
			MaxAge:           300, // Maximum value not ignored by any of major browsers
		}))

		r.Use(customMiddlewares.Authenticate)

		NewItemHandler(r, s.db)
	})
}

func addPublicHandlers(r chi.Router, options func(r chi.Router)) {
	r.Group(func(r chi.Router) {
		r.Use(customMiddlewares.Logging)
		options(r)

		NewApiHandler(r)
	})
}

func (s *Server) Start() {
	zap.L().Debug("Starting from port 8080")

	zap.L().Info(fmt.Sprintf("Listening 👂 on %s 🚪", s.Port))
	zap.L().Info("To close connection CTRL+C 🔌 ")

	http.ListenAndServe(s.Port, s.router)
}
