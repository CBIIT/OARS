#Log groups for ECS
resource "aws_cloudwatch_log_group" "service" {
  name              = "/ecs/${var.project_name}-${var.environment_name}-service"
  retention_in_days = 731
  kms_key_id        = aws_kms_key.logs_key.arn
  tags              = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)
}