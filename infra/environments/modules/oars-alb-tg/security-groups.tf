#Security groups for the Application LoadBalancer and a target Security Group

resource "aws_security_group" "alb_sg" {
  name        = "${var.project_name}-${var.environment_name}-alb-sg"
  description = "Allow HTTP/HTTPS inbound and LB to target SG outbound"
  vpc_id      = var.vpc_id

  ingress {
    description = "Allow HTTP from anywhere"
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"] 
  }
  ingress {
    description = "Allow HTTP from anywhere"
    from_port   = 8080
    to_port     = 8080
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"] 
  }

  ingress {
    description = "Allow HTTPS from anywhere"
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    description = "Allow HTTP to target security group"
    from_port   = 80 
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"] 
  }
  egress {
    description = "Allow HTTP to target security group"
    from_port   = 8080
    to_port     = 8080
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"] 
  }
  tags          = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)
}

resource "aws_security_group" "target_sg" {
  name        = "${var.project_name}-${var.environment_name}-target-sg"
  description = "Allow traffic from ALB"
  vpc_id      = var.vpc_id

  ingress {
    description     = "Allow traffic from ALB SG"
    from_port       = 8080
    to_port         = 8080
    protocol        = "tcp"
    security_groups = [aws_security_group.alb_sg.id] 
  }  
  ingress {
    description     = "Allow traffic from ALB SG"
    from_port       = 80
    to_port         = 80
    protocol        = "tcp"
    security_groups = [aws_security_group.alb_sg.id] 
  }  
  tags              = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)
}