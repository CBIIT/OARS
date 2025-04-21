#AWS security group for a Fargate service 
resource "aws_security_group" "theradex_sg" {
  description = "pipeline/AppStage/dev/FargateService/Service/SecurityGroup"
  egress {
      cidr_blocks      = ["0.0.0.0/0"]
      from_port        = 0
      protocol         = "-1"
      to_port          = 0
    }

  ingress {
      from_port       = 80
      protocol        = "TCP"
      security_groups = ["${var.load_balancer_sg}"]
      to_port         = 80
      cidr_blocks     = ["0.0.0.0/0"]
    }
  ingress {
      from_port       = 8080
      protocol        = "TCP"
      security_groups = ["${var.load_balancer_sg}"]
      to_port         = 8080
      cidr_blocks     = ["0.0.0.0/0"]
    }
  name = "${var.project_name}-${var.environment_name}-FargateServiceSecurityGroup262B61DD-YB75F6TZR5BF"
  vpc_id                 = var.vpc_id
  revoke_rules_on_delete = true
  tags                   = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)
}
