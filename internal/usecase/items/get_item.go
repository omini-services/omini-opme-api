package usecase

import (
	"github.com/omini-services/omini-opme-be/internal/entity"
)

type GetItemOutputDTO struct {
	ID   int    `json:"id"`
	Name string `json:"name"`
}

type GetItemUseCase struct {
	ItemRepository entity.ItemRepositoryInterface
}

func NewGetItemUseCase(
	ItemRepository entity.ItemRepositoryInterface,
) *GetItemUseCase {
	return &GetItemUseCase{
		ItemRepository: ItemRepository,
	}
}

func (u *GetItemUseCase) Execute(id int) (GetItemOutputDTO, []error) {
	item, err := u.ItemRepository.GetByID(id)
	if err != nil {
		return GetItemOutputDTO{}, []error{err}
	}

	getItemOutput := itemToGetItemOutputDTO(*item)

	return getItemOutput, nil
}

func itemToGetItemOutputDTO(item entity.Item) GetItemOutputDTO {
	return GetItemOutputDTO{
		ID:   item.ID,
		Name: item.Name,
	}
}
