package entity

import (
	"errors"
)

type Item struct {
	ID   int
	Name string
}

func NewItem(name string) (*Item, []error) {
	item := &Item{
		Name: name,
	}
	err := item.IsValid()
	if err != nil {
		return nil, err
	}
	return item, nil
}

func (i *Item) IsValid() []error {
	validationErrors := make([]error, 0)
	if i.Name == "" {
		validationErrors = append(validationErrors, errors.New("invalid name"))
	}

	if len(validationErrors) > 0 {
		return validationErrors
	}

	return nil
}
