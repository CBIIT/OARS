resource "aws_codebuild_project" "theradex_vendorintg_dev_codebuild_project" {
  name = "theradex-vendorintegration-development-build"
  description = "[Dev] Theradex Vendor Integration Build Project"
  build_timeout = "60"
  service_role = aws_iam_role.theradex_vendorintg_dev_codebuild_role.arn
  source {
    type = "CODEPIPELINE"
    location = "theradex_vendorintg_dev_source_output"
    buildspec = "ecs/dev-build.yaml"  
  }
  artifacts {
    type = "CODEPIPELINE"
    location = "theradex_vendorintg_dev_build_output"
  }
  environment {
    type = "LINUX_CONTAINER"
    image = "aws/codebuild/standard:6.0"
    compute_type = "BUILD_GENERAL1_SMALL"
    privileged_mode = true 
  } 
}

resource "aws_codebuild_project" "theradex_vendorintg_uat_deploy_project" {
  name = "theradex-vendorintegration-uat-deploy"
  description = "[UAT] Theradex Vendor Integration Deploy Project"
  build_timeout = "60"
  service_role = aws_iam_role.theradex_vendorintg_dev_codebuild_role.arn
  source {
    type = "CODEPIPELINE"
    #location = "theradex_vendorintg_uat_source_output"
    buildspec = "ecs/push-uat.yaml"  
  }
  artifacts {
    type = "CODEPIPELINE"
    location = "theradex_vendorintg_uat_deploy_output"
  }
  environment {
    type = "LINUX_CONTAINER"
    image = "aws/codebuild/standard:6.0"
    compute_type = "BUILD_GENERAL1_SMALL"
    privileged_mode = true 
  } 
}

resource "aws_codebuild_project" "theradex_vendorintg_prod_deploy_project" {
  name = "theradex-vendorintegration-prod-deploy"
  description = "[Prod] Theradex Vendor Integration Deploy Project"
  build_timeout = "60"
  service_role = aws_iam_role.theradex_vendorintg_dev_codebuild_role.arn
  source {
    type = "CODEPIPELINE"
    #location = "theradex_vendorintg_prod_source_output"
    buildspec = "ecs/push-prod.yaml"  
  }
  artifacts {
    type = "CODEPIPELINE"
    location = "theradex_vendorintg_prod_deploy_output"
  }
  environment {
    type = "LINUX_CONTAINER"
    image = "aws/codebuild/standard:6.0"
    compute_type = "BUILD_GENERAL1_SMALL"
    privileged_mode = true 
  } 
}

resource "aws_codepipeline" "theradex_vendorintgdev_codepipeline" {
  name     = "theradex-vendorintegration-development"
  role_arn = aws_iam_role.theradex_vendorintg_dev_codepipeline_role.arn

  artifact_store {
    location = aws_s3_bucket.theradex_vendorintg_dev_codepipeline_bucket.bucket
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
      output_artifacts = ["theradex_vendorintg_dev_source_output"]

      configuration = {
        ConnectionArn        = aws_codestarconnections_connection.codestar_theradex.arn
        FullRepositoryId     = "Theradex/vendor-integration"
        BranchName           = "dev-nci"
      }
    }
  } 

  stage {
    name = "Build-PushToDev"

    action {
      name             = "theradex-vendorintegration-development-build"
      category         = "Build"
      owner            = "AWS"
      provider         = "CodeBuild"
      input_artifacts  = ["theradex_vendorintg_dev_source_output"]
      output_artifacts = ["theradex_vendorintg_dev_build_output"]
      version          = "1"
      run_order        = 1

      configuration = {
        ProjectName = "theradex-vendorintegration-development-build"
      }
    }
  }

  stage {
    name = "UAT-CodeOwnerApproval"
    action {
      name      = "Apply"
      category  = "Approval"
      owner     = "AWS"
      provider  = "Manual"
      version   = "1"
      run_order = 2
      configuration = {
        NotificationArn = aws_sns_topic.codepipeline_integrationcodeowners_notification_topic.arn
      }
    }
  }

  stage {
    name = "PushToUAT"

    action {
      name             = "theradex-vendorintegration-uat-deploy"
      category         = "Build"
      owner            = "AWS"
      provider         = "CodeBuild"
      input_artifacts  = ["theradex_vendorintg_dev_source_output"]
      output_artifacts = ["theradex_vendorintg_uat_deploy_output"]
      version          = "1"
      run_order        = 1

      configuration = {
        ProjectName = "theradex-vendorintegration-uat-deploy"
      }
    }
  }

    stage {
    name = "Prod-CodeOwnerApproval"
    action {
      name      = "Apply"
      category  = "Approval"
      owner     = "AWS"
      provider  = "Manual"
      version   = "1"
      run_order = 2
      configuration = {
        NotificationArn = aws_sns_topic.codepipeline_integrationcodeowners_notification_topic.arn
      }
    }
  }

  stage {
    name = "PushToProd"

    action {
      name             = "theradex-vendorintegration-prod-deploy"
      category         = "Build"
      owner            = "AWS"
      provider         = "CodeBuild"
      input_artifacts  = ["theradex_vendorintg_dev_source_output"]
      output_artifacts = ["theradex_vendorintg_prod_deploy_output"]
      version          = "1"
      run_order        = 1

      configuration = {
        ProjectName = "theradex-vendorintegration-prod-deploy"
      }
    }
  }

}

resource "aws_s3_bucket" "theradex_vendorintg_dev_codepipeline_bucket" {
  bucket = "theradex-vendorintegration-pipeline-artifacts-development"
}

resource "aws_iam_role" "theradex_vendorintg_dev_codepipeline_role" {
  name = "theradex-vendorintegration-codepipeline-role-development"

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

resource "aws_iam_role" "theradex_vendorintg_dev_codebuild_role" {
  name = "theradex-vendorintegration-codebuild-role-development"

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

resource "aws_iam_role_policy" "theradex_vendorintg_dev_codepipeline_policy" {
  name = "theradex_vendorintg_dev_codepipeline_policy"
  role = aws_iam_role.theradex_vendorintg_dev_codepipeline_role.id

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
        "${aws_s3_bucket.theradex_vendorintg_dev_codepipeline_bucket.arn}",
        "${aws_s3_bucket.theradex_vendorintg_dev_codepipeline_bucket.arn}/*"
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
    },
    {
      "Effect":"Allow",
      "Action":"sns:Publish",
      "Resource":"${aws_sns_topic.codepipeline_integrationcodeowners_notification_topic.arn}"
    }
  ]
}
EOF
}

resource "aws_iam_role_policy" "theradex_vendorintg_dev_codebuild_policy" {
  name = "theradex_vendorintg_dev_codebuild_policy"
  role = aws_iam_role.theradex_vendorintg_dev_codebuild_role.id

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
        "${aws_s3_bucket.theradex_vendorintg_dev_codepipeline_bucket.arn}",
        "${aws_s3_bucket.theradex_vendorintg_dev_codepipeline_bucket.arn}/*"
      ]
    }
  ]
}
POLICY
}
