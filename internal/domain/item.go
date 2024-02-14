package domain

import (
	"errors"
	"time"
)

type Item struct {
	ID        int
	Name      string
	UpdatedAt time.Time
	CreatedAt time.Time
}

type ItemRepository interface {
	Get() ([]Item, error)
	Add(item *Item) error
	GetByID(id int) (Item, error)
	Update(id int, item *Item) error
}

type ItemUsecase interface {
	Get() ([]Item, *ValidationError)
	GetByID(id int) (Item, *ValidationError)
	Update(id int, item *Item) *ValidationError
	Add(item *Item) *ValidationError
}

func NewItem(name string) (*Item, *ValidationError) {
	item := &Item{
		Name:      name,
		UpdatedAt: time.Now().UTC(),
		CreatedAt: time.Now().UTC(),
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
