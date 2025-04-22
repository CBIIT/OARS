module "oars-qa-ecr" {
  source           = "../../../../../modules/oars-ecr"
  environment_name = var.qa_environment_name
  project_name     = var.oars_project_name
  tags             = var.tags_qa
}