resource "aws_codebuild_project" "theradex_qrcg_codebuild_project" {
  name = "theradex-qrcodegeneration-production-build"
  description = "[Production] Theradex QR CODE Generator Build Project"
  build_timeout = "60"
  service_role = aws_iam_role.theradex_qrcg_codebuild_role.arn
  source {
    type = "CODEPIPELINE"
    location = "theradex_qrcg_source_output"
    buildspec = "ecs/build.yaml"  
  }
  artifacts {
    type = "CODEPIPELINE"
    location = "theradex_qrcg_build_output"
  }
  environment {
    type = "LINUX_CONTAINER"
    image = "aws/codebuild/standard:6.0"
    compute_type = "BUILD_GENERAL1_SMALL"
    privileged_mode = true 
  } 
}

resource "aws_codepipeline" "theradex_qrcg_codepipeline" {
  name     = "theradex-qrcodegeneration-production"
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
        ConnectionArn        = aws_codestarconnections_connection.codestar_theradex.arn
        FullRepositoryId     = "Theradex/QRCodeWS_AWS"
        BranchName           = "production-nci"
      }
    }
  }

  stage {
    name = "Build"

    action {
      name             = "theradex-qrcodegeneration-production-build"
      category         = "Build"
      owner            = "AWS"
      provider         = "CodeBuild"
      input_artifacts  = ["theradex_qrcg_source_output"]
      output_artifacts = ["theradex_qrcg_build_output"]
      version          = "1"
      run_order        = 1

      configuration = {
        ProjectName = "theradex-qrcodegeneration-production-build"
      }
    }
  }
}

resource "aws_s3_bucket" "theradex_qrcg_codepipeline_bucket" {
  bucket = "theradex-qrcodegeneration-pipeline-artifacts"
}

resource "aws_s3_bucket_acl" "theradex_qrcg_codepipeline_bucket_acl" {
  bucket = aws_s3_bucket.theradex_qrcg_codepipeline_bucket.id
  acl    = "private"
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
        "${aws_s3_bucket.theradex_qrcg_codepipeline_bucket.arn}/*"
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
        "${aws_s3_bucket.theradex_qrcg_codepipeline_bucket.arn}/*"
      ]
    }
  ]
}
POLICY
}
