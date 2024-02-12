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

func (r *ItemRepository) GetByID(id int64) (entity.Item, error) {
	var item entity.Item
	err := r.Db.First(&item, "id = ?", id).Error
	return item, err
}

func (r *ItemRepository) Create(item *entity.Item) (entity.Item, error) {
	result := r.Db.Create(&item)
	if result.Error != nil {
		return entity.Item{}, result.Error
	}
	itemCreated := *item

	return itemCreated, nil
}
