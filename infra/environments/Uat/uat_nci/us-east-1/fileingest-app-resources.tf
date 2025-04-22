
resource "aws_ssm_parameter" "theradex_app_fileingesttomedidata_rwsserver" {
  name        = "/${var.fileingest_env}/app/FileIngestToMedidata/AppSettings/RWSServer"
  description = "FileIngestToMedidata"
  type        = "String"
  value       = "https://theradex.mdsol.com"
}

resource "aws_ssm_parameter" "theradex_app_fileingesttomedidata_rwsusername" {
  name        = "/${var.fileingest_env}/app/FileIngestToMedidata/AppSettings/RWSUsername"
  description = "FileIngestToMedidata"
  type        = "String"
  value       = "SPEC_TRACK_RWS"
}

resource "aws_ssm_parameter" "theradex_app_fileingesttomedidata_rwspassword" {
  name        = "/${var.fileingest_env}/app/FileIngestToMedidata/AppSettings/RWSPassword"
  description = "FileIngestToMedidata"
  type        = "String"
  value       = "Password@01"
}

resource "aws_ssm_parameter" "theradex_app_fileingesttomedidata_rwstimeoutinsecs" {
  name        = "/${var.fileingest_env}/app/FileIngestToMedidata/AppSettings/RWSTimeOutInSecs"
  description = "FileIngestToMedidata"
  type        = "String"
  value       = "3600"
}

resource "aws_ssm_parameter" "theradex_app_fileingesttomedidata_RequestReconcileSQS_Url" {
  name        = "/${var.fileingest_env}/app/FileIngestToMedidata/AppSettings/RequestReconcileSQS_Url"
  description = "FileIngestToMedidata"
  type        = "String"
  value       = aws_sqs_queue.fileingest-requestreconcile-queue.url
}

resource "aws_ssm_parameter" "theradex_app_fileingesttomedidata_ProcessMedidataSQS_Url" {
  name        = "/${var.fileingest_env}/app/FileIngestToMedidata/AppSettings/ProcessMedidataSQS_Url"
  description = "FileIngestToMedidata"
  type        = "String"
  value       = aws_sqs_queue.fileingest-updatemedidata-queue.url
}
