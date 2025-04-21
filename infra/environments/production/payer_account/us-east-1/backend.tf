# For new accounts copy this file to the new account's
# folder and only update the key value

terraform {

  required_version = ">= 1.8.4"

  backend "s3" {
    bucket         = "theradex-payer-account-terraform"
    key            = "production/us-east-1/terraform.tfstate"
    kms_key_id     = "arn:aws:kms:us-east-1:747371226450:key/0ff4e311-8e6a-48c1-b727-276967b125c9"
    region         = "us-east-1"
    encrypt        = true
    dynamodb_table = "theradex-payer-account-terraform"
    profile        = "theradex-payer-account"
  }
}
