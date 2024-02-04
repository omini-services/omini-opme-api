package database

import (
	"github.com/omini-services/omini-opme-be/internal/entity"
	"gorm.io/gorm"
)

type ItemRepository struct {
	Db *gorm.DB
}

func NewItemRepository(db *gorm.DB) *ItemRepository {
	return &ItemRepository{Db: db}
}

func (r *ItemRepository) GetItems() ([]entity.Item, error) {
	var items []entity.Item
	err := r.Db.Find(&items).Error
	if err != nil {
		return nil, err
	}
	return items, nil
}
