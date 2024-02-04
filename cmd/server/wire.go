//go:build wireinject
// +build wireinject

package main

import (
	"github.com/google/wire"
	"github.com/omini-services/omini-opme-be/internal/entity"
	"github.com/omini-services/omini-opme-be/internal/infra/database"
	"github.com/omini-services/omini-opme-be/internal/infra/web"
	"gorm.io/gorm"
)

var setItemRepositoryDependency = wire.NewSet(
	database.NewItemRepository,
	wire.Bind(new(entity.ItemRepositoryInterface), new(*database.ItemRepository)),
)

func NewWebItemHandler(db *gorm.DB) *web.WebItemHandler {
	wire.Build(
		setItemRepositoryDependency,
		web.NewWebItemHandler,
	)
	return &web.WebItemHandler{}
}
