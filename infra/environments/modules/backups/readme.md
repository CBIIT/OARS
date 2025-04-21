# General Notes and Implementation

See additional details in the terraform module readme.

- AWS Backup KMS key

## Backup Types

New resources will be added as they're released by AWS.  To keep up to date, refer to the [AWS Backup website](https://docs.aws.amazon.com/aws-backup/latest/devguide/whatisbackup.html).

Currently Available:

- DynamoDB
- EBS
- EC2 Instances (generates an AMI and its corresponding EBS snapshots)
- EFS
- RDS (non-Aurora)
- Storage Gateway

## Implementation

Before or after deploying the template, tag resources with a backup=`X`.  `X` is the value of `target_backup_tag_value` in the chosen mapping.

For example, in the `default` map we're looking for instances where the `backup` tag is **yes**.

Similarly, EC2 instances that will be backed up via DLM will be tagged with dlm=`X`.  `X` is the value of `TargetBackupTagValue` in the chosen mapping.

### Recovery Point Cleanup

AWS hasn't implemented automatic deletion of expired recovery points, so we have added lambda functions to handle that.  A lambda function will run daily, to clear them, so expired is considered DELETED.  There is also a lambda function that can be run manually to purge all recovery points.

## Notes

### Tagging

Tagging requirements vary between DLM and AWS Backups.

- DLM requires EC2 _instance_ tags.
- AWS Backups requires tags placed on:
  - DynamoDB Tables
  - EBS volumes
  - EC2 Instances
  - EFS volumes
  - RDS Instances
  - Storage volume gateways

### Scheduling

DLM is designed to only run daily versus EBS AWS Backups which can run weekly or monthly.

There will be duplicate backups on days the weeklys and monthlys run.  This is minor since only the differences between backups create additional storage costs.

### Data Consistency

EBS AWS Backups **do not do consistent backups across volumes**. This means an instance backup does not necessarily have a backup of each volume from the exact same point in time.

DLM should be used for instances where RAID or databases are in use.  Regardless of AWS level backups, those systems should be considered for an application level backup to S3 as well.

## References

AWS Backup - <https://docs.aws.amazon.com/aws-backup/latest/devguide/whatisbackup.html>

Data Lifecycle Manager - <https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/snapshot-lifecycle.html>
