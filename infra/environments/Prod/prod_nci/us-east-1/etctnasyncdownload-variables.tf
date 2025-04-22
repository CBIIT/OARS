variable "etctnasyncdownload_files_s3_bucket" {
  description = "ETCTN Async Download Files S3 Bucket"
  type        = string
  default     = "prod-nci-etctndatashare-files"
}

variable "etctnasyncdownload_env" {
  description = "ETCTNDataShare Lambda Environment"
  type        = string
  default     = "prod-nci"
}

variable "etctnasyncdownload-sf_logs_retention_in_days" {
  type        = number
  default     = 90
  description = "Specifies the number of days you want to retain log events"
}

variable "etctnasyncdownload-exec-sf_cloudwatch_group_name" {
  description = "ETCTN Download Data SF Cloud Watch Group Name"
  type        = string
  default     = "etctnasyncdownload-exec-sf"
}

variable "etctnasyncdownload-init-sf_cloudwatch_group_name" {
  description = "ETCTN Async Download Init SF Cloud Watch Group Name"
  type        = string
  default     = "etctnasyncdownload-init-sf"
}