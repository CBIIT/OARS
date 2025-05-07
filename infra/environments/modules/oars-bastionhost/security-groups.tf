#Security Group for Bastion Host instance

resource "aws_security_group" "bastion_sg" {
  name        = "${var.project_name}-${var.environment_name}-bastion-sg"
  description = "Allow all outbound traffic by default"
  vpc_id      = var.vpc_id
  
  # ingress {
  #   description = "Allow SSH from our public IP"
  #   from_port   = 22
  #   to_port     = 22
  #   protocol    = "tcp"
  #   cidr_blocks = ["0.0.0.0/0"]
  # }
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"] 
  }
  
  tags        = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)


}