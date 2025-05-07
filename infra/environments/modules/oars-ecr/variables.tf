#ECR variable

variable "tags" {
  type         = map(any)
  description  = "Tags apply to all resources."
}
variable "project_name" {
  type        = string
  description = "Name of the project"
}
variable "environment_name" {
  type        = string
  description = "Name of the environment"
}