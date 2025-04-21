#IAM role for ECS task execution
data "aws_iam_policy_document" "ecs_execution_policy" {
  statement {
    actions   = ["ecr:BatchCheckLayerAvailability", "ecr:GetDownloadUrlForLayer", "ecr:BatchGetImage"]
    resources = ["arn:aws:ecr:**:*:repository/${var.project_name}-${var.environment_name}"]
    effect    = "Allow"
  }
  statement {
    actions   = ["ecr:GetAuthorizationToken","ecr:InitiateLayerUpload"]
    resources = ["*"]
     effect   = "Allow"
  }
   statement {
    actions   = ["logs:CreateLogStream", "logs:PutLogEvents"]
    resources = ["arn:aws:logs:*:*:log-group:/ecs/${var.project_name}-${var.environment_name}-service:*"]
     effect   = "Allow"
  }
   statement {
    actions   = ["logs:DescribeLogGroups"]
    resources = ["*"]
     effect   = "Allow"
  }
   statement {
    actions   = ["secretsmanager:*"]
    resources = ["arn:aws:secretsmanager:*:*:secret:/${var.project_name}/${var.environment_name}/*"]
     effect   = "Allow"
  }
 
}
resource "aws_iam_role" "ecs_task_execution_role" {
  name               = "${var.project_name}-${var.environment_name}-Execution-Role"
  assume_role_policy = data.aws_iam_policy_document.ecs_task_policy.json
  managed_policy_arns= ["arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"] 
  inline_policy {
    name   = "${var.project_name}-${var.environment_name}-ecs-policy"
    policy = data.aws_iam_policy_document.ecs_execution_policy.json
  }
  tags = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)
}

#IAM role for ECS task role
data "aws_iam_policy_document" "ecs_task_policy" {
  statement {
    actions = ["sts:AssumeRole"]
    principals {
      type        = "Service"
      identifiers = ["ecs-tasks.amazonaws.com"]
    }
  }

}
resource "aws_iam_role" "ecs_task_role" {
  name               = "${var.project_name}-${var.environment_name}-Task-Role"
  assume_role_policy = data.aws_iam_policy_document.ecs_task_policy.json
  managed_policy_arns = [
    "arn:aws:iam::aws:policy/AmazonS3FullAccess",
    "arn:aws:iam::aws:policy/AmazonSESFullAccess", 
    "arn:aws:iam::aws:policy/CloudWatchFullAccess", 
    "arn:aws:iam::aws:policy/CloudWatchLogsFullAccess",
    "arn:aws:iam::aws:policy/AmazonDynamoDBFullAccess"
]
  tags = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)
}

#CICD role
data "aws_iam_policy_document" "deployment_policy" {
  statement {
    actions = ["kms:Decrypt"]
    resources = ["*"]
    effect = "Allow"
  }

  statement {
    actions = [
      "ecs:DescribeServices",
      "ecs:DescribeTaskDefinition",
      "ecs:DescribeTasks",
      "ecs:ListTasks",
      "ecs:RegisterTaskDefinition",
      "ecs:UpdateService"
    ]
    resources = ["*"]
    effect = "Allow"
  }

  statement {
    actions = ["iam:PassRole"]
    resources = ["*"]
    effect = "Allow"

    condition {
      test = "StringEqualsIfExists"
      values = [
        "ec2.amazonaws.com",
        "ecs-tasks.amazonaws.com"
      ]
      variable = "iam:PassedToService"
    }
  }

  statement {
    actions = [
      "s3:GetObject*",
      "s3:GetBucket*",
      "s3:List*"
    ]
    resources = [
      "arn:aws:s3:::${var.project_name}-pipeline",
      "arn:aws:s3:::${var.project_name}-pipeline/*"
    ]
    effect = "Allow"
  }

  statement {
    actions = [
      "kms:Decrypt",
      "kms:DescribeKey"
    ]
    resources = ["*"]
    effect = "Allow"
  }
   statement {
    actions   = ["logs:CreateLogStream", "logs:PutLogEvents"]
    resources = ["arn:aws:logs:*:*:log-group:/ecs/${var.project_name}-${var.environment_name}-service:*"]
     effect   = "Allow"
  }
   statement {
    actions   = ["logs:DescribeLogGroups"]
    resources = ["*"]
     effect   = "Allow"
  }
   statement {
    actions   = ["secretsmanager:Get*", "secretsmanager:Describe*", "secretsmanager:List*"]
    resources = ["arn:aws:secretsmanager:*:*:secret:/${var.project_name}/${var.environment_name}/*"]
     effect   = "Allow"
  }
}

resource "aws_iam_role" "deploy_role" {
  name               = "${var.project_name}-${var.environment_name}-DeployRole"
  assume_role_policy = jsonencode({
    Version   = "2012-10-17"
    Statement = [
      {
        Action    = "sts:AssumeRole"
        Effect    = "Allow"
        Principal = {
          "AWS": "arn:aws:iam::606199607275:root"
        }
      },
    ]
  })
  inline_policy {
    name   = "deploy-policy"
    policy = data.aws_iam_policy_document.deployment_policy.json
  }  
  tags     = merge({"ProjectName"= var.project_name},var.tags) 
}
