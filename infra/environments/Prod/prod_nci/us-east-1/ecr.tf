# This file defines the ECR Repos and Replication
# for containers: theradexqrcodegeneration

resource "aws_ecr_repository" "theradex_qrcg" {
  name                 = "theradexqrcodegeneration"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_qrcg" {
  repository = aws_ecr_repository.theradex_qrcg.name
  policy     = <<EOF
{
    "rules": [
        {
            "rulePriority": 1,
            "description": "Keep only 9 untagged images.",
            "selection": {
                "tagStatus": "untagged",
                "countType": "imageCountMoreThan",
                "countNumber": 9 
            },
            "action": {
                "type": "expire"
            }
        }
    ]
}
EOF
}

resource "aws_ecr_repository_policy" "theradex_qrcg_policy" {
  repository = aws_ecr_repository.theradex_qrcg.name
  policy     = <<EOF
{
    "Version": "2008-10-17",
    "Statement": [
      {
        "Sid": "AllowPushPull",
        "Effect": "Allow",
        "Principal": {
          "AWS": "arn:aws:iam::${var.aws_accounts.theradex-shared-service.account_id}:root"
        },
        "Action": [
          "ecr:BatchCheckLayerAvailability",
          "ecr:BatchGetImage",
          "ecr:CompleteLayerUpload",
          "ecr:DescribeImages",
          "ecr:DescribeRepositories",
          "ecr:GetDownloadUrlForLayer",
          "ecr:InitiateLayerUpload",
          "ecr:PutImage",
          "ecr:UploadLayerPart"
        ],
        "Condition": {
          "ForAnyValue:StringLike": {
            "aws:PrincipalOrgPaths": [
              "${var.org-id}/*"
            ]
          }
        }
      }
    ]
}
EOF
}

resource "aws_ecr_registry_policy" "theradex_qrcg_registry_policy" {
  policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Sid    = "AllowReplication",
        Effect = "Allow",
        Principal = {
          "AWS" : "arn:aws:iam::${var.aws_accounts.theradex-shared-service.account_id}:root"
        },
        Action = [
          "ecr:ReplicateImage",
          "ecr:CreateRepository"
        ],
        Resource = [
          "*"
        ]
      }
    ]
  })
}
