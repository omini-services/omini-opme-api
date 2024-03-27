package middlewares

import (
	"net/http"

	"gorm.io/gorm"
)

func DbMiddleware(db *gorm.DB) func(http.Handler) http.Handler {
	return func(next http.Handler) http.Handler {
		return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
			ctx := r.Context()
			//db.WithContext(ctx)
			next.ServeHTTP(w, r.WithContext(ctx))
		})
	}
}
