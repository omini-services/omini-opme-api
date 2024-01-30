package main

import (
	"fmt"
	"log"
	"net/http"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/chi/v5/middleware"
)

var (
	version string
	build   string
)

func main() {
	r := chi.NewRouter()
	r.Use(middleware.Logger)

	log.Printf("Version=%s", version)
	log.Printf("Build=%s", build)
	//cognitoClient.()

	r.Get("/", func(w http.ResponseWriter, r *http.Request) {
		w.Write([]byte("welcome test"))
	})

	log.Printf("Listening 👂 on %s 🚪", "80")
	fmt.Println("To close connection CTRL+C 🔌")
	http.ListenAndServe(":80", r)
}
