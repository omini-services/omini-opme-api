resource "azurerm_service_plan" "function_auth_serviceplan" {
  name                = "asp-omni-opme-fcauth-eastus"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = "Y1"
}

resource "azurerm_linux_function_app" "function_auth" {
  name                        = "func-omni-opme-auth-eastus"
  location                    = azurerm_resource_group.rg.location
  resource_group_name         = azurerm_resource_group.rg.name
  service_plan_id             = azurerm_service_plan.function_auth_serviceplan.id
  storage_account_name        = azurerm_storage_account.storage.name
  storage_account_access_key  = azurerm_storage_account.storage.primary_access_key
  functions_extension_version = "~4"

  site_config {
    application_stack {
      use_dotnet_isolated_runtime = true
      dotnet_version              = "8.0"
    }
  }

  app_settings = {
    "APIConnectors__ClientId"                                 = "25c1f96b-1d38-42de-a3ac-051c80189600"
    "APIConnectors__SignInSignUpExtension__BasicAuthPassword" = "admin"
    "APIConnectors__SignInSignUpExtension__BasicAuthUsername" = "admin"
  }
}
