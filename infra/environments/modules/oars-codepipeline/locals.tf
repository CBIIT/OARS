locals {
  deploy_role_arns = { 
    for env in var.environment : env => "arn:aws:iam::${var.environment_account[env]}:role/${var.project_name}-${env}-DeployRole" 
  }
}
locals {
  ecr_push_role_arns = {
    for env in var.environment :
    env => aws_iam_role.ecr_push_role2[env].arn
  }
}

data "aws_codestarconnections_connection" "github" {
  name = "GitHub-Theradex"
}

data "aws_kms_key" "by_alias" {
  key_id = "alias/nci-web-reporting/pipeline"
}