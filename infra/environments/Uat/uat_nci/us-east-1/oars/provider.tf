terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "= 5.53.0"
    }
  }
}

provider "aws" {
# profile = "theradex-uat-nci"
region  = "us-east-1"
}
