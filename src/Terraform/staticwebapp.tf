resource "azurerm_static_site" "staticwebapp" {
  name                = "appswa-omini-opme-eastus"
  resource_group_name = azurerm_resource_group.rg_app.name
  location            = "eastus2"
}
