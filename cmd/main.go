// Package classification Petstore API.
//
// the purpose of this application is to provide an application
// that is using plain go code to define an API
//
// This should demonstrate all the possible comment annotations
// that are available to turn go code into a fully compliant swagger 2.0 spec
//
// Terms Of Service:
//
// there are no TOS at this moment, use at your own risk we take no responsibility
//
//	Schemes: http, https
//	Host: localhost
//	BasePath: /v2
//	Version: 0.0.1
//	License: MIT http://opensource.org/licenses/MIT
//	Contact: John Doe<john.doe@example.com> http://john.doe.com
//
//	Consumes:
//	- application/json
//	- application/xml
//
//	Produces:
//	- application/json
//	- application/xml
//
//	Security:
//	- api_key:
//
//	SecurityDefinitions:
//	api_key:
//	     type: apiKey
//	     name: KEY
//	     in: header
//	oauth2:
//	    type: oauth2
//	    authorizationUrl: /oauth2/auth
//	    tokenUrl: /oauth2/token
//	    in: header
//	    scopes:
//	      bar: foo
//	    flow: accessCode
//
//	Extensions:
//	x-meta-value: value
//	x-meta-array:
//	  - value1
//	  - value2
//	x-meta-array-obj:
//	  - name: obj
//	    value: field
//
// swagger:meta
package main

import (
	"fmt"
	"log"

	"github.com/go-chi/chi/v5"
	"github.com/omini-services/omini-opme-be/cmd/api"
	"github.com/omini-services/omini-opme-be/configs"
	"github.com/omini-services/omini-opme-be/pkg/logger"
	"go.uber.org/zap"
	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

var (
	version string
	build   string
)

func main() {
	logger.NewLog()

	configs.SetBuild(build)
	configs.SetVersion(version)

	configs, err := configs.LoadConfig(".")
	if err != nil {
		panic(err)
	}
	dsn := "postgresql://dk:rQjN27iNUWiqEBNosrrlqQ@peppy-orc-7035.g8z.gcp-us-east1.cockroachlabs.cloud:26257/defaultdb?sslmode=verify-full"

	db, err := gorm.Open(postgres.Open(dsn), &gorm.Config{})
	if err != nil {
		log.Fatal("failed to connect database", zap.Error(err))
	}

	dbClient, err := db.DB()

	if err != nil {
		log.Fatal("failed to connect database", zap.Error(err))
	}

	defer dbClient.Close()

	server := api.NewServer(configs.WebServerPort)
	server.UseDb(db)

	server.AddHandlers(func(r chi.Router) {
		server.UseSwagger(r)
	})

	fmt.Println("Starting web server on port", configs.WebServerPort)
	server.Start()
}
