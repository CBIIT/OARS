# module "oars-studydev-ecs" {
#   source            = "../modules/oars-ecs"
#   subnets           = var.ecs_subnet_ids
#   tags              = var.tags
#   environment_name  = var.studydev_environment_name
#   project_name      = var.oars_project_name
#   task_def_cpu      = var.task_def_cpu
#   task_def_memory   = var.task_def_memory
#   desired_count     = var.desired_count
#   vpc_id            = var.vpc_id
#   load_balancer_sg  = module.oars-studydev-alb.load_balancer_sg
#   target_group_arn  = module.oars-studydev-alb.target_group_arn
# }