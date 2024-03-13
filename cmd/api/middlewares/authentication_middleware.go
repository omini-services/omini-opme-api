package middlewares

import (
	"context"
	"net/http"
	"strings"
	"time"

	"github.com/lestrrat-go/jwx/jwk"
	"github.com/lestrrat-go/jwx/jwt"
	"github.com/omini-services/omini-opme-be/internal/constants"
	"github.com/omini-services/omini-opme-be/pkg/logger"
	"go.uber.org/zap"
)

func Authenticate(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		ctx := r.Context()
		logger := logger.FromContext(ctx)

		authHeader := r.Header.Get("Authorization")
		splitAuthHeader := strings.Split(authHeader, " ")
		if len(splitAuthHeader) != 2 {
			http.Error(w, "", http.StatusUnauthorized)
			logger.Error("Missing JWT")
			return
		}

		keySet, err := getJWTkeyset(r.Context())
		if err != nil {
			http.Error(w, err.Error(), http.StatusInternalServerError)
			logger.Error("Could not get JWTkeyset", zap.Error(err))
			return
		}

		token, err := jwt.Parse(
			[]byte(splitAuthHeader[1]),
			jwt.WithKeySet(keySet),
			jwt.WithValidate(true),
			jwt.InferAlgorithmFromKey(true),
			jwt.WithIssuer("https://sts.windows.net/79e0bbd4-b85d-4ad3-a59d-23268b4c65ec/"),
		)

		if err != nil {
			http.Error(w, "", http.StatusUnauthorized)
			logger.Error("Invalid JWT", zap.Error(err))
			return
		}

		ctx = context.WithValue(ctx, constants.JwtKey, token)

		next.ServeHTTP(w, r.WithContext(ctx))
	})
}

func getJWTkeyset(ctx context.Context) (jwk.Set, error) {
	const jwksURL = "https://login.microsoftonline.com/79e0bbd4-b85d-4ad3-a59d-23268b4c65ec/discovery/v2.0/keys"

	ar := jwk.NewAutoRefresh(ctx)

	ar.Configure(jwksURL, jwk.WithMinRefreshInterval(15*time.Minute))

	keyset, err := ar.Refresh(ctx, jwksURL)
	if err != nil {
		return nil, err
	}

	return keyset, nil
}
