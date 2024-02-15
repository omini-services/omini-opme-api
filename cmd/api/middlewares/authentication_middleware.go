package middlewares

import (
	"context"
	"fmt"
	"net/http"
	"strings"
	"time"

	"github.com/lestrrat-go/jwx/jwk"
	"github.com/lestrrat-go/jwx/jwt"
)

func Authenticate(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		authHeader := r.Header.Get("Authorization")
		splitAuthHeader := strings.Split(authHeader, " ")
		if len(splitAuthHeader) != 2 {
			http.Error(w, "", http.StatusUnauthorized)
			return
		}

		keySet, err := getJWTkeyset(r.Context())
		if err != nil {
			http.Error(w, err.Error(), http.StatusInternalServerError)
			return
		}

		_, err = jwt.Parse(
			[]byte(splitAuthHeader[1]),
			jwt.WithKeySet(keySet),
			jwt.WithValidate(true),
			jwt.InferAlgorithmFromKey(true),
			jwt.WithIssuer("https://sts.windows.net/79e0bbd4-b85d-4ad3-a59d-23268b4c65ec/"),
			jwt.WithAudience("api://35133114-8f8f-4094-9c95-51a22540c178"),
		)

		if err != nil {
			http.Error(w, "", http.StatusUnauthorized)
			return
		}

		next.ServeHTTP(w, r)
	})
}

func getJWTkeyset(ctx context.Context) (jwk.Set, error) {
	const jwksURL = "https://login.microsoftonline.com/79e0bbd4-b85d-4ad3-a59d-23268b4c65ec/discovery/v2.0/keys?appid=35133114-8f8f-4094-9c95-51a22540c178"

	ar := jwk.NewAutoRefresh(ctx)

	ar.Configure(jwksURL, jwk.WithMinRefreshInterval(15*time.Minute))

	keyset, err := ar.Refresh(ctx, jwksURL)
	if err != nil {
		fmt.Printf("failed to refresh JWKS: %s\n", err)
		return nil, err
	}

	return keyset, nil
}
