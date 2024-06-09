resource "azurerm_storage_account" "storage" {
  name                             = "stomniopmeeus"
  resource_group_name              = azurerm_resource_group.rg_app.name
  location                         = azurerm_resource_group.rg_app.location
  account_tier                     = "Standard"
  account_replication_type         = "LRS"
  cross_tenant_replication_enabled = false
}
