resource "aws_ssm_parameter" "theradex_app_qrcg_ecscluster" {
  name        = "/theradex/app/qrcodegeneration/ecscluster"
  description = "Theradex APP QR Code Generation ECS Cluster"
  type        = "String"
  value       = var.qrcg_cluster_name
}

resource "aws_ssm_parameter" "theradex_app_qrcg_taskname" {
  name        = "/theradex/app/qrcodegeneration/taskname"
  description = "Theradex APP QR Code Generation Task Definition Name"
  type        = "String"
  value       = var.qrcg_taskdefinition_name
}

resource "aws_ssm_parameter" "theradex_app_qrcg_subnet1" {
  name        = "/theradex/app/qrcodegeneration/subnet1"
  description = "Theradex APP QR Code Generation Subnet1 ID"
  type        = "String"
  value       = local.private_subnet_ids[0]
}

resource "aws_ssm_parameter" "theradex_app_qrcg_subnet2" {
  name        = "/theradex/app/qrcodegeneration/subnet2"
  description = "Theradex APP QR Code Generation Subnet2 ID"
  type        = "String"
  value       = local.private_subnet_ids[1]
}

resource "aws_ssm_parameter" "theradex_app_qrcg_sg" {
  name        = "/theradex/app/qrcodegeneration/securitygroup"
  description = "Theradex APP QR Code Generation Security Group"
  type        = "String"
  value       = aws_security_group.theradex_app_qrcg_sg.id
}

resource "aws_ssm_parameter" "theradex_app_qrcg_containername" {
  name        = "/theradex/app/qrcodegeneration/containername"
  description = "Theradex APP QR Code Generation Container Name"
  type        = "String"
  value       = var.qrcg_container_name
}

