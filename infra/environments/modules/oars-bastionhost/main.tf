#Bastion Host EC2 Instance

resource "aws_instance" "bastion_ec2" {
  subnet_id              = var.subnet_id
  vpc_security_group_ids = [aws_security_group.bastion_sg.id]
  iam_instance_profile   = aws_iam_instance_profile.profile.name
  ami                    = var.bastion_ami
  instance_type          = var.bastion_instance_type
  
  tags                   = merge({"ProjectName"= "${var.project_name}"},{"EnvironmentName"= "${var.environment_name}"},var.tags, {"Name" = "${var.project_name}-${var.environment_name}-BastionHost"})

  lifecycle {
    ignore_changes = [tags ["QSConfigName-079hl"]]
  }

}