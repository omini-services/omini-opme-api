//go:build wireinject
// +build wireinject

package main

import (
	"github.com/google/wire"
	"github.com/omini-services/omini-opme-be/internal/entity"
	"github.com/omini-services/omini-opme-be/internal/infra/database"
	"github.com/omini-services/omini-opme-be/internal/infra/web/handler"
	"gorm.io/gorm"
)

var setItemRepositoryDependency = wire.NewSet(
	database.NewItemRepository,
	wire.Bind(new(entity.ItemRepositoryInterface), new(*database.ItemRepository)),
)

func NewWebItemHandler(db *gorm.DB) *handler.WebItemHandler {
	wire.Build(
		setItemRepositoryDependency,
		handler.NewWebItemHandler,
	)
	return &handler.WebItemHandler{}
}

func NewApiHandler() *handler.ApiHandler {
	wire.Build(
		handler.NewApiHandler,
	)
	return &handler.ApiHandler{}
}
