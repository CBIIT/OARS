
resource "aws_iam_role" "cleanup" {
  name        = "${var.name}-cleanup-service-role"
  path        = "/"
  description = "${var.name} Cleanup Service Role"

  assume_role_policy = <<POLICY
{
    "Version": "2012-10-17",
    "Statement": [
      {
        "Effect": "Allow",
        "Principal": {
          "Service": "lambda.amazonaws.com"
        },
        "Action": "sts:AssumeRole"
      }
    ]
}
POLICY

  tags = var.tags
}

resource "aws_iam_role_policy_attachment" "cleanup_lambda_execution" {
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
  role       = aws_iam_role.cleanup.name
}

resource "aws_iam_role_policy_attachment" "cleanup" {
  policy_arn = aws_iam_policy.cleanup.arn
  role       = aws_iam_role.cleanup.name
}

resource "aws_iam_policy" "cleanup" {
  name = "${var.name}-backup-cleanup"

  policy = <<POLICY
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Action": [
                "elasticfilesystem:Backup",
                "elasticfilesystem:DescribeTags",
                "elasticfilesystem:DescribeFilesystems",
                "elasticfilesystem:DeleteFilesystem"
            ],
            "Resource": "arn:aws:elasticfilesystem:*:*:file-system/*",
            "Effect": "Allow"
        },
        {
            "Action": [
                "dynamodb:DescribeTable"
            ],
            "Resource": "arn:aws:dynamodb:*:*:table/*",
            "Effect": "Allow"
        },
        {
            "Action": [
                "dynamodb:DescribeBackup",
                "dynamodb:DeleteBackup"
            ],
            "Resource": "arn:aws:dynamodb:*:*:table/*/backup/*",
            "Effect": "Allow"
        },
        {
            "Action": [
                "rds:ListTagsForResource",
                "rds:DescribeDBSnapshots",
                "rds:DescribeDBInstances"
            ],
            "Resource": "*",
            "Effect": "Allow"
        },
        {
            "Action": [
                "rds:DeleteDBSnapshot"
            ],
            "Resource": [
                "arn:aws:rds:*:*:snapshot:awsbackup:*"
            ],
            "Effect": "Allow"
        },
        {
            "Action": [
                "ec2:DeleteSnapshot"
            ],
            "Resource": "arn:aws:ec2:*::snapshot/*",
            "Effect": "Allow"
        },
        {
            "Action": [
                "ec2:DescribeSnapshots",
                "ec2:DescribeTags",
                "ec2:DescribeImages",
                "ec2:DescribeInstances",
                "ec2:DescribeInstanceAttribute",
                "ec2:DescribeInstanceCreditSpecifications",
                "ec2:DescribeNetworkInterfaces",
                "ec2:DescribeElasticGpus",
                "ec2:DescribeSpotInstanceRequests"
            ],
            "Resource": "*",
            "Effect": "Allow"
        },
        {
            "Action": [
                "backup:DescribeBackupVault",
                "backup:DeleteRecoveryPoint",
                "backup:ListRecoveryPointsByBackupVault"
            ],
            "Resource": "arn:aws:backup:*:*:backup-vault:*",
            "Effect": "Allow"
        },
        {
            "Action": [
                "ec2:DeleteSnapshot",
                "ec2:DescribeVolumes",
                "ec2:DescribeSnapshots"
            ],
            "Resource": [
                "arn:aws:ec2:*::snapshot/*",
                "arn:aws:ec2:*:*:volume/*"
            ],
            "Effect": "Allow"
        },
        {
            "Action": "kms:DescribeKey",
            "Resource": "*",
            "Effect": "Allow"
        },
        {
            "Condition": {
                "Bool": {
                    "kms:GrantIsForAWSResource": "true"
                }
            },
            "Action": "kms:CreateGrant",
            "Resource": "*",
            "Effect": "Allow"
        },
        {
            "Condition": {
                "StringLike": {
                    "kms:ViaService": [
                        "ec2.*.amazonaws.com"
                    ]
                }
            },
            "Action": [
                "kms:GenerateDataKeyWithoutPlaintext"
            ],
            "Resource": "arn:aws:kms:*:*:key/*",
            "Effect": "Allow"
        },
        {
            "Action": [
                "tag:GetResources"
            ],
            "Resource": "*",
            "Effect": "Allow"
        }
    ]
}
POLICY
}
