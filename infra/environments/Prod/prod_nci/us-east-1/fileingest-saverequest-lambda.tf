###############################
##### Create FileIngest-SaveRequest in PROD Environment #####
###############################

resource "aws_iam_role" "fileingest-saverequest_lambda_role" {
  name = "fileingest-saverequest_lambda_role"

  assume_role_policy = <<EOF
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
EOF
}

resource "aws_iam_policy" "fileingest-saverequest_lambda_ssmpolicy" {
  name = "fileingest-saverequest_lambda_ssmpolicy"

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

resource "aws_iam_policy" "fileingest-saverequest_lambda_s3policy" {
  name = "fileingest-saverequest_lambda_s3policy"

  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [
      {
        "Sid" : "FileingestSaverequestLambdaAllowS3Access",
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
          "arn:aws:s3:::${var.fileingest_files_s3_bucket}/*",
          "arn:aws:s3:::${var.fileingest_files_s3_bucket}"
        ]
      }
    ]
  })
}


resource "aws_iam_policy" "fileingest-saverequest_lambda_policy" {

  name        = "fileingest-saverequest_lambda_policy"
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

resource "aws_iam_role_policy_attachment" "fileingest-saverequest_lambda_execution_role_policy_attachment1" {
  role       = aws_iam_role.fileingest-saverequest_lambda_role.name
  policy_arn = aws_iam_policy.fileingest-saverequest_lambda_policy.arn
}

resource "aws_iam_role_policy_attachment" "fileingest-saverequest_lambda_execution_role_policy_attachment2" {
  role       = aws_iam_role.fileingest-saverequest_lambda_role.name
  policy_arn = aws_iam_policy.fileingest-saverequest_lambda_ssmpolicy.arn
}

resource "aws_iam_role_policy_attachment" "fileingest-saverequest_lambda_execution_role_policy_attachment3" {
  role       = aws_iam_role.fileingest-saverequest_lambda_role.name
  policy_arn = aws_iam_policy.fileingest-saverequest_lambda_s3policy.arn
}

resource "aws_iam_role_policy_attachment" "fileingest-saverequest_lambda_execution_role_policy_attachment4" {
  role       = aws_iam_role.fileingest-saverequest_lambda_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonDynamoDBFullAccess"
}

resource "aws_lambda_function" "fileingest-saverequest_lambda_function" {
  function_name = "fileingest-saverequest"
  role          = aws_iam_role.fileingest-saverequest_lambda_role.arn
  memory_size   = 256
  timeout       = 300 # 5min
  architectures = ["x86_64"]
  package_type  = "Image"
  image_uri     = "${var.aws_accounts.theradex-shared-service.account_id}.dkr.ecr.us-east-1.amazonaws.com/prod-fileingesttomedidata-saverequest-lambda:latest"
  environment {
    variables = {
      ASPNETCORE_ENVIRONMENT = "Production Non-Commercial"
      IntegrationEnvironment = var.fileingest_env
    }
  }
}

# Adding S3 bucket as trigger to the lambda and give the permissions
resource "aws_s3_bucket_notification" "fileingest-saverequest_lambda-s3-trigger_notification" {
  bucket = var.fileingest_files_s3_bucket
  lambda_function {
    lambda_function_arn = aws_lambda_function.fileingest-saverequest_lambda_function.arn
    events              = ["s3:ObjectCreated:Put", "s3:ObjectCreated:Post", "s3:ObjectCreated:Copy"] # s3:ObjectCreated:*"
    filter_prefix       = "Metadata/"
    filter_suffix       = ".json"
  }
}

resource "aws_lambda_permission" "fileingest-saverequest_lambda-s3-trigger_allow_bucket" {
  statement_id  = "AllowS3Invoke"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.fileingest-saverequest_lambda_function.function_name
  principal     = "s3.amazonaws.com"
  source_arn    = "arn:aws:s3:::${var.fileingest_files_s3_bucket}"
}

# resource "aws_lambda_function_event_invoke_config" "fileingest-saverequest_lambda_async invocation_config" {
#   function_name = aws_lambda_function.fileingest-saverequest_lambda_function.function_name
#   maximum_event_age_in_seconds = "21600" //6 hours
#   maximum_retry_attempts = 2 //default 2

#   depends_on = [
#     aws_lambda_function.lambda-deploy
#   ]
# }