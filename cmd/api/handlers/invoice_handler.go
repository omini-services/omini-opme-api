package handlers

import (
	"encoding/json"
	"errors"
	"net/http"
	"strconv"

	"github.com/brianvoe/gofakeit"
	"github.com/go-chi/chi/v5"
	"github.com/omini-services/omini-opme-be/cmd/api/dto"
	"github.com/omini-services/omini-opme-be/cmd/api/response"
	"github.com/omini-services/omini-opme-be/internal/domain"
)

type InvoiceHandler struct {
	iUsecase domain.InvoiceUsecase
}

func NewInvoiceHandler(r chi.Router, u domain.InvoiceUsecase) *InvoiceHandler {
	handler := &InvoiceHandler{
		iUsecase: u,
	}

	r.Route("/api/invoices", func(r chi.Router) {
		r.Get("/", handler.Get)
		r.Get("/{id}", handler.GetByID)
		r.Post("/", handler.Add)
		r.Put("/{id}", handler.Update)
	})

	return handler
}

func (h *InvoiceHandler) Get(w http.ResponseWriter, r *http.Request) {
	_, err := h.iUsecase.Get()

	outputs := []dto.GetInvoicesOutputDTO{}

	for i := 1; i <= 100; i++ {
		var output dto.GetInvoicesOutputDTO
		gofakeit.Struct(&output)
		outputs = append(outputs, output)
	}

	if err != nil {
		if err.ErrCode == domain.Unexpected {
			response.JsonFail(w, err.Error, http.StatusInternalServerError)
			return
		}
		response.JsonFail(w, err.Error, http.StatusBadRequest)
		return
	}

	response.JsonSuccess(w, outputs, http.StatusOK)
}

func (h *InvoiceHandler) GetByID(w http.ResponseWriter, r *http.Request) {
	id := chi.URLParam(r, "id")
	parsedId, routeError := strconv.Atoi(id)

	if routeError != nil {
		response.JsonFail(w, []error{errors.New("invalid id")}, http.StatusBadRequest)
		return
	}

	data, err := h.iUsecase.GetByID(parsedId)
	if err != nil {
		if err.ErrCode == domain.Unexpected {
			response.JsonFail(w, err.Error, http.StatusInternalServerError)
			return
		}
		response.JsonFail(w, err.Error, http.StatusBadRequest)
		return
	}

	output := invoiceToGetInvoiceOutputDTO(&data)

	response.JsonSuccess(w, output, http.StatusOK)
}

func (h *InvoiceHandler) Add(w http.ResponseWriter, r *http.Request) {
	var input dto.AddInvoiceInputDTO
	decodeError := json.NewDecoder(r.Body).Decode(&input)
	if decodeError != nil {
		http.Error(w, decodeError.Error(), http.StatusBadRequest)
		return
	}

	data, domainError := domain.NewInvoice(input.CustomerID)

	if domainError != nil {
		response.JsonFail(w, domainError.Error, http.StatusBadRequest)
	}

	err := h.iUsecase.Add(data)

	if err != nil {
		if err.ErrCode == domain.Unexpected {
			response.JsonFail(w, err.Error, http.StatusInternalServerError)
			return
		}
		response.JsonFail(w, err.Error, http.StatusBadRequest)
		return
	}

	output := dto.AddInvoiceOutputDTO{
		ID:         data.ID,
		CustomerID: data.CustomerID,
	}

	response.JsonSuccess(w, output, http.StatusOK)
}

func (h *InvoiceHandler) Update(w http.ResponseWriter, r *http.Request) {
	stringId := chi.URLParam(r, "id")
	id, routeError := strconv.Atoi(stringId)

	if routeError != nil {
		response.JsonFail(w, []error{errors.New("invalid id")}, http.StatusBadRequest)
		return
	}

	var input dto.UpdateInvoiceInputDTO
	decodeError := json.NewDecoder(r.Body).Decode(&input)
	if decodeError != nil {
		http.Error(w, decodeError.Error(), http.StatusBadRequest)
		return
	}

	if id != input.ID {
		response.JsonFail(w, []error{errors.New("invalid id")}, http.StatusBadRequest)
		return
	}

	data, domainError := domain.NewInvoice(input.CustomerID)

	if domainError != nil {
		response.JsonFail(w, domainError.Error, http.StatusBadRequest)
	}

	err := h.iUsecase.Update(id, data)
	if err != nil {
		if err.ErrCode == domain.Unexpected {
			response.JsonFail(w, err.Error, http.StatusInternalServerError)
			return
		}
		response.JsonFail(w, err.Error, http.StatusBadRequest)
		return
	}

	output := dto.UpdateInvoiceOutputDTO{
		ID:         data.ID,
		CustomerID: data.CustomerID,
	}

	response.JsonSuccess(w, output, http.StatusOK)
}

func invoiceToGetInvoiceOutputDTO(data *domain.Invoice) dto.GetInvoiceOutputDTO {
	return dto.GetInvoiceOutputDTO{
		ID:         data.ID,
		CustomerID: data.CustomerID,
	}
}
