package entity

type ItemRepositoryInterface interface {
	GetItems() ([]Item, error)
	Create(item *Item) (Item, error)
	GetByID(id int64) (Item, error)
	// GetTotal() (int, error)
}
