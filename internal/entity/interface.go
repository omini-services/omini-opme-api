package entity

type ItemRepositoryInterface interface {
	GetItems() ([]Item, error)
	Create(item *Item) (Item, error)
	GetByID(id int) (*Item, error)
	Edit(id int, item *Item) (Item, error)
	// GetTotal() (int, error)
}
