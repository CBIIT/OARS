
#Application LoadBalancer with its listeners
resource "aws_lb" "oars_alb" {
  name               = "${var.project_name}-${var.environment_name}-lb"
  internal           = false
  load_balancer_type = "application"
  subnets            = var.subnet_ids
  security_groups    = [aws_security_group.alb_sg.id]
  tags               = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)
  lifecycle {
    prevent_destroy = true
  }
}

resource "aws_lb_listener" "http" {
  load_balancer_arn = aws_lb.oars_alb.arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type = "redirect"

    redirect {
      port        = 443
      protocol    = "HTTPS"
      status_code = "HTTP_301"
    }
  }
  tags            = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)
  lifecycle {
    replace_triggered_by = [ aws_lb_target_group.tg1 ]
  }
}

resource "aws_lb_listener" "https" {
  load_balancer_arn = aws_lb.oars_alb.arn
  port              = 443
  protocol          = "HTTPS"
  ssl_policy        = "ELBSecurityPolicy-2016-08"
  certificate_arn   = aws_acm_certificate.oars_cert.arn
  
  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.tg1.arn
  }
  tags              = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)
  lifecycle {
    replace_triggered_by = [ aws_lb_target_group.tg1 ]
  }
}

