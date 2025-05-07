module "oars-dev-ecr" {
  source           = "../modules/oars-ecr"
  environment_name = var.dev_environment_name
  project_name     = var.oars_project_name
  tags             = var.tags
}