###############################
##### Terraform Variables #####
###############################

############################
##### Config Variables #####
############################

variable "org-id" {
    description = "ID of the Organization"
    type = string
    default = "o-0v4zacm3u8"
}

variable "secure-vpc-tgw-1-id" {
    description = "ID of the secure-vpc-tgw-1 Transit Gateway"
    type = string
    default = "tgw-0b5475750f7169121"
}

variable "aws_accounts" {
    description = "Map of Relevant Organization AWS accounts"
    type        = map
    default     = {
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
    type        = map
    default     = {
        "Managed By"            = "Terraform"
        "Owner"                 = "CBV"
        "account"               = "dev_nci"
        "environment"           = "development"
        "Application Type"      = "empty"
        "Application Version"   = "empty"
        "BackupPlan"            = "empty"
        "Description"           = "empty"
        "ExpireDate"            = "empty"
        "Operating System"      = "empty"
        "Project"               = "empty"
        "Region"                = "empty"
        "Team DL"               = "empty"
        "Temporary"             = "empty"
    }
}

variable "qrcg_tld_domain" {
    description = "QR Code Generation Cluster TLD"
    type        = string
    default     = "theradex.com"
}

#########################
##### OARS Variables ####
#########################

# General variables
variable "tags" {
  type        = map(any)
  description = "Tags apply to all resources."
}
variable "tags_qa" {
  type        = map(any)
  description = "Tags apply to all resources."
}

variable "oars_project_name" {
  type        = string
  description = "Name of the project"
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
# variable "bastion_subnet_id" {
#   type        = string
#   description = "Subnet id for BastionHost"
# }

variable "lb_subnet_ids" {
    type        = list(string)
    description = "List of subnet ids for the Loadbalancer"
}

#ORAS Environments
variable "dev_environment_name" {
  type        = string
  description = "Name of the environment"
}
variable "qa_environment_name" {
  type        = string
  description = "Name of the environment"
}
variable "studydev_environment_name" {
  type        = string
  description = "Name of the environment"
}

variable "dev_aspnetcore_environment_name" {
  type        = string
  description = "Name of the environment"
}
variable "qa_aspnetcore_environment_name" {
  type        = string
  description = "Name of the environment"
}
variable "studydev_aspnetcore_environment_name" {
  type        = string
  description = "Name of the environment"
}

variable "dev_task_def_host_port" {
  type        = number
  description = "Port Number"
}
variable "qa_task_def_host_port" {
  type        = number
  description = "Port Number"
}
variable "studydev_task_def_host_port" {
  type        = number
  description = "Port Number"
}
variable "dev_task_def_container_port" {
  type        = number
  description = "Port Number"
}
variable "qa_task_def_container_port" {
  type        = number
  description = "Port Number"
}
variable "studydev_task_def_container_port" {
  type        = number
  description = "Port Number"
}
variable "dev_load_balancer_container_port" {
  type        = number
  description = "Port Number"
}
variable "qa_load_balancer_container_port" {
  type        = number
  description = "Port Number"
}
variable "studydev_load_balancer_container_port" {
  type        = number
  description = "Port Number"
}
variable "dev_oars_environment" {
  type        = string
  description = "Name of the environment"
}
variable "qa_oars_environment" {
  type        = string
  description = "Name of the environment"
}
variable "studydev_oars_environment" {
  type        = string
  description = "Name of the environment"
}
# variable "dev_gov_certificate_arn" {
#   type = string
#   description = "certificate ARN for the Gov domain"
# }
# variable "qa_gov_certificate_arn" {
#   type = string
#   description = "certificate ARN for the Gov domain"
# }
# variable "dev_gov_listener_arn" {
#   type = string
#   description = "listener arn for the Gov Certificate"
# }
# variable "qa_gov_listener_arn" {
#   type = string
#   description = "listener arn for the Gov Certificate"
# }
variable "waf_name" {
  type = string
  default = "oars"
}
variable "nci-oars-dev-bucket-name" {
  type = string
  default = "nci-oars-dev"
}
variable "nci-oars-dev-object-name" {
  type = string
  default = "DashboardHelp/"
}
variable "nci-oars-qa-bucket-name" {
  type = string
  default = "nci-oars-qa"
}
variable "nci-oars-qa-object-name" {
  type = string
  default = "DashboardHelp/"
}
variable "dev_alb_port" {
  type        = string
  description = "Domain name for the app"
}

variable "qa_alb_port" {
  type        = string
  description = "Domain name for the app"
}

variable "studydev_alb_port" {
  type        = string
  description = "Domain name for the app"
}

#ACM variables
variable "dev_domain_name" {
  type        = string
  description = "Domain name for the app"
}

variable "qa_domain_name" {
  type        = string
  description = "Domain name for the app"
}

variable "studydev_domain_name" {
  type        = string
  description = "Domain name for the app"
}

#BastionHost variables
# variable "bastion_instance_type" {
#   type        = string
#   description = "Type of instance for Bastion Host"
# }
# variable "bastion_ami" {
#   type        = string
#   description = "AMI used for Bastion Host"
# }
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

variable "ecs_subnet_ids" {
  type        = list(string)
  description = "Subnets IDs where the containers will be running"
}