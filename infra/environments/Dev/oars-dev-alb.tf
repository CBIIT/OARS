module "oars-dev-alb" {
  source           = "../modules/oars-alb-tg"
  project_name     = var.oars_project_name
  environment_name = var.dev_environment_name
  vpc_id           = var.vpc_id
  subnet_ids       = var.lb_subnet_ids
  domain_name      = var.dev_domain_name
  tags             = var.tags
  port             = var.dev_alb_port
  providers = {
    aws.us-east-1 = aws.us-east-1
  }
}
# resource "aws_lb_listener_certificate" "dev_oars_gov_cert" {
#   certificate_arn = var.dev_gov_certificate_arn
#   listener_arn    = var.dev_gov_listener_arn
#   depends_on = [ module.oars-dev-alb ]
# }