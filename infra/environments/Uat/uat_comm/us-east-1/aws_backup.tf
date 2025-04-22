################################
##### AWS Backup Resources #####
################################

module "aws_backups" {
  source                    = "../../../../modules/backups/aws-backup"

  name                      = "theradex-backup"
  environment               = "uat-commercial"
  owner                     = "Theradex"
  tags                      = var.common_tags

  kms_key_arn               = module.kms.backup_key_arn
  role_arn                  = module.ec2_iam_profile_role.service_role_backup_arn

  completion_window_minutes = 480
  start_window_minutes      = 60

  daily_retention_days      = 30
  daily_cron                = "cron(0 9 * * ? *)"

  weekly_retention_days     = 90
  weekly_cron               = "cron(0 9 ? * SUN *)"

  monthly_retention_days    = 365
  monthly_cron              = "cron(0 9 1 * ? *)"

  backup_tag_value          = "yes"

  opt_in_services           = {
    "Aurora": true,
    "DocumentDB": false,
    "DynamoDB": true,
    "EBS": true,
    "S3": true,
    "CloudFormation": false,
    "Redshift": false,
    "Timestream": true,
    "EC2": true,
    "EFS": true,
    "FSx": true,
    "Neptune": false,
    "RDS": true,
    "Storage Gateway": true
    "SAP HANA on Amazon EC2": false
    "VirtualMachine": true
  }
}
