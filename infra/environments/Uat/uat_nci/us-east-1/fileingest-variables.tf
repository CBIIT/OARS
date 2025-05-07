variable "fileingest_files_s3_bucket" {
  description = "FileIngest Files S3 Bucket"
  type        = string
  default     = "uat-nci-etctn-fileingestrequests"
}

variable "fileingest_env" {
  description = "FileIngest Lambda Environment"
  type        = string
  default     = "uat-nci"
}