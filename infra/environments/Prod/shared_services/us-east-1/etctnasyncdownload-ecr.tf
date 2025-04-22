###############################
##### Create ECR for etctnasyncdownload-submitjob-lambda in shared Environment #####
###############################

###############################
##### dev #####
###############################

resource "aws_ecr_repository" "theradex_etctnasyncdownload-submitjob_lambda_shared_rep_dev" {
  name = "dev-etctnasyncdownload-submitjob-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_etctnasyncdownload-submitjob_lambda_shared_lifecycle_policy_dev" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-submitjob_lambda_shared_rep_dev.name
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

resource "aws_ecr_repository_policy" "theradex_etctnasyncdownload-submitjob_lambda_shared_rep_policy_dev" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-submitjob_lambda_shared_rep_dev.name
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

resource "aws_ecr_repository" "theradex_etctnasyncdownload-submitjob_lambda_shared_rep_uat" {
  name = "uat-etctnasyncdownload-submitjob-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_etctnasyncdownload-submitjob_lambda_shared_lifecycle_policy_uat" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-submitjob_lambda_shared_rep_uat.name
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

resource "aws_ecr_repository_policy" "theradex_etctnasyncdownload-submitjob_lambda_shared_rep_policy_uat" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-submitjob_lambda_shared_rep_uat.name
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

resource "aws_ecr_repository" "theradex_etctnasyncdownload-submitjob_lambda_shared_rep_prod" {
  name = "prod-etctnasyncdownload-submitjob-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_etctnasyncdownload-submitjob_lambda_shared_lifecycle_policy_prod" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-submitjob_lambda_shared_rep_prod.name
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

resource "aws_ecr_repository_policy" "theradex_etctnasyncdownload-submitjob_lambda_shared_rep_policy_prod" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-submitjob_lambda_shared_rep_prod.name
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
##### Create ECR for etctnasyncdownload-polljob-lambda in shared Environment #####
###############################

###############################
##### dev #####
###############################

resource "aws_ecr_repository" "theradex_etctnasyncdownload-polljob_lambda_shared_rep_dev" {
  name = "dev-etctnasyncdownload-polljob-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_etctnasyncdownload-polljob_lambda_shared_lifecycle_policy_dev" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-polljob_lambda_shared_rep_dev.name
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

resource "aws_ecr_repository_policy" "theradex_etctnasyncdownload-polljob_lambda_shared_rep_policy_dev" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-polljob_lambda_shared_rep_dev.name
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

resource "aws_ecr_repository" "theradex_etctnasyncdownload-polljob_lambda_shared_rep_uat" {
  name = "uat-etctnasyncdownload-polljob-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_etctnasyncdownload-polljob_lambda_shared_lifecycle_policy_uat" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-polljob_lambda_shared_rep_uat.name
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

resource "aws_ecr_repository_policy" "theradex_etctnasyncdownload-polljob_lambda_shared_rep_policy_uat" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-polljob_lambda_shared_rep_uat.name
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

resource "aws_ecr_repository" "theradex_etctnasyncdownload-polljob_lambda_shared_rep_prod" {
  name = "prod-etctnasyncdownload-polljob-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_etctnasyncdownload-polljob_lambda_shared_lifecycle_policy_prod" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-polljob_lambda_shared_rep_prod.name
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

resource "aws_ecr_repository_policy" "theradex_etctnasyncdownload-polljob_lambda_shared_rep_policy_prod" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-polljob_lambda_shared_rep_prod.name
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
##### Create ECR for etctnasyncdownload-getdata-lambda in shared Environment #####
###############################

###############################
##### dev #####
###############################

resource "aws_ecr_repository" "theradex_etctnasyncdownload-getdata_lambda_shared_rep_dev" {
  name = "dev-etctnasyncdownload-getdata-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_etctnasyncdownload-getdata_lambda_shared_lifecycle_policy_dev" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-getdata_lambda_shared_rep_dev.name
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

resource "aws_ecr_repository_policy" "theradex_etctnasyncdownload-getdata_lambda_shared_rep_policy_dev" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-getdata_lambda_shared_rep_dev.name
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

resource "aws_ecr_repository" "theradex_etctnasyncdownload-getdata_lambda_shared_rep_uat" {
  name = "uat-etctnasyncdownload-getdata-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_etctnasyncdownload-getdata_lambda_shared_lifecycle_policy_uat" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-getdata_lambda_shared_rep_uat.name
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

resource "aws_ecr_repository_policy" "theradex_etctnasyncdownload-getdata_lambda_shared_rep_policy_uat" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-getdata_lambda_shared_rep_uat.name
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

resource "aws_ecr_repository" "theradex_etctnasyncdownload-getdata_lambda_shared_rep_prod" {
  name = "prod-etctnasyncdownload-getdata-lambda"
  image_tag_mutability = "MUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}

resource "aws_ecr_lifecycle_policy" "theradex_etctnasyncdownload-getdata_lambda_shared_lifecycle_policy_prod" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-getdata_lambda_shared_rep_prod.name
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

resource "aws_ecr_repository_policy" "theradex_etctnasyncdownload-getdata_lambda_shared_rep_policy_prod" {
  repository = aws_ecr_repository.theradex_etctnasyncdownload-getdata_lambda_shared_rep_prod.name
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