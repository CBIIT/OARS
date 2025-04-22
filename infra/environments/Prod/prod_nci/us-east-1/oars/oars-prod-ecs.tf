module "oars-prod-ecs" {
  source                       = "../../../../../modules/oars-ecs"
  subnets                      = var.ecs_subnet_ids
  tags                         = var.tags
  environment_name             = var.production_environment_name
  aspnetcore_environment_name  = var.production_aspnetcore_environment_name
  oars_environment             = var.production_oars_environment
  project_name                 = var.oars_project_name
  task_def_cpu                 = var.task_def_cpu
  task_def_memory              = var.task_def_memory
  desired_count                = var.desired_count
  vpc_id                       = var.vpc_id
  load_balancer_sg             = module.oars-prod-alb.load_balancer_sg
  target_group_arn             = module.oars-prod-alb.target_group_arn
  task_def_host_port           = var.production_task_def_host_port
  task_def_container_port      = var.production_task_def_container_port
  load_balancer_container_port = var.production_load_balancer_container_port
}

resource "aws_appautoscaling_target" "ecs_target" {
  max_capacity       = 5
  min_capacity       = 1
  resource_id        = var.ecs_service_resource_id
  role_arn           = var.ecs_autoscaling_role_arn
  scalable_dimension = var.ecs_scalable_dimension
  service_namespace  = var.ecs_service_namespace
  // ... other configurations ...
}

resource "aws_appautoscaling_policy" "memory_policy_oars_ecs" {
  name               = "${var.ecs_service_name}-memory-appautoscaling-target"
  policy_type        = "TargetTrackingScaling"
  resource_id        = var.ecs_service_resource_id
  scalable_dimension = var.ecs_scalable_dimension
  service_namespace  = var.ecs_service_namespace
  depends_on         = [aws_appautoscaling_target.ecs_target]

  target_tracking_scaling_policy_configuration {
    disable_scale_in   = false
    scale_in_cooldown  = var.ecs_scale_in_cooldown
    scale_out_cooldown = var.ecs_scale_out_cooldown
    target_value       = var.ecs_memory_target_value

    predefined_metric_specification {
      predefined_metric_type = "ECSServiceAverageMemoryUtilization"
    }
  }
}

resource "aws_appautoscaling_policy" "cpu_policy_oars_ecs" {
  name               = "${var.ecs_service_name}-cpu-appautoscaling-target"
  policy_type        = "TargetTrackingScaling"
  resource_id        = var.ecs_service_resource_id
  scalable_dimension = var.ecs_scalable_dimension
  service_namespace  = var.ecs_service_namespace
  depends_on         = [aws_appautoscaling_target.ecs_target]

  target_tracking_scaling_policy_configuration {
    disable_scale_in   = false
    scale_in_cooldown  = var.ecs_scale_in_cooldown
    scale_out_cooldown = var.ecs_scale_out_cooldown
    target_value       = var.ecs_cpu_target_value

    predefined_metric_specification {
      predefined_metric_type = "ECSServiceAverageCPUUtilization"
    }
  }
}