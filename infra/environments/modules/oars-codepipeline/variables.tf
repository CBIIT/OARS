variable "project_name" {
  type        = string
  description = "Project name"
}
variable "environment" {
  description = "List of environments to deploy to"
  type        = set(string)
  # default     = ["dev", "qa", "uat"]
}

variable "environment2" {
  type = list(object({
    id   = number
    name = string
  }))
  default = [
    {
      id   = 1
      name = "qa1"
    },
    {
      id   = 10
      name = "staging"
    }
    , {
      id   = 20
      name = "production"
    }
  ]
}

variable "account_id" {
  type        = string
  description = "Account ID #"
}
variable "tags" {
  type        = map(any)
  description = "Tags to apply to all resources"
}
variable "environment_account" {
  type        = map(string)
  description = "the environments and their corresponding account ids"
}
