# For new accounts copy this file to the new account's
# folder and only update the key value

terraform {

  required_version = ">= 1.8.4"

    backend "s3" {
      bucket         = "theradex-uat-nci-terraform"
      key            = "production/us-east-1/oars-terraform.tfstate"
      kms_key_id     = "arn:aws:kms:us-east-1:352847057549:key/9eb36105-0646-45e8-a49f-899dcfa0d1b6"
      region         = "us-east-1"
      encrypt        = true
      dynamodb_table = "theradex-uat-nci-terraform"
    #  profile        = "theradex-uat-nci"
    }
  }

#   backend "local" {
#   }
# }
