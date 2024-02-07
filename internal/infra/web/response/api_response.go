package response

import (
	"encoding/json"
	"net/http"
)

type apiResponse struct {
	Success     bool        `json:"success"`
	SuccessBody interface{} `json:"data,omitempty"`
	ErrorBody   []Error     `json:"errors,omitempty"`
}

type Error struct {
	Name    string `json:"name"`
	Message string `json:"message"`
}

func sendSuccess(responseBody interface{}) *apiResponse {
	return &apiResponse{
		Success:     true,
		SuccessBody: responseBody}
}

func sendError(errors []Error) *apiResponse {
	return &apiResponse{
		Success:   false,
		ErrorBody: errors}
}

func JsonSuccess(w http.ResponseWriter, i interface{}, statusCode int) {
	w.Header().Set("Content-Type", "application/json")

	jsonData, err := json.Marshal(sendSuccess(i))

	if err != nil {
		err = json.NewEncoder(w).Encode(sendError([]Error{{Name: "Root", Message: "Could not parse response"}}))
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

func JsonFail(w http.ResponseWriter, e []Error, statusCode int) {
	w.Header().Set("Content-Type", "application/json")

	jsonData, err := json.Marshal(sendError(e))

	if err != nil {
		err = json.NewEncoder(w).Encode(sendError([]Error{{Name: "Root", Message: "Could not parse response"}}))
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
