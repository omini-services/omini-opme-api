package handler

import (
	"encoding/json"
	"errors"
	"net/http"
	"strconv"

	"github.com/go-chi/chi/v5"
	"github.com/omini-services/omini-opme-be/internal/entity"
	"github.com/omini-services/omini-opme-be/internal/infra/web/response"
	items_usecase "github.com/omini-services/omini-opme-be/internal/usecase/items"
)

type WebItemHandler struct {
	ItemRepository entity.ItemRepositoryInterface
}

func NewWebItemHandler(
	ItemRepository entity.ItemRepositoryInterface,
) *WebItemHandler {
	return &WebItemHandler{
		ItemRepository: ItemRepository,
	}
}

func (h *WebItemHandler) GetAll(w http.ResponseWriter, r *http.Request) {
	var dto items_usecase.GetItemsInputDTO

	getItems := items_usecase.NewGetItemsUseCase(h.ItemRepository)
	output, err := getItems.Execute(dto)
	if err != nil {
		response.JsonFail(w, []error{errors.New("could not get items")}, http.StatusInternalServerError)
		return
	}

	response.JsonSuccess(w, output, http.StatusOK)
}

func (h *WebItemHandler) Get(w http.ResponseWriter, r *http.Request) {
	id := chi.URLParam(r, "id")
	parsedId, routeError := strconv.Atoi(id)

	if routeError != nil {
		response.JsonFail(w, []error{errors.New("invalid id")}, http.StatusBadRequest)
		return
	}

	getItem := items_usecase.NewGetItemUseCase(h.ItemRepository)
	output, err := getItem.Execute(parsedId)
	if err != nil {
		response.JsonFail(w, err, http.StatusBadRequest)
		return
	}

	response.JsonSuccess(w, output, http.StatusOK)
}

func (h *WebItemHandler) Create(w http.ResponseWriter, r *http.Request) {
	var dto items_usecase.CreateItemInputDTO
	decodeError := json.NewDecoder(r.Body).Decode(&dto)
	if decodeError != nil {
		http.Error(w, decodeError.Error(), http.StatusBadRequest)
		return
	}

	createItem := items_usecase.NewCreateItemUseCase(h.ItemRepository)
	output, err := createItem.Execute(dto)

	if err != nil {
		response.JsonFail(w, err, http.StatusBadRequest)
		return
	}

	response.JsonSuccess(w, output, http.StatusOK)
}

func (h *WebItemHandler) Edit(w http.ResponseWriter, r *http.Request) {
	stringId := chi.URLParam(r, "id")
	id, routeError := strconv.Atoi(stringId)

	if routeError != nil {
		response.JsonFail(w, []error{errors.New("invalid id")}, http.StatusBadRequest)
		return
	}

	var dto items_usecase.EditItemInputDTO
	decodeError := json.NewDecoder(r.Body).Decode(&dto)
	if decodeError != nil {
		http.Error(w, decodeError.Error(), http.StatusBadRequest)
		return
	}

	if id != dto.ID {
		response.JsonFail(w, []error{errors.New("invalid id")}, http.StatusBadRequest)
		return
	}

	getItem := items_usecase.NewEditItemUseCase(h.ItemRepository)
	output, err := getItem.Execute(id, dto)
	if err != nil {
		response.JsonFail(w, err, http.StatusBadRequest)
		return
	}

	response.JsonSuccess(w, output, http.StatusOK)
}
