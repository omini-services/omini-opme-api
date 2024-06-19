resource "azurerm_service_plan" "function_auth_serviceplan" {
  name                = "asp-omni-opme-fcauth-eastus"
  resource_group_name = azurerm_resource_group.rg_app.name
  location            = azurerm_resource_group.rg_app.location
  os_type             = "Linux"
  sku_name            = "Y1"
}

resource "azurerm_linux_function_app" "function_auth" {
  name                       = "func-omni-opme-auth-eastus"
  location                   = azurerm_resource_group.rg_app.location
  resource_group_name        = azurerm_resource_group.rg_app.name
  service_plan_id            = azurerm_service_plan.function_auth_serviceplan.id
  storage_account_name       = azurerm_storage_account.storage.name
  storage_account_access_key = azurerm_storage_account.storage.primary_access_key

  site_config {
    application_stack {
      use_dotnet_isolated_runtime = true
      dotnet_version              = "8.0"
    }
  }

  app_settings = {
    "APIConnectors__ClientId"                                 = "c8c5ce24-820f-41ba-8560-d7a282d80d29"
    "APIConnectors__SignInSignUpExtension__BasicAuthPassword" = "admin"
    "APIConnectors__SignInSignUpExtension__BasicAuthUsername" = "admin"
    "FUNCTIONS_WORKER_RUNTIME"                                = "dotnet-isolated"
  }

  lifecycle {
    ignore_changes = [
      app_settings["WEBSITE_RUN_FROM_PACKAGE"],
      app_settings["WEBSITE_ENABLE_SYNC_UPDATE_SITE"]
    ]
  }
}
