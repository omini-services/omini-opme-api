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
      version = "< 3.90.0"
    }
  }

  required_version = ">= 1.1.0"
}

provider "azurerm" {
  features {
    resource_group {
      prevent_deletion_if_contains_resources = false
    }
  }
}

resource "azurerm_resource_group" "rg_app" {
  name     = local.common_settings.resource_group
  location = local.common_settings.location
}

data "azurerm_client_config" "current" {}