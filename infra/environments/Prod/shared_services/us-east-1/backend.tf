# For new accounts copy this file to the new account's
# folder and only update the key value

terraform {

  required_version = ">= 1.8.4"

  backend "s3" {
    bucket         = "theradex-shared-services-terraform"
    key            = "production/us-east-1/terraform.tfstate"
    kms_key_id     = "arn:aws:kms:us-east-1:606199607275:key/ff0681d1-ce1b-4e7b-a7b4-67593f502f94"
    region         = "us-east-1"
    encrypt        = true
    dynamodb_table = "theradex-shared-services-terraform"
  #  profile        = "theradex-shared-service"
  }
}