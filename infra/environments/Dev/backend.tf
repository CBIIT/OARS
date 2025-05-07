# For new accounts copy this file to the new account's
# folder and only update the key value

terraform {

  required_version = ">= 1.8.4"

 backend "s3" {
   bucket         = "nci-development-nci-terraform"
   key            = "dev/us-east-1/oars-terraform.tfstate"
   kms_key_id     = "arn:aws:kms:us-east-1:888577055755:key/ddb446d3-0096-44e2-b5ef-cf97e7dcabf7"
   region         = "us-east-1"
   encrypt        = true
   dynamodb_table = "nci-development-nci-terraform"
    #profile        = "theradex-development-nci"
    # assume_role = {
    #   role_arn = "arn:aws:iam::993530973844:role/Terraform-oidc"
    # }
  }

  # backend "local" {
  # }
}