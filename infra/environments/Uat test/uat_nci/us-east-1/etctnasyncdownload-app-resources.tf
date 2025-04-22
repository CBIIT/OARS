resource "aws_ssm_parameter" "theradex_app_etctnasyncdownload_api_url" {
  name        = "/${var.etctnasyncdownload_env}/app/ETCTNAsyncDownload/AppSettings/ETCTN_Datashare_Url"
  description = "Theradex APP ETCTNDataShare Url"
  type        = "String"
  value       = "https://etctn-datashare.bpc-apps.nchri.org"
}

resource "aws_ssm_parameter" "theradex_app_etctnasyncdownload_api_auth_key" {
  name        = "/${var.etctnasyncdownload_env}/app/ETCTNAsyncDownload/AppSettings/ETCTN_Auth_Key"
  description = "Theradex APP ETCTNDataShare Auth Key"
  type        = "String"
  value       = "HofR1ii80hNzEKSbiBYRfubPNrsA0trK2sQLcu2L"
}

resource "aws_ssm_parameter" "theradex_app_etctnasyncdownload_maxrunningtime" {
  name        = "/${var.etctnasyncdownload_env}/app/ETCTNAsyncDownload/AppSettings/MaxRunningTimeInMins"
  description = "Theradex APP ETCTNAsyncDownload MaxRunningTimeInMins"
  type        = "String"
  value       = "10"
}

resource "aws_ssm_parameter" "theradex_app_etctnasyncdownload_bucket" {
  name        = "/${var.etctnasyncdownload_env}/app/ETCTNAsyncDownload/AppSettings/Bucket"
  description = "Theradex APP ETCTNAsyncDownload Bucket"
  type        = "String"
  value       = var.etctnasyncdownload_files_s3_bucket
}

resource "aws_ssm_parameter" "theradex_app_etctnasyncdownload_bucketdatapath" {
  name        = "/${var.etctnasyncdownload_env}/app/ETCTNAsyncDownload/AppSettings/BucketDataPath"
  description = "Theradex APP ETCTNAsyncDownload BucketDataPath"
  type        = "String"
  value       = "Data"
}

resource "aws_ssm_parameter" "theradex_app_etctnasyncdownload_fromemailaddress" {
  name        = "/${var.etctnasyncdownload_env}/app/ETCTNAsyncDownload/AppSettings/FromEmailAddress"
  description = "Theradex APP ETCTNAsyncDownload FromEmailAddress"
  type        = "String"
  value       = "noreply@theradex.com"
}

resource "aws_ssm_parameter" "theradex_app_etctnasyncdownload_emailforprotocols" {
  name        = "/${var.etctnasyncdownload_env}/app/ETCTNAsyncDownload/AppSettings/EmailForProtocols"
  description = "Theradex APP ETCTNAsyncDownload EmailForProtocols"
  type        = "String"
  value       = "10334,10496,10382,10300,10466,10476,10388,10355,10579,NCICOVID"
}

resource "aws_ssm_parameter" "theradex_app_etctnasyncdownload_emailforforms" {
  name        = "/${var.etctnasyncdownload_env}/app/ETCTNAsyncDownload/AppSettings/EmailForForms"
  description = "Theradex APP ETCTNAsyncDownload EmailForForms"
  type        = "String"
  value       = "RECEIVING_STATUS,SHIPPING_STATUS"
}

resource "aws_ssm_parameter" "theradex_app_etctnasyncdownload_emails" {
  name        = "/${var.etctnasyncdownload_env}/app/ETCTNAsyncDownload/AppSettings/Emails"
  description = "Theradex APP ETCTNAsyncDownload Emails"
  type        = "String"
  value       = "mrathi@theradex.com,nsarakhawas@theradex.com"
}