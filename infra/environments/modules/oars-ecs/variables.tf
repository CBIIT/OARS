#General Variables
variable "tags" {
  type        = map(any)
  description = "Tags apply to all resources."
}
variable "project_name" {
  type        = string
  description = "Name of the project"
}
variable "environment_name" {
  type        = string
  description = "Name of the environment"
}
variable "aspnetcore_environment_name" {
  type        = string
  description = "Name of the environment (DeployedDev,DeployedQA,DeployedUAT,DeployedProd)"
}
variable "oars_environment" {
  type        = string
  description = "Name of the oars environment (dev,qa,uat,prod)"
}
#ECS variables
variable "task_def_cpu" {
  type        = number
  description = "Nci thor container cpu resources"
}
variable "task_def_memory" {
  type        = number
  description = "Nci thor container memory resources"
}
variable "desired_count" {
  type        = number
  description = "Desired task count for ECS service"
}
variable "subnets" {
  type        = list(string)
  description = "Subnets IDs where the containers will be running"
}
variable "vpc_id" {
  type        = string
  description = "Vpc id"
}
variable "load_balancer_sg" {
  type        = string
  description = "Security group of load balancer"
}
variable "target_group_arn" {
  type        = string
  description = ""
}
variable "task_def_host_port" {
  type        = number
  description = "Nci OARS host port (80,8080)"
}

variable "task_def_container_port" {
  type        = number
  description = "Nci OARS container port (80,8080)"
}
variable "load_balancer_container_port" {
  type        = number
  description = "Nci OARS Load Balancer container port (80,8080)"
}