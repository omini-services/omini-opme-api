package handlers

import (
	"encoding/json"
	"errors"
	"net/http"

	"github.com/brianvoe/gofakeit"
	"github.com/go-chi/chi/v5"
	"github.com/google/uuid"
	"github.com/omini-services/omini-opme-be/cmd/api/dto"
	"github.com/omini-services/omini-opme-be/cmd/api/response"
	"github.com/omini-services/omini-opme-be/internal/domain"
)

type ItemHandler struct {
	iUsecase domain.ItemUsecase
}

func NewItemHandler(r chi.Router, u domain.ItemUsecase) *ItemHandler {
	handler := &ItemHandler{
		iUsecase: u,
	}

	r.Route("/items", func(r chi.Router) {
		r.Get("/", handler.Get)
		r.Get("/{id}", handler.GetByID)
		r.Post("/", handler.Add)
		r.Put("/{id}", handler.Update)
	})

	return handler
}

func (h *ItemHandler) Get(w http.ResponseWriter, r *http.Request) {
	_, err := h.iUsecase.Get()

	outputs := []dto.ItemOutputDTO{}

	for i := 1; i <= 100; i++ {
		var output dto.ItemOutputDTO
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

func (h *ItemHandler) GetByID(w http.ResponseWriter, r *http.Request) {
	uuidString := chi.URLParam(r, "id")
	id, routeError := uuid.Parse(uuidString)

	if routeError != nil {
		response.JsonFail(w, []error{errors.New("invalid id")}, http.StatusBadRequest)
		return
	}

	data, err := h.iUsecase.GetByID(id)
	if err != nil {
		if err.ErrCode == domain.Unexpected {
			response.JsonFail(w, err.Error, http.StatusInternalServerError)
			return
		}
		response.JsonFail(w, err.Error, http.StatusBadRequest)
		return
	}

	output := MapItemToOutputItem(&data)

	response.JsonSuccess(w, output, http.StatusOK)
}

func (h *ItemHandler) Add(w http.ResponseWriter, r *http.Request) {
	var input dto.AddItemInputDTO
	decodeError := json.NewDecoder(r.Body).Decode(&input)
	if decodeError != nil {
		http.Error(w, decodeError.Error(), http.StatusBadRequest)
		return
	}

	data := MapAddItemInputToItem(&input)

	// if domainError != nil {
	// 	response.JsonFail(w, domainError.Error, http.StatusBadRequest)
	// }

	err := h.iUsecase.Add(data)

	if err != nil {
		if err.ErrCode == domain.Unexpected {
			response.JsonFail(w, err.Error, http.StatusInternalServerError)
			return
		}
		response.JsonFail(w, err.Error, http.StatusBadRequest)
		return
	}

	output := MapItemToOutputItem(data)

	response.JsonSuccess(w, output, http.StatusOK)
}

func (h *ItemHandler) Update(w http.ResponseWriter, r *http.Request) {
	uuidString := chi.URLParam(r, "id")
	id, routeError := uuid.Parse(uuidString)

	if routeError != nil {
		response.JsonFail(w, []error{errors.New("invalid id")}, http.StatusBadRequest)
		return
	}

	var input dto.UpdateItemInputDTO
	decodeError := json.NewDecoder(r.Body).Decode(&input)
	if decodeError != nil {
		http.Error(w, decodeError.Error(), http.StatusBadRequest)
		return
	}

	if id != input.ID {
		response.JsonFail(w, []error{errors.New("invalid id")}, http.StatusBadRequest)
		return
	}

	data, domainError := domain.NewItem(input.Name)

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

	output := MapItemToOutputItem(data)

	response.JsonSuccess(w, output, http.StatusOK)
}

func MapAddItemInputToItem(dto *dto.AddItemInputDTO) *domain.Item {
	return &domain.Item{
		Code:          dto.Code,
		Name:          dto.Name,
		SalesName:     dto.SalesName,
		Description:   dto.Description,
		Uom:           dto.Uom,
		AnvisaCode:    dto.AnvisaCode,
		AnvisaDueDate: dto.AnvisaDueDate,
		SupplierCode:  dto.SupplierCode,
		Cst:           dto.Cst,
		SusCode:       dto.SusCode,
		NcmCode:       dto.NcmCode,
	}
}

func MapItemToOutputItem(data *domain.Item) *dto.ItemOutputDTO {
	return &dto.ItemOutputDTO{
		Code:          data.Code,
		Name:          data.Name,
		SalesName:     data.SalesName,
		Description:   data.Description,
		Uom:           data.Uom,
		AnvisaCode:    data.AnvisaCode,
		AnvisaDueDate: data.AnvisaDueDate,
		SupplierCode:  data.SupplierCode,
		Cst:           data.Cst,
		SusCode:       data.SusCode,
		NcmCode:       data.NcmCode,
	}
}
