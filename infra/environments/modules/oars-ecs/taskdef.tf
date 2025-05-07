#ECS task definition for a Fargate service
resource "aws_ecs_task_definition" "task" {
  family                   = "${var.project_name}-${var.environment_name}-taskdefinition"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = var.task_def_cpu
  memory                   = var.task_def_memory
  execution_role_arn       = aws_iam_role.ecs_task_execution_role.arn
  task_role_arn            = aws_iam_role.ecs_task_role.arn
  container_definitions = templatefile("${path.module}/container_definitions/config.tfpl",
      {
        current_region            = "${data.aws_region.current.name}"
        log_group_name            = "/ecs/${var.project_name}-${var.environment_name}-service",
        ecr_image_endpoint        = "${data.aws_caller_identity.current.account_id}.dkr.ecr.${data.aws_region.current.name}.amazonaws.com/${var.project_name}-${var.environment_name}:latest",
        host_port                 = "${var.task_def_host_port}",
        container_port            = "${var.task_def_container_port}",
        container_definition_name = "web",
        ASPNETCORE_ENVIRONMENT = "${var.aspnetcore_environment_name}",
        ENVIRONMENT = "${var.oars_environment}",
        powerbi_clientid          = "${aws_secretsmanager_secret.powerbi_clientid.arn}",
        powerbi_tenantid          = "${aws_secretsmanager_secret.powerbi_tenantid.arn}",
        powerbi_clientsecret      = "${aws_secretsmanager_secret.powerbi_clientsecret.arn}",
        okta_clientsecret         = "${aws_secretsmanager_secret.okta_clientsecret.arn}",
        db_connection_string      = "${aws_secretsmanager_secret.db_connection_string.arn}"
      }
    )
  }


  