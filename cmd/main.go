package main

import (
	"fmt"
	"log"

	"github.com/omini-services/omini-opme-be/cmd/api"
	"github.com/omini-services/omini-opme-be/configs"

	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

var (
	version string
	build   string
)

func main() {
	configs.SetBuild(build)
	configs.SetVersion(version)

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

	server := api.NewServer(configs.WebServerPort)
	server.UseDb(db)

	server.AddMiddlewares()
	server.AddHandlers()

	// server.AddRoutes("/api", func(r chi.Router) {
	// 	r.Get("/health", apiHandler.Health)
	// })

	fmt.Println("Starting web server on sport", configs.WebServerPort)
	server.Start()
}
