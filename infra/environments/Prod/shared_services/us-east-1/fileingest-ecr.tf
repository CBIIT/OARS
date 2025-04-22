###############################
##### Create ECR for fileingesttomedidata-saverequest-lambda in shared Environment #####
###############################

###############################
##### dev #####
###############################

resource "aws_ecr_repository" "theradex_fileingesttomedidata-saverequest_lambda_shared_rep_dev" {
  name = "dev-fileingesttomedidata-saverequest-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_fileingesttomedidata-saverequest_lambda_shared_lifecycle_policy_dev" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-saverequest_lambda_shared_rep_dev.name
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

resource "aws_ecr_repository_policy" "theradex_fileingesttomedidata-saverequest_lambda_shared_rep_policy_dev" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-saverequest_lambda_shared_rep_dev.name
  policy = <<EOF
{
    "Version": "2008-10-17",
    "Statement": [
      {
        "Sid": "CrossAccountPermission",
        "Effect": "Allow",
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
        "Principal": {
                "AWS": [
                    "arn:aws:iam::${var.aws_accounts.theradex-development-nci.account_id}:root"
                ]
        }
      },
      {
        "Sid": "LambdaECRImageCrossAccountRetrievalPolicy",
        "Effect": "Allow",
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
        "Principal": {
          "Service": "lambda.amazonaws.com"
        },
        "Condition": {
                "StringLike": {
                  "aws:sourceARN": [
                    "arn:aws:lambda:us-east-1:${var.aws_accounts.theradex-development-nci.account_id}:function:*"
                  ]
                }
              }
      }      
    ]
}
EOF
}

###############################
##### uat #####
###############################

resource "aws_ecr_repository" "theradex_fileingesttomedidata-saverequest_lambda_shared_rep_uat" {
  name = "uat-fileingesttomedidata-saverequest-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_fileingesttomedidata-saverequest_lambda_shared_lifecycle_policy_uat" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-saverequest_lambda_shared_rep_uat.name
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

resource "aws_ecr_repository_policy" "theradex_fileingesttomedidata-saverequest_lambda_shared_rep_policy_uat" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-saverequest_lambda_shared_rep_uat.name
  policy = <<EOF
{
    "Version": "2008-10-17",
    "Statement": [
      {
        "Sid": "CrossAccountPermission",
        "Effect": "Allow",
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
        "Principal": {
                "AWS": [
                    "arn:aws:iam::${var.aws_accounts.theradex-uat-nci.account_id}:root"
                ]
        }
      },
      {
        "Sid": "LambdaECRImageCrossAccountRetrievalPolicy",
        "Effect": "Allow",
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
        "Principal": {
          "Service": "lambda.amazonaws.com"
        },
        "Condition": {
                "StringLike": {
                  "aws:sourceARN": [
                    "arn:aws:lambda:us-east-1:${var.aws_accounts.theradex-uat-nci.account_id}:function:*"
                  ]
                }
              }
      }      
    ]
}
EOF
}

###############################
##### prod #####
###############################

resource "aws_ecr_repository" "theradex_fileingesttomedidata-saverequest_lambda_shared_rep_prod" {
  name = "prod-fileingesttomedidata-saverequest-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_fileingesttomedidata-saverequest_lambda_shared_lifecycle_policy_prod" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-saverequest_lambda_shared_rep_prod.name
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

resource "aws_ecr_repository_policy" "theradex_fileingesttomedidata-saverequest_lambda_shared_rep_policy_prod" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-saverequest_lambda_shared_rep_prod.name
  policy = <<EOF
{
    "Version": "2008-10-17",
    "Statement": [
      {
        "Sid": "CrossAccountPermission",
        "Effect": "Allow",
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
        "Principal": {
                "AWS": [
                    "arn:aws:iam::${var.aws_accounts.theradex-production-nci.account_id}:root"
                ]
        }
      },
      {
        "Sid": "LambdaECRImageCrossAccountRetrievalPolicy",
        "Effect": "Allow",
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
        "Principal": {
          "Service": "lambda.amazonaws.com"
        },
        "Condition": {
                "StringLike": {
                  "aws:sourceARN": [
                    "arn:aws:lambda:us-east-1:${var.aws_accounts.theradex-production-nci.account_id}:function:*"
                  ]
                }
              }
      }      
    ]
}
EOF
}

###############################
##### Create ECR for fileingesttomedidata-validateandsave-lambda in shared Environment #####
###############################

###############################
##### dev #####
###############################

resource "aws_ecr_repository" "theradex_fileingesttomedidata-validateandsave_lambda_shared_rep_dev" {
  name = "dev-fileingesttomedidata-validateandsave-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_fileingesttomedidata-validateandsave_lambda_shared_lifecycle_policy_dev" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-validateandsave_lambda_shared_rep_dev.name
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

resource "aws_ecr_repository_policy" "theradex_fileingesttomedidata-validateandsave_lambda_shared_rep_policy_dev" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-validateandsave_lambda_shared_rep_dev.name
  policy = <<EOF
{
    "Version": "2008-10-17",
    "Statement": [
      {
        "Sid": "CrossAccountPermission",
        "Effect": "Allow",
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
        "Principal": {
                "AWS": [
                    "arn:aws:iam::${var.aws_accounts.theradex-development-nci.account_id}:root"
                ]
        }
      },
      {
        "Sid": "LambdaECRImageCrossAccountRetrievalPolicy",
        "Effect": "Allow",
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
        "Principal": {
          "Service": "lambda.amazonaws.com"
        },
        "Condition": {
                "StringLike": {
                  "aws:sourceARN": [
                    "arn:aws:lambda:us-east-1:${var.aws_accounts.theradex-development-nci.account_id}:function:*"
                  ]
                }
              }
      }      
    ]
}
EOF
}

###############################
##### uat #####
###############################

resource "aws_ecr_repository" "theradex_fileingesttomedidata-validateandsave_lambda_shared_rep_uat" {
  name = "uat-fileingesttomedidata-validateandsave-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_fileingesttomedidata-validateandsave_lambda_shared_lifecycle_policy_uat" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-validateandsave_lambda_shared_rep_uat.name
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

resource "aws_ecr_repository_policy" "theradex_fileingesttomedidata-validateandsave_lambda_shared_rep_policy_uat" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-validateandsave_lambda_shared_rep_uat.name
  policy = <<EOF
{
    "Version": "2008-10-17",
    "Statement": [
      {
        "Sid": "CrossAccountPermission",
        "Effect": "Allow",
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
        "Principal": {
                "AWS": [
                    "arn:aws:iam::${var.aws_accounts.theradex-uat-nci.account_id}:root"
                ]
        }
      },
      {
        "Sid": "LambdaECRImageCrossAccountRetrievalPolicy",
        "Effect": "Allow",
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
        "Principal": {
          "Service": "lambda.amazonaws.com"
        },
        "Condition": {
                "StringLike": {
                  "aws:sourceARN": [
                    "arn:aws:lambda:us-east-1:${var.aws_accounts.theradex-uat-nci.account_id}:function:*"
                  ]
                }
              }
      }      
    ]
}
EOF
}

###############################
##### prod #####
###############################

resource "aws_ecr_repository" "theradex_fileingesttomedidata-validateandsave_lambda_shared_rep_prod" {
  name = "prod-fileingesttomedidata-validateandsave-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_fileingesttomedidata-validateandsave_lambda_shared_lifecycle_policy_prod" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-validateandsave_lambda_shared_rep_prod.name
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

resource "aws_ecr_repository_policy" "theradex_fileingesttomedidata-validateandsave_lambda_shared_rep_policy_prod" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-validateandsave_lambda_shared_rep_prod.name
  policy = <<EOF
{
    "Version": "2008-10-17",
    "Statement": [
      {
        "Sid": "CrossAccountPermission",
        "Effect": "Allow",
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
        "Principal": {
                "AWS": [
                    "arn:aws:iam::${var.aws_accounts.theradex-production-nci.account_id}:root"
                ]
        }
      },
      {
        "Sid": "LambdaECRImageCrossAccountRetrievalPolicy",
        "Effect": "Allow",
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
        "Principal": {
          "Service": "lambda.amazonaws.com"
        },
        "Condition": {
                "StringLike": {
                  "aws:sourceARN": [
                    "arn:aws:lambda:us-east-1:${var.aws_accounts.theradex-production-nci.account_id}:function:*"
                  ]
                }
              }
      }      
    ]
}
EOF
}

###############################
##### Create ECR for fileingesttomedidata-generatemedidataodm-lambda in shared Environment #####
###############################

###############################
##### dev #####
###############################

resource "aws_ecr_repository" "theradex_fileingesttomedidata-generatemedidataodm_lambda_shared_rep_dev" {
  name = "dev-fileingesttomedidata-generatemedidataodm-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_fileingesttomedidata-generatemedidataodm_lambda_shared_lifecycle_policy_dev" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-generatemedidataodm_lambda_shared_rep_dev.name
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

resource "aws_ecr_repository_policy" "theradex_fileingesttomedidata-generatemedidataodm_lambda_shared_rep_policy_dev" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-generatemedidataodm_lambda_shared_rep_dev.name
  policy = <<EOF
{
    "Version": "2008-10-17",
    "Statement": [
      {
        "Sid": "CrossAccountPermission",
        "Effect": "Allow",
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
        "Principal": {
                "AWS": [
                    "arn:aws:iam::${var.aws_accounts.theradex-development-nci.account_id}:root"
                ]
        }
      },
      {
        "Sid": "LambdaECRImageCrossAccountRetrievalPolicy",
        "Effect": "Allow",
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
        "Principal": {
          "Service": "lambda.amazonaws.com"
        },
        "Condition": {
                "StringLike": {
                  "aws:sourceARN": [
                    "arn:aws:lambda:us-east-1:${var.aws_accounts.theradex-development-nci.account_id}:function:*"
                  ]
                }
              }
      }      
    ]
}
EOF
}

###############################
##### uat #####
###############################

resource "aws_ecr_repository" "theradex_fileingesttomedidata-generatemedidataodm_lambda_shared_rep_uat" {
  name = "uat-fileingesttomedidata-generatemedidataodm-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_fileingesttomedidata-generatemedidataodm_lambda_shared_lifecycle_policy_uat" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-generatemedidataodm_lambda_shared_rep_uat.name
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

resource "aws_ecr_repository_policy" "theradex_fileingesttomedidata-generatemedidataodm_lambda_shared_rep_policy_uat" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-generatemedidataodm_lambda_shared_rep_uat.name
  policy = <<EOF
{
    "Version": "2008-10-17",
    "Statement": [
      {
        "Sid": "CrossAccountPermission",
        "Effect": "Allow",
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
        "Principal": {
                "AWS": [
                    "arn:aws:iam::${var.aws_accounts.theradex-uat-nci.account_id}:root"
                ]
        }
      },
      {
        "Sid": "LambdaECRImageCrossAccountRetrievalPolicy",
        "Effect": "Allow",
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
        "Principal": {
          "Service": "lambda.amazonaws.com"
        },
        "Condition": {
                "StringLike": {
                  "aws:sourceARN": [
                    "arn:aws:lambda:us-east-1:${var.aws_accounts.theradex-uat-nci.account_id}:function:*"
                  ]
                }
              }
      }      
    ]
}
EOF
}

###############################
##### prod #####
###############################

resource "aws_ecr_repository" "theradex_fileingesttomedidata-generatemedidataodm_lambda_shared_rep_prod" {
  name = "prod-fileingesttomedidata-generatemedidataodm-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_fileingesttomedidata-generatemedidataodm_lambda_shared_lifecycle_policy_prod" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-generatemedidataodm_lambda_shared_rep_prod.name
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

resource "aws_ecr_repository_policy" "theradex_fileingesttomedidata-generatemedidataodm_lambda_shared_rep_policy_prod" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-generatemedidataodm_lambda_shared_rep_prod.name
  policy = <<EOF
{
    "Version": "2008-10-17",
    "Statement": [
      {
        "Sid": "CrossAccountPermission",
        "Effect": "Allow",
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
        "Principal": {
                "AWS": [
                    "arn:aws:iam::${var.aws_accounts.theradex-production-nci.account_id}:root"
                ]
        }
      },
      {
        "Sid": "LambdaECRImageCrossAccountRetrievalPolicy",
        "Effect": "Allow",
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
        "Principal": {
          "Service": "lambda.amazonaws.com"
        },
        "Condition": {
                "StringLike": {
                  "aws:sourceARN": [
                    "arn:aws:lambda:us-east-1:${var.aws_accounts.theradex-production-nci.account_id}:function:*"
                  ]
                }
              }
      }      
    ]
}
EOF
}

###############################
##### Create ECR for fileingesttomedidata-savetomedidata-lambda in shared Environment #####
###############################

###############################
##### dev #####
###############################

resource "aws_ecr_repository" "theradex_fileingesttomedidata-savetomedidata_lambda_shared_rep_dev" {
  name = "dev-fileingesttomedidata-savetomedidata-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_fileingesttomedidata-savetomedidata_lambda_shared_lifecycle_policy_dev" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-savetomedidata_lambda_shared_rep_dev.name
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

resource "aws_ecr_repository_policy" "theradex_fileingesttomedidata-savetomedidata_lambda_shared_rep_policy_dev" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-savetomedidata_lambda_shared_rep_dev.name
  policy = <<EOF
{
    "Version": "2008-10-17",
    "Statement": [
      {
        "Sid": "CrossAccountPermission",
        "Effect": "Allow",
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
        "Principal": {
                "AWS": [
                    "arn:aws:iam::${var.aws_accounts.theradex-development-nci.account_id}:root"
                ]
        }
      },
      {
        "Sid": "LambdaECRImageCrossAccountRetrievalPolicy",
        "Effect": "Allow",
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
        "Principal": {
          "Service": "lambda.amazonaws.com"
        },
        "Condition": {
                "StringLike": {
                  "aws:sourceARN": [
                    "arn:aws:lambda:us-east-1:${var.aws_accounts.theradex-development-nci.account_id}:function:*"
                  ]
                }
              }
      }      
    ]
}
EOF
}

###############################
##### uat #####
###############################

resource "aws_ecr_repository" "theradex_fileingesttomedidata-savetomedidata_lambda_shared_rep_uat" {
  name = "uat-fileingesttomedidata-savetomedidata-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_fileingesttomedidata-savetomedidata_lambda_shared_lifecycle_policy_uat" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-savetomedidata_lambda_shared_rep_uat.name
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

resource "aws_ecr_repository_policy" "theradex_fileingesttomedidata-savetomedidata_lambda_shared_rep_policy_uat" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-savetomedidata_lambda_shared_rep_uat.name
  policy = <<EOF
{
    "Version": "2008-10-17",
    "Statement": [
      {
        "Sid": "CrossAccountPermission",
        "Effect": "Allow",
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
        "Principal": {
                "AWS": [
                    "arn:aws:iam::${var.aws_accounts.theradex-uat-nci.account_id}:root"
                ]
        }
      },
      {
        "Sid": "LambdaECRImageCrossAccountRetrievalPolicy",
        "Effect": "Allow",
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
        "Principal": {
          "Service": "lambda.amazonaws.com"
        },
        "Condition": {
                "StringLike": {
                  "aws:sourceARN": [
                    "arn:aws:lambda:us-east-1:${var.aws_accounts.theradex-uat-nci.account_id}:function:*"
                  ]
                }
              }
      }      
    ]
}
EOF
}

###############################
##### prod #####
###############################

resource "aws_ecr_repository" "theradex_fileingesttomedidata-savetomedidata_lambda_shared_rep_prod" {
  name = "prod-fileingesttomedidata-savetomedidata-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_fileingesttomedidata-savetomedidata_lambda_shared_lifecycle_policy_prod" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-savetomedidata_lambda_shared_rep_prod.name
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

resource "aws_ecr_repository_policy" "theradex_fileingesttomedidata-savetomedidata_lambda_shared_rep_policy_prod" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-savetomedidata_lambda_shared_rep_prod.name
  policy = <<EOF
{
    "Version": "2008-10-17",
    "Statement": [
      {
        "Sid": "CrossAccountPermission",
        "Effect": "Allow",
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
        "Principal": {
                "AWS": [
                    "arn:aws:iam::${var.aws_accounts.theradex-production-nci.account_id}:root"
                ]
        }
      },
      {
        "Sid": "LambdaECRImageCrossAccountRetrievalPolicy",
        "Effect": "Allow",
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
        "Principal": {
          "Service": "lambda.amazonaws.com"
        },
        "Condition": {
                "StringLike": {
                  "aws:sourceARN": [
                    "arn:aws:lambda:us-east-1:${var.aws_accounts.theradex-production-nci.account_id}:function:*"
                  ]
                }
              }
      }      
    ]
}
EOF
}

###############################
##### Create ECR for fileingesttomedidata-updaterequeststatus-lambda in shared Environment #####
###############################

###############################
##### dev #####
###############################

resource "aws_ecr_repository" "theradex_fileingesttomedidata-updaterequeststatus_lambda_shared_rep_dev" {
  name = "dev-fileingesttomedidata-updaterequeststatus-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_fileingesttomedidata-updaterequeststatus_lambda_shared_lifecycle_policy_dev" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-updaterequeststatus_lambda_shared_rep_dev.name
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

resource "aws_ecr_repository_policy" "theradex_fileingesttomedidata-updaterequeststatus_lambda_shared_rep_policy_dev" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-updaterequeststatus_lambda_shared_rep_dev.name
  policy = <<EOF
{
    "Version": "2008-10-17",
    "Statement": [
      {
        "Sid": "CrossAccountPermission",
        "Effect": "Allow",
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
        "Principal": {
                "AWS": [
                    "arn:aws:iam::${var.aws_accounts.theradex-development-nci.account_id}:root"
                ]
        }
      },
      {
        "Sid": "LambdaECRImageCrossAccountRetrievalPolicy",
        "Effect": "Allow",
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
        "Principal": {
          "Service": "lambda.amazonaws.com"
        },
        "Condition": {
                "StringLike": {
                  "aws:sourceARN": [
                    "arn:aws:lambda:us-east-1:${var.aws_accounts.theradex-development-nci.account_id}:function:*"
                  ]
                }
              }
      }      
    ]
}
EOF
}

###############################
##### uat #####
###############################

resource "aws_ecr_repository" "theradex_fileingesttomedidata-updaterequeststatus_lambda_shared_rep_uat" {
  name = "uat-fileingesttomedidata-updaterequeststatus-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_fileingesttomedidata-updaterequeststatus_lambda_shared_lifecycle_policy_uat" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-updaterequeststatus_lambda_shared_rep_uat.name
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

resource "aws_ecr_repository_policy" "theradex_fileingesttomedidata-updaterequeststatus_lambda_shared_rep_policy_uat" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-updaterequeststatus_lambda_shared_rep_uat.name
  policy = <<EOF
{
    "Version": "2008-10-17",
    "Statement": [
      {
        "Sid": "CrossAccountPermission",
        "Effect": "Allow",
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
        "Principal": {
                "AWS": [
                    "arn:aws:iam::${var.aws_accounts.theradex-uat-nci.account_id}:root"
                ]
        }
      },
      {
        "Sid": "LambdaECRImageCrossAccountRetrievalPolicy",
        "Effect": "Allow",
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
        "Principal": {
          "Service": "lambda.amazonaws.com"
        },
        "Condition": {
                "StringLike": {
                  "aws:sourceARN": [
                    "arn:aws:lambda:us-east-1:${var.aws_accounts.theradex-uat-nci.account_id}:function:*"
                  ]
                }
              }
      }      
    ]
}
EOF
}

###############################
##### prod #####
###############################

resource "aws_ecr_repository" "theradex_fileingesttomedidata-updaterequeststatus_lambda_shared_rep_prod" {
  name = "prod-fileingesttomedidata-updaterequeststatus-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_fileingesttomedidata-updaterequeststatus_lambda_shared_lifecycle_policy_prod" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-updaterequeststatus_lambda_shared_rep_prod.name
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

resource "aws_ecr_repository_policy" "theradex_fileingesttomedidata-updaterequeststatus_lambda_shared_rep_policy_prod" {
  repository = aws_ecr_repository.theradex_fileingesttomedidata-updaterequeststatus_lambda_shared_rep_prod.name
  policy = <<EOF
{
    "Version": "2008-10-17",
    "Statement": [
      {
        "Sid": "CrossAccountPermission",
        "Effect": "Allow",
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
        "Principal": {
                "AWS": [
                    "arn:aws:iam::${var.aws_accounts.theradex-production-nci.account_id}:root"
                ]
        }
      },
      {
        "Sid": "LambdaECRImageCrossAccountRetrievalPolicy",
        "Effect": "Allow",
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
        "Principal": {
          "Service": "lambda.amazonaws.com"
        },
        "Condition": {
                "StringLike": {
                  "aws:sourceARN": [
                    "arn:aws:lambda:us-east-1:${var.aws_accounts.theradex-production-nci.account_id}:function:*"
                  ]
                }
              }
      }      
    ]
}
EOF
}