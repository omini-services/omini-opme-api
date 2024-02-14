package dto

type AddInvoiceInputDTO struct {
	CustomerID int `json:"customerId"`
}

type AddInvoiceOutputDTO struct {
	ID         int `json:"id" fake:"{number:1,100}"`
	CustomerID int `json:"customerId" fake:"{productname}"`
}

type UpdateInvoiceInputDTO struct {
	ID         int `json:"id" fake:"{number:1,100}"`
	CustomerID int `json:"customerId" fake:"{productname}"`
}

type UpdateInvoiceOutputDTO struct {
	ID         int `json:"id" fake:"{number:1,100}"`
	CustomerID int `json:"customerId" fake:"{productname}"`
}

type GetInvoiceOutputDTO struct {
	ID         int `json:"id" fake:"{number:1,100}"`
	CustomerID int `json:"customerId" fake:"{productname}"`
}

type GetInvoicesInputDTO struct {
}

type GetInvoicesOutputDTO struct {
	ID         int `json:"id" fake:"{number:1,100}"`
	CustomerID int `json:"customerId" fake:"{productname}"`
}
