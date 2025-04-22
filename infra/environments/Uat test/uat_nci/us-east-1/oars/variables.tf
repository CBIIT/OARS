###############################
##### Terraform Variables #####
###############################

# locals {
#   private_subnet_ids = [
#     for az, id in module.uat_noncomm_vpc.private_subnet_ids :
#     id
#   ]
#   public_subnets_ids = [
#     for az, id in module.uat_noncomm_vpc.public_subnet_ids :
#     id
#   ]
#   myaccount = var.aws_accounts.theradex-uat-nci.account_id
# }

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
    theradex-development-commercial = {
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
    theradex-development-nci = {
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
    "account"             = "uat_nci"
    "environment"         = "uat"
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



variable "qrcg_taskdefinition_name" {
  description = "QR Code Generation Task Definition Name"
  type        = string
  default     = "TheradexQRCodeGeneration"
}

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
variable "uat_alb_port" {
  type        = number
  description = "Alb Port Number"
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
variable "uat_environment_name" {
  type        = string
  description = "Name of the environment"
}
variable "uat_aspnetcore_environment_name" {
  type        = string
  description = "Name of the environment (DeployedDev,DeployedQA,DeployedUAT,DeployedProd)"
}
variable "uat_oars_environment" {
  type        = string
  description = "Name of the oars environment (dev,qa,uat,prod)"
}
variable "uat_gov_certificate_arn" {
  type = string
  description = "certificate ARN for the Gov domain"
}
variable "uat_gov_listener_arn" {
  type = string
  description = "listener arn for the Gov Certificate"
}
variable "uat_task_def_host_port" {
  type        = number
  description = "Port Number"
}
variable "uat_task_def_container_port" {
  type        = number
  description = "Port Number"
}
variable "uat_load_balancer_container_port" {
  type        = number
  description = "Port Number"
}
variable "nci-oars-uat-bucket-name" {
  type = string
  default = "nci-oars-uat"
}
variable "nci-oars-uat-object-name" {
  type = string
  default = "DashboardHelp/"
}
#ACM variables
variable "uat_domain_name" {
  type        = string
  description = "Domain name for the app"
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
  description = "Nci thor container cpu resources"
}

variable "task_def_memory" {
  type        = number
  description = "Nci thor container memory resources"
}

variable "desired_count" {
  type        = string
  description = "Desired task count for ECS service"
}

variable "ecs_subnet_ids" {
  type        = list(string)
  description = "Subnets IDs where the containers will be running"
}
