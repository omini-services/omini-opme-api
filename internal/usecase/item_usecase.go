package usecase

import (
	"errors"
	"time"

	"github.com/google/uuid"
	"github.com/omini-services/omini-opme-be/internal/domain"
	"gorm.io/gorm"
)

type GetItemsOutputDTO struct {
	ID           string `json:"id" fake:"{number:1,100}"`
	Name         string `json:"name" fake:"{productname}"`
	Manufacturer string `json:"manufacturer" fake:"{company}"`
	Group        string `json:"group" fake:"{productcategory}"`
}

type ItemUsecase struct {
	itemRepository domain.ItemRepository
}

func NewItemUsecase(r domain.ItemRepository) *ItemUsecase {
	return &ItemUsecase{
		itemRepository: r,
	}
}

func (u *ItemUsecase) Get() ([]domain.Item, *domain.ValidationError) {
	items, err := u.itemRepository.Get()
	if err != nil {
		return []domain.Item{}, &domain.ValidationError{ErrCode: domain.Unexpected, Error: []error{err}}
	}

	return items, nil
}

func (u *ItemUsecase) GetByID(id uuid.UUID) (domain.Item, *domain.ValidationError) {
	item, err := u.itemRepository.GetByID(id)
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return domain.Item{}, &domain.ValidationError{ErrCode: domain.InvalidRequest, Error: []error{err}}
		}

		return domain.Item{}, &domain.ValidationError{ErrCode: domain.Unexpected, Error: []error{err}}
	}

	return item, nil
}

func (u *ItemUsecase) Update(id uuid.UUID, item *domain.Item) *domain.ValidationError {
	err := u.itemRepository.Update(id, item)

	if err != nil {
		return &domain.ValidationError{ErrCode: domain.Unexpected, Error: []error{err}}
	}

	return nil
}

func (i *ItemUsecase) Add(item *domain.Item) *domain.ValidationError {
	item.ID = uuid.New()
	item.CreatedBy = uuid.New()
	item.CreatedAt = time.Now()
	item.UpdatedBy = uuid.New()
	item.UpdatedAt = time.Now()

	err := i.itemRepository.Add(item)

	if err != nil {
		return &domain.ValidationError{ErrCode: domain.Unexpected, Error: []error{err}}
	}

	return nil
}
