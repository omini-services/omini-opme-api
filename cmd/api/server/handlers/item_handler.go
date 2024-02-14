package handler

import (
	"encoding/json"
	"errors"
	"net/http"
	"strconv"

	"github.com/brianvoe/gofakeit"
	"github.com/go-chi/chi/v5"
	"github.com/omini-services/omini-opme-be/internal/domain"
	"github.com/omini-services/omini-opme-be/internal/infra/web/response"
	"github.com/omini-services/omini-opme-be/internal/item/dto"
)

type ItemHandler struct {
	iUsecase domain.ItemUsecase
}

func NewItemHandler(r chi.Router, u domain.ItemUsecase) *ItemHandler {
	handler := &ItemHandler{
		iUsecase: u,
	}

	r.Route("/items", func(r chi.Router) {
		r.Get("/", handler.GetItems)
		r.Get("/{id}", handler.GetByID)
		r.Post("/{id}", handler.Add)
		r.Put("/{id}", handler.Update)
	})

	return handler
}

func (h *ItemHandler) GetItems(w http.ResponseWriter, r *http.Request) {
	_, err := h.iUsecase.GetItems()

	getItemsOutput := []dto.GetItemsOutputDTO{}

	for i := 1; i <= 100; i++ {
		var getItemOutput dto.GetItemsOutputDTO
		gofakeit.Struct(&getItemOutput)
		getItemsOutput = append(getItemsOutput, getItemOutput)
	}

	if err != nil {
		if err.ErrCode == domain.Unexpected {
			response.JsonFail(w, err.Error, http.StatusInternalServerError)
			return
		}
		response.JsonFail(w, err.Error, http.StatusBadRequest)
		return
	}

	response.JsonSuccess(w, getItemsOutput, http.StatusOK)
}

func (h *ItemHandler) GetByID(w http.ResponseWriter, r *http.Request) {
	id := chi.URLParam(r, "id")
	parsedId, routeError := strconv.Atoi(id)

	if routeError != nil {
		response.JsonFail(w, []error{errors.New("invalid id")}, http.StatusBadRequest)
		return
	}

	item, err := h.iUsecase.GetByID(parsedId)
	if err != nil {
		if err.ErrCode == domain.Unexpected {
			response.JsonFail(w, err.Error, http.StatusInternalServerError)
			return
		}
		response.JsonFail(w, err.Error, http.StatusBadRequest)
		return
	}

	output := itemToGetItemOutputDTO(&item)

	response.JsonSuccess(w, output, http.StatusOK)
}

func (h *ItemHandler) Add(w http.ResponseWriter, r *http.Request) {
	var input dto.AddItemInputDTO
	decodeError := json.NewDecoder(r.Body).Decode(&input)
	if decodeError != nil {
		http.Error(w, decodeError.Error(), http.StatusBadRequest)
		return
	}

	item, domainError := domain.NewItem(input.Name)

	if domainError != nil {
		response.JsonFail(w, domainError.Error, http.StatusBadRequest)
	}

	err := h.iUsecase.Add(item)

	if err != nil {
		if err.ErrCode == domain.Unexpected {
			response.JsonFail(w, err.Error, http.StatusInternalServerError)
			return
		}
		response.JsonFail(w, err.Error, http.StatusBadRequest)
		return
	}

	output := dto.AddItemOutputDTO{
		ID:   item.ID,
		Name: item.Name,
	}

	response.JsonSuccess(w, output, http.StatusOK)
}

func (h *ItemHandler) Update(w http.ResponseWriter, r *http.Request) {
	stringId := chi.URLParam(r, "id")
	id, routeError := strconv.Atoi(stringId)

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

	item, domainError := domain.NewItem(input.Name)

	if domainError != nil {
		response.JsonFail(w, domainError.Error, http.StatusBadRequest)
	}

	err := h.iUsecase.Update(id, item)
	if err != nil {
		if err.ErrCode == domain.Unexpected {
			response.JsonFail(w, err.Error, http.StatusInternalServerError)
			return
		}
		response.JsonFail(w, err.Error, http.StatusBadRequest)
		return
	}

	output := dto.UpdateItemOutputDTO{
		ID:   item.ID,
		Name: item.Name,
	}

	response.JsonSuccess(w, output, http.StatusOK)
}

func itemToGetItemOutputDTO(item *domain.Item) dto.GetItemOutputDTO {
	return dto.GetItemOutputDTO{
		ID:   item.ID,
		Name: item.Name,
	}
}
