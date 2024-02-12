package response

import (
	"encoding/json"
	"errors"
	"net/http"
)

type apiResponse struct {
	Success     bool        `json:"success"`
	SuccessBody interface{} `json:"data,omitempty"`
	ErrorBody   []string    `json:"errors,omitempty"`
}

func sendSuccess(responseBody interface{}) *apiResponse {
	return &apiResponse{
		Success:     true,
		SuccessBody: responseBody}
}

func sendError(errors []error) *apiResponse {
	errorMessages := []string{}

	for _, err := range errors {
		errorMessages = append(errorMessages, err.Error())
	}

	return &apiResponse{
		Success:   false,
		ErrorBody: errorMessages}
}

func JsonSuccess(w http.ResponseWriter, i interface{}, statusCode int) {
	w.Header().Set("Content-Type", "application/json")

	jsonData, err := json.Marshal(sendSuccess(i))

	if err != nil {
		err = json.NewEncoder(w).Encode(sendError([]error{errors.New("could not parse response")}))
		if err != nil {
			http.Error(w, "An error has occured", http.StatusInternalServerError)
		}
		return
	}

	w.WriteHeader(statusCode)

	_, err = w.Write(jsonData)
	if err != nil {
		http.Error(w, "An error has occured", http.StatusInternalServerError)
	}
}

func JsonFail(w http.ResponseWriter, e []error, statusCode int) {
	w.Header().Set("Content-Type", "application/json")

	jsonData, err := json.Marshal(sendError(e))

	if err != nil {
		err = json.NewEncoder(w).Encode(sendError([]error{errors.New("could not parse response")}))
		if err != nil {
			http.Error(w, "An error has occured", http.StatusInternalServerError)
		}
		return
	}

	w.WriteHeader(statusCode)

	_, err = w.Write(jsonData)
	if err != nil {
		http.Error(w, "An error has occured", http.StatusInternalServerError)
	}
}
