package usecase

import "github.com/omini-services/omini-opme-be/internal/entity"

type GetItemsInputDTO struct {
}

type GetItemsOutputDTO struct {
	ID   string `json:"id"`
	Name string `json:"name"`
}

type GetItemsUseCase struct {
	ItemRepository entity.ItemRepositoryInterface
}

func NewGetItemsUseCase(
	ItemRepository entity.ItemRepositoryInterface,
) *GetItemsUseCase {
	return &GetItemsUseCase{
		ItemRepository: ItemRepository,
	}
}

func (c *GetItemsUseCase) Execute(input GetItemsInputDTO) ([]GetItemsOutputDTO, error) {
	items, err := c.ItemRepository.GetItems()
	if err != nil {
		return []GetItemsOutputDTO{}, err
	}

	getItemsOutput := []GetItemsOutputDTO{}

	for _, item := range items {
		getItemOutput := itemToGetItemOutputDTO(item)
		getItemsOutput = append(getItemsOutput, getItemOutput)
	}

	return getItemsOutput, nil
}

func itemToGetItemOutputDTO(item entity.Item) GetItemsOutputDTO {
	return GetItemsOutputDTO{
		ID:   item.ID,
		Name: item.Name,
	}
}
