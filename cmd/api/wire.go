//go:build wireinject
// +build wireinject

package api

import (
	"github.com/go-chi/chi/v5"
	"github.com/google/wire"
	"github.com/omini-services/omini-opme-be/cmd/api/handlers"
	"github.com/omini-services/omini-opme-be/internal/domain"
	"github.com/omini-services/omini-opme-be/internal/repository"
	"github.com/omini-services/omini-opme-be/internal/usecase"

	"gorm.io/gorm"
)

func NewApiHandler(r chi.Router) *handlers.ApiHandler {
	wire.Build(
		handlers.NewApiHandler,
	)

	return &handlers.ApiHandler{}
}

var (
	setItemRepositoryDependency = wire.NewSet(
		repository.NewItemRepository,
		wire.Bind(new(domain.ItemRepository), new(*repository.ItemRepository)),
	)

	setItemUsecaseDependency = wire.NewSet(
		usecase.NewItemUsecase,
		wire.Bind(new(domain.ItemUsecase), new(*usecase.ItemUsecase)),
	)
)

func NewItemHandler(r chi.Router, db *gorm.DB) *handlers.ItemHandler {
	wire.Build(
		setItemUsecaseDependency,
		setItemRepositoryDependency,
		handlers.NewItemHandler,
	)
	return &handlers.ItemHandler{}
}

var (
	setInvoiceRepositoryDependency = wire.NewSet(
		repository.NewInvoiceRepository,
		wire.Bind(new(domain.InvoiceRepository), new(*repository.InvoiceRepository)),
	)

	setInvoiceUsecaseDependency = wire.NewSet(
		usecase.NewInvoiceUsecase,
		wire.Bind(new(domain.InvoiceUsecase), new(*usecase.InvoiceUsecase)),
	)
)

func NewInvoiceHandler(r chi.Router, db *gorm.DB) *handlers.InvoiceHandler {
	wire.Build(
		setInvoiceUsecaseDependency,
		setInvoiceRepositoryDependency,
		handlers.NewInvoiceHandler,
	)
	return &handlers.InvoiceHandler{}
}
