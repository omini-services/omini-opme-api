package dto

import (
	"time"

	"github.com/google/uuid"
)

type AddItemInputDTO struct {
	ParentID      uuid.UUID `json:"parentId"`
	Code          string    `json:"code"`
	Name          string    `json:"name"`
	SalesName     string    `json:"salesName"`
	Description   string    `json:"description"`
	Uom           string    `json:"uom"`
	AnvisaCode    string    `json:"anvisaCode"`
	AnvisaDueDate time.Time `json:"anvisaDueDate"`
	SupplierCode  string    `json:"supplierCode"`
	Cst           string    `json:"cst"`
	SusCode       string    `json:"susCode"`
	NcmCode       string    `json:"ncmCode"`
}

type ItemOutputDTO struct {
	ID            uuid.UUID `json:"id"`
	ParentID      uuid.UUID `json:"parentId"`
	Code          string    `json:"code"`
	Name          string    `json:"name"`
	SalesName     string    `json:"salesName"`
	Description   string    `json:"description"`
	Uom           string    `json:"uom"`
	AnvisaCode    string    `json:"anvisaCode"`
	AnvisaDueDate time.Time `json:"anvisaDueDate"`
	SupplierCode  string    `json:"supplierCode"`
	Cst           string    `json:"cst"`
	SusCode       string    `json:"susCode"`
	NcmCode       string    `json:"ncmCode"`
	CreatedBy     uuid.UUID `json:"createdBy"`
	CreatedAt     time.Time `json:"createdAt"`
	UpdatedBy     uuid.UUID `json:"updatedBy"`
	UpdatedAt     time.Time `json:"updatedAt"`
}

type UpdateItemInputDTO struct {
	ID            uuid.UUID `json:"id"`
	ParentID      uuid.UUID `json:"parentId"`
	Code          string    `json:"code"`
	Name          string    `json:"name"`
	SalesName     string    `json:"salesName"`
	Description   string    `json:"description"`
	Uom           string    `json:"uom"`
	AnvisaCode    string    `json:"anvisaCode"`
	AnvisaDueDate time.Time `json:"anvisaDueDate"`
	SupplierCode  string    `json:"supplierCode"`
	Cst           string    `json:"cst"`
	SusCode       string    `json:"susCode"`
	NcmCode       string    `json:"ncmCode"`
}
