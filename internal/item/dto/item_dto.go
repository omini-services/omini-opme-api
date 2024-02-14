package dto

type AddItemInputDTO struct {
	Name string `json:"name"`
}

type AddItemOutputDTO struct {
	ID   int    `json:"id" fake:"{number:1,100}"`
	Name string `json:"name" fake:"{productname}"`
}

type UpdateItemInputDTO struct {
	ID   int    `json:"id"`
	Name string `json:"name"`
}

type UpdateItemOutputDTO struct {
	ID   int    `json:"id" fake:"{number:1,100}"`
	Name string `json:"name" fake:"{productname}"`
}

type GetItemOutputDTO struct {
	ID   int    `json:"id"`
	Name string `json:"name"`
}

type GetItemsInputDTO struct {
}

type GetItemsOutputDTO struct {
	ID           string `json:"id" fake:"{number:1,100}"`
	Name         string `json:"name" fake:"{productname}"`
	Manufacturer string `json:"manufacturer" fake:"{company}"`
	Group        string `json:"group" fake:"{productcategory}"`
}
