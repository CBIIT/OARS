variable "name" {
  description = "Used in AWS resource naming."
  type        = string
}

variable "owner" {
  description = "Owner of the resources.  Person/Department, etc."
  type        = string
}

variable "environment" {
  description = "Environment type for default resource tagging."
  type        = string
}

variable "role_arn" {
  description = "AWS Backup service role arn."
  type        = string
}

variable "kms_key_arn" {
  description = "KMS Key arn for encrypting s3 bucket."
  type        = string
}

variable "tags" {
  description = "Tags to apply to all module resources."
  default     = {}
  type        = map
}

variable "completion_window_minutes" {
  description = "How long the completion window is, in minutes"
  default     = 480
  type        = number
}

variable "start_window_minutes" {
  description = "How long after the start time the job should start, in minutes"
  default     = 60
  type        = number
}

variable "daily_retention_days" {
  description = "Daily backup retentions, in days"
  default     = 30
  type        = number
}

variable "weekly_retention_days" {
  description = "Weekly backup retentions, in days"
  default     = 90
  type        = number
}

variable "monthly_retention_days" {
  description = "Monthly backup retentions, in days"
  default     = 365
  type        = number
}

variable "daily_cron" {
  description = "Daily backup cron schedule"
  default     = "cron(0 1 * * ? *)"
  type        = string
}

variable "weekly_cron" {
  description = "Weekly backup cron schedule"
  default     = "cron(0 1 ? * SUN *)"
  type        = string
}

variable "monthly_cron" {
  description = "Monthly backup cron schedule"
  default     = "cron(0 1 1 * ? *)"
  type        = string
}

variable "backup_tag_value" {
  description = "Tag items with <backup_tag_key>=<thisvalue> for aws backups to target them"
  default     = "yes"
  type        = string
}

variable "backup_tag_key" {
  description = "Tag items with <thisvalue>=<backup_tag_value> for aws backups to target them"
  default     = "backup"
  type        = string
}

variable "opt_in_services" {
  description  = "Map of services to opt in to AWS Backup"
  default     = {
    "DynamoDB"        = true
    "Aurora"          = true
    "Neptune"         = true
    "EBS"             = true
    "EC2"             = true
    "EFS"             = true
    "FSx"             = true
    "RDS"             = true
    "Storage Gateway" = true
    "DocumentDB"      = true
  }
  type        = map
}