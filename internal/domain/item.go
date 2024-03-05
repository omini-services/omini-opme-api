package domain

import (
	"errors"
	"time"

	"github.com/google/uuid"
)

type Item struct {
	ID            uuid.UUID
	ParentID      uuid.UUID
	Code          string
	Name          string
	SalesName     string
	Description   string
	Uom           string
	AnvisaCode    string
	AnvisaDueDate time.Time
	SupplierCode  string
	Cst           string
	SusCode       string
	NcmCode       string
	CreatedBy     uuid.UUID
	CreatedAt     time.Time
	UpdatedBy     uuid.UUID
	UpdatedAt     time.Time
}

type ItemRepository interface {
	Get() ([]Item, error)
	Add(item *Item) error
	GetByID(id uuid.UUID) (Item, error)
	Update(id uuid.UUID, item *Item) error
}

type ItemUsecase interface {
	Get() ([]Item, *ValidationError)
	GetByID(id uuid.UUID) (Item, *ValidationError)
	Update(id uuid.UUID, item *Item) *ValidationError
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
