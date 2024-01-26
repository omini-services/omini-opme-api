package auth

import (
	"context"

	"github.com/aws/aws-sdk-go-v2/config"
	cip "github.com/aws/aws-sdk-go-v2/service/cognitoidentityprovider"
)

type CognitoClient struct {
	AppClientId string
	*cip.Client
}

func Init() *CognitoClient {
	// Load the Shared AWS Configuration (~/.aws/config)
	cfg, err := config.LoadDefaultConfig(context.Background())
	if err != nil {
		panic(err)
	}

	return &CognitoClient{
		"1eroc789rnnkb1a756l7gijd5h",
		cip.NewFromConfig(cfg),
	}
}

type SignUpRequest struct {
	Email    string `json:"email"`
	Password string `json:"password"`
}

// func signUp(w http.ResponseWriter, r *http.Request) {
// 	var req SignUpRequest
// 	err := json.NewDecoder(r.Body).Decode(&req)

// 	if err != nil {
// 		http.Error(w, err.Error(), http.StatusBadRequest)
// 	}

// 	cognitoClient, ok := r.Context().Value("CognitoClient").(*auth.CognitoClient)

// 	if !ok {
// 		http.Error(w, err.Error(), http.StatusBadRequest)
// 		return
// 	}

// 	awsReq := &cip.SignUpInput{
// 		ClientId: aws.String(cognitoClient.AppClientId),
// 		Password: aws.String(req.Password),
// 	}

// 	_, err = cognitoClient.SignUp(r.Context(), awsReq)

// 	if !ok {
// 		http.Error(w, err.Error(), http.StatusBadRequest)
// 		return
// 	}

// 	w.Write([]byte("sigip!"))
// }
