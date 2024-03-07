package domain

import (
	"errors"
	"time"

	"github.com/google/uuid"
)

type Item struct {
	ID            uuid.UUID `gorm:"type:uuid;primaryKey;column:ID"`
	Code          string    `gorm:"type:varchar(50);not null;column:Code"`
	Name          string    `gorm:"type:varchar(400);not null;column:Name"`
	SalesName     string    `gorm:"type:varchar(400);not null;column:SalesName"`
	Description   string    `gorm:"type:varchar(1000);not null;column:Description"`
	Uom           string    `gorm:"type:varchar(20);not null;column:Uom"`
	AnvisaCode    string    `gorm:"type:varchar(20);not null;column:AnvisaCode"`
	AnvisaDueDate time.Time `gorm:"type:timestamptz;not null;column:AnvisaDueDate"`
	SupplierCode  string    `gorm:"type:varchar(50);not null;column:SupplierCode"`
	Cst           string    `gorm:"type:varchar(10);not null;column:Cst"`
	SusCode       string    `gorm:"type:varchar(20);not null;column:SusCode"`
	NcmCode       string    `gorm:"type:varchar(10);not null;column:NcmCode"`
	CreatedBy     uuid.UUID `gorm:"type:uuid;not null;column:CreatedBy"`
	CreatedAt     time.Time `gorm:"type:timestamptz;not null;column:CreatedAt"`
	UpdatedBy     uuid.UUID `gorm:"type:uuid;not null;column:UpdatedBy"`
	UpdatedAt     time.Time `gorm:"type:timestamptz;not null;column:UpdatedAt"`
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
