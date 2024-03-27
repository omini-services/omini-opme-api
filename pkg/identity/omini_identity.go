package ominiidentity

import (
	"context"

	"github.com/lestrrat-go/jwx/jwt"
	"github.com/omini-services/omini-opme-be/internal/constants"
)

type OminiIdentity struct {
	Token jwt.Token
}

func NewOminiIdentity(token jwt.Token) *OminiIdentity {
	return &OminiIdentity{
		Token: token,
	}
}

func WithContext(ctx context.Context, l *OminiIdentity) context.Context {
	if lp, ok := ctx.Value(constants.JwtKey).(*OminiIdentity); ok {
		if lp == l {
			return ctx
		}
	}

	return context.WithValue(ctx, constants.JwtKey, l)
}

func FromContext(ctx context.Context) *OminiIdentity {
	if l, ok := ctx.Value(constants.JwtKey).(*OminiIdentity); ok {
		return l
	}

	return nil
}
