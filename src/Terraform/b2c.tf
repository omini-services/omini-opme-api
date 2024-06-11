#API_CONNECTORS
#B2C_USERFLOWS
#CUSTOM USER_ATTRIBUTES
#CUSTOM CLAIMS
# CHANGES TO B2C NEEDS TO BE APPLIED MANUALLY

resource "azurerm_aadb2c_directory" "b2c" {
  data_residency_location = "United States"
  domain_name             = local.b2c_settings.domain
  resource_group_name     = "rg-omni-opme-b2c-eastus-dev"
  sku_name                = "PremiumP1"
}

resource "azuread_application" "omini_opme_api" {
  display_name                   = "omni-opme-api"
  identifier_uris = [
    "https://b2comniopme.onmicrosoft.com/43ea57b0-e39b-4222-8a37-ca419c33171e/omni-opme-api",
  ]

  sign_in_audience              = "AzureADandPersonalMicrosoftAccount"

  api {
    mapped_claims_enabled          = false
    requested_access_token_version = 2

    oauth2_permission_scope {
      admin_consent_description  = "Access to the API"
      admin_consent_display_name = "Access to the API"
      enabled                    = true
      id                         = "9b242151-fc14-4543-93d6-e8d30657ff50"
      type                       = "Admin"
      value                      = "api.access"
    }
  }

  required_resource_access {
    resource_app_id = "00000003-0000-0000-c000-000000000000"

    resource_access {
      id   = "37f7f235-527c-4136-accd-4a02d197296e"
      type = "Scope"
    }
    resource_access {
      id   = "7427e0e9-2fba-42fe-b0c0-848c9e6a8182"
      type = "Scope"
    }
  }
}

# azuread_application.omini_opme_app:
resource "azuread_application" "omini_opme_app" {
  display_name             = "omni-opme-app"
  sign_in_audience         = "AzureADandPersonalMicrosoftAccount"

  api {
    mapped_claims_enabled          = false
    requested_access_token_version = 2
  }

  required_resource_access {
    resource_app_id = "00000003-0000-0000-c000-000000000000"

    resource_access {
      id   = "37f7f235-527c-4136-accd-4a02d197296e"
      type = "Scope"
    }
    resource_access {
      id   = "7427e0e9-2fba-42fe-b0c0-848c9e6a8182"
      type = "Scope"
    }
  }
  required_resource_access {
    resource_app_id = azuread_application.omini_opme_api.client_id

    resource_access {
      id   = azuread_application.omini_opme_api.api[0].oauth2_permission_scope.*.id[0]
      type = "Scope"
    }
  }

  single_page_application {
    redirect_uris = [
      "http://localhost/",
      "http://localhost:5173/",
      "https://jwt.ms/",
      "https://oauth.pstmn.io/v1/callback/",
      "https://5173-idx-omini-opme-fe-1717780507351.cluster-kc2r6y3mtba5mswcmol45orivs.cloudworkstations.dev/"
    ]
  }
}