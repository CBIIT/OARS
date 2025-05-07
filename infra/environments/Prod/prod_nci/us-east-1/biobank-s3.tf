#S3 Bucket for Sharing Files with EET Biobank
resource "aws_s3_bucket" "theradex-biobank-share" {
  bucket = "theradex-biobank-share"
  tags = {
    "Managed By" = "Terraform"
    "Client"     = "EETBiobank"
  }
}

#IAM User for the S3 bucket
resource "aws_iam_user" "theradex-biobank-share-access" {
  name = "theradex-biobank-share-access"
  tags = {
    "Managed By" = "Terraform"
    "Client"     = "EETBiobank"
  }
}

#IAM User Access Key
resource "aws_iam_access_key" "theradex-biobank-share-access" {
  user = aws_iam_user.theradex-biobank-share-access.name
}

#Policy for access to the theradex-biobank-share S3 bucket
data "aws_iam_policy_document" "theradex-biobank-share-accesspolicy" {
  statement {
    effect    = "Allow"
    actions   = ["s3:GetObject", "s3:GetObjectAttributes", "s3:ListBucket"]
    resources = ["arn:aws:s3:::theradex-biobank-share", "arn:aws:s3:::theradex-biobank-share/*"]
  }
  #  statement {
  #    effect    = "Allow"
  #    actions   = ["s3:ListAllMyBuckets"]
  #    resources = ["arn:aws:s3:::*"]
  #  }
}

resource "aws_iam_user_policy" "theradex-biobank-share-access-userpolicy" {
  name   = "theradex-biobank-share-access-userpolicy"
  user   = aws_iam_user.theradex-biobank-share-access.name
  policy = data.aws_iam_policy_document.theradex-biobank-share-accesspolicy.json
}

#output "secret" {
#  value = nonsenstivie(aws_iam_access_key.theradex-biobank-share-access.secret)
#}