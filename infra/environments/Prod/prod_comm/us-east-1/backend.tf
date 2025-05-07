# For new accounts copy this file to the new account's
# folder and only update the key value

terraform {

  required_version = ">= 1.8.4"

  backend "s3" {
    bucket         = "theradex-production-commercial-terraform"
    key            = "production/us-east-1/terraform.tfstate"
    kms_key_id     = "arn:aws:kms:us-east-1:078250543436:key/7e28522f-2d40-4cf6-bf65-20f9e5b19ebc"
    region         = "us-east-1"
    encrypt        = true
    dynamodb_table = "theradex-production-commercial-terraform"
    profile        = "theradex-production-commercial"
  }
}