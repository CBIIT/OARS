resource "aws_codebuild_project" "theradex_qrcg_codebuild_project" {
  name          = "theradex-qrcodegeneration-build"
  description   = "Theradex QR CODE Generator Build Project"
  build_timeout = "60"
  service_role  = aws_iam_role.theradex_qrcg_codebuild_role.arn
  source {
    type      = "CODEPIPELINE"
    location  = "theradex_qrcg_source_output"
    buildspec = "serverless/build.yaml"
  }
  artifacts {
    type     = "CODEPIPELINE"
    location = "theradex_qrcg_build_output"
  }
  environment {
    type            = "LINUX_CONTAINER"
    image           = "aws/codebuild/standard:6.0"
    compute_type    = "BUILD_GENERAL1_SMALL"
    privileged_mode = true
  }
}

resource "aws_codepipeline" "theradex_qrcg_codepipeline" {
  name     = "theradex-qrcodegeneration"
  role_arn = aws_iam_role.theradex_qrcg_codepipeline_role.arn

  artifact_store {
    location = aws_s3_bucket.theradex_qrcg_codepipeline_bucket.bucket
    type     = "S3"

    encryption_key {
      id   = module.kms.s3_key_id
      type = "KMS"
    }
  }

  stage {
    name = "Source"

    action {
      name             = "Source"
      category         = "Source"
      owner            = "AWS"
      provider         = "CodeStarSourceConnection"
      version          = "1"
      run_order        = 1
      output_artifacts = ["theradex_qrcg_source_output"]

      configuration = {
        ConnectionArn    = aws_codestarconnections_connection.codestar_theradex.arn
        FullRepositoryId = "Theradex/QRCodeWS_AWS"
        BranchName       = "uat-nci"
      }
    }
  }

  stage {
    name = "Build"

    action {
      name             = "theradex-qrcodegeneration-serverless-deploy"
      category         = "Build"
      owner            = "AWS"
      provider         = "CodeBuild"
      input_artifacts  = ["theradex_qrcg_source_output"]
      output_artifacts = ["theradex_qrcg_build_output"]
      version          = "1"
      run_order        = 1

      configuration = {
        ProjectName = "theradex-qrcodegeneration-build"
      }
    }
  }
}

resource "aws_s3_bucket" "theradex_qrcg_codepipeline_bucket" {
  bucket = "theradex-qrcodegeneration-serverless-uat-noncomm-artifacts"
}

resource "aws_s3_bucket_acl" "theradex_qrcg_codepipeline_bucket_acl" {
  bucket = aws_s3_bucket.theradex_qrcg_codepipeline_bucket.id
  acl    = "private"
}

resource "aws_s3_bucket" "theradex_qrcg_app_bucket" {
  bucket = "theradex-qrcode-uat-noncomm"
}

resource "aws_s3_bucket_acl" "theradex_qrcg_app_bucket_acl" {
  bucket = aws_s3_bucket.theradex_qrcg_app_bucket.id
  acl    = "private"
}

resource "aws_s3_bucket_server_side_encryption_configuration" "theradex_qrcg_app_bucket_kms" {
  bucket = aws_s3_bucket.theradex_qrcg_app_bucket.bucket

  rule {
    apply_server_side_encryption_by_default {
      kms_master_key_id = module.kms.s3_key_arn
      sse_algorithm     = "aws:kms"
    }
  }
}

resource "aws_iam_role" "theradex_qrcg_codepipeline_role" {
  name = "theradex-qrcodegeneration-codepipeline-role"

  assume_role_policy = <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Principal": {
        "Service": "codepipeline.amazonaws.com"
      },
      "Action": "sts:AssumeRole"
    }
  ]
}
EOF
}

resource "aws_iam_role" "theradex_qrcg_codebuild_role" {
  name = "theradex-qrcodegeneration-codebuild-role"

  assume_role_policy = <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Principal": {
        "Service": "codebuild.amazonaws.com"
      },
      "Action": "sts:AssumeRole"
    }
  ]
}
EOF
}

resource "aws_iam_role_policy" "theradex_qrcg_codepipeline_policy" {
  name = "theradex_qrcg_codepipeline_policy"
  role = aws_iam_role.theradex_qrcg_codepipeline_role.id

  policy = <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect":"Allow",
      "Action": [
        "s3:GetObject",
        "s3:GetObjectVersion",
        "s3:GetBucketVersioning",
        "s3:PutObjectAcl",
        "s3:PutObject"
      ],
      "Resource": [
        "${aws_s3_bucket.theradex_qrcg_codepipeline_bucket.arn}",
        "${aws_s3_bucket.theradex_qrcg_codepipeline_bucket.arn}/*",
        "${aws_s3_bucket.theradex_qrcg_app_bucket.arn}",
        "${aws_s3_bucket.theradex_qrcg_app_bucket.arn}/*"
      ]
    },
    {
      "Effect": "Allow",
      "Action": [
        "codestar-connections:UseConnection"
      ],
      "Resource": "${aws_codestarconnections_connection.codestar_theradex.arn}"
    },
    {
      "Effect":"Allow",
      "Action": [
         "kms:Encrypt",
         "kms:Decrypt",
         "kms:ReEncrypt**",
         "kms:GenerateDataKey*",
         "kms:DescribeKey"
      ],
      "Resource": "${module.kms.s3_key_arn}"

    },
    {
      "Effect": "Allow",
      "Action": [
        "codebuild:BatchGetBuilds",
        "codebuild:StartBuild"
      ],
      "Resource": "*"
    }
  ]
}
EOF
}

resource "aws_iam_role_policy" "theradex_qrcg_codebuild_policy" {
  name = "theradex_qrcg_codebuild_policy"
  role = aws_iam_role.theradex_qrcg_codebuild_role.id

  policy = <<POLICY
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Resource": [
        "*"
      ],
      "Action": [
        "logs:CreateLogGroup",
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ]
    },
    {
      "Effect":"Allow",
      "Action": [
         "kms:Encrypt",
         "kms:Decrypt",
         "kms:ReEncrypt**",
         "kms:GenerateDataKey*",
         "kms:DescribeKey"
      ],
      "Resource": "${module.kms.s3_key_arn}"

    },
    {
      "Effect": "Allow",
      "Action": [
        "ec2:CreateNetworkInterface",
        "ec2:DescribeDhcpOptions",
        "ec2:DescribeNetworkInterfaces",
        "ec2:DeleteNetworkInterface",
        "ec2:DescribeSubnets",
        "ec2:DescribeSecurityGroups",
        "ec2:DescribeVpcs"
      ],
      "Resource": "*"
    },
    {
      "Effect": "Allow",
      "Action": [
         "cloudformation:*"
      ],
      "Resource": "*"
    },
    {
      "Effect": "Allow",
      "Action": [
         "lambda:*"
      ],
      "Resource": "*"
    },
    {
      "Effect": "Allow",
      "Action": [
         "logs:CreateLogGroup",
         "logs:Get*",
         "logs:Describe*",
         "logs:List*",
         "logs:DeleteLogGroup",
         "logs:PutResourcePolicy",
         "logs:DeleteResourcePolicy",
         "logs:PutRetentionPolicy",
         "logs:DeleteRetentionPolicy",
         "logs:TagLogGroup",
         "logs:UntagLogGroup",
         "logs:CreateLogDelivery",
         "logs:DeleteLogDelivery",
         "logs:DescribeResourcePolicies",
         "logs:DescribeLogGroups"
      ],
      "Resource": "*"
    },
    {
      "Effect": "Allow",
      "Action": [
         "iam:Get*",
         "iam:List*",
         "iam:PassRole",
         "iam:CreateRole",
         "iam:DeleteRole",
         "iam:AttachRolePolicy",
         "iam:DeleteRolePolicy",
         "iam:PutRolePolicy",
         "iam:TagRole",
         "iam:UntagRole"
      ],
      "Resource": "*"
    },
    {
      "Effect": "Allow",
      "Action": [
         "apigateway:GET",
         "apigateway:POST",
         "apigateway:PUT",
         "apigateway:PATCH",
         "apigateway:DELETE"
      ],
      "Resource": "*"
    },
    {
      "Effect": "Allow",
      "Action": [
         "events:Describe*",
         "events:Get*",
         "events:List*",
         "events:CreateEventBus",
         "events:DeleteEventBus",
         "events:PutRule",
         "events:DeleteRule",
         "events:PutTargets",
         "events:RemoveTargets",
         "events:TagResource",
         "events:UntagResource"
      ],
      "Resource": "*"
    },
    {
      "Effect": "Allow",
      "Action": [
         "sns:Get*",
         "sns:Describe*",
         "sns:CreateTopic",
         "sns:DeleteTopic",
         "sns:SetTopicAttributes",
         "sns:Subscribe",
         "sns:Unsubscribe",
         "sns:TagResource"
      ],
      "Resource": "*"
    },
    {
      "Effect": "Allow",
      "Action": [
         "dynamodb:CreateTable",
         "dynamodb:CreateTableReplica",
         "dynamodb:CreateGlobalTable",
         "dynamodb:DeleteTable",
         "dynamodb:DeleteGlobalTable",
         "dynamodb:DeleteTableReplica",
         "dynamodb:Describe*",
         "dynamodb:List*",
         "dynamodb:Get*",
         "dynamodb:TagResource",
         "dynamodb:UntagResource",
         "dynamodb:UpdateContinuousBackups",
         "dynamodb:UpdateGlobalTable",
         "dynamodb:UpdateGlobalTableSettings",
         "dynamodb:UpdateTable",
         "dynamodb:UpdateTableReplicaAutoScaling",
         "dynamodb:UpdateTimeToLive"
      ],
      "Resource": "*"
    },
    {
      "Effect": "Allow",
      "Action": [
         "sqs:CreateQueue",
         "sqs:DeleteQueue",
         "sqs:SetQueueAttributes",
         "sqs:AddPermission",
         "sqs:RemovePermission",
         "sqs:TagQueue",
         "sqs:UntagQueue",
         "sqs:Get*",
         "sqs:List*"
      ],
      "Resource": "*"
    },
    {
      "Effect": "Allow",
      "Action": [
         "cloudformation:CreateChangeSet",
         "cloudformation:CreateStack",
         "cloudformation:DeleteChangeSet",
         "cloudformation:DeleteStack",
         "cloudformation:DescribeChangeSet",
         "cloudformation:DescribeStackEvents",
         "cloudformation:DescribeStackResource",
         "cloudformation:DescribeStackResources",
         "cloudformation:DescribeStacks",
         "cloudformation:ExecuteChangeSet",
         "cloudformation:ListStackResources",
         "cloudformation:SetStackPolicy",
         "cloudformation:UpdateStack",
         "cloudformation:UpdateTerminationProtection",
         "cloudformation:GetTemplate",
         "cloudformation:ValidateTemplate"
      ],
      "Resource": "*"
    },
    {
      "Effect": "Allow",
      "Action": [
         "ssm:GetParameter*",
         "ssm:DescribeParameters",
         "ssm:DeleteParameter*",
         "ssm:PutParameter"
      ],
      "Resource": "*"
    },
    {
      "Effect": "Allow",
      "Action": [
        "ecr:*"
      ],
      "Resource": "*"
    },
    {
      "Effect": "Allow",
      "Action": [
        "s3:*"
      ],
      "Resource": [
        "${aws_s3_bucket.theradex_qrcg_codepipeline_bucket.arn}",
        "${aws_s3_bucket.theradex_qrcg_codepipeline_bucket.arn}/*",
        "${aws_s3_bucket.theradex_qrcg_app_bucket.arn}",
        "${aws_s3_bucket.theradex_qrcg_app_bucket.arn}/*"
      ]
    }
  ]
}
POLICY
}
