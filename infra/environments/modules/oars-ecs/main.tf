# AWS ECS Cluster
resource "aws_ecs_cluster" "main" {
  name   = "${var.project_name}-${var.environment_name}-cluster"
  tags   = merge(
    { 
      "ProjectName"= var.project_name
      "EnvironmentName"= var.environment_name
    },
    var.tags)
  configuration {
    execute_command_configuration {
      kms_key_id = aws_kms_key.logs_key.arn
      logging    = "OVERRIDE"

      log_configuration {
        cloud_watch_encryption_enabled = true
        cloud_watch_log_group_name     = aws_cloudwatch_log_group.service.name
      }
    }
  }
}
resource "random_string" "alias_suffix" {
  length  = 4
  special = false
}
resource "aws_kms_alias" "a" {
  name          = "alias/${var.project_name}-${var.environment_name}-service-logs-key-${random_string.alias_suffix.result}"
  target_key_id = aws_kms_key.logs_key.key_id
}
resource "aws_kms_key" "logs_key" {
  description             = "KMS key for ${var.environment_name} logs"
  deletion_window_in_days = 7
  enable_key_rotation = true
  # rotation_period_in_days = 365
  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [
      {
        "Effect" : "Allow",
        "Principal" : {
          "AWS" : "arn:aws:iam::${data.aws_caller_identity.current.account_id}:root"
        },
        "Action" : "kms:*",
        "Resource" : "*"
      },
      {
        "Effect" : "Allow",
        "Principal" : {
          "Service" : "logs.${data.aws_region.current.name}.amazonaws.com"
        },
        "Action" : [
         "kms:Decrypt",
         "kms:Encrypt",
         "kms:ReEncrypt*",
         "kms:GenerateDataKey*"
        ],
        "Resource" : "*",
        "Condition" : {
          "ArnLike" : {
            "kms:EncryptionContext:aws:logs:arn" : "arn:aws:logs:${data.aws_region.current.name}:${data.aws_caller_identity.current.account_id}:*"
          }
        }
      }
    ]
  })
}