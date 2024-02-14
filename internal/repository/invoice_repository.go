package repository

import (
	"github.com/omini-services/omini-opme-be/internal/domain"
	"gorm.io/gorm"
)

type InvoiceRepository struct {
	db *gorm.DB
}

func NewInvoiceRepository(db *gorm.DB) *InvoiceRepository {
	return &InvoiceRepository{db}
}

func (r *InvoiceRepository) Get() ([]domain.Invoice, error) {
	var invoices []domain.Invoice
	err := r.db.Find(&invoices).Error
	if err != nil {
		return nil, err
	}
	return invoices, nil
}

func (r *InvoiceRepository) Add(item *domain.Invoice) error {
	result := r.db.Create(&item)
	if result.Error != nil {
		return result.Error
	}

	return nil
}

func (r *InvoiceRepository) GetByID(id int) (domain.Invoice, error) {
	var invoice domain.Invoice
	err := r.db.First(&invoice, "id = ?", id).Error
	return invoice, err
}

func (r *InvoiceRepository) Update(id int, invoice *domain.Invoice) error {
	invoice.ID = id
	result := r.db.Save(invoice)
	if result.Error != nil {
		return result.Error
	}

	return nil
}
