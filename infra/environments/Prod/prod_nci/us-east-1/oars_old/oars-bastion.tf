module "oars-prod-bastion" {
  source                = "../../../../modules/oars-bastionhost"
  project_name          = var.oars_project_name
  environment_name      = var.environment_name
  vpc_id                = var.vpc_id
  subnet_id             = var.bastion_subnet_id
  bastion_instance_type = var.bastion_instance_type
  tags                  = var.tags
  bastion_ami           = var.bastion_ami
}