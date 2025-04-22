
module "oars_codepipeline" {
  source              = "../../../../modules/oars-codepipeline"
  project_name        = var.project_name
  account_id          = var.account_id 
  environment         = var.environment 
  environment2        = var.environment2
  tags                = var.tags
  environment_account = var.environment_account 
}
