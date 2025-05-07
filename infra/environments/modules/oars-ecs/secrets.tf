# AWS Secrets Manager resources
resource "aws_secretsmanager_secret" "powerbi_clientid" {
  name = "/${var.project_name}/${var.environment_name}/powerbiclientid"
  description = "Secret for PowerBI Client ID"
  tags   = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)
  recovery_window_in_days = 0
}

resource "aws_secretsmanager_secret" "powerbi_tenantid" {
  name = "/${var.project_name}/${var.environment_name}/powerbitenantid"
  description = "Secret for PowerBI Tenantid"
  tags   = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)
  recovery_window_in_days = 0
}

resource "aws_secretsmanager_secret" "powerbi_clientsecret" {
  name = "/${var.project_name}/${var.environment_name}/powerbiclientsecret"
  description = "Secret for PowerBI Client Secret"
  tags   = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)
  recovery_window_in_days = 0
}

resource "aws_secretsmanager_secret" "okta_clientsecret" {
  name = "/${var.project_name}/${var.environment_name}/oktaclientsecret"
  description = "Secret for Okta Client Secret"
  tags   = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)
  recovery_window_in_days = 0
}

resource "aws_secretsmanager_secret" "db_connection_string" {
  name = "/${var.project_name}/${var.environment_name}/dbconnection"
  description = "Secret for Database Connection String"
  tags   = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)
  recovery_window_in_days = 0
}


