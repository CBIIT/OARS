data "aws_iam_policy_document" "pipeline_role_policy" {

  statement {
    actions   = ["codestar-connections:UseConnection"]
    resources = [ 
      "${data.aws_codestarconnections_connection.github.arn}" 
      ]
    effect    = "Allow"
  }
  dynamic "statement" {
    for_each = toset(var.environment)
    content {
    actions = [
      "codebuild:BatchGetBuilds",
      "codebuild:StartBuild",
      "codebuild:StopBuild"
    ]
    resources = [ "arn:aws:codebuild:${data.aws_region.current.name}:${var.account_id}:project/${var.project_name}-${statement.value}-ECRpush" ]
    effect    = "Allow"

    }
  }
  statement {
    actions = [
      "s3:PutObjectAcl",
      "s3:PutObjectVersionAcl",
      "s3:GetBucketVersioning",
      "s3:GetObject",
      "s3:GetObjectVersion"
    ]
    resources = [
      "${aws_s3_bucket.codepipeline_bucket.arn}/*",
      ]
    effect    = "Allow"
  }

  statement {
    actions = [
      "codebuild:BatchGetBuilds",
      "codebuild:StartBuild",
      "codebuild:StopBuild"
    ]
    resources = [
      "arn:aws:codebuild:${data.aws_region.current.name}:${var.account_id}:project/${var.project_name}-ContainerImage-Build",
      "arn:aws:codebuild:${data.aws_region.current.name}:${var.account_id}:project/${var.project_name}-dev-ECRpush",
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
      "${aws_s3_bucket.codepipeline_bucket.arn}/*",
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

  statement { 
    actions = ["sts:AssumeRole"]
    resources = [
        for arn in local.deploy_role_arns : arn      
    ]
    effect    = "Allow"
  }
   statement {  
    actions = ["sts:AssumeRole"]
    resources = [
      "arn:aws:iam::993530973844:role/${var.project_name}-dev-DeployRole",
    ]
    effect = "Allow"
  }
}


resource "aws_iam_role" "pipeline_role" {
  name               = "${var.project_name}-PipelineRole"
  assume_role_policy = jsonencode({
    Version   = "2012-10-17"
    Statement = [
      {
        Action    = "sts:AssumeRole"
        Effect    = "Allow"
        Principal = {
          Service = "codepipeline.amazonaws.com" 
         }
      },
    ]
  })
  inline_policy {
    name   = "pipeline-policy"
    policy = data.aws_iam_policy_document.pipeline_role_policy.json
  }  
  tags     = merge({"ProjectName"= var.project_name},var.tags) 
}
