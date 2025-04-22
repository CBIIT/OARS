# For new accounts copy this file to the new account's
# folder and only update the key value

terraform {

  required_version = ">= 1.8.4"

  backend "s3" {
    bucket         = "theradex-uat-commercial-terraform"
    key            = "production/us-east-1/terraform.tfstate"
    kms_key_id     = "arn:aws:kms:us-east-1:855492361387:key/161ddefd-8fd6-42a6-aaa5-9fdfa1ef99b7"
    region         = "us-east-1"
    encrypt        = true
    dynamodb_table = "theradex-uat-commercial-terraform"
    profile        = "theradex-uat-commercial"
  }
}