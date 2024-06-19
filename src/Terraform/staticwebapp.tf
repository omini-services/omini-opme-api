resource "azurerm_static_web_app" "staticwebapp" {
  name                = "appswa-omini-opme-eastus"
  resource_group_name = azurerm_resource_group.rg_app.name
  location            = azurerm_resource_group.rg_app.location
}
