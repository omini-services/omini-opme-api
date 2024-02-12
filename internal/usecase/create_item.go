package usecase

import (
	"github.com/omini-services/omini-opme-be/internal/entity"
)

type CreateItemInputDTO struct {
	Name string `json:"name"`
}

type CreateItemOutputDTO struct {
	ID   int64  `json:"id" fake:"{number:1,100}"`
	Name string `json:"name" fake:"{productname}"`
}

type CreateItemUseCase struct {
	ItemRepository entity.ItemRepositoryInterface
}

func NewCreateItemUseCase(
	ItemRepository entity.ItemRepositoryInterface,
) *CreateItemUseCase {
	return &CreateItemUseCase{
		ItemRepository: ItemRepository,
	}
}

func (c *CreateItemUseCase) Execute(input CreateItemInputDTO) (CreateItemOutputDTO, []error) {
	item, validationErrors := entity.NewItem(input.Name)

	if validationErrors != nil {
		return CreateItemOutputDTO{}, validationErrors
	}

	itemCreated, err := c.ItemRepository.Create(item)

	if err != nil {
		return CreateItemOutputDTO{}, []error{err}
	}

	dto := CreateItemOutputDTO{
		ID:   itemCreated.ID,
		Name: itemCreated.Name,
	}

	return dto, nil
}
