# variable "common_settings" {
#   type = object({
#     resource_group = string
#     location       = string
#   })
# }

# variable "b2c_settings" {
#   type = object({
#     resource_group = string
#     domain         = string
#     location       = string
#   })
# }

variable "new_relic_license_key" {
  type      = string
  nullable  = false
  sensitive = true
}
