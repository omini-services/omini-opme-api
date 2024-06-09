locals {
  common_settings = {
    resource_group = "rg-omni-opme-eastus-dev"
    location       = "eastus"
  }

  b2c_settings = {
    resource_group = "rg-omni-opme-b2c-eastus-dev"
    domain         = "b2comniopme.onmicrosoft.com"
    location       = "eastus"
  }
}
