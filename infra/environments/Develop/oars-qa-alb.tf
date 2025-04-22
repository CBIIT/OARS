module "oars-qa-alb" {
  source           = "../../../../../modules/oars-alb-tg"
  project_name     = var.oars_project_name
  environment_name = var.qa_environment_name
  vpc_id           = var.vpc_id
  subnet_ids       = var.lb_subnet_ids
  domain_name      = var.qa_domain_name
  tags             = var.tags_qa
  port             = var.qa_alb_port
}
resource "aws_lb_listener_certificate" "qa_oars_gov_cert" {
  certificate_arn = var.qa_gov_certificate_arn
  listener_arn    = var.qa_gov_listener_arn
  depends_on = [ module.oars-qa-alb ]
}