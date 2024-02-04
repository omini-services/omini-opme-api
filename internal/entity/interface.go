package entity

type ItemRepositoryInterface interface {
	GetItems() ([]Item, error)
	// GetTotal() (int, error)
}
