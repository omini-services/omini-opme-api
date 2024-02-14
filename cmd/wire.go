//go:build wireinject
// +build wireinject

package main

import (
	"github.com/go-chi/chi/v5"
	"github.com/google/wire"
	"github.com/omini-services/omini-opme-be/internal/domain"
	itemHandler "github.com/omini-services/omini-opme-be/internal/item/handler"
	itemRepo "github.com/omini-services/omini-opme-be/internal/item/repository"
	itemCase "github.com/omini-services/omini-opme-be/internal/item/usecase"
	"gorm.io/gorm"
)

var (
	setItemRepositoryDependency = wire.NewSet(
		itemRepo.NewItemRepository,
		wire.Bind(new(domain.ItemRepository), new(*itemRepo.ItemRepository)),
	)

	setItemUsecaseDependency = wire.NewSet(
		itemCase.NewItemUsecase,
		wire.Bind(new(domain.ItemUsecase), new(*itemCase.ItemUsecase)),
	)
)

func NewItemHandler(r chi.Router, db *gorm.DB) *itemHandler.ItemHandler {
	wire.Build(
		setItemUsecaseDependency,
		setItemRepositoryDependency,
		itemHandler.NewItemHandler,
	)
	return &itemHandler.ItemHandler{}
}
