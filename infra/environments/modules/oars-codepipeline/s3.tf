
resource "aws_s3_bucket_server_side_encryption_configuration" "codepipeline_bucket" {
  bucket = aws_s3_bucket.codepipeline_bucket.bucket

  rule {
    apply_server_side_encryption_by_default {
      kms_master_key_id = "arn:aws:kms:us-east-1:606199607275:key/782afb96-5057-4890-a4f9-accfd8c566f0"
      sse_algorithm     = "aws:kms"
    }
  }  
}
resource "aws_s3_bucket_public_access_block" "codepipeline_bucket" {
  bucket = aws_s3_bucket.codepipeline_bucket.id

  block_public_acls   = false
  block_public_policy = false
}


data "aws_iam_policy_document" "s3_bucket_policy" {

  statement {
    actions = [
      "s3:PutObject", 
      "s3:PutObjectLegalHold", 
      "s3:PutObjectRetention", 
      "s3:PutObjectTagging", 
      "s3:PutObjectVersionTagging", 
      "s3:Abort*"
    ]

    principals {
      type        = "AWS"
      identifiers = [ 
         "arn:aws:iam::993530973844:root"
      ] 
    }

    resources = [ 
      "${aws_s3_bucket.codepipeline_bucket.arn}/*" 
    ]
  }

  statement {
    actions = [
      "s3:GetObject*", 
      "s3:GetBucket*", 
      "s3:List*"
    ]

    principals {
      type        = "AWS"
      identifiers = [
        "arn:aws:iam::993530973844:root"
      ]
    }

    resources = [
      aws_s3_bucket.codepipeline_bucket.arn,
      "${aws_s3_bucket.codepipeline_bucket.arn}/*"
    ]
  }

  statement {
    actions = [
        "s3:GetObject*", 
        "s3:GetBucket*", 
        "s3:List*", 
      ]

    principals {
      type        = "AWS"
      identifiers = [
        "arn:aws:iam::993530973844:role/${var.project_name}-dev-DeployRole"
      ]
    }

    resources = [
      aws_s3_bucket.codepipeline_bucket.arn,
      "${aws_s3_bucket.codepipeline_bucket.arn}/*"
    ]
  }

  dynamic "statement" {
    for_each = toset(var.environment)
    content {
      actions = [
        "s3:GetObject*", 
        "s3:GetBucket*", 
        "s3:List*"
    ] 
    principals {
      type        = "AWS"
      identifiers = [
        "arn:aws:iam::${lookup(var.environment_account, statement.value)}:role/${var.project_name}-${statement.value}-DeployRole"
      ]
    }
    resources = [
      "${aws_s3_bucket.codepipeline_bucket.arn}",
      "${aws_s3_bucket.codepipeline_bucket.arn}/*"
    ]
    }
  }
  statement {
    actions = [
    "s3:PutObject", 
    "s3:PutObjectLegalHold", 
    "s3:PutObjectRetention", 
    "s3:PutObjectTagging", 
    "s3:PutObjectVersionTagging", 
    "s3:Abort*"
  ]

    principals {
      type        = "AWS"
      identifiers = [
        "arn:aws:iam::352847057549:root"
      ]
    }

    resources = [
      "${aws_s3_bucket.codepipeline_bucket.arn}/*"
    ]
  }

  statement {
    actions = [
      "s3:GetObject*", 
      "s3:GetBucket*", 
      "s3:List*"
    ]

    principals {
      type        = "AWS" 
      identifiers = [
        "arn:aws:iam::352847057549:root"
      ]
    }

    resources = [
      aws_s3_bucket.codepipeline_bucket.arn,
      "${aws_s3_bucket.codepipeline_bucket.arn}/*"
    ]
  }


}
resource "aws_s3_bucket_policy" "codepipeline_bucket" {
  bucket = aws_s3_bucket.codepipeline_bucket.id
  policy = data.aws_iam_policy_document.s3_bucket_policy.json
}  
resource "aws_s3_bucket" "codepipeline_bucket" {
  bucket = "${var.project_name}-pipeline"
  tags   = merge({"ProjectName"= var.project_name},var.tags)
}