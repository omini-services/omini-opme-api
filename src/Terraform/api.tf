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
    "AzureAd__ClientId"              = "25c1f96b-1d38-42de-a3ac-051c80189600"
    "AzureAd__Domain"                = "b2comniopme.onmicrosoft.com"
    "AzureAd__Instance"              = "https://b2comniopme.b2clogin.com"
    "AzureAd__SignedOutCallbackPath" = "/signout/B2C_1_SignUp_SignIn"
    "AzureAd__SignUpSignInPolicyId"  = "B2C_1_SignUp_SignIn"
    "DOTNET_ENVIRONMENT"             = "Development"
  }
}
