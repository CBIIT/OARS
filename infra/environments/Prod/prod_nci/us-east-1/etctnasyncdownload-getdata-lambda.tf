###############################
##### Create etctnasyncdownload-getdataLambda in Production Environment #####
###############################

resource "aws_iam_role" "etctnasyncdownload-getdata_lambda_role" {
  name = "etctnasyncdownload-getdata_lambda_executionrole"

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

resource "aws_iam_policy" "etctnasyncdownload-getdata_lambda_ssmpolicy" {
  name = "etctnasyncdownload-getdata_lambda_ssmpolicy"

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

resource "aws_iam_policy" "etctnasyncdownload-getdata_lambda_sespolicy" {
  name = "etctnasyncdownload-getdata_lambda_sespolicy"

  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [
      {
        "Effect" : "Allow",
        "Action" : [
          "ses:SendEmail",
          "ses:SendRawEmail"
        ],
        "Resource" : "*"
      }
    ]
  })
}

resource "aws_iam_policy" "etctnasyncdownload-getdata_lambda_s3policy" {
  name = "etctnasyncdownload-getdata_lambda_s3policy"

  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [
      {
        "Sid" : "ETCTNAsyncDownloadGetDataAllowS3Access",
        "Effect" : "Allow",
        "Action" : [
          "s3:PutObject",
          "s3:GetObject",
          "s3:AbortMultipartUpload",
          "s3:ListBucket",
          "s3:DeleteObject",
          "s3:GetObjectVersion",
          "s3:ListMultipartUploadParts"
        ],
        "Resource" : [
          "arn:aws:s3:::${var.etctnasyncdownload_files_s3_bucket}/*",
          "arn:aws:s3:::${var.etctnasyncdownload_files_s3_bucket}"
        ]
      }
    ]
  })
}

resource "aws_iam_policy" "etctnasyncdownload-getdata_lambda_kmspolicy" {
  name = "etctnasyncdownload-getdata_lambda_kmspolicy"

  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [
      {
        "Effect" : "Allow",
        "Action" : [
          "kms:Decrypt",
          "kms:Encrypt",
          "kms:ReEncrypt*",
          "kms:GenerateDataKey*"
        ],
        "Resource" : module.kms.s3_key_arn
      }
    ]
  })
}

resource "aws_iam_policy" "etctnasyncdownload-getdata_lambda_policy" {

  name        = "etctnasyncdownload-getdata_lambda_policy"
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

resource "aws_iam_role_policy_attachment" "etctnasyncdownload-getdata_lambda_execution_role_policy_attachment1" {
  role       = aws_iam_role.etctnasyncdownload-getdata_lambda_role.name
  policy_arn = aws_iam_policy.etctnasyncdownload-getdata_lambda_policy.arn
}

resource "aws_iam_role_policy_attachment" "etctnasyncdownload-getdata_lambda_execution_role_policy_attachment2" {
  role       = aws_iam_role.etctnasyncdownload-getdata_lambda_role.name
  policy_arn = aws_iam_policy.etctnasyncdownload-getdata_lambda_ssmpolicy.arn
}

resource "aws_iam_role_policy_attachment" "etctnasyncdownload-getdata_lambda_execution_role_policy_attachment3" {
  role       = aws_iam_role.etctnasyncdownload-getdata_lambda_role.name
  policy_arn = aws_iam_policy.etctnasyncdownload-getdata_lambda_s3policy.arn
}

resource "aws_iam_role_policy_attachment" "etctnasyncdownload-getdata_lambda_execution_role_policy_attachment4" {
  role       = aws_iam_role.etctnasyncdownload-getdata_lambda_role.name
  policy_arn = aws_iam_policy.etctnasyncdownload-getdata_lambda_sespolicy.arn
}

resource "aws_iam_role_policy_attachment" "etctnasyncdownload-getdata_lambda_execution_role_policy_attachment5" {
  role       = aws_iam_role.etctnasyncdownload-getdata_lambda_role.name
  policy_arn = aws_iam_policy.etctnasyncdownload-getdata_lambda_kmspolicy.arn
}

resource "aws_lambda_function" "etctnasyncdownload-getdata_lambda_function" {
  function_name = "etctnasyncdownload-getdata"
  role          = aws_iam_role.etctnasyncdownload-getdata_lambda_role.arn
  memory_size   = 256
  timeout       = 300
  architectures = ["x86_64"]
  package_type  = "Image"
  image_uri     = "${var.aws_accounts.theradex-shared-service.account_id}.dkr.ecr.us-east-1.amazonaws.com/prod-etctnasyncdownload-getdata-lambda:latest"
  environment {
    variables = {
      ASPNETCORE_ENVIRONMENT = "Production Non-Commercial"
      IntegrationEnvironment = var.etctnasyncdownload_env
    }
  }
}