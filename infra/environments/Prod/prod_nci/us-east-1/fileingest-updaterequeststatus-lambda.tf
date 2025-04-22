###############################
##### Create FileIngest-UpateRequestStatus in PROD Environment #####
###############################

resource "aws_iam_role" "fileingest-updaterequeststatuslambda_role" {
  name = "fileingest-updaterequeststatuslambda_role"

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

resource "aws_iam_policy" "fileingest-updaterequeststatuslambda_ssmpolicy" {
  name = "fileingest-updaterequeststatuslambda_ssmpolicy"

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

resource "aws_iam_policy" "fileingest-updaterequeststatuslambda_sqspolicy" {
  name = "fileingest-updaterequeststatuslambda_sqspolicy"

  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [
      {
        "Effect" : "Allow",
        "Action" : [
          "sqs:SendMessage",
          "sqs:ReceiveMessage",
          "sqs:DeleteMessage",
          "sqs:GetQueueAttributes"
        ],
        "Resource" : [
          aws_sqs_queue.fileingest-requestreconcile-queue.arn
        ]
      }
    ]
  })
}


resource "aws_iam_policy" "fileingest-updaterequeststatuslambda_policy" {

  name        = "fileingest-updaterequeststatuslambda_policy"
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

resource "aws_iam_role_policy_attachment" "fileingest-updaterequeststatuslambda_execution_role_policy_attachment1" {
  role       = aws_iam_role.fileingest-updaterequeststatuslambda_role.name
  policy_arn = aws_iam_policy.fileingest-updaterequeststatuslambda_policy.arn
}

resource "aws_iam_role_policy_attachment" "fileingest-updaterequeststatuslambda_execution_role_policy_attachment2" {
  role       = aws_iam_role.fileingest-updaterequeststatuslambda_role.name
  policy_arn = aws_iam_policy.fileingest-updaterequeststatuslambda_ssmpolicy.arn
}

resource "aws_iam_role_policy_attachment" "fileingest-updaterequeststatuslambda_execution_role_policy_attachment3" {
  role       = aws_iam_role.fileingest-updaterequeststatuslambda_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonDynamoDBFullAccess"
}

resource "aws_iam_role_policy_attachment" "fileingest-updaterequeststatuslambda_execution_role_policy_attachment4" {
  role       = aws_iam_role.fileingest-updaterequeststatuslambda_role.name
  policy_arn = aws_iam_policy.fileingest-updaterequeststatuslambda_sqspolicy.arn
}


resource "aws_lambda_function" "fileingest-updaterequeststatus_lambda_function" {
  function_name = "fileingest-updaterequeststatus"
  role          = aws_iam_role.fileingest-updaterequeststatuslambda_role.arn
  memory_size   = 256
  timeout       = 600 # 10min
  architectures = ["x86_64"]
  package_type  = "Image"
  image_uri     = "${var.aws_accounts.theradex-shared-service.account_id}.dkr.ecr.us-east-1.amazonaws.com/prod-fileingesttomedidata-updaterequeststatus-lambda:latest"
  environment {
    variables = {
      ASPNETCORE_ENVIRONMENT = "Production Non-Commercial"
      IntegrationEnvironment = var.fileingest_env
    }
  }
}

# Adding SQS as trigger to the lambda and give the permissions
resource "aws_lambda_event_source_mapping" "fileingest-updaterequeststatus_lambda-event_source_mapping" {
  event_source_arn        = aws_sqs_queue.fileingest-requestreconcile-queue.arn
  enabled                 = true
  function_name           = aws_lambda_function.fileingest-updaterequeststatus_lambda_function.function_name
  batch_size              = 1
  function_response_types = ["ReportBatchItemFailures"]
}