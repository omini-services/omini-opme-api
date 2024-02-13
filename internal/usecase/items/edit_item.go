package usecase

import (
	"github.com/omini-services/omini-opme-be/internal/entity"
)

type EditItemInputDTO struct {
	ID   int    `json:"id"`
	Name string `json:"name"`
}

type EditItemOutputDTO struct {
	ID   int    `json:"id" fake:"{number:1,100}"`
	Name string `json:"name" fake:"{productname}"`
}

type EditItemUseCase struct {
	ItemRepository entity.ItemRepositoryInterface
}

func NewEditItemUseCase(
	ItemRepository entity.ItemRepositoryInterface,
) *EditItemUseCase {
	return &EditItemUseCase{
		ItemRepository: ItemRepository,
	}
}

func (u *EditItemUseCase) Execute(id int, input EditItemInputDTO) (EditItemOutputDTO, []error) {
	item, validationErrors := entity.NewItem(input.Name)

	if validationErrors != nil {
		return EditItemOutputDTO{}, validationErrors
	}

	itemEdited, err := u.ItemRepository.Edit(id, item)

	if err != nil {
		return EditItemOutputDTO{}, []error{err}
	}

	dto := EditItemOutputDTO{
		ID:   itemEdited.ID,
		Name: itemEdited.Name,
	}

	return dto, nil
}
