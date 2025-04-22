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
  "Managed By" = "Terraform"
  "Owner"      = "CBV"

  }

}






############################
##### Config Variables #####
############################

variable "config_bucket_name" {
  description = "Name of the config bucket"
  type        = string
  default     = "theradex-master-account-config-bucket"
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

