module "oars-prod-ecr" {
  source           = "../../../../modules/oars-ecr"
  environment_name = var.production_environment_name
  project_name     = var.oars_project_name
  tags             = var.tags
}