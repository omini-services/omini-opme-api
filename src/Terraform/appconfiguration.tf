resource "azurerm_app_configuration" "appconfiguration" {
  name                = "appconf-omni-opme-eastus"
  resource_group_name = azurerm_resource_group.rg_app.name
  location            = azurerm_resource_group.rg_app.location
}

# resource "azurerm_app_configuration_key" "test" {
#   configuration_store_id = azurerm_app_configuration.appconfiguration.id
#   key                    = "appConfKey1"
#   label                  = "somelabel"
#   value                  = "a test"
# }