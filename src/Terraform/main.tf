terraform {
  backend "azurerm" {
    resource_group_name  = "rg-omni-opme-terraform-eastus-dev"
    storage_account_name = "stomniopmetfstateeusdev"
    container_name       = "terraform-state"
    key                  = "terraform.tfstate"
  }

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.83.0"
    }
  }

  required_version = ">= 1.1.0"
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg_app" {
  name     = var.common_settings.resource_group
  location = var.common_settings.location
}

# resource "azurerm_resource_group" "rg_b2c" {
#   name     = var.b2c_settings.resource_group
#   location = var.b2c_settings.location
# }
