package main

import (
	"fmt"
	"log"

	"github.com/omini-services/omini-opme-be/configs"

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

	server := NewServer(configs.WebServerPort)
	server.AddMiddlewares()
	server.AddHandlers(db)

	// server.AddRoutes("/api/items", func(r chi.Router) {
	// 	r.Get("/", itemHandler.GetAll)
	// 	r.Get("/{id}", itemHandler.Get)
	// 	r.Post("/{id}", itemHandler.Create)
	// 	r.Put("/{id}", itemHandler.Edit)
	// })

	// server.AddRoutes("/api", func(r chi.Router) {
	// 	r.Get("/health", apiHandler.Health)
	// })

	fmt.Println("Starting web server on sport", configs.WebServerPort)
	server.Start()
}
