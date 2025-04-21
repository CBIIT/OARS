
data "aws_iam_policy_document" "ecr_push_role1_policy" {

  statement {
    actions = [
      "logs:CreateLogStream",
      "logs:PutLogEvents"
    ]
    resources = [
      aws_cloudwatch_log_group.ecr_push_logs1.arn
    ]
    effect = "Allow"
  }

  statement {
    actions = [
      "logs:CreateLogGroup",
      "logs:CreateLogStream",
      "logs:PutLogEvents"
    ]
    resources = [
      aws_cloudwatch_log_group.ecr_push_logs1.arn,
      "${aws_cloudwatch_log_group.ecr_push_logs1.arn}*"
    ]
    effect = "Allow"
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
      "arn:aws:codebuild:${data.aws_region.current.name}:${var.account_id}:project/${var.project_name}-dev-ECRpush*"
    ]
    effect = "Allow"
  }

  statement {
    actions = [
      "ecr:BatchCheckLayerAvailability",
      "ecr:CompleteLayerUpload",
      "ecr:InitiateLayerUpload",
      "ecr:PutImage",
      "ecr:UploadLayerPart"
    ]
    resources = [
      "arn:aws:ecr:us-east-1:993530973844:repository/${var.project_name}-dev"
    ]
    effect = "Allow"
  }

  statement {
    actions = [
      "ecr:GetAuthorizationToken"
    ]
    resources = [
      "*"
    ]
    effect = "Allow"
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
    effect = "Allow"
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

resource "aws_iam_role" "ecr_push_role1" {
  name               = "${var.project_name}-dev-ECRPushRole"
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
    name   = "dev-push-policy"
    policy = data.aws_iam_policy_document.ecr_push_role1_policy.json
  }  
  tags               = merge({"ProjectName"= var.project_name},var.tags)
}

data "aws_iam_policy_document" "ecr_push_role2_policy" {
  for_each = var.environment 
  statement {
    actions = [
      "logs:CreateLogStream",
      "logs:PutLogEvents"
    ]
    resources = [
      aws_cloudwatch_log_group.ecr_push_logs[each.key].arn
    ]
    effect = "Allow"
  }

  statement {
    actions = [
      "logs:CreateLogGroup",
      "logs:CreateLogStream",
      "logs:PutLogEvents"
    ]
    resources = [
      aws_cloudwatch_log_group.ecr_push_logs[each.key].arn,
      "${aws_cloudwatch_log_group.ecr_push_logs[each.key].arn}*"
    ]
    effect = "Allow"
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
      "arn:aws:codebuild:${data.aws_region.current.name}:${var.account_id}:project/${var.project_name}-${each.key}-ECRpush*",
    ]
    effect = "Allow"
  }

  statement {
    actions = [
      "ecr:BatchCheckLayerAvailability",
      "ecr:CompleteLayerUpload",
      "ecr:InitiateLayerUpload",
      "ecr:PutImage",
      "ecr:UploadLayerPart"
    ]
    resources = [
    "arn:aws:ecr:us-east-1:*:repository/${var.project_name}-${each.key}"
    ]
    effect   = "Allow"
  }  

  statement {
    actions = [
      "ecr:GetAuthorizationToken"
    ]
    resources = [ "*" ]
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

resource "aws_iam_role" "ecr_push_role2" {
  for_each           = var.environment 
  name               = "${var.project_name}-${each.value}-ECRPushRole"
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
    name = "push-policy" 
    policy   = data.aws_iam_policy_document.ecr_push_role2_policy[each.key].json
  }  
  tags               = merge({"ProjectName"= var.project_name},var.tags)
}

