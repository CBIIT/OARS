variable "project_name" {
  type        = string
  description = "Name of the project"
}
variable "environment_name" {
  type        = string
  description = "Name of the environment"
}
variable "vpc_id" {
  type        = string
  description = "VPC id"
}

variable "subnet_id" {
  type        = string
  description = "Subnet id for BastionHost"
}
variable "tags" {
  type        = map(any)
  description = "Tags apply to all resources."
}
variable "bastion_instance_type" {
  type        = string  
  description = "Type of instance for Bastion Host"
}
variable "bastion_ami" {
  type        = string  
  description = "AMI used for Bastion Host"
}