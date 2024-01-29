package main

import (
	"fmt"
	"log"
	"net/http"

	"github.com/go-chi/chi/v5"
	"github.com/go-chi/chi/v5/middleware"
)

func main() {

	//cognitoClient.()

	r := chi.NewRouter()
	r.Use(middleware.Logger)

	r.Get("/", func(w http.ResponseWriter, r *http.Request) {
		w.Write([]byte("welcome test"))
	})

	log.Printf("Listening 👂 on %s 🚪", "8000")
	fmt.Println("To close connection CTRL+C 🔌")
	http.ListenAndServe(":8000", r)
}
