############################
##### App Variables #####
############################

variable "vendorintg_env" {
  description = "Vendor Integration Environment"
  type        = string
  default     = "uat-nci"
}

variable "vendorintg_cluster_name" {
  description = "Vendor Integration Cluster Name"
  type        = string
  default     = "theradex-uat-nci-cluster-vendorintg"
}

variable "vendorintg_container_name" {
  description = "Vendor Integration ECS Container Name"
  type        = string
  default     = "UAT-TheradexVendorIntegration"
}

variable "vendorintg_taskdefinition_name" {
  description = "Vendor Integration Task Definition Name"
  type        = string
  default     = "TheradexVendorIntegration"
}

variable "vendorintg_taskdefinition_cloudwatch_group_name" {
  description = "Vendor Integration Task Definition Cloud Watch Group Name"
  type        = string
  default     = "uat-nci-vendorintg"
}

variable "vendorintg_outputfile_s3_bucket" {
  description = "Vendor Integration S3 Bucket Integration Files"
  type        = string
  default     = "uat-nci-vendorintg-files"
}

variable "vendorintg_data_s3_bucket" {
  description = "Vendor Integration S3 Bucket Data Files"
  type        = string
  default     = "uat-nci-etctndatashare-files"
}

variable "vendorintg_region" {
  description = "Vendor Integration S3 Bucket Integration Files"
  type        = string
  default     = "us-east-1"
}

variable "vendorintg_logs_retention_in_days" {
  type        = number
  default     = 90
  description = "Specifies the number of days you want to retain log events"
}

variable "vendorintg_biobank_share_s3_bucket" {
    description = "Vendor Integration S3 Bucket Data Files"
    type        = string
    default     = "theradex-biobank-share-uat"
}