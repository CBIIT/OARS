# module "oars-studydev-alb" {
#   source           = "../../../../modules/oars-alb-tg"
#   project_name     = var.oars_project_name
#   environment_name = var.studydev_environment_name
#   vpc_id           = var.vpc_id
#   subnet_ids       = var.lb_subnet_ids
#   domain_name      = var.studydev_domain_name
#   certificate_arn  = var.studydev_certificate_arn
#   tags             = var.tags
#   port             = var.studydev_alb_port
# }