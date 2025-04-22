module "oars-uat-alb" {
  source           = "../../../../../modules/oars-alb-tg"
  project_name     = var.oars_project_name
  environment_name = var.uat_environment_name
  vpc_id           = var.vpc_id
  subnet_ids       = var.lb_subnet_ids
  domain_name      = var.uat_domain_name
  tags             = var.tags
  port             = var.uat_alb_port
}
resource "aws_lb_listener_certificate" "oars_gov_cert" {
  certificate_arn = var.uat_gov_certificate_arn
  listener_arn    = var.uat_gov_listener_arn
  depends_on = [ module.oars-uat-alb]  
}

