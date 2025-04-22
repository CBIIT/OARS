# Here lives the definition for the qrcg task & service definition

resource "aws_iam_role" "qrcg_ecs_task_role" {
  name = "qrcg-ecsTaskRole"

  assume_role_policy = <<EOF
{
   "Version":"2012-10-17",
   "Statement":[
      {
         "Effect":"Allow",
         "Principal":{
            "Service":[
               "ecs-tasks.amazonaws.com"
            ]
         },
         "Action":"sts:AssumeRole",
         "Condition":{
            "ArnLike":{
            "aws:SourceArn":"arn:aws:ecs:us-east-1:${local.myaccount}:*"
            },
            "StringEquals":{
               "aws:SourceAccount":"${local.myaccount}"
            }
         }
      }
   ]
}
EOF
}

resource "aws_iam_role" "qrcg_ecs_task_execution_role" {
  name = "qrcg-ecsTaskExecutionRole"

  assume_role_policy = <<EOF
{
   "Version":"2012-10-17",
   "Statement":[
      {
         "Effect":"Allow",
         "Principal":{
            "Service":[
               "ecs-tasks.amazonaws.com"
            ]
         },
         "Action":"sts:AssumeRole",
         "Condition":{
            "ArnLike":{
            "aws:SourceArn":"arn:aws:ecs:us-east-1:${local.myaccount}:*"
            },
            "StringEquals":{
               "aws:SourceAccount":"${local.myaccount}"
            }
         }
      }
   ]
}
EOF
}

resource "aws_iam_role_policy_attachment" "qrcg_ecs_task_execution_role_policy_attachment" {
  role       = aws_iam_role.qrcg_ecs_task_execution_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

resource "aws_ecs_task_definition" "qrcg" {
  family                   = var.qrcg_taskdefinition_name
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = 256
  memory                   = 512
  execution_role_arn       = aws_iam_role.qrcg_ecs_task_execution_role.arn
  task_role_arn            = aws_iam_role.qrcg_ecs_task_role.arn
  container_definitions = jsonencode([{
    name      = "${var.qrcg_container_name}"
    image     = "${aws_ecr_repository.theradex_qrcg_uat.repository_url}:uat-latest"
    essential = true
    environment = [
      {
        name  = "ASPNETCORE_ENVIRONMENT"
        value = "uat-nci"
      }
    ]
    portMappings = [
      {
        protocol      = "tcp"
        containerPort = 80
        hostPort      = 80
      },
      {
        protocol      = "tcp"
        containerPort = 443
        hostPort      = 443
      },
    ]
  }])
}

resource "aws_ecs_service" "qrcg" {
  name            = var.qrcg_service_name
  cluster         = aws_ecs_cluster.theradex_uat_noncomm_cluster.id
  task_definition = aws_ecs_task_definition.qrcg.arn
  desired_count   = 0
  launch_type     = "FARGATE"
  # scheduling_strategy = "REPLICA"
  # deployment_minimum_healthy_percent = 50
  # deployment_maximum_percent = 200

  network_configuration {
    security_groups  = [aws_security_group.theradex_app_qrcg_sg.id]
    subnets          = local.private_subnet_ids
    assign_public_ip = false
  }
  # 
  # load_balancer {
  #   target_group_arn = var.aws_alb_target_group_arn
  #   container_name = "${var.name}-container-${var.environment}"
  #   container_port = var.container_port
  # }
  # 
  # lifecycle {
  #   ignore_changes = [task_definition, desired_count]
  # }
}

resource "aws_security_group" "theradex_app_qrcg_sg" {
  name        = "qrcg-sg"
  description = "Allow TLS inbound traffic"
  vpc_id      = module.uat_noncomm_vpc.id

  ingress {
    description = "HTTPS from all accounts"
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["10.0.0.0/10"]
  }

  ingress {
    description = "HTTP from all accounts"
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["10.0.0.0/10"]
  }

  egress {
    from_port        = 0
    to_port          = 0
    protocol         = "-1"
    cidr_blocks      = ["0.0.0.0/0"]
    ipv6_cidr_blocks = ["::/0"]
  }

  tags = {
    Name = "allow_tls"
  }
}
