resource "aws_kms_key" "theradex_uat_nci_cluster_vendorintg_kms_key" {
  description             = "Theradex UAT Non-Commercial VENDOR INTG Cluster KMS KEY"
  deletion_window_in_days = 7
}

resource "aws_cloudwatch_log_group" "theradex_uat_nci_cluster_vendorintg_log_group" {
  name = "${var.vendorintg_cluster_name}-log-group"
  tags = {
    Environment = "uat"
  }
}

resource "aws_ecs_cluster" "theradex_uat_nci_cluster_vendorintg" {
  name = var.vendorintg_cluster_name
  configuration {
    execute_command_configuration {
      kms_key_id = aws_kms_key.theradex_uat_nci_cluster_vendorintg_kms_key.arn
      logging    = "OVERRIDE"

      log_configuration {
        cloud_watch_encryption_enabled = true
        cloud_watch_log_group_name     = aws_cloudwatch_log_group.theradex_uat_nci_cluster_vendorintg_log_group.name
      }
    }
  }
}