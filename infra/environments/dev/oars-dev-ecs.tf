module "oars-dev-ecs" {
  source            = "../../../../../modules/oars-ecs"
  subnets           = var.ecs_subnet_ids
  tags              = var.tags
  environment_name = var.dev_environment_name
  aspnetcore_environment_name = var.dev_aspnetcore_environment_name
  oars_environment = var.dev_oars_environment
  project_name      = var.oars_project_name
  task_def_cpu      = var.task_def_cpu
  task_def_memory   = var.task_def_memory
  desired_count     = var.desired_count
  vpc_id            = var.vpc_id
  load_balancer_sg  = module.oars-dev-alb.load_balancer_sg
  target_group_arn  = module.oars-dev-alb.target_group_arn
  task_def_host_port = var.dev_task_def_host_port
  task_def_container_port = var.dev_task_def_container_port
  load_balancer_container_port =  var.dev_load_balancer_container_port
}