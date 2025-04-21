resource "aws_cloudwatch_log_group" "container_build_logs" {
  name              = "${var.project_name}-ContainerBuildLogs"
  retention_in_days = 7
  tags              = merge({"ProjectName"= var.project_name},var.tags)
}

resource "aws_cloudwatch_log_group" "ecr_push_logs1" {
  name              = "${var.project_name}-dev-EcrPushLogs"
  retention_in_days = 7
  tags              = merge({"ProjectName"= var.project_name},var.tags)
}

resource "aws_cloudwatch_log_group" "ecr_push_logs" {
  for_each = var.environment
  name              = "${var.project_name}-${each.key}-EcrPushLogs"
  retention_in_days = 7
  tags              = merge({"ProjectName"= var.project_name},var.tags)
}