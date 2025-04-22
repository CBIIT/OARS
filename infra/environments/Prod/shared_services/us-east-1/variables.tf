###############################
##### Terraform Variables #####
###############################


#####################################
##### Generic Tagging Variables #####
#####################################

variable "common_tags" {
    description = "Common tags that should be applied where no specific tags are required, ie CloudTrail, Config, GuardDuty, etc."
    type        = map
    default     = {
        "Managed By"  = "Terraform"
        "Owner"       = "CBV"
        "account"     = "shared-services"
        "environment" = "production"
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

############################
##### Config Variables #####
############################

variable "org-id" {
    description = "ID of the Organization"
    type = string
    default = "o-0v4zacm3u8"
}

variable "config_bucket_name" {
    description = "Name of the config bucket"
    type = string
    default = "cbv-shared-services-account-config-bucket"
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
        theradex-dr = {
            account_id = "303587441114"
            attribute1 = ""
        }
    }
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
## Fortigate Variables ##

variable "region" {
  default = "us-east-1"
}

// Availability zone 1 for the region
variable "az1" {
  default = "us-east-1a"
}

// Availability zone 2 for the region
variable "az2" {
  default = "us-east-1b"
}

// IAM Role for Fortigates
variable "ftg-iam-role" {
  default = "FTG_EC2_Role"  //Put in the IAM Role name created
}

/*
variable "vpcid" {
  description = "VPC ID"
  default     = module.security_vpc.id
}
*/

variable "vpccidr" {
  default = "10.10.0.0/16"
}

variable "license_type" {
  default = "payg"
}

// instance architect
// Either arm or x86
variable "arch" {
  default = "x86_64"
}

// instance type needs to match the architect
// c5n.xlarge is x86_64
// c6g.xlarge is arm
// For detail, refer to https://aws.amazon.com/ec2/instance-types/
variable "size" {
  default = "c5n.xlarge"
}

// AMIs for FGTVM-7.0.9
variable "fgtami" {
  type = map(any)
  default = {
    us-east-1 = {
      arm = {
        payg = "ami-0cb77c60ba71972de"
        byol = "ami-07df7e29cde684c4e"
      },
      x86_64 = {
        payg = "ami-0ffcff5f5cca7156f"
        byol = "ami-09b9aeeeccd53e7d9"
      }
    }
  }
}

//  Existing SSH Key on the AWS 
variable "keyname" {
  default = "theradex-ss-ftg-kp"
}

//  Admin HTTPS access port
variable "adminsport" {
  default = "443"
}

variable "activeport1" {
  description = "Active FGT port1 ip address, needs to be same subnet as Public Subnet in AZ1"
  default     = "10.10.0.10"
}

variable "activeport1mask" {
  default = "255.255.255.0"
}

variable "activeport2" {
  description = "Active FGT port2 ip address, needs to be same subnet as Private Subnet in AZ1"
  default     = "10.10.10.10"
}

variable "activeport2mask" {
  default = "255.255.255.0"
}

variable "activeport3" {
  description = "Active FGT port3 ip address, needs to be same subnet as HASYNC Subnet in AZ1"
  default     = "10.10.32.10"
}

variable "activeport3mask" {
  default = "255.255.255.0"
}

variable "activeport4" {
  description = "Active FGT port4 ip address, needs to be same subnet as HAMGMT Subnet in AZ1"
  default     = "10.10.34.10"
}

variable "activeport4mask" {
  default = "255.255.255.0"
}

variable "passiveport1" {
  description = "Passive FGT port1 ip address, needs to be same subnet as Public Subnet in AZ2"
  default     = "10.10.1.10"
}

variable "passiveport1mask" {
  default = "255.255.255.0"
}

variable "passiveport2" {
  description = "Passive FGT port2 ip address, needs to be same subnet as Private Subnet in AZ2"
  default     = "10.10.11.10"
}

variable "passiveport2mask" {
  default = "255.255.255.0"
}

variable "passiveport3" {
  description = "Passive FGT port3 ip address, needs to be same subnet as HASYNC Subnet in AZ2"
  default     = "10.10.33.10"
}

variable "passiveport3mask" {
  default = "255.255.255.0"
}

variable "passiveport4" {
  description = "Passive FGT port4 ip address, needs to be same subnet as HAMGMT Subnet in AZ2"
  default     = "10.10.35.10"
}

variable "passiveport4mask" {
  default = "255.255.255.0"
}

variable "activeport1gateway" {
  default = "10.10.0.1"
}

variable "activeport2gateway" {
  default = "10.10.10.1"
}

variable "activeport4gateway" {
  default = "10.10.34.1"
}

variable "passiveport1gateway" {
  default = "10.10.1.1"
}

variable "passiveport2gateway" {
  default = "10.10.11.1"
}

variable "passiveport4gateway" {
  default = "10.10.35.1"
}


variable "bootstrap-active" {
  // Change to your own path
  type    = string
  default = "ftg-config-active.conf"
}

variable "bootstrap-passive" {
  // Change to your own path
  type    = string
  default = "ftg-config-passive.conf"
}

// license file for the active fgt
variable "license" {
  // Change to your own byol license file, license.lic
  type    = string
  default = "license.lic"
}

// license file for the passive fgt
variable "license2" {
  // Change to your own byol license file, license2.lic
  type    = string
  default = "license2.lic"
}

#Codepipeline Oars variables
variable "tags" {
  type            = map(any)
  description     = "Tags apply to all resources."
}
variable "environment" {
  description = "List of environments to deploy to"
  type        = set(string)
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


variable "project_name" {
  type        = string
  description = "Project name"
}
variable "account_id" {
  type        = string
  description = "Account ID #"
}
variable "environment_account"{
  type        = map(string)
  description = "the environments and their corresponding account ids"
}