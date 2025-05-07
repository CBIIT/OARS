#Target group for Aplication LoadBalancer and its attachment
resource "aws_lb_target_group" "tg1" {
  name        = "${var.project_name}-${var.environment_name}-tg"
  port        = var.port
  protocol    = "HTTP"
  vpc_id      = var.vpc_id
  target_type = "ip"
  tags        = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)
  # lifecycle {
  #   create_before_destroy = true
  # }
}