###############################
##### Terraform Variables #####
###############################

locals {
  private_subnet_ids = [
    for az, id in module.uat_comm_vpc.private_subnet_ids: 
      id   
  ]
  public_subnets_ids = [
    for az, id in module.uat_comm_vpc.public_subnet_ids: 
      id   
  ]
  myaccount = var.aws_accounts.theradex-uat-commercial.account_id
}

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
        "Managed By"  = "Terraform"
        "Owner"       = "CBV"
        "account"     = "uat_comm"
        "environment" = "uat"
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

variable "qrcg_cluster_name" {
    description = "QR Code Generation Cluster Name"
    type        = string
    default     = "theradex-uat-comm-cluster"
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
