variable "name" {
  description = "Used in AWS resource naming."
  type        = string
}

variable "tags" {
  description = "Tags to apply to all module resources."
  default     = {}
  type        = map
}

variable "dlm_ebs_retention" {
  description = "dlm ebs retentions, in days"
  default     = 30
  type        = number
}
 
variable "dlm_ebs_backup_time" {
  description = "dlm backup time"
  default     = "01:00"
  type        = string
}

variable "backup_tag_value" {
  description = "Tag items with <backup_tag_key>=<thisvalue> for DLM to target them"
  default     = "yes"
  type        = string
}

variable "backup_tag_key" {
  description = "Tag items with <thisvalue>=<thisvabackup_tag_valuelue> for DLM to target them"
  default     = "dlm"
  type        = string
}

variable "dlm_lifecycle_role_arn" {
  description = "DLM Lifecycle service role arn."
  type        = string
}
