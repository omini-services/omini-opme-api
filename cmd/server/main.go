package main

import (
	"fmt"
	"log"

	"github.com/go-chi/chi/v5"
	"github.com/omini-services/omini-opme-be/configs"
	"github.com/omini-services/omini-opme-be/internal/infra/web/webserver"

	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

var (
	version string
	build   string
)

func main() {
	configs, err := configs.LoadConfig(".")
	if err != nil {
		panic(err)
	}

	dsn := configs.DBConnectionString

	db, err := gorm.Open(postgres.Open(dsn), &gorm.Config{})
	if err != nil {
		log.Fatal("failed to connect database", err)
	}

	dbClient, err := db.DB()

	if err != nil {
		log.Fatal("failed to connect database", err)
	}

	defer dbClient.Close()

	server := webserver.NewWebServer(configs.WebServerPort)

	itemHandler := NewWebItemHandler(db)

	server.AddRoutes("/api/items", func(r chi.Router) {
		r.Get("/", itemHandler.GetAll)
		r.Get("/{id}", itemHandler.Get)
		r.Post("/{id}", itemHandler.Create)
		r.Put("/{id}", itemHandler.Edit)
	})

	apiHandler := NewApiHandler()

	server.AddRoutes("/api", func(r chi.Router) {
		r.Get("/health", apiHandler.Health)
	})

	fmt.Println("Starting web server on sport", configs.WebServerPort)
	server.Start()
}
