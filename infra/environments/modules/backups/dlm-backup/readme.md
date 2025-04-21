# Module - dlm-backup

Terraform implementation for dlm backup.

## Usage

If RAID is in use across ebs volumes, you'd want to use this module for backing up that instance.  In most cases, the aws-backup module would be the preferred backup methoud.

```terraform

module "dlm_backups" {
  source = "../../modules/backups/dlm-backup"

  name = "dlm"
  tags = var.tags

  dlm_lifecycle_role_arn = "arn:aws:iam::123456789012:role/service-role/dlm"

  backup_tag_key    = "dlm"
  backup_tag_value  = "yes"

  dlm_ebs_backup_time   = "01:00"
  dlm_ebs_retention     = 30

}

```

## Providers

| Name | Version |
|------|---------|
| aws | n/a |

## Inputs

| Name | Description | Type | Default | Required |
|------|-------------|------|---------|:-----:|
| backup\_tag\_key | Tag items with <thisvalue>=<thisvabackup\_tag\_valuelue> for DLM to target them | `string` | `"dlm"` | no |
| backup\_tag\_value | Tag items with <backup\_tag\_key>=<thisvalue> for DLM to target them | `string` | `"yes"` | no |
| dlm\_ebs\_backup\_time | dlm backup time | `string` | `"01:00"` | no |
| dlm\_ebs\_retention | dlm ebs retentions, in days | `number` | `30` | no |
| dlm\_lifecycle\_role\_arn | DLM Lifecycle service role arn. | `string` | n/a | yes |
| name | Used in AWS resource naming. | `string` | n/a | yes |
| tags | Tags to apply to all module resources. | `map` | `{}` | no |

## Outputs

No output.
