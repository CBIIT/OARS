variable "project_name" {
  type        = string
  description = "Name of the project"
}
variable "environment_name" {
  type        = string
  description = "Name of the environment"
}
variable "tags" {
  type        = map(any)
  description = "Tags apply to all resources."
}
variable "vpc_id" {
  type        = string
  description = "VPC id"
}
variable "subnet_ids" {
  type        = list(string)
  description = "List of subnet ids"
} 
variable "domain_name" {
  type        = string
  description = "Domain name for the project"
}
variable "port" {
  type        = number
  description = "port number for the project"
}