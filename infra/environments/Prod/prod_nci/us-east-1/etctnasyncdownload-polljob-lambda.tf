###############################
##### Create etctnasyncdownload-polljobLambda in Production Environment #####
###############################

resource "aws_iam_role" "etctnasyncdownload-polljob_lambda_role" {
  name = "etctnasyncdownload-polljob_lambda_executionrole"

  assume_role_policy = <<EOF
{
 "Version": "2012-10-17",
 "Statement": [
   {
     "Action": "sts:AssumeRole",
     "Principal": {
       "Service": "lambda.amazonaws.com"
     },
     "Effect": "Allow",
     "Sid": ""
   }
 ]
}
EOF
}

resource "aws_iam_policy" "etctnasyncdownload-polljob_lambda_ssmpolicy" {
  name = "etctnasyncdownload-polljob_lambda_ssmpolicy"

  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [
      {
        "Effect" : "Allow",
        "Action" : [
          "ssm:GetParametersByPath"
        ],
        "Resource" : [
          "*"
        ]
      }
    ]
  })
}

resource "aws_iam_policy" "etctnasyncdownload-polljob_lambda_policy" {

  name        = "etctnasyncdownload-polljob_lambda_policy"
  path        = "/"
  description = "AWS IAM Policy for managing aws lambda role"
  policy      = <<EOF
{
 "Version": "2012-10-17",
 "Statement": [
   {
     "Action": [
       "logs:CreateLogGroup",
       "logs:CreateLogStream",
       "logs:PutLogEvents"
     ],
     "Resource": "arn:aws:logs:*:*:*",
     "Effect": "Allow"
   }
 ]
}
EOF
}

resource "aws_iam_role_policy_attachment" "etctnasyncdownload-polljob_lambda_execution_role_policy_attachment1" {
  role       = aws_iam_role.etctnasyncdownload-polljob_lambda_role.name
  policy_arn = aws_iam_policy.etctnasyncdownload-polljob_lambda_policy.arn
}

resource "aws_iam_role_policy_attachment" "etctnasyncdownload-polljob_lambda_execution_role_policy_attachment2" {
  role       = aws_iam_role.etctnasyncdownload-polljob_lambda_role.name
  policy_arn = aws_iam_policy.etctnasyncdownload-polljob_lambda_ssmpolicy.arn
}

resource "aws_lambda_function" "etctnasyncdownload-polljob_lambda_function" {
  function_name = "etctnasyncdownload-polljob"
  role          = aws_iam_role.etctnasyncdownload-polljob_lambda_role.arn
  memory_size   = 256
  timeout       = 300
  architectures = ["x86_64"]
  package_type  = "Image"
  image_uri     = "${var.aws_accounts.theradex-shared-service.account_id}.dkr.ecr.us-east-1.amazonaws.com/prod-etctnasyncdownload-polljob-lambda:latest"
  environment {
    variables = {
      ASPNETCORE_ENVIRONMENT = "Production Non-Commercial"
      IntegrationEnvironment = var.etctnasyncdownload_env
    }
  }
}