# Module - aws-backup

Terraform implementation of flexible aws backup plans, with daily, weekly, and monthly backup plans based on a tag.

AWS Backup Supports:

- Amazon EC2 instances (AWS Backup does not support Amazon EC2 instance store-backed instances.)
- Amazon EBS Volumes
- Aurora clusters
- Amazon RDS databases
- Amazon DynamoDB tables
- Amazon EFS file systems
- Amazon FSx file systems
- AWS Storage Gateway volumes

## Minimum Required Configuration

```terraform

module "kms" {
  source = "../../modules/kms"

  ebs            = true
  rds            = true
  ssm            = true
}

module "aws_backups" {
  source                    = "../../modules/backups/aws-backup"

  name                      = "company-backup-name"
  environment               = "development"
  owner                     = "Owner"
  tags                      = var.common_tags

  kms_key_arn               = module.kms.backup_key_arn
  role_arn                  = module.iam_general.service_role_backup_arn

  completion_window_minutes = 480
  start_window_minutes      = 60

  daily_retention_days      = 30
  daily_cron                = "cron(0 1 * * ? *)"

  weekly_retention_days     = 90
  weekly_cron               = "cron(0 1 ? * SUN *)"

  monthly_retention_days    = 365
  monthly_cron              = "cron(0 1 1 * ? *)"

  backup_tag_value          = "yes"
}
```

## Inputs and Outputs

Inputs and outputs are generated with [terraform-docs](https://github.com/segmentio/terraform-docs)

```bash
terraform-docs markdown table ./
```
## Requirements

No requirements.

## Providers

| Name | Version |
|------|---------|
| <a name="provider_aws"></a> [aws](#provider\_aws) | n/a |

## Modules

No modules.

## Resources

| Name | Type |
|------|------|
| [aws_backup_plan.default](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/backup_plan) | resource |
| [aws_backup_region_settings.default](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/backup_region_settings) | resource |
| [aws_backup_selection.default](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/backup_selection) | resource |
| [aws_backup_vault.default](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/backup_vault) | resource |
| [aws_cloudwatch_event_rule.expired_cleanup](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/cloudwatch_event_rule) | resource |
| [aws_cloudwatch_event_target.expired_cleanup](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/cloudwatch_event_target) | resource |
| [aws_cloudwatch_log_group.all_cleanup](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/cloudwatch_log_group) | resource |
| [aws_cloudwatch_log_group.expired_cleanup](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/cloudwatch_log_group) | resource |
| [aws_iam_policy.cleanup](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/iam_policy) | resource |
| [aws_iam_role.cleanup](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/iam_role) | resource |
| [aws_iam_role_policy_attachment.cleanup](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/iam_role_policy_attachment) | resource |
| [aws_iam_role_policy_attachment.cleanup_lambda_execution](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/iam_role_policy_attachment) | resource |
| [aws_lambda_function.all_cleanup](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/lambda_function) | resource |
| [aws_lambda_function.expired_cleanup](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/lambda_function) | resource |
| [aws_lambda_permission.expired_cleanup](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/lambda_permission) | resource |

## Inputs

| Name | Description | Type | Default | Required |
|------|-------------|------|---------|:--------:|
| <a name="input_backup_tag_key"></a> [backup\_tag\_key](#input\_backup\_tag\_key) | Tag items with <thisvalue>=<backup\_tag\_value> for aws backups to target them | `string` | `"backup"` | no |
| <a name="input_backup_tag_value"></a> [backup\_tag\_value](#input\_backup\_tag\_value) | Tag items with <backup\_tag\_key>=<thisvalue> for aws backups to target them | `string` | `"yes"` | no |
| <a name="input_completion_window_minutes"></a> [completion\_window\_minutes](#input\_completion\_window\_minutes) | How long the completion window is, in minutes | `number` | `480` | no |
| <a name="input_daily_cron"></a> [daily\_cron](#input\_daily\_cron) | Daily backup cron schedule | `string` | `"cron(0 1 * * ? *)"` | no |
| <a name="input_daily_retention_days"></a> [daily\_retention\_days](#input\_daily\_retention\_days) | Daily backup retentions, in days | `number` | `30` | no |
| <a name="input_environment"></a> [environment](#input\_environment) | Environment type for default resource tagging. | `string` | n/a | yes |
| <a name="input_kms_key_arn"></a> [kms\_key\_arn](#input\_kms\_key\_arn) | KMS Key arn for encrypting s3 bucket. | `string` | n/a | yes |
| <a name="input_monthly_cron"></a> [monthly\_cron](#input\_monthly\_cron) | Monthly backup cron schedule | `string` | `"cron(0 1 1 * ? *)"` | no |
| <a name="input_monthly_retention_days"></a> [monthly\_retention\_days](#input\_monthly\_retention\_days) | Monthly backup retentions, in days | `number` | `365` | no |
| <a name="input_name"></a> [name](#input\_name) | Used in AWS resource naming. | `string` | n/a | yes |
| <a name="input_opt_in_services"></a> [opt\_in\_services](#input\_opt\_in\_services) | Map of services to opt in to AWS Backup | `map` | <pre>{<br>  "Aurora": true,<br>  "DocumentDB": true,<br>  "DynamoDB": true,<br>  "EBS": true,<br>  "EC2": true,<br>  "EFS": true,<br>  "FSx": true,<br>  "Neptune": true,<br>  "RDS": true,<br>  "Storage Gateway": true<br>}</pre> | no |
| <a name="input_owner"></a> [owner](#input\_owner) | Owner of the resources.  Person/Department, etc. | `string` | n/a | yes |
| <a name="input_role_arn"></a> [role\_arn](#input\_role\_arn) | AWS Backup service role arn. | `string` | n/a | yes |
| <a name="input_start_window_minutes"></a> [start\_window\_minutes](#input\_start\_window\_minutes) | How long after the start time the job should start, in minutes | `number` | `60` | no |
| <a name="input_tags"></a> [tags](#input\_tags) | Tags to apply to all module resources. | `map` | `{}` | no |
| <a name="input_weekly_cron"></a> [weekly\_cron](#input\_weekly\_cron) | Weekly backup cron schedule | `string` | `"cron(0 1 ? * SUN *)"` | no |
| <a name="input_weekly_retention_days"></a> [weekly\_retention\_days](#input\_weekly\_retention\_days) | Weekly backup retentions, in days | `number` | `90` | no |

## Outputs

No outputs.
