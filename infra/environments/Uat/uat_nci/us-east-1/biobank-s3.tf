#S3 Bucket for Sharing Files with EET Biobank
resource "aws_s3_bucket" "theradex-biobank-share-uat" {
   bucket = "theradex-biobank-share-uat"
   tags = {
    "Managed By" = "Terraform"
    "Client" = "EETBiobank"
}
}

#IAM User for the S3 bucket
resource "aws_iam_user" "theradex-biobank-share-uat-access" {
    name = "theradex-biobank-share-uat-access"
      tags = {
       "Managed By" = "Terraform"
       "Client" = "EETBiobank"
  }
}

#IAM User Access Key
resource "aws_iam_access_key" "theradex-biobank-share-uat-access" {
  user = aws_iam_user.theradex-biobank-share-uat-access.name
}

#Policy for access to the theradex-biobank-share-uat S3 bucket
data "aws_iam_policy_document" "theradex-biobank-share-uat-accesspolicy" {
  statement {
    effect    = "Allow"
    actions   = ["s3:GetObject","s3:GetObjectAttributes","s3:ListBucket"]
    resources = ["arn:aws:s3:::theradex-biobank-share-uat","arn:aws:s3:::theradex-biobank-share-uat/*"]
  }
#  statement {
#    effect    = "Allow"
#    actions   = ["s3:ListAllMyBuckets"]
#    resources = ["arn:aws:s3:::*"]
#  }
}

resource "aws_iam_user_policy" "theradex-biobank-share-uat-access-userpolicy" {
  name   = "theradex-biobank-share-uat-access-userpolicy"
  user   = aws_iam_user.theradex-biobank-share-uat-access.name
  policy = data.aws_iam_policy_document.theradex-biobank-share-uat-accesspolicy.json
}

#output "secret" {
#  value = nonsenstivie(aws_iam_access_key.theradex-biobank-share-uat-access.secret)
#}