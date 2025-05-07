# This file defines the ECR Repos and Replication
# for containers: theradexqrcodegeneration

resource "aws_ecr_repository" "theradex_qrcg_dev" {
  name = "dev-theradexqrcodegeneration"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_qrcg_dev" {
  repository = aws_ecr_repository.theradex_qrcg_dev.name
  policy = <<EOF
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

resource "aws_ecr_repository_policy" "theradex_qrcg_dev_policy" {
  repository = aws_ecr_repository.theradex_qrcg_dev.name
  policy = <<EOF
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

resource "aws_ecr_repository" "theradex_qrcg_uat" {
  name = "uat-theradexqrcodegeneration"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_qrcg_uat" {
  repository = aws_ecr_repository.theradex_qrcg_uat.name
  policy = <<EOF
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

resource "aws_ecr_repository_policy" "theradex_qrcg_uat_policy" {
  repository = aws_ecr_repository.theradex_qrcg_uat.name
  policy = <<EOF
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

resource "aws_ecr_repository" "theradex_qrcg" {
  name = "theradexqrcodegeneration"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_qrcg" {
  repository = aws_ecr_repository.theradex_qrcg.name
  policy = <<EOF
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
  policy = <<EOF
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

resource "aws_ecr_replication_configuration" "theradex_qrcg_ecr_replication" {
  replication_configuration {
    rule {
      destination {
        region = "us-east-1"
        registry_id = var.aws_accounts.theradex-development-nci.account_id
      }
      repository_filter {
        filter          = "dev-theradexqrcodegeneration"
        filter_type     = "PREFIX_MATCH"
      }
      repository_filter {
        filter          = "dev-theradexvendorintegration"
        filter_type     = "PREFIX_MATCH"
      }
    }  
    rule {
      destination {
        region = "us-east-1"
        registry_id = var.aws_accounts.theradex-uat-nci.account_id
      }
      repository_filter {
        filter          = "uat-theradexqrcodegeneration"
        filter_type     = "PREFIX_MATCH"
      }
      repository_filter {
        filter          = "uat-theradexvendorintegration"
        filter_type     = "PREFIX_MATCH"
      } 	  
    }
    rule {
      destination {
        region = "us-east-1"
        registry_id = var.aws_accounts.theradex-production-nci.account_id
      }
      repository_filter {
        filter          = "theradexqrcodegeneration"
        filter_type     = "PREFIX_MATCH"
      }
      repository_filter {
        filter          = "prod-theradexvendorintegration"
        filter_type     = "PREFIX_MATCH"
      }       
    }
  }
}

#      destination {
#        region = "us-east-1"
#        registry_id = var.aws_accounts.theradex-development-commercial.account_id
#      }
#      destination {
#        region = "us-east-1"
#        registry_id = var.aws_accounts.theradex-uat-commercial.account_id
#      }
#      destination {
#        region = "us-east-1"
#        registry_id = var.aws_accounts.theradex-production-commercial.account_id
#      }
#      destination {
#        region = "us-east-1"
#        registry_id = var.aws_accounts.theradex-dr.account_id
#      }
