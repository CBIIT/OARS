###############################
##### KMS Encryption Keys #####
###############################

module "kms" {
  source                    = "../../../../modules/kms"
  ebs_encryption_by_default = true

  s3                        = true
  backup                    = true
  rds                       = true
  ebs                       = true
  ssm                       = true
}
