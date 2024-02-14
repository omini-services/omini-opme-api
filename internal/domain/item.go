package domain

import (
	"errors"
	"time"
)

type a struct {
}

type Item struct {
	ID        int       `json:"id"`
	Name      string    `json:"title" validate:"required"`
	UpdatedAt time.Time `json:"updated_at"`
	CreatedAt time.Time `json:"created_at"`
}

type ItemRepository interface {
	GetItems() ([]Item, error)
	Add(item *Item) error
	GetByID(id int) (Item, error)
	Update(id int, item *Item) error
}

type ItemUsecase interface {
	GetItems() ([]Item, *ValidationError)
	GetByID(id int) (Item, *ValidationError)
	Update(id int, item *Item) *ValidationError
	Add(item *Item) *ValidationError
}

func NewItem(name string) (*Item, *ValidationError) {
	item := &Item{
		Name: name,
	}
	err := item.isValid()
	if err != nil {
		return nil, err
	}
	return item, nil
}

func (i *Item) isValid() *ValidationError {
	domainError := ValidationError{
		ErrCode: InvalidRequest,
		Error:   []error{},
	}

	if i.Name == "" {
		domainError.Error = append(domainError.Error, errors.New("invalid name"))
	}

	if len(domainError.Error) > 0 {
		return &domainError
	}

	return nil
}
