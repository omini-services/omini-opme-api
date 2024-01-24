package entity

import (
	"errors"
	"time"

	"github.com/omini-services/omini-opme-be/pkg/entity"
)

var (
	ErrIDIsRequired = errors.New("id is required")
)

type Product struct {
	ID          entity.ID `json:"id"`
	Name        string    `json:"name"`
	Description string    `json:"description"`
	CreatedAt   time.Time `json:"created_at"`
}

func NewProduct(name string) (*Product, error) {
	product := &Product{
		ID:   entity.NewID(),
		Name: name,
	}

	err := product.Validate()
	if err != nil {
		return nil, err
	}

	return product, nil
}

func (p *Product) Validate() error {
	if p.ID.String() == "" {
		return ErrIDIsRequired
	}
	if _, err := entity.ParseID(p.ID.String()); err != nil {
		return ErrIDIsRequired
	}

	return nil
}
