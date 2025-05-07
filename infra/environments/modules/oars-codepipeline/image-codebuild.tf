resource "aws_codebuild_project" "containerimage_build" {
  name          = "${var.project_name}-ContainerImage-Build"
  description   = "CodeBuild project building container image"
  build_timeout = 8
  service_role  = aws_iam_role.containerimage_role.arn

  artifacts {
    type     = "CODEPIPELINE"
    location = aws_s3_bucket.codepipeline_bucket.arn   
  }

  cache {
    type     = "NO_CACHE"    
  }

  environment {
    compute_type                = "BUILD_GENERAL1_SMALL"
    image                       = "aws/codebuild/standard:5.0"
    type                        = "LINUX_CONTAINER"
    image_pull_credentials_type = "CODEBUILD"
    privileged_mode             = true
    environment_variable {
      name  = "ARTIFACT_BUCKET_NAME"
      value = "${var.project_name}-pipeline"
      type  = "PLAINTEXT"
    }
     environment_variable {
      name  = "APP_NAME"
      value = "${var.project_name}"
      type  = "PLAINTEXT"
    }
    
  }

  logs_config {
    cloudwatch_logs {
      group_name = aws_cloudwatch_log_group.container_build_logs.name
    }

    s3_logs {
      status = "DISABLED"
    }
  }

  source {
    type            = "CODEPIPELINE"
    buildspec       = "buildspec-build.yml"
    git_clone_depth = 0

  }
   
  source_version = aws_s3_bucket.codepipeline_bucket.arn
  tags           = merge({"ProjectName"= var.project_name},var.tags)
}