module "oars-qa-ecs" {
  source            = "../modules/oars-ecs"
  subnets           = var.ecs_subnet_ids
  tags              = var.tags_qa
  environment_name  = var.qa_environment_name
  aspnetcore_environment_name = var.qa_aspnetcore_environment_name
  oars_environment = var.qa_oars_environment  
  project_name      = var.oars_project_name
  task_def_cpu      = var.task_def_cpu
  task_def_memory   = var.task_def_memory
  desired_count     = var.desired_count
  vpc_id            = var.vpc_id
  load_balancer_sg  = module.oars-qa-alb.load_balancer_sg
  target_group_arn  = module.oars-qa-alb.target_group_arn
  task_def_host_port = var.qa_task_def_host_port
  task_def_container_port = var.qa_task_def_container_port
  load_balancer_container_port =  var.qa_load_balancer_container_port
}