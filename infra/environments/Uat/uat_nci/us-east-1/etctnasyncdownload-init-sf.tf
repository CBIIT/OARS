############################
##### Cloud Watch Log Group #####
############################
resource "aws_cloudwatch_log_group" "etctnasyncdownload-init-sf_cloudwatch_log_group" {
  name = "/sf/${var.etctnasyncdownload-init-sf_cloudwatch_group_name}"
  tags = {
    Environment = var.etctnasyncdownload_env
  }
  retention_in_days = var.etctnasyncdownload-sf_logs_retention_in_days
}

############################
##### Role/Policy #####
############################

resource "aws_iam_role" "etctnasyncdownload-init-sf_role" {
  name = "etctnasyncdownload-init-sf_role"

  assume_role_policy = <<EOF
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Principal": {
                "Service": "states.us-east-1.amazonaws.com"
            },
            "Action": "sts:AssumeRole"
        }
    ]
}
EOF
}

resource "aws_iam_policy" "etctnasyncdownload-init-sf_role_cloudwatchlogspolicy" {
  name = "etctnasyncdownload-init-sf_role_cloudwatchlogspolicy"

  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [
      {
        "Sid" : "CloudWatchLogsFullAccess",
        "Effect" : "Allow",
        "Action" : [
          "logs:*",
          "cloudwatch:GenerateQuery"
        ],
        "Resource" : "*"
      }
    ]
  })
}

resource "aws_iam_policy" "etctnasyncdownload-init-sf_role_lambdainvokepolicy" {
  name = "etctnasyncdownload-init-sf_role_lambdainvokepolicy"

  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [
      {
        "Effect" : "Allow",
        "Action" : [
          "lambda:InvokeFunction"
        ],
        "Resource" : [
          "${aws_lambda_function.etctnasyncdownload-submitjob_lambda_function.arn}:*",
          "${aws_lambda_function.etctnasyncdownload-polljob_lambda_function.arn}:*",
          "${aws_lambda_function.etctnasyncdownload-getdata_lambda_function.arn}:*"
        ]
      },
      {
        "Effect" : "Allow",
        "Action" : [
          "lambda:InvokeFunction"
        ],
        "Resource" : [
          "${aws_lambda_function.etctnasyncdownload-submitjob_lambda_function.arn}",
          "${aws_lambda_function.etctnasyncdownload-polljob_lambda_function.arn}",
          "${aws_lambda_function.etctnasyncdownload-getdata_lambda_function.arn}"
        ]
      }
    ]
  })
}

resource "aws_iam_policy" "etctnasyncdownload-init-sf_role_xrayaccesspolicy" {
  name = "etctnasyncdownload-init-sf_role_xrayaccesspolicy"

  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [
      {
        "Effect" : "Allow",
        "Action" : [
          "xray:PutTraceSegments",
          "xray:PutTelemetryRecords",
          "xray:GetSamplingRules",
          "xray:GetSamplingTargets"
        ],
        "Resource" : [
          "*"
        ]
      }
    ]
  })
}

resource "aws_iam_policy" "etctnasyncdownload-init-sf_role_sfinvokepolicy" {
  name = "etctnasyncdownload-init-sf_role_sfinvokepolicy"

  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [
      {
        "Effect" : "Allow",
        "Action" : [
          "states:StartExecution"
        ],
        "Resource" : [
          "${aws_sfn_state_machine.etctnasyncdownload-exec-sf_state_machine.arn}"
        ]
      }
    ]
  })
}

resource "aws_iam_policy" "etctnasyncdownload-init-sf_role_logpolicy" {
  name = "etctnasyncdownload-init-sf_role_logpolicy"

  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [
      {
        "Effect" : "Allow",
        "Action" : [
          "logs:CreateLogDelivery",
          "logs:CreateLogStream",
          "logs:GetLogDelivery",
          "logs:UpdateLogDelivery",
          "logs:DeleteLogDelivery",
          "logs:ListLogDeliveries",
          "logs:PutLogEvents",
          "logs:PutResourcePolicy",
          "logs:DescribeResourcePolicies",
          "logs:DescribeLogGroups",
          "logs:PutDestination",
          "logs:PutDestinationPolicy"
        ],
        "Resource" : "*"
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "etctnasyncdownload-init-sf_role_policy_attachment1" {
  role       = aws_iam_role.etctnasyncdownload-init-sf_role.name
  policy_arn = aws_iam_policy.etctnasyncdownload-init-sf_role_cloudwatchlogspolicy.arn
}

resource "aws_iam_role_policy_attachment" "etctnasyncdownload-init-sf_role_policy_attachment2" {
  role       = aws_iam_role.etctnasyncdownload-init-sf_role.name
  policy_arn = aws_iam_policy.etctnasyncdownload-init-sf_role_lambdainvokepolicy.arn
}

resource "aws_iam_role_policy_attachment" "etctnasyncdownload-init-sf_role_policy_attachment3" {
  role       = aws_iam_role.etctnasyncdownload-init-sf_role.name
  policy_arn = aws_iam_policy.etctnasyncdownload-init-sf_role_xrayaccesspolicy.arn
}

resource "aws_iam_role_policy_attachment" "etctnasyncdownload-init-sf_role_policy_attachment4" {
  role       = aws_iam_role.etctnasyncdownload-init-sf_role.name
  policy_arn = aws_iam_policy.etctnasyncdownload-init-sf_role_sfinvokepolicy.arn
}

resource "aws_iam_role_policy_attachment" "etctnasyncdownload-init-sf_role_policy_attachment5" {
  role       = aws_iam_role.etctnasyncdownload-init-sf_role.name
  policy_arn = aws_iam_policy.etctnasyncdownload-init-sf_role_logpolicy.arn
}

############################
##### State Machine #####
############################

resource "aws_sfn_state_machine" "etctnasyncdownload-init-sf_state_machine" {
  name     = "etctnasyncdownload-initiate"
  role_arn = aws_iam_role.etctnasyncdownload-init-sf_role.arn

  definition = <<EOF
{
  "Comment": "ETCTN Async Download Initiate",
  "StartAt": "Map",
  "States": {
    "Map": {
      "Type": "Map",
      "ItemProcessor": {
        "ProcessorConfig": {
          "Mode": "INLINE"
        },
        "StartAt": "Step Functions StartExecution",
        "States": {
          "Step Functions StartExecution": {
            "Type": "Task",
            "Resource": "arn:aws:states:::states:startExecution",
            "Parameters": {
              "StateMachineArn": "${aws_sfn_state_machine.etctnasyncdownload-exec-sf_state_machine.arn}",
              "Input": {
                "form_oid.$": "$.form_oid",
                "query_id.$": "$.query_id",                
                "protocol.$": "$.protocol",
                "AWS_STEP_FUNCTIONS_STARTED_BY_EXECUTION_ID.$": "$$.Execution.Id"
              }
            },
            "End": true
          }
        }
      },
      "End": true,
      "ItemsPath": "$.protocols",
      "ItemSelector": {
        "protocol.$": "$$.Map.Item.Value",
        "form_oid.$": "$.form_oid",
        "query_id.$": "$.query_id"
      }
    }
  },
  "TimeoutSeconds": 1200
}
EOF

  logging_configuration {
    log_destination        = "${aws_cloudwatch_log_group.etctnasyncdownload-init-sf_cloudwatch_log_group.arn}:*"
    include_execution_data = true
    level                  = "ALL"
  }
}