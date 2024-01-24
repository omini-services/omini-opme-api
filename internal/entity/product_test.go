package entity

import (
	"testing"

	"github.com/stretchr/testify/assert"
)

func TestNewProduct(t *testing.T) {
	p, err := NewProduct("product 1")

	assert.Nil(t, err)
	assert.NotNil(t, p)
	assert.NotEmpty(t, p.ID)
	assert.Equal(t, "produc 1", p.Name)
}

//TODO: Test validates
// func TestProductWhenNameIsRequired(t *testing.T){

// }
