data "aws_region" "current" {}

data "aws_iam_policy_document" "containerimage_role_policy" {

  statement {
    actions = [
      "logs:CreateLogStream",
      "logs:PutLogEvents"
    ]
    resources = [
      aws_cloudwatch_log_group.container_build_logs.arn 
    ]
    effect    = "Allow"
  }

  statement {
    actions = [
      "logs:CreateLogGroup",
      "logs:CreateLogStream",
      "logs:PutLogEvents"
    ]
    resources = [
      aws_cloudwatch_log_group.container_build_logs.arn ,
      "${aws_cloudwatch_log_group.container_build_logs.arn}*"
    ]
    effect    = "Allow"
  }
  statement {
    actions = [
      "codebuild:CreateReportGroup",
      "codebuild:CreateReport",
      "codebuild:UpdateReport",
      "codebuild:BatchPutTestCases",
      "codebuild:BatchPutCodeCoverages"
    ]
    resources = [
      "arn:aws:codebuild:${data.aws_region.current.name}:${var.account_id}:project/${var.project_name}-ContainerImage-build" 
    ]
    effect    = "Allow"
  }

  statement {
    actions = [
      "s3:GetObject*",
      "s3:GetBucket*",
      "s3:List*",
      "s3:DeleteObject*",
      "s3:PutObject",
      "s3:PutObjectLegalHold",
      "s3:PutObjectRetention",
      "s3:PutObjectTagging",
      "s3:PutObjectVersionTagging",
      "s3:Abort*"
    ]
    resources = [
      "${aws_s3_bucket.codepipeline_bucket.arn}",
      "${aws_s3_bucket.codepipeline_bucket.arn}/*"
    ]
    effect    = "Allow"
  }

  statement {
    actions = [
      "kms:Decrypt",
      "kms:DescribeKey",
      "kms:Encrypt",
      "kms:ReEncrypt*",
      "kms:GenerateDataKey*"
    ]
    resources = [
       "${data.aws_kms_key.by_alias.arn}"
    ]
    effect    = "Allow"
  }

}

resource "aws_iam_role" "containerimage_role" {
  name               = "${var.project_name}-ContainerImageBuildRole"
  assume_role_policy = jsonencode({
    Version   = "2012-10-17"
    Statement = [
      {
        Action    = "sts:AssumeRole"
        Effect    = "Allow"
        Principal = {
          Service = "codebuild.amazonaws.com"
        }
      },
    ]
  })
  inline_policy {
    name   = "containerimage-build-policy"
    policy = data.aws_iam_policy_document.containerimage_role_policy.json
  }  
  tags     = merge({"ProjectName"= var.project_name},var.tags) 
}