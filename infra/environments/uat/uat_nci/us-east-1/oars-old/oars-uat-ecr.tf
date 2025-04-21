module "oars-uat-ecr" {
  source           = "../../../../modules/oars-ecr"
  environment_name = var.uat_environment_name
  project_name     = var.oars_project_name
  tags             = var.tags
}