package database

import (
	"log"

	"go.uber.org/zap"
	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

func UseDatabase() (*gorm.DB, error) {
	dsn := "postgresql://dk:rQjN27iNUWiqEBNosrrlqQ@peppy-orc-7035.g8z.gcp-us-east1.cockroachlabs.cloud:26257/defaultdb?sslmode=verify-full"

	db, err := gorm.Open(postgres.Open(dsn), &gorm.Config{})
	if err != nil {
		log.Fatal("failed to connect database", zap.Error(err))

		return nil, err
	}

	dbClient, err := db.DB()

	if err != nil {
		log.Fatal("failed to connect database", zap.Error(err))

		return nil, err
	}

	defer dbClient.Close()

	//db.Use(plugins.NewAuditableEntity())

	return db, nil
}
