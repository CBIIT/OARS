# For new accounts copy this file to the new account's
# folder and only update the key value

terraform {

  required_version = ">= 1.8.4"

  backend "s3" {
    bucket         = "theradex-production-nci-terraform"
    key            = "production/us-east-1/terraform.tfstate"
    kms_key_id     = "arn:aws:kms:us-east-1:590811900011:key/18193c3b-fb69-4407-97dd-294d5eaca43a"
    region         = "us-east-1"
    encrypt        = true
    dynamodb_table = "theradex-production-nci-terraform"
    # profile        = "theradex-production-nci"
  }
}