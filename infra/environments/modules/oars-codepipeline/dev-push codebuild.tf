resource "aws_codebuild_project" "ecr_push_build1" {
  name          = "${var.project_name}-dev-ECRpush"
  description   = "Codebuild project for pushing dev image to ECR"
  build_timeout = 8
  service_role  = aws_iam_role.ecr_push_role1.arn 

  artifacts {
    type     = "CODEPIPELINE"
    location = aws_s3_bucket.codepipeline_bucket.arn
  }

  cache {
    type = "NO_CACHE"    
  }

  environment {
    compute_type                = "BUILD_GENERAL1_SMALL"
    image                       = "aws/codebuild/standard:5.0"
    type                        = "LINUX_CONTAINER"
    image_pull_credentials_type = "CODEBUILD"
    privileged_mode             = true
     environment_variable {
      name  = "ECR_REGION"
      value = "us-east-1"
      type = "PLAINTEXT"
    }
     environment_variable {
      name  = "APP_NAME"
      value = "${var.project_name}"
      type = "PLAINTEXT"
    }
     environment_variable {
      name  = "REPOSITORY_URI"
      value = "993530973844.dkr.ecr.us-east-1.amazonaws.com/${var.project_name}-dev"
      type = "PLAINTEXT"
    }
     environment_variable {
      name  = "ECS_SERVICE_NAME"
      value = "${var.project_name}-dev-service"
      type = "PLAINTEXT"
    }
     environment_variable {
      name  = "AWS_ACCOUNT_ID"
      value = "993530973844"
      type = "PLAINTEXT"
    }
     environment_variable {
      name  = "ENV_NAME"
      value = "dev"
      type = "PLAINTEXT"
    }
  }

  logs_config {
    cloudwatch_logs {
      group_name  = aws_cloudwatch_log_group.ecr_push_logs1.name 
    }

    s3_logs {
      status   = "DISABLED"
    }
  }

  source {
    type            = "CODEPIPELINE"
    buildspec       = "buildspec-push.yml"
    git_clone_depth = 0

  }
  
  source_version = aws_s3_bucket.codepipeline_bucket.arn 
  tags           = merge({"ProjectName"= var.project_name},var.tags)
}
