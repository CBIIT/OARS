###############################
##### Terraform Variables #####
###############################

############################

locals {
  private_subnet_ids = [
    for az, id in module.prod_noncomm_vpc.private_subnet_ids :
    id
  ]
  public_subnets_ids = [
    for az, id in module.prod_noncomm_vpc.public_subnet_ids :
    id
  ]
  myaccount = var.aws_accounts.theradex-production-nci.account_id
}

############################
##### Config Variables #####
############################

variable "org-id" {
  description = "ID of the Organization"
  type        = string
  default     = "o-0v4zacm3u8"
}

variable "secure-vpc-tgw-1-id" {
  description = "ID of the secure-vpc-tgw-1 Transit Gateway"
  type        = string
  default     = "tgw-0b5475750f7169121"
}

variable "aws_accounts" {
  description = "Map of Relevant Organization AWS accounts"
  type        = map(any)
  default = {
    theradex-prodelopment-commercial = {
      account_id = "104324776186"
      attribute1 = ""
    }
    theradex-production-commercial = {
      account_id = "078250543436"
      attribute1 = ""
    }
    theradex-uat-commercial = {
      account_id = "855492361387"
      attribute1 = ""
    }
    theradex-prodelopment-nci = {
      account_id = "993530973844"
      attribute1 = ""
    }
    theradex-production-nci = {
      account_id = "590811900011"
      attribute1 = ""
    }
    theradex-uat-nci = {
      account_id = "352847057549"
      attribute1 = ""
    }
    theradex-shared-service = {
      account_id = "606199607275"
      attribute1 = ""
    }
  }
}

variable "common_tags" {
  description = "Map of Common Tags"
  type        = map(any)
  default = {
    "Managed By"          = "Terraform"
    "Owner"               = "CBV"
    "account"             = "production_nci"
    "environment"         = "production"
    "Application Type"    = "empty"
    "Application Version" = "empty"
    "BackupPlan"          = "empty"
    "Description"         = "empty"
    "ExpireDate"          = "empty"
    "Operating System"    = "empty"
    "Project"             = "empty"
    "Region"              = "empty"
    "Team DL"             = "empty"
    "Temporary"           = "empty"
  }
}


variable "qrcg_cluster_name" {
  description = "QR Code Generation Cluster Name"
  type        = string
  default     = "theradex-production-nci-cluster"
}

variable "qrcg_container_name" {
  description = "QR Code Generation ECS Container Name"
  type        = string
  default     = "TheradexQRCodeGeneration"
}

variable "qrcg_service_name" {
  description = "QR Code Generation ECS Service Name"
  type        = string
  default     = "TheradexQRCodeGeneration"
}

variable "qrcg_taskdefinition_name" {
  description = "QR Code Generation Task Definition Name"
  type        = string
  default     = "TheradexQRCodeGeneration"
}
################################
##### CloudTrail Variables #####
################################

#########################
##### EC2 Variables #####
#########################

#######################
# Domain Controller 1 #
#######################

#######################
# Domain Controller 2 #
#######################

#########################
##### FSX Variables #####
#########################


#########################
##### OARS Variables #####
#########################
#General variables
variable "tags" {
  type        = map(any)
  description = "Tags apply to all resources."
}

variable "oars_project_name" {
  type        = string
  description = "Name of the OARS project"
}

variable "environment_name" {
  type        = string
  description = "Name of the environment"
}
variable "region" {
  type        = string
  description = "AWS region"
}
variable "vpc_id" {
  type        = string
  description = "VPC id"
}

variable "bastion_subnet_id" {
  type        = string
  description = "Subnet id for BastionHost"
}

variable "lb_subnet_ids" {
  type        = list(string)
  description = "List of subnet ids for the loadbalancer"
}

#OARS Environments
variable "production_environment_name" {
  type        = string
  description = "Name of the environment"
}

variable "production_aspnetcore_environment_name" {
  type        = string
  description = "Name of the environment (DeployedDev,DeployedQA,DeployedUAT,DeployedProd)"
}
variable "production_oars_environment" {
  type        = string
  description = "Name of the oars environment (dev,qa,uat,prod)"
}

variable "production_task_def_host_port" {
  type        = number
  description = "Port Number"
}
variable "production_task_def_container_port" {
  type        = number
  description = "Port Number"
}
variable "production_load_balancer_container_port" {
  type        = number
  description = "Port Number"
}
variable "production_alb_port" {
  type        = number
  description = "Port Number"
}
variable "nci-oars-bucket-name" {
  type = string
  default = "nci-oars"
}
variable "nci-oars-object-name" {
  type = string
  default = "DashboardHelp/"
}

#ACM variables
variable "production_domain_name" {
  type        = string
  description = "Domain name for the app"
}
variable "production_gov_certificate_arn" {
  type        = string
  description = "certificate ARN for the Gov domain"
}
variable "production_gov_listener_arn" {
  type        = string
  description = "listener arn for the Gov Certificate"
}

#BastionHost variables
variable "bastion_instance_type" {
  type        = string
  description = "Type of instance for Bastion Host"
}
variable "bastion_ami" {
  type        = string
  description = "AMI used for Bastion Host"
}

#ECS variables
variable "task_def_cpu" {
  type        = number
  description = "Nci OARS container cpu resources"
}

variable "task_def_memory" {
  type        = number
  description = "Nci OARS container memory resources"
}

variable "desired_count" {
  type        = number
  description = "Desired task count for ECS service"
}

variable "ecs_subnet_ids" {
  type        = list(string)
  description = "Subnets IDs where the containers will be running"
}

variable "ecs_service_resource_id" {
  description = "Resource ID for the ECS service"
  type        = string
  default     = "service/nci-oars-production-cluster/nci-oars-production-service"
}

variable "ecs_autoscaling_role_arn" {
  description = "ARN of the IAM role for ECS autoscaling"
  type        = string
  default     = "arn:aws:iam::590811900011:role/aws-service-role/ecs.application-autoscaling.amazonaws.com/AWSServiceRoleForApplicationAutoScaling_ECSService"
}

variable "ecs_scalable_dimension" {
  description = "Scalable dimension for the ECS service"
  type        = string
  default     = "ecs:service:DesiredCount"
}

variable "ecs_service_namespace" {
  description = "Service namespace for the ECS service"
  type        = string
  default     = "ecs"
}

variable "ecs_service_name" {
  description = "Name of the ECS service"
  type        = string
  default     = "nci-oars-production-service"
}

variable "ecs_scale_in_cooldown" {
  description = "The amount of time, in seconds, after a scale in activity completes before another scale in activity can start"
  type        = number
  default     = 60
}

variable "ecs_scale_out_cooldown" {
  description = "The amount of time, in seconds, after a scale out activity completes before another scale out activity can start"
  type        = number
  default     = 60
}

variable "ecs_memory_target_value" {
  description = "Target value for memory utilization"
  type        = number
  default     = 60
}

variable "ecs_cpu_target_value" {
  description = "Target value for CPU utilization"
  type        = number
  default     = 50
}