package handlers

import (
	"encoding/json"
	"fmt"
	"net/http"
	"strings"
	"time"

	"github.com/go-chi/jwtauth"
	"github.com/lestrrat-go/jwx/jwk"
	"github.com/lestrrat-go/jwx/jwt"

	"github.com/omini-services/omini-opme-be/internal/entity"
	"github.com/omini-services/omini-opme-be/internal/entity/dto"
	"github.com/omini-services/omini-opme-be/internal/infra/database"
	"github.com/omini-services/omini-opme-be/pkg/auth"
)

type UserHandler struct {
	UserDB       database.UserInterface
	Jwt          *jwtauth.JWTAuth
	JwtExpiresIn int
}

func NewUserHandler(db database.UserInterface, jwt *jwtauth.JWTAuth, JwtExpiresIn int) *UserHandler {
	return &UserHandler{
		UserDB:       db,
		Jwt:          jwt,
		JwtExpiresIn: JwtExpiresIn,
	}
}

func (h *UserHandler) GetJWT(w http.ResponseWriter, r *http.Request) {
	var user dto.GetJWTInput
	err := json.NewDecoder(r.Body).Decode(&user)

	if err != nil {
		w.WriteHeader(http.StatusBadRequest)
		return
	}

	u, err := h.UserDB.FindByEmail(user.Email)

	if err != nil {
		w.WriteHeader(http.StatusBadRequest)
		return
	}

	if !u.ValidatePassword(user.Password) {
		w.WriteHeader(http.StatusUnauthorized)
		return
	}

	_, tokenString, _ := h.Jwt.Encode(map[string]interface{}{
		"sub": u.ID,
		"exp": time.Now().Add(time.Second * time.Duration(h.JwtExpiresIn)).Unix(),
	})

	accessToken := struct {
		AccessToken string `json:"access_token"`
	}{
		AccessToken: tokenString,
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusOK)
	json.NewEncoder(w).Encode(accessToken)
}

func (h *UserHandler) CreateUser(w http.ResponseWriter, r *http.Request) {
	var user dto.CreateUserInput
	err := json.NewDecoder(r.Body).Decode(&user)

	if err != nil {
		w.WriteHeader(http.StatusBadRequest)
		return
	}

	p, err := entity.NewUser(user.Name, user.Email, user.Password)

	if err != nil {
		w.WriteHeader(http.StatusBadRequest)
		return
	}

	err = h.UserDB.Create(p)
	if err != nil {
		w.WriteHeader(http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusCreated)
}

func (h *UserHandler) SignUp(w http.ResponseWriter, r *http.Request) {
	var req dto.SignUpRequest
	err := json.NewDecoder(r.Body).Decode(&req)

	if err != nil {
		http.Error(w, err.Error(), http.StatusBadRequest)
		return
	}

	w.Write([]byte("signup"))
}

func (h *UserHandler) SignIn(w http.ResponseWriter, r *http.Request) {
	http.Redirect(w, r, "https://omini.auth.us-east-1.amazoncognito.com/login?client_id=315u3ukt8s3rd6i8dt9j8g8u60&response_type=code&scope=email+openid&redirect_uri=http://localhost", http.StatusSeeOther)
}

func (h *UserHandler) Callback(w http.ResponseWriter, r *http.Request) {
	var data map[string]json.RawMessage
	_ = json.NewDecoder(r.Body).Decode(&data)

	fmt.Println("Decoded Data:")
	for key, value := range data {
		fmt.Printf("%s: %s\n", key, value)
	}

	w.WriteHeader(http.StatusOK)
}

func (h *UserHandler) VerifyUser(w http.ResponseWriter, r *http.Request) {
	authHeader := r.Header.Get("Authorization")
	splitAuthHeader := strings.Split(authHeader, " ")
	if len(splitAuthHeader) != 2 {
		http.Error(w, "err", http.StatusInternalServerError)
		return
	}

	formattedUrl := "https://cognito-idp.us-east-1.amazonaws.com/us-east-1_e4sSJXrAv/.well-known/jwks.json"

	_, ok := r.Context().Value("CognitoClient").(*auth.CognitoClient)

	if !ok {
		http.Error(w, "err", http.StatusInternalServerError)
		return
	}

	// fmt.Sprintf(pubKeyURL, "us-east-1", cognitoClient.UserPoolId)

	keySet, err := jwk.Fetch(r.Context(), formattedUrl)
	if err != nil {
		http.Error(w, "err", http.StatusInternalServerError)
		return
	}

	token, err := jwt.Parse(
		[]byte(splitAuthHeader[1]),
		jwt.WithKeySet(keySet),
		jwt.WithValidate(true),
	)

	if err != nil {
		http.Error(w, "invalid", http.StatusInternalServerError)
		return
	}

	fmt.Println(token)

	return
}
