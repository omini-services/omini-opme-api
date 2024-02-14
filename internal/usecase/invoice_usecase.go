package usecase

import (
	"errors"

	"github.com/omini-services/omini-opme-be/internal/domain"
	"gorm.io/gorm"
)

type GetInvoicesOutputDTO struct {
	ID         string `json:"id" fake:"{number:1,100}"`
	CustomerID string `json:"name" fake:"{productname}"`
}

type InvoiceUsecase struct {
	invoiceRepository domain.InvoiceRepository
}

func NewInvoiceUsecase(r domain.InvoiceRepository) *InvoiceUsecase {
	return &InvoiceUsecase{
		invoiceRepository: r,
	}
}

func (u *InvoiceUsecase) Get() ([]domain.Invoice, *domain.ValidationError) {
	invoices, err := u.invoiceRepository.Get()
	if err != nil {
		return []domain.Invoice{}, &domain.ValidationError{ErrCode: domain.Unexpected, Error: []error{err}}
	}

	return invoices, nil
}

func (u *InvoiceUsecase) GetByID(id int) (domain.Invoice, *domain.ValidationError) {
	invoice, err := u.invoiceRepository.GetByID(id)
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return domain.Invoice{}, &domain.ValidationError{ErrCode: domain.InvalidRequest, Error: []error{err}}
		}

		return domain.Invoice{}, &domain.ValidationError{ErrCode: domain.Unexpected, Error: []error{err}}
	}

	return invoice, nil
}

func (u *InvoiceUsecase) Update(id int, item *domain.Invoice) *domain.ValidationError {
	err := u.invoiceRepository.Update(id, item)

	if err != nil {
		return &domain.ValidationError{ErrCode: domain.Unexpected, Error: []error{err}}
	}

	return nil
}

func (i *InvoiceUsecase) Add(item *domain.Invoice) *domain.ValidationError {
	err := i.invoiceRepository.Add(item)

	if err != nil {
		return &domain.ValidationError{ErrCode: domain.Unexpected, Error: []error{err}}
	}

	return nil
}
