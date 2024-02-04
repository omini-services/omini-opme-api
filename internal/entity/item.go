package entity

type Item struct {
	ID   string
	Name string
}

func NewItem(id string, name string) (*Item, error) {
	item := &Item{
		ID:   id,
		Name: name,
	}
	//err := item.IsValid()
	// if err != nil {
	// 	return nil, err
	// }
	return item, nil
}

// func (o *Order) IsValid() error {
// 	if o.ID == "" {
// 		return errors.New("invalid id")
// 	}
// 	if o.Price <= 0 {
// 		return errors.New("invalid price")
// 	}
// 	if o.Tax <= 0 {
// 		return errors.New("invalid tax")
// 	}
// 	return nil
// }

// func (o *Order) CalculateFinalPrice() error {
// 	o.FinalPrice = o.Price + o.Tax
// 	err := o.IsValid()
// 	if err != nil {
// 		return err
// 	}
// 	return nil
// }
