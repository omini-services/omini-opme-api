package logger

import (
	"context"
	"net/http"
	"os"

	"github.com/omini-services/omini-opme-be/configs"
	"github.com/omini-services/omini-opme-be/internal/constants"
	"go.uber.org/zap"
	"go.uber.org/zap/zapcore"
)

type Logger struct {
	log *zap.Logger
}

func NewLog() *Logger {
	encoderCfg := zap.NewProductionEncoderConfig()
	encoderCfg.TimeKey = "timestamp"
	encoderCfg.EncodeTime = zapcore.ISO8601TimeEncoder
	encoderCfg.EncodeLevel = zapcore.CapitalColorLevelEncoder

	config := zap.Config{
		Level:             zap.NewAtomicLevelAt(zap.InfoLevel),
		Development:       false,
		DisableCaller:     false,
		DisableStacktrace: false,
		Sampling:          nil,
		Encoding:          "json",
		EncoderConfig:     encoderCfg,
		OutputPaths: []string{
			"stdout",
		},
		ErrorOutputPaths: []string{
			"stderr",
		},
		InitialFields: map[string]interface{}{
			"pid":     os.Getpid(),
			"build":   configs.GetBuild(),
			"version": configs.GetVersion(),
		},
	}

	return &Logger{
		log: zap.Must(config.Build()),
	}
}

func WithContext(ctx context.Context, l *Logger) context.Context {
	if lp, ok := ctx.Value(constants.LoggerKey).(*Logger); ok {
		if lp == l {
			return ctx
		}
	}

	return context.WithValue(ctx, constants.LoggerKey, l)
}

func FromContext(ctx context.Context) *Logger {
	if l, ok := ctx.Value(constants.LoggerKey).(*Logger); ok {
		return l
	} else {
		return NewLog()
	}
}

func (l *Logger) WithRequest(r *http.Request) *Logger {
	l.log = l.log.With(
		zap.String(string(constants.HostKey), r.Host),
		zap.String(string(constants.RequestMethodKey), r.Method),
		zap.String(string(constants.RequestUriKey), r.RequestURI),
	)

	return l
}

func (l *Logger) WithCorrelationId(correlationID string) *Logger {
	l.log = l.log.With(
		zap.String(string(constants.CorrelationIdKey), correlationID))

	return l
}

func (l *Logger) Error(msg string, fields ...zapcore.Field) {
	l.log.Error(msg, fields...)
}

func (l *Logger) Info(msg string, fields ...zapcore.Field) {
	l.log.Info(msg, fields...)
}

func (l *Logger) Fatal(msg string, fields ...zapcore.Field) {
	l.log.Fatal(msg, fields...)
}
