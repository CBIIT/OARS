############################
##### Cloud Watch Log Group #####
############################
resource "aws_cloudwatch_log_group" "etctnasyncdownload-exec-sf_cloudwatch_log_group" {
  name = "/sf/${var.etctnasyncdownload-exec-sf_cloudwatch_group_name}"
  tags = {
    Environment = var.etctnasyncdownload_env
  }
  retention_in_days = var.etctnasyncdownload-sf_logs_retention_in_days
}

############################
##### Role/Policy #####
############################

resource "aws_iam_role" "etctnasyncdownload-exec-sf_role" {
  name = "etctnasyncdownload-exec-sf_role"

  assume_role_policy = <<EOF
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Principal": {
                "Service": "states.amazonaws.com"
            },
            "Action": "sts:AssumeRole"
        }
    ]
}
EOF
}

resource "aws_iam_policy" "etctnasyncdownload-exec-sf_role_lambdainvokepolicy" {
  name = "etctnasyncdownload-exec-sf_role_lambdainvokepolicy"

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

resource "aws_iam_policy" "etctnasyncdownload-exec-sf_role_xrayaccesspolicy" {
  name = "etctnasyncdownload-exec-sf_role_xrayaccesspolicy"

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

resource "aws_iam_policy" "etctnasyncdownload-exec-sf_role_logpolicy" {
  name = "etctnasyncdownload-exec-sf_role_logpolicy"

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

resource "aws_iam_role_policy_attachment" "etctnasyncdownload-exec-sf_role_policy_attachment1" {
  role       = aws_iam_role.etctnasyncdownload-exec-sf_role.name
  policy_arn = aws_iam_policy.etctnasyncdownload-exec-sf_role_lambdainvokepolicy.arn
}

resource "aws_iam_role_policy_attachment" "etctnasyncdownload-exec-sf_role_policy_attachment2" {
  role       = aws_iam_role.etctnasyncdownload-exec-sf_role.name
  policy_arn = aws_iam_policy.etctnasyncdownload-exec-sf_role_xrayaccesspolicy.arn
}

resource "aws_iam_role_policy_attachment" "etctnasyncdownload-exec-sf_role_policy_attachment3" {
  role       = aws_iam_role.etctnasyncdownload-exec-sf_role.name
  policy_arn = aws_iam_policy.etctnasyncdownload-exec-sf_role_logpolicy.arn
}

############################
##### State Machine #####
############################

resource "aws_sfn_state_machine" "etctnasyncdownload-exec-sf_state_machine" {
  name     = "etctnasyncdownload-execute"
  role_arn = aws_iam_role.etctnasyncdownload-exec-sf_role.arn

  definition = <<EOF
{
  "Comment": "ETCTN Async Download Execute",
  "StartAt": "SubmitJob",
  "States": {
    "SubmitJob": {
      "Type": "Task",
      "Resource": "${aws_lambda_function.etctnasyncdownload-submitjob_lambda_function.arn}",
      "ResultPath": "$.submit_job",
      "Next": "Wait",
      "Retry": [
        {
          "ErrorEquals": [
            "States.ALL"
          ],
          "IntervalSeconds": 1,
          "MaxAttempts": 3,
          "BackoffRate": 2
        }
      ]
    },
    "Wait": {
      "Type": "Wait",
      "Next": "PollJobStatus",
      "Seconds": 30
    },
    "PollJobStatus": {
      "Type": "Task",
      "Resource": "${aws_lambda_function.etctnasyncdownload-polljob_lambda_function.arn}",
      "Next": "IsJobComplete?",
      "ResultPath": "$.job_status",
      "Retry": [
        {
          "ErrorEquals": [
            "States.ALL"
          ],
          "IntervalSeconds": 1,
          "MaxAttempts": 3,
          "BackoffRate": 2
        }
      ]
    },
    "IsJobComplete?": {
      "Type": "Choice",
      "Choices": [
        {
          "Variable": "$.job_status.poll_job_status",
          "StringEquals": "FAILED",
          "Next": "JobFailed"
        },
        {
          "Variable": "$.job_status.poll_job_status",
          "StringEquals": "SUCCESS",
          "Next": "GetData"
        },
        {
          "Variable": "$.job_status.is_running_long_time",
          "BooleanEquals": true,
          "Next": "JobTimeout"
        }
      ],
      "Default": "Wait"
    },
    "JobTimeout": {
      "Type": "Fail"
    },
    "GetData": {
      "Type": "Task",
      "Resource": "${aws_lambda_function.etctnasyncdownload-getdata_lambda_function.arn}",
      "Retry": [
        {
          "ErrorEquals": [
            "States.ALL"
          ],
          "IntervalSeconds": 1,
          "MaxAttempts": 3,
          "BackoffRate": 2
        }
      ],
      "End": true
    },
    "JobFailed": {
      "Type": "Fail",
      "Cause": "AWS Batch Job Failed",
      "Error": "DescribeJob returned FAILED"
    }
  },
  "TimeoutSeconds": 1200
}
EOF

  logging_configuration {
    log_destination        = "${aws_cloudwatch_log_group.etctnasyncdownload-exec-sf_cloudwatch_log_group.arn}:*"
    include_execution_data = true
    level                  = "ALL"
  }
}