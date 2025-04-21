#AWS ECR repository for storing images 
resource "aws_ecr_repository" "repo" {
  name = "${var.project_name}-${var.environment_name}"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
  tags = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)
}

data "aws_iam_policy_document" "repo_policy" {
  statement {
    sid    = "repo policy"
    effect = "Allow"

    principals {
      type        = "AWS"
      identifiers = ["606199607275"]
    }

    actions = [
      "ecr:BatchCheckLayerAvailability",
      "ecr:CompleteLayerUpload",
      "ecr:GetAuthorizationToken",
      "ecr:InitiateLayerUpload",
      "ecr:PutImage",
      "ecr:UploadLayerPart",
    ]
  }
}

resource "aws_ecr_repository_policy" "repo_policy" {
  repository = aws_ecr_repository.repo.name
  policy     = data.aws_iam_policy_document.repo_policy.json
}