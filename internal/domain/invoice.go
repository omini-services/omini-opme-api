package domain

import (
	"errors"
	"time"
)

type Invoice struct {
	ID         int
	CustomerID int
	UpdatedAt  time.Time
	CreatedAt  time.Time
}

type InvoiceRepository interface {
	Get() ([]Invoice, error)
	Add(item *Invoice) error
	GetByID(id int) (Invoice, error)
	Update(id int, item *Invoice) error
}

type InvoiceUsecase interface {
	Get() ([]Invoice, *ValidationError)
	GetByID(id int) (Invoice, *ValidationError)
	Update(id int, item *Invoice) *ValidationError
	Add(item *Invoice) *ValidationError
}

func NewInvoice(customerID int) (*Invoice, *ValidationError) {
	invoice := &Invoice{
		CustomerID: customerID,
		UpdatedAt:  time.Now().UTC(),
		CreatedAt:  time.Now().UTC(),
	}
	err := invoice.isValid()
	if err != nil {
		return nil, err
	}
	return invoice, nil
}

func (i *Invoice) isValid() *ValidationError {
	domainError := ValidationError{
		ErrCode: InvalidRequest,
		Error:   []error{},
	}

	if i.CustomerID == 0 {
		domainError.Error = append(domainError.Error, errors.New("invalid customer"))
	}

	if len(domainError.Error) > 0 {
		return &domainError
	}

	return nil
}
