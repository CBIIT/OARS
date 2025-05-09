# data "aws_acm_certificate" "cancer_gov" {
#   domain      = "*.cancer.gov"
#   statuses    = ["ISSUED"]
#   types       = ["IMPORTED"]
#   most_recent = true
# }

module "oars-dev-alb" {
  source           = "../modules/oars-alb-tg"
  project_name     = var.oars_project_name
  environment_name = var.dev_environment_name
  vpc_id           = var.vpc_id
  subnet_ids       = var.lb_subnet_ids
  domain_name      = var.dev_domain_name
  tags             = var.tags
  port             = var.dev_alb_port
  certificate_arn = "arn:aws:acm:us-east-1:888577055755:certificate/020e8607-4ea0-4366-bcb3-cc672d1d36e8"
  
}
# resource "aws_lb_listener_certificate" "dev_oars_gov_cert" {
#   certificate_arn = var.dev_gov_certificate_arn
#   listener_arn    = var.dev_gov_listener_arn
#   depends_on = [ module.oars-dev-alb ]
# }