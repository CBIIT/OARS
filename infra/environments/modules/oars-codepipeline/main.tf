resource "aws_codepipeline" "Pipeline" {
  name       = var.project_name
  role_arn   = aws_iam_role.pipeline_role.arn
  depends_on = [aws_iam_role.pipeline_role]

  artifact_store {
    location = aws_s3_bucket.codepipeline_bucket.bucket
    type     = "S3"
    encryption_key {
      id   = data.aws_kms_key.by_alias.arn
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
      output_artifacts = ["SourceArtifact"]

      configuration = {
        ConnectionArn    = "${data.aws_codestarconnections_connection.github.arn}"
        FullRepositoryId = "Theradex/nci-web-reporting"
        BranchName       = "main"
      }
    }
  }

  stage {
    name = "Build"

    action {
      name             = "Build"
      category         = "Build"
      owner            = "AWS"
      provider         = "CodeBuild"
      input_artifacts  = ["SourceArtifact"]
      output_artifacts = ["ContainerImageBuildArtifact"]
      version          = "1"

      configuration = {
        ProjectName = "${var.project_name}-ContainerImage-Build"
      }
    }
  }

  stage {
    name = "Push-dev"

    action {
      name             = "Push-dev"
      category         = "Build"
      owner            = "AWS"
      provider         = "CodeBuild"
      input_artifacts  = ["ContainerImageBuildArtifact"]
      output_artifacts = ["EcrPushArtifact-dev"]
      version          = "1"

      configuration = {
        ProjectName = "${var.project_name}-dev-ECRpush"
      }
    }
  }

  stage {
    name = "Deploy-dev"

    action {
      name            = "Deploy-dev"
      category        = "Deploy"
      owner           = "AWS"
      provider        = "ECS"
      input_artifacts = ["EcrPushArtifact-dev"]
      version         = "1"
      role_arn        = "arn:aws:iam::993530973844:role/${var.project_name}-dev-DeployRole"

      configuration = {
        ClusterName       = "${var.project_name}-dev-cluster"
        ServiceName       = "${var.project_name}-dev-service"
        DeploymentTimeout = "30"
      }
    }
  }


  dynamic "stage" {
    for_each = var.environment2
    content {
      name =  stage.value["name"]

      action {
        name             = "Approval-${stage.value["name"]}"
        category         = "Approval"
        owner            = "AWS"
        provider         = "Manual"
        input_artifacts  = []
        version          = "1"
        output_artifacts = []
        run_order        = 1
      }

      action {
        name             = "Push-${stage.value["name"]}"
        category         = "Build"
        owner            = "AWS"
        provider         = "CodeBuild"
        input_artifacts  = ["ContainerImageBuildArtifact"]
        version          = "1"
        output_artifacts = ["EcrPushArtifact-${stage.value["name"]}"]
        run_order        = 2
        configuration = {
          ProjectName = "${var.project_name}-${stage.value["name"]}-ECRpush"
        }
      }

      action {
        name            = "Deploy-${stage.value["name"]}"
        category        = "Deploy"
        owner           = "AWS"
        provider        = "ECS"
        input_artifacts = ["EcrPushArtifact-${stage.value["name"]}"]
        version         = "1"
        role_arn        = lookup(local.deploy_role_arns, stage.value["name"], null)
        run_order       = 3
        configuration = {
          ClusterName       = "${var.project_name}-${stage.value["name"]}-cluster"
          ServiceName       = "${var.project_name}-${stage.value["name"]}-service"
          DeploymentTimeout = "30"
        }
      }
    }
  }

  tags = merge({ "ProjectName" = var.project_name }, var.tags)

}
