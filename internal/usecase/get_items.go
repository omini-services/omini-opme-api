package usecase

import (
	"github.com/brianvoe/gofakeit/v6"
	"github.com/omini-services/omini-opme-be/internal/entity"
)

type GetItemsInputDTO struct {
}

type GetItemsOutputDTO struct {
	ID           string `json:"id" fake:"{number:1,100}"`
	Name         string `json:"name" fake:"{productname}"`
	Manufacturer string `json:"manufacturer" fake:"{company}"`
	Group        string `json:"group" fake:"{productcategory}"`
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
	//items, err := c.ItemRepository.GetItems()
	_, err := c.ItemRepository.GetItems()
	if err != nil {
		return []GetItemsOutputDTO{}, err
	}

	getItemsOutput := []GetItemsOutputDTO{}

	for i := 1; i <= 100; i++ {
		var getItemOutput GetItemsOutputDTO
		gofakeit.Struct(&getItemOutput)
		getItemsOutput = append(getItemsOutput, getItemOutput)
	}

	// for _, item := range items {
	// 	getItemOutput := itemToGetItemOutputDTO(item)
	// 	getItemsOutput = append(getItemsOutput, getItemOutput)
	// }

	return getItemsOutput, nil
}

// func itemToGetItemOutputDTO(item entity.Item) GetItemsOutputDTO {
// 	return GetItemsOutputDTO{
// 		ID:   item.ID,
// 		Name: item.Name,
// 	}
// }
