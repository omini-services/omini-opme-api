package constants

import "github.com/omini-services/omini-opme-be/pkg"

var (
	CorrelationIdKey = pkg.ContextKey("correlationId")
	LoggerKey        = pkg.ContextKey("logger")
	RequestMethodKey = pkg.ContextKey("method")
	RequestUriKey    = pkg.ContextKey("requestUri")
	HostKey          = pkg.ContextKey("host")
)
