#AWS ECS Fargate service with replica scheduling and network config
resource "aws_ecs_service" "ecs_service" {
  name                               = "${var.project_name}-${var.environment_name}-service"
  cluster                            = aws_ecs_cluster.main.id
  task_definition                    = aws_ecs_task_definition.task.arn
  desired_count                      = var.desired_count
  deployment_minimum_healthy_percent = 50
  deployment_maximum_percent         = 200
  launch_type                        = "FARGATE"
  scheduling_strategy                = "REPLICA"
  enable_execute_command             = true

  network_configuration {
    security_groups  = [aws_security_group.theradex_sg.id]
    subnets          = var.subnets
    assign_public_ip = false
  }
   load_balancer {
    target_group_arn = "${var.target_group_arn}"
    container_name   = "web"
    container_port   = "${var.load_balancer_container_port}"
  }

  lifecycle {
    # ignore_changes = [desired_count]
    ignore_changes = [task_definition]
  }
}