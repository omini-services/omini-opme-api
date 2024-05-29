resource "azurerm_service_plan" "appserviceplan" {
  name                = "asp-omni-opme-api-eastus"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = "F1"
}

resource "azurerm_linux_web_app" "appservice" {
  name                = "appsvc-omni-opme-eastus"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_service_plan.appserviceplan.location
  service_plan_id     = azurerm_service_plan.appserviceplan.id

  site_config {
    always_on        = false
    app_command_line = "dotnet Omini.Opme.Api.dll"

    application_stack {
      dotnet_version = "8.0"
    }
  }

  app_settings = {
    "AzureAd__ClientId"              = ""
    "AzureAd__Domain"                = ""
    "AzureAd__Instance"              = ""
    "AzureAd__SignedOutCallbackPath" = ""
    "AzureAd__SignUpSignInPolicyId"  = ""
    "DOTNET_ENVIRONMENT"             = "Development"
  }
}
