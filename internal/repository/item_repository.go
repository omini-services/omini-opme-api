package repository

import (
	"github.com/omini-services/omini-opme-be/internal/domain"
	"gorm.io/gorm"
)

type ItemRepository struct {
	db *gorm.DB
}

func NewItemRepository(db *gorm.DB) *ItemRepository {
	return &ItemRepository{db}
}

func (r *ItemRepository) Get() ([]domain.Item, error) {
	var items []domain.Item
	err := r.db.Find(&items).Error
	if err != nil {
		return nil, err
	}
	return items, nil
}

func (r *ItemRepository) Add(item *domain.Item) error {
	result := r.db.Create(&item)
	if result.Error != nil {
		return result.Error
	}

	return nil
}

func (r *ItemRepository) GetByID(id int) (domain.Item, error) {
	var item domain.Item
	err := r.db.First(&item, "id = ?", id).Error
	return item, err
}

func (r *ItemRepository) Update(id int, item *domain.Item) error {
	item.ID = id
	result := r.db.Save(item)
	if result.Error != nil {
		return result.Error
	}

	return nil
}
