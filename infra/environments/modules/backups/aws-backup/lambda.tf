locals {
  expire_name = "${var.name}-expired-recovery-point-cleanup"
}

resource "aws_cloudwatch_event_rule" "expired_cleanup" {
  name                = local.expire_name
  description         = "Daily expired aws backup recovery point cleanup"
  schedule_expression = "cron(30 7 * * ? *)"
}

resource "aws_cloudwatch_event_target" "expired_cleanup" {
  target_id = local.expire_name
  arn       = aws_lambda_function.expired_cleanup.arn
  rule      = aws_cloudwatch_event_rule.expired_cleanup.name
}

resource "aws_lambda_permission" "expired_cleanup" {
  statement_id   = "AllowExecutionFromCloudWatchEvents"
  action         = "lambda:InvokeFunction"
  function_name  = local.expire_name
  principal      = "events.amazonaws.com"
  source_arn     = aws_cloudwatch_event_rule.expired_cleanup.arn
}


resource "aws_cloudwatch_log_group" "expired_cleanup" {
  name              = "/aws/lambda/${local.expire_name}"
  retention_in_days = 180

  tags = var.tags
}

resource "aws_lambda_function" "expired_cleanup" {
  filename      = "${path.module}/code/removeExpiredRecoveryPointsLambda.zip"
  function_name = local.expire_name
  role          = aws_iam_role.cleanup.arn
  handler       = "removeExpiredRecoveryPointsLambda.lambda_handler_remove_expired"

  # The filebase64sha256() function is available in Terraform 0.11.12 and later
  # For Terraform 0.11.11 and earlier, use the base64sha256() function and the file() function:
  # source_code_hash = "${base64sha256(file("lambda_function_payload.zip"))}"
  source_code_hash = filebase64sha256("${path.module}/code/removeExpiredRecoveryPointsLambda.zip")

  runtime = "python3.11"
  timeout = 300

  environment {
    variables = {
      BACKUP_VAULT_NAME   = var.name
    }
  }

  tags = var.tags
}

resource "aws_cloudwatch_log_group" "all_cleanup" {
  name              = "/aws/lambda/${var.name}-removal-all-recovery-points"
  retention_in_days = 180

  tags = var.tags
}

resource "aws_lambda_function" "all_cleanup" {
  filename      = "${path.module}/code/removeAllRecoveryPointsLambda.zip"
  function_name = "${var.name}-removal-all-recovery-points"
  role          = aws_iam_role.cleanup.arn
  handler       = "removeAllRecoveryPointsLambda.lambda_handler_remove_all"

  # The filebase64sha256() function is available in Terraform 0.11.12 and later
  # For Terraform 0.11.11 and earlier, use the base64sha256() function and the file() function:
  # source_code_hash = "${base64sha256(file("lambda_function_payload.zip"))}"
  source_code_hash = filebase64sha256("${path.module}/code/removeAllRecoveryPointsLambda.zip")

  runtime = "python3.11"
  timeout = 300

  tags        = var.tags
}
