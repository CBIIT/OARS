###############################
##### Create FileIngest-ValidateAndSaveRequestItems in UAT Environment #####
###############################

resource "aws_iam_role" "fileingest-validateandsaverequestitems_lambda_role" {
  name = "fileingest-validateandsaverequestitems_lambda_role"

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

resource "aws_iam_policy" "fileingest-validateandsaverequestitems_lambda_ssmpolicy" {
  name = "fileingest-validateandsaverequestitems_lambda_ssmpolicy"

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

resource "aws_iam_policy" "fileingest-validateandsaverequestitems_lambda_s3policy" {
  name = "fileingest-validateandsaverequestitems_lambda_s3policy"

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


resource "aws_iam_policy" "fileingest-validateandsaverequestitems_lambda_policy" {

  name        = "fileingest-validateandsaverequestitems_lambda_policy"
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

resource "aws_iam_role_policy_attachment" "fileingest-validateandsaverequestitems_lambda_execution_role_policy_attachment1" {
  role       = aws_iam_role.fileingest-validateandsaverequestitems_lambda_role.name
  policy_arn = aws_iam_policy.fileingest-validateandsaverequestitems_lambda_policy.arn
}

resource "aws_iam_role_policy_attachment" "fileingest-validateandsaverequestitems_lambda_execution_role_policy_attachment2" {
  role       = aws_iam_role.fileingest-validateandsaverequestitems_lambda_role.name
  policy_arn = aws_iam_policy.fileingest-validateandsaverequestitems_lambda_ssmpolicy.arn
}

resource "aws_iam_role_policy_attachment" "fileingest-validateandsaverequestitems_lambda_execution_role_policy_attachment3" {
  role       = aws_iam_role.fileingest-validateandsaverequestitems_lambda_role.name
  policy_arn = aws_iam_policy.fileingest-validateandsaverequestitems_lambda_s3policy.arn
}

resource "aws_iam_role_policy_attachment" "fileingest-validateandsaverequestitems_lambda_execution_role_policy_attachment4" {
  role       = aws_iam_role.fileingest-validateandsaverequestitems_lambda_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonDynamoDBFullAccess"
}

resource "aws_lambda_function" "fileingest-validateandsaverequestitems_lambda_function" {
  function_name = "fileingest-validateandsaverequestitems"
  role          = aws_iam_role.fileingest-validateandsaverequestitems_lambda_role.arn
  memory_size   = 256
  timeout       = 600 # 10min
  architectures = ["x86_64"]
  package_type  = "Image"
  image_uri     = "${var.aws_accounts.theradex-shared-service.account_id}.dkr.ecr.us-east-1.amazonaws.com/uat-fileingesttomedidata-validateandsave-lambda:latest"
  environment {
    variables = {
      ASPNETCORE_ENVIRONMENT = "UAT Non-Commercial"
      IntegrationEnvironment = var.fileingest_env
    }
  }
}

# Adding DynamoDB streams as trigger to the lambda and give the permissions
resource "aws_lambda_event_source_mapping" "fileingest-saverequest_lambda-event_source_mapping" {
  event_source_arn       = aws_dynamodb_table.FileIngestRequest-dynamodb-table.stream_arn
  function_name          = aws_lambda_function.fileingest-validateandsaverequestitems_lambda_function.function_name
  starting_position      = "LATEST"
  batch_size             = 1
  maximum_retry_attempts = 3
}

# resource "aws_iam_role" "lambda_assume_role" {
#   name               = "lambda-dynamodb-role"
#   assume_role_policy = <<EOF
# {
#   "Version": "2012-10-17",
#   "Statement": [
#     {
#       "Action": "sts:AssumeRole",
#       "Principal": {
#         "Service": "lambda.amazonaws.com"
#       },
#       "Effect": "Allow",
#       "Sid": "LambdaAssumeRole"
#     }
#   ]
# }
# EOF
# }

# resource "aws_iam_role_policy" "dynamodb_read_log_policy" {
#   name   = "lambda-dynamodb-log-policy"
#   role   = aws_iam_role.lambda_assume_role.id
#   policy = <<EOF
# {
#   "Version": "2012-10-17",
#   "Statement": [
#     {
#         "Action": [ "logs:*" ],
#         "Effect": "Allow",
#         "Resource": [ "arn:aws:logs:*:*:*" ]
#     },
#     {
#         "Action": [ "dynamodb:BatchGetItem",
#                     "dynamodb:GetItem",
#                     "dynamodb:GetRecords",
#                     "dynamodb:Scan", We will have the recores inside of the lambda function in event `object`. We can also configure the stream to capture additional data such as "before" and "after" images of modified items.
#                     "dynamodb:Query",
#                     "dynamodb:GetShardIterator",
#                     "dynamodb:DescribeStream",
#                     "dynamodb:ListStreams" ],
#         "Effect": "Allow",
#         "Resource": [
#           "${aws_dynamodb_table.FileIngestRequest-dynamodb-table.arn}",
#           "${aws_dynamodb_table.FileIngestRequest-dynamodb-table.arn}/*"
#         ]
#     }
#   ]
# }
# EOF
# }