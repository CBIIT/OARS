resource "aws_dlm_lifecycle_policy" "default" {
  description        = "${var.name} - DLM lifecycle policy"
  execution_role_arn = var.dlm_lifecycle_role_arn
  state              = "ENABLED"

    policy_details {
      resource_types = ["INSTANCE"]

      schedule {
        name = "${var.name} dlm ebs snapshots"

        create_rule {
          interval      = 24
          interval_unit = "HOURS"
          times         = [ var.dlm_ebs_backup_time ]
        }

        retain_rule {
          count = var.dlm_ebs_retention
        }

        tags_to_add = {
          type = "${var.name}-dlm-ebs-snapshot-backup"
        }

        copy_tags = true
      }

      target_tags = {
        var.backup_tag_key = var.backup_tag_value
    }
  }
}