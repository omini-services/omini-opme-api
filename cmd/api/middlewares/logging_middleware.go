package middlewares

import (
	"net/http"
	"time"

	"github.com/google/uuid"
	"github.com/omini-services/omini-opme-be/pkg/logger"
	"go.uber.org/zap"
)

type loggingResponseWriter struct {
	http.ResponseWriter
	statusCode int
}

func NewLoggingResponseWriter(w http.ResponseWriter) *loggingResponseWriter {
	return &loggingResponseWriter{w, http.StatusNotAcceptable}
}

func (lrw *loggingResponseWriter) WriteHeader(code int) {
	lrw.statusCode = code

	if code != 200 {
		lrw.ResponseWriter.WriteHeader(code)
	}
}

func Logging(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		correlationID := uuid.New().String()

		timeMs := time.Now()
		l := logger.NewLog().WithCorrelationId(correlationID).WithRequest(r)

		l.Info("Request Started")

		lrw := NewLoggingResponseWriter(w)
		next.ServeHTTP(lrw, r.WithContext(logger.WithContext(r.Context(), l)))

		l.Info("Request Finished",
			zap.Int64("duration", time.Since(timeMs).Milliseconds()),
			zap.Int64("responseCode", int64(lrw.statusCode)),
		)
	})
}
