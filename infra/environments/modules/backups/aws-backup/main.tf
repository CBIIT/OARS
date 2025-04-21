resource "aws_backup_vault" "default" {
  name        = var.name
  tags        = var.tags
  kms_key_arn = var.kms_key_arn
}

resource "aws_backup_plan" "default" {
  name = var.name
  tags = var.tags

    rule {
        rule_name           = "DailyBackups"
        target_vault_name   = aws_backup_vault.default.name
        schedule            = var.daily_cron
        start_window        = var.start_window_minutes
        completion_window   = var.completion_window_minutes
        lifecycle {
            delete_after    = var.daily_retention_days
        }
    }

    rule {
        rule_name           = "WeeklyBackups"
        target_vault_name   = aws_backup_vault.default.name
        schedule            = var.weekly_cron
        start_window        = var.start_window_minutes
        completion_window   = var.completion_window_minutes
        lifecycle {
            delete_after    = var.weekly_retention_days
        }
    }

    rule {
        rule_name           = "MonthlyBackups"
        target_vault_name   = aws_backup_vault.default.name
        schedule            = var.monthly_cron
        start_window        = var.start_window_minutes
        completion_window   = var.completion_window_minutes
        lifecycle {
            delete_after    = var.monthly_retention_days
        }
    }

}

resource "aws_backup_selection" "default" {
  iam_role_arn = var.role_arn
  name         = var.name
  plan_id      = aws_backup_plan.default.id

  selection_tag {
    type  = "STRINGEQUALS"
    key   = var.backup_tag_key
    value = var.backup_tag_value
  }
}


resource "aws_backup_region_settings" "default" {
  resource_type_opt_in_preference = var.opt_in_services
}