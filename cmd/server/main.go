package main

import (
	"fmt"
	"log"

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

	dsn := fmt.Sprintf("%s://%s:%s@%s:%s/%s?sslmode=verify-full", configs.DBDriver, configs.DBUser, configs.DBPassword, configs.DBHost, configs.DBPort, configs.DBName)

	db, err := gorm.Open(postgres.Open(dsn), &gorm.Config{})
	if err != nil {
		log.Fatal("failed to connect database", err)
	}

	dbClient, err := db.DB()

	if err != nil {
		log.Fatal("failed to connect database", err)
	}

	defer dbClient.Close()

	itemHandler := NewWebItemHandler(db)

	webserver := webserver.NewWebServer(configs.WebServerPort)

	webserver.AddHandler("/items", itemHandler.GetItems)
	fmt.Println("Starting web server on port", configs.WebServerPort)
	webserver.Start()
}
