###############################
##### RWSSettings  #####
###############################

resource "aws_ssm_parameter" "theradex_app_vendorintg_rwssettings_rwsserver" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/RWSSettings/RWSServer"
  description = "Theradex APP Vendor Integration RWSSettings RWSServer"
  type        = "String"
  value       = "https://theradex.mdsol.com"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_rwssettings_rwsusername" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/RWSSettings/RWSUserName"
  description = "Theradex APP Vendor Integration RWSSettings RWSUserName"
  type        = "String"
  value       = "SPEC_TRACK_RWS"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_rwssettings_rwspassword" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/RWSSettings/RWSPassword"
  description = "Theradex APP Vendor Integration RWSSettings RWSPassword"
  type        = "String"
  value       = "Password@01"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_rwssettings_timeoutinsecs" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/RWSSettings/TimeoutInSecs"
  description = "Theradex APP Vendor Integration RWSSettings TimeoutInSecs"
  type        = "String"
  value       = "90"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_rwssettings_archivesubfolder" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/RWSSettings/ArchiveSubFolder"
  description = "Theradex APP Vendor Integration RWSSettings ArchiveSubFolder"
  type        = "String"
  value       = "Rave"
}

###############################
##### EmailSettings #####
###############################

resource "aws_ssm_parameter" "theradex_app_vendorintg_emailsettings_fromaddress" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/EmailSettings/FromAddress"
  description = "Theradex APP Vendor Integration EmailSettings FromAddress"
  type        = "String"
  value       = "noreply@theradex.com"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_emailsettings_toaddress" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/EmailSettings/ToAddress"
  description = "Theradex APP Vendor Integration EmailSettings ToAddress"
  type        = "String"
  value       = "uvarada@theradex.com,mrathi@theradex.com,nsarakhawas@theradex.com"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_emailsettings_zippassword" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/EmailSettings/ZipPassword"
  description = "Theradex APP Vendor Integration EmailSettings ZipPassword"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_emailsettings_compressattachments" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/EmailSettings/CompressAttachments"
  description = "Theradex APP Vendor Integration EmailSettings CompressAttachments"
  type        = "String"
  value       = "true"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_emailsettings_passwordprotect" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/EmailSettings/PasswordProtect"
  description = "Theradex APP Vendor Integration EmailSettings PasswordProtect"
  type        = "String"
  value       = "true"
}


###############################
##### AppSettings #####
###############################

resource "aws_ssm_parameter" "theradex_app_vendorintg_appsettings_archivebucket" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/AppSettings/ArchiveBucket"
  description = "Theradex APP Vendor Integration AppSettings Archive Bucket"
  type        = "String"
  value       = var.vendorintg_outputfile_s3_bucket
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_appsettings_databucket" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/AppSettings/DataBucket"
  description = "Theradex APP Vendor Integration AppSettings Data Bucket"
  type        = "String"
  value       = var.vendorintg_data_s3_bucket
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_appsettings_datafolder" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/AppSettings/DataFolder"
  description = "Theradex APP Vendor Integration AppSettings Data Folder"
  type        = "String"
  value       = "Data"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_appsettings_timedelaybeforesnapshot" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/AppSettings/TimeDelayInSecsBeforeSnapshot"
  description = "Theradex APP Vendor Integration AppSettings TimeDelayInSecsBeforeSnapshot"
  type        = "String"
  value       = "20"
}

###############################
##### IntegrationSettings PreAnalyticResults #####
###############################

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_subjects_0" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/0/Subjects"
  description = "Theradex APP Vendor Integration Integrations Subjects 0"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_protcolnames_0" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/0/ProtocolNames"
  description = "Theradex APP Vendor Integration Integrations ProtocolNames 0"
  type        = "String"
  value       = "10330,10355,10358,10388,10434,10440,10499,10579,10538,10563"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_environment_0" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/0/Environment"
  description = "Theradex APP Vendor Integration Integrations Environment 0"
  type        = "String"
  value       = "(Prod)"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_integrationform_0" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/0/IntegrationForm"
  description = "Theradex APP Vendor Integration Integrations IntegrationForm 0"
  type        = "String"
  value       = "PREANALYTIC_RESULTS"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_integrationvendor_0" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/0/IntegrationVendor"
  description = "Theradex APP Vendor Integration Integrations IntegrationVendor 0"
  type        = "String"
  value       = "EETBioBank"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_enabled_0" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/0/Enabled"
  description = "Theradex APP Vendor Integration Integrations Enabled 0"
  type        = "String"
  value       = "true"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_awsregion_0" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/0/VendorSettings/AWSRegion"
  description = "Theradex APP Vendor Integration Integrations VendorSettings AWSRegion 0"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_accesskey_0" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/0/VendorSettings/AccessKey"
  description = "Theradex APP Vendor Integration Integrations VendorSettings AccessKey 0"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_secretkey_0" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/0/VendorSettings/SecretKey"
  description = "Theradex APP Vendor Integration Integrations VendorSettings SecretKey 0"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_archivesubfolder_0" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/0/VendorSettings/ArchiveSubFolder"
  description = "Theradex APP Vendor Integration Integrations VendorSettings ArchiveSubFolder 0"
  type        = "String"
  value       = "External"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_url_0" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/0/VendorSettings/Url"
  description = "Theradex APP Vendor Integration Integrations VendorSettings Url 0"
  type        = "String"
  value       = "https://etctn-datashare.bpc-apps.nchri.org/api/"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_key_0" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/0/VendorSettings/Key"
  description = "Theradex APP Vendor Integration Integrations VendorSettings Key 0"
  type        = "String"
  value       = "HofR1ii80hNzEKSbiBYRfubPNrsA0trK2sQLcu2L"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_timeoutinsecs_0" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/0/VendorSettings/TimeoutInSecs"
  description = "Theradex APP Vendor Integration Integrations VendorSettings TimeoutInSecs 0"
  type        = "String"
  value       = "1200"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_endpoint_0" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/0/VendorSettings/EndPoint"
  description = "Theradex APP Vendor Integration Integrations VendorSettings EndPoint 0"
  type        = "String"
  value       = "queries/8/results.json"
}

###############################
##### IntegrationSettings Solid Tissue #####
###############################

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_subjects_1" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/1/Subjects"
  description = "Theradex APP Vendor Integration Integrations Subjects 1"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_protcolnames_1" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/1/ProtocolNames"
  description = "Theradex APP Vendor Integration Integrations ProtocolNames 1"
  type        = "String"
  value       = "10330,10355,10358,10388,10434,10440,10499,10579,10538,10563"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_environment_1" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/1/Environment"
  description = "Theradex APP Vendor Integration Integrations Environment 1"
  type        = "String"
  value       = "(Prod)"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_integrationform_1" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/1/IntegrationForm"
  description = "Theradex APP Vendor Integration Integrations IntegrationForm 1"
  type        = "String"
  value       = "SOLID_TISSUE_PATHOLOGY_VERIFICATION_AND_ASSESSMENT"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_integrationvendor_1" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/1/IntegrationVendor"
  description = "Theradex APP Vendor Integration Integrations IntegrationVendor 1"
  type        = "String"
  value       = "EETBioBank"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_enabled_1" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/1/Enabled"
  description = "Theradex APP Vendor Integration Integrations Enabled 1"
  type        = "String"
  value       = "true"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_awsregion_1" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/1/VendorSettings/AWSRegion"
  description = "Theradex APP Vendor Integration Integrations VendorSettings AWSRegion 1"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_accesskey_1" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/1/VendorSettings/AccessKey"
  description = "Theradex APP Vendor Integration Integrations VendorSettings AccessKey 1"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_secretkey_1" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/1/VendorSettings/SecretKey"
  description = "Theradex APP Vendor Integration Integrations VendorSettings SecretKey 1"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_archivesubfolder_1" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/1/VendorSettings/ArchiveSubFolder"
  description = "Theradex APP Vendor Integration Integrations VendorSettings ArchiveSubFolder 1"
  type        = "String"
  value       = "External"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_url_1" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/1/VendorSettings/Url"
  description = "Theradex APP Vendor Integration Integrations VendorSettings Url 1"
  type        = "String"
  value       = "https://etctn-datashare.bpc-apps.nchri.org/api/"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_key_1" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/1/VendorSettings/Key"
  description = "Theradex APP Vendor Integration Integrations VendorSettings Key 1"
  type        = "String"
  value       = "HofR1ii80hNzEKSbiBYRfubPNrsA0trK2sQLcu2L"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_timeoutinsecs_1" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/1/VendorSettings/TimeoutInSecs"
  description = "Theradex APP Vendor Integration Integrations VendorSettings TimeoutInSecs 1"
  type        = "String"
  value       = "1200"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_endpoint_1" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/1/VendorSettings/EndPoint"
  description = "Theradex APP Vendor Integration Integrations VendorSettings EndPoint 1"
  type        = "String"
  value       = "queries/11/results.json"
}

###############################
##### IntegrationSettings Hematologic #####
###############################

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_subjects_2" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/2/Subjects"
  description = "Theradex APP Vendor Integration Integrations Subjects 2"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_protcolnames_2" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/2/ProtocolNames"
  description = "Theradex APP Vendor Integration Integrations ProtocolNames 2"
  type        = "String"
  value       = "10330,10355,10358,10388,10434,10440,10499,10579,10538,10563"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_environment_2" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/2/Environment"
  description = "Theradex APP Vendor Integration Integrations Environment 2"
  type        = "String"
  value       = "(Prod)"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_integrationform_2" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/2/IntegrationForm"
  description = "Theradex APP Vendor Integration Integrations IntegrationForm 2"
  type        = "String"
  value       = "HEMATOLOGIC_MALIGNANCY_VERIFICATION_AND_ASSESSMENT"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_integrationvendor_2" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/2/IntegrationVendor"
  description = "Theradex APP Vendor Integration Integrations IntegrationVendor 2"
  type        = "String"
  value       = "EETBioBank"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_enabled_2" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/2/Enabled"
  description = "Theradex APP Vendor Integration Integrations Enabled 2"
  type        = "String"
  value       = "true"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_awsregion_2" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/2/VendorSettings/AWSRegion"
  description = "Theradex APP Vendor Integration Integrations VendorSettings AWSRegion 2"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_accesskey_2" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/2/VendorSettings/AccessKey"
  description = "Theradex APP Vendor Integration Integrations VendorSettings AccessKey 2"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_secretkey_2" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/2/VendorSettings/SecretKey"
  description = "Theradex APP Vendor Integration Integrations VendorSettings SecretKey 2"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_archivesubfolder_2" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/2/VendorSettings/ArchiveSubFolder"
  description = "Theradex APP Vendor Integration Integrations VendorSettings ArchiveSubFolder 2"
  type        = "String"
  value       = "External"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_url_2" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/2/VendorSettings/Url"
  description = "Theradex APP Vendor Integration Integrations VendorSettings Url 2"
  type        = "String"
  value       = "https://etctn-datashare.bpc-apps.nchri.org/api/"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_key_2" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/2/VendorSettings/Key"
  description = "Theradex APP Vendor Integration Integrations VendorSettings Key 2"
  type        = "String"
  value       = "HofR1ii80hNzEKSbiBYRfubPNrsA0trK2sQLcu2L"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_timeoutinsecs_2" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/2/VendorSettings/TimeoutInSecs"
  description = "Theradex APP Vendor Integration Integrations VendorSettings TimeoutInSecs 2"
  type        = "String"
  value       = "1200"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_endpoint_2" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/2/VendorSettings/EndPoint"
  description = "Theradex APP Vendor Integration Integrations VendorSettings EndPoint 2"
  type        = "String"
  value       = "queries/11/results.json"
}

###############################
##### IntegrationSettings Shipping Status #####
###############################

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_subjects_3" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/3/Subjects"
  description = "Theradex APP Vendor Integration Integrations Subjects 3"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_protcolnames_3" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/3/ProtocolNames"
  description = "Theradex APP Vendor Integration Integrations ProtocolNames 3"
  type        = "String"
  value       = "10330,10355,10358,10388,10434,10440,10499,10579,10538,10563"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_environment_3" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/3/Environment"
  description = "Theradex APP Vendor Integration Integrations Environment 3"
  type        = "String"
  value       = "(Prod)"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_integrationform_3" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/3/IntegrationForm"
  description = "Theradex APP Vendor Integration Integrations IntegrationForm 3"
  type        = "String"
  value       = "SHIPPING_STATUS"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_integrationvendor_3" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/3/IntegrationVendor"
  description = "Theradex APP Vendor Integration Integrations IntegrationVendor 3"
  type        = "String"
  value       = "EETBioBank"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_enabled_3" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/3/Enabled"
  description = "Theradex APP Vendor Integration Integrations Enabled 3"
  type        = "String"
  value       = "true"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_awsregion_3" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/3/VendorSettings/AWSRegion"
  description = "Theradex APP Vendor Integration Integrations VendorSettings AWSRegion 3"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_accesskey_3" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/3/VendorSettings/AccessKey"
  description = "Theradex APP Vendor Integration Integrations VendorSettings AccessKey 3"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_secretkey_3" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/3/VendorSettings/SecretKey"
  description = "Theradex APP Vendor Integration Integrations VendorSettings SecretKey 3"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_archivesubfolder_3" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/3/VendorSettings/ArchiveSubFolder"
  description = "Theradex APP Vendor Integration Integrations VendorSettings ArchiveSubFolder 3"
  type        = "String"
  value       = "External"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_url_3" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/3/VendorSettings/Url"
  description = "Theradex APP Vendor Integration Integrations VendorSettings Url 3"
  type        = "String"
  value       = "https://etctn-datashare.bpc-apps.nchri.org/api/"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_key_3" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/3/VendorSettings/Key"
  description = "Theradex APP Vendor Integration Integrations VendorSettings Key 3"
  type        = "String"
  value       = "HofR1ii80hNzEKSbiBYRfubPNrsA0trK2sQLcu2L"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_timeoutinsecs_3" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/3/VendorSettings/TimeoutInSecs"
  description = "Theradex APP Vendor Integration Integrations VendorSettings TimeoutInSecs 3"
  type        = "String"
  value       = "1200"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_endpoint_3" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/3/VendorSettings/EndPoint"
  description = "Theradex APP Vendor Integration Integrations VendorSettings EndPoint 3"
  type        = "String"
  value       = "queries/7/results.json"
}


###############################
##### IntegrationSettings Receiving Status #####
###############################

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_subjects_4" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/4/Subjects"
  description = "Theradex APP Vendor Integration Integrations Subjects 4"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_protcolnames_4" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/4/ProtocolNames"
  description = "Theradex APP Vendor Integration Integrations ProtocolNames 4"
  type        = "String"
  value       = "10330,10355,10358,10388,10434,10440,10499,10579,10538,10563"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_environment_4" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/4/Environment"
  description = "Theradex APP Vendor Integration Integrations Environment 4"
  type        = "String"
  value       = "(Prod)"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_integrationform_4" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/4/IntegrationForm"
  description = "Theradex APP Vendor Integration Integrations IntegrationForm 4"
  type        = "String"
  value       = "RECEIVING_STATUS"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_integrationvendor_4" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/4/IntegrationVendor"
  description = "Theradex APP Vendor Integration Integrations IntegrationVendor 4"
  type        = "String"
  value       = "EETBioBank"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_enabled_4" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/4/Enabled"
  description = "Theradex APP Vendor Integration Integrations Enabled 4"
  type        = "String"
  value       = "true"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_awsregion_4" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/4/VendorSettings/AWSRegion"
  description = "Theradex APP Vendor Integration Integrations VendorSettings AWSRegion 4"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_accesskey_4" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/4/VendorSettings/AccessKey"
  description = "Theradex APP Vendor Integration Integrations VendorSettings AccessKey 4"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_secretkey_4" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/4/VendorSettings/SecretKey"
  description = "Theradex APP Vendor Integration Integrations VendorSettings SecretKey 4"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_archivesubfolder_4" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/4/VendorSettings/ArchiveSubFolder"
  description = "Theradex APP Vendor Integration Integrations VendorSettings ArchiveSubFolder 4"
  type        = "String"
  value       = "External"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_url_4" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/4/VendorSettings/Url"
  description = "Theradex APP Vendor Integration Integrations VendorSettings Url 4"
  type        = "String"
  value       = "https://etctn-datashare.bpc-apps.nchri.org/api/"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_key_4" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/4/VendorSettings/Key"
  description = "Theradex APP Vendor Integration Integrations VendorSettings Key 4"
  type        = "String"
  value       = "HofR1ii80hNzEKSbiBYRfubPNrsA0trK2sQLcu2L"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_timeoutinsecs_4" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/4/VendorSettings/TimeoutInSecs"
  description = "Theradex APP Vendor Integration Integrations VendorSettings TimeoutInSecs 4"
  type        = "String"
  value       = "1200"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_endpoint_4" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/4/VendorSettings/EndPoint"
  description = "Theradex APP Vendor Integration Integrations VendorSettings EndPoint 4"
  type        = "String"
  value       = "queries/3/results.json"
}

###############################
##### IntegrationSettings BSR #####
###############################

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_subjects_5" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/5/Subjects"
  description = "Theradex APP Vendor Integration Integrations Subjects 5"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_protcolnames_5" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/5/ProtocolNames"
  description = "Theradex APP Vendor Integration Integrations ProtocolNames 5"
  type        = "String"
  value       = "10330,10355,10358,10388,10434,10440,10499,10579"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_environment_5" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/5/Environment"
  description = "Theradex APP Vendor Integration Integrations Environment 5"
  type        = "String"
  value       = "(Prod)"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_integrationform_5" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/5/IntegrationForm"
  description = "Theradex APP Vendor Integration Integrations IntegrationForm 5"
  type        = "String"
  value       = "BIOSPECIMEN_ROADMAP_ASSAY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_integrationvendor_5" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/5/IntegrationVendor"
  description = "Theradex APP Vendor Integration Integrations IntegrationVendor 5"
  type        = "String"
  value       = "theradex"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_enabled_5" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/5/Enabled"
  description = "Theradex APP Vendor Integration Integrations Enabled 5"
  type        = "String"
  value       = "true"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_awsregion_5" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/5/VendorSettings/AWSRegion"
  description = "Theradex APP Vendor Integration Integrations VendorSettings AWSRegion 5"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_accesskey_5" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/5/VendorSettings/AccessKey"
  description = "Theradex APP Vendor Integration Integrations VendorSettings AccessKey 5"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_secretkey_5" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/5/VendorSettings/SecretKey"
  description = "Theradex APP Vendor Integration Integrations VendorSettings SecretKey 5"
  type        = "String"
  value       = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_archivesubfolder_5" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/5/VendorSettings/ArchiveSubFolder"
  description = "Theradex APP Vendor Integration Integrations VendorSettings ArchiveSubFolder 5"
  type        = "String"
  value       = "External"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_url_5" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/5/VendorSettings/Url"
  description = "Theradex APP Vendor Integration Integrations VendorSettings Url 5"
  type        = "String"
  value       = "https://etctn-datashare.bpc-apps.nchri.org/api/"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_key_5" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/5/VendorSettings/Key"
  description = "Theradex APP Vendor Integration Integrations VendorSettings Key 5"
  type        = "String"
  value       = "HofR1ii80hNzEKSbiBYRfubPNrsA0trK2sQLcu2L"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_timeoutinsecs_5" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/5/VendorSettings/TimeoutInSecs"
  description = "Theradex APP Vendor Integration Integrations VendorSettings TimeoutInSecs 5"
  type        = "String"
  value       = "1200"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_endpoint_5" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/5/VendorSettings/EndPoint"
  description = "Theradex APP Vendor Integration Integrations VendorSettings EndPoint 5"
  type        = "String"
  value       = "queries/3/results.json"
}
###############################
##### IntegrationSettings RECONCILIATION_DATASHARE #####
###############################
resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_subjects_6" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/6/Subjects"
  description = "Theradex APP Vendor Integration Integrations Subjects 6"
  type        = "String"
  value       = "EMPTY"
}
resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_protcolnames_6" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/6/ProtocolNames"
  description = "Theradex APP Vendor Integration Integrations ProtocolNames 6"
  type        = "String"
  value       = "10330,10355,10358"
}
resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_environment_6" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/6/Environment"
  description = "Theradex APP Vendor Integration Integrations Environment 6"
  type        = "String"
  value       = "(PROD)"
}
resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_integrationform_6" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/6/IntegrationForm"
  description = "Theradex APP Vendor Integration Integrations IntegrationForm 6"
  type        = "String"
  value       = "RECONCILIATION_DATASHARE"
}
resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_integrationvendor_6" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/6/IntegrationVendor"
  description = "Theradex APP Vendor Integration Integrations IntegrationVendor 6"
  type        = "String"
  value       = "NationwideChildrens"
}
resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_enabled_6" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/6/Enabled"
  description = "Theradex APP Vendor Integration Integrations Enabled 6"
  type        = "String"
  value       = "true"
}
resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_awsregion_6" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/6/VendorSettings/AWSRegion"
  description = "Theradex APP Vendor Integration Integrations VendorSettings AWSRegion 6"
  type        = "String"
  value       = "EMPTY"
}
resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_accesskey_6" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/6/VendorSettings/AccessKey"
  description = "Theradex APP Vendor Integration Integrations VendorSettings AccessKey 6"
  type        = "String"
  value       = "EMPTY"
}
resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_secretkey_6" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/6/VendorSettings/SecretKey"
  description = "Theradex APP Vendor Integration Integrations VendorSettings SecretKey 6"
  type        = "String"
  value       = "EMPTY"
}
resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_archivesubfolder_6" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/6/VendorSettings/ArchiveSubFolder"
  description = "Theradex APP Vendor Integration Integrations VendorSettings ArchiveSubFolder 6"
  type        = "String"
  value       = "External"
}
resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_url_6" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/6/VendorSettings/Url"
  description = "Theradex APP Vendor Integration Integrations VendorSettings Url 6"
  type        = "String"
  value       = "https://etctn-datashare.bpc-apps.nchri.org/api/"
}
resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_key_6" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/6/VendorSettings/Key"
  description = "Theradex APP Vendor Integration Integrations VendorSettings Key 6"
  type        = "String"
  value       = "HofR1ii80hNzEKSbiBYRfubPNrsA0trK2sQLcu2L"
}
resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_timeoutinsecs_6" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/6/VendorSettings/TimeoutInSecs"
  description = "Theradex APP Vendor Integration Integrations VendorSettings TimeoutInSecs 6"
  type        = "String"
  value       = "1200"
}
resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_endpoint_6" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/6/VendorSettings/EndPoint"
  description = "Theradex APP Vendor Integration Integrations VendorSettings EndPoint 6"
  type        = "String"
  value       = "queries/3/results.json"
}
resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_s3deliverydestination_6" {
  name        = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/6/VendorSettings/S3DeliveryDestination"
  description = "Theradex APP Vendor Integration Integrations VendorSettings S3DeliveryDestination 6"
  type        = "String"
  value       = "theradex-biobank-share"
}

###############################
##### IntegrationSettings EET Inventory #####
###############################

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_subjects_7" {
  name          = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/7/Subjects"
  description   = "Theradex APP Vendor Integration Integrations Subjects 7"
  type          = "String"
  value         = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_protcolnames_7" {
  name          = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/7/ProtocolNames"
  description   = "Theradex APP Vendor Integration Integrations ProtocolNames 7"
  type          = "String"
  value         = "10330,10355,10358,10388,10434,10440,10499,10579,10538,10563"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_environment_7" {
  name          = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/7/Environment"
  description   = "Theradex APP Vendor Integration Integrations Environment 7"
  type          = "String"
  value         = "(PROD)"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_integrationform_7" {
  name          = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/7/IntegrationForm"
  description   = "Theradex APP Vendor Integration Integrations IntegrationForm 7"
  type          = "String"
  value         = "EET_INVENTORY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_integrationvendor_7" {
  name          = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/7/IntegrationVendor"
  description   = "Theradex APP Vendor Integration Integrations IntegrationVendor 7"
  type          = "String"
  value         = "EETBioBank"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_enabled_7" {
  name          = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/7/Enabled"
  description   = "Theradex APP Vendor Integration Integrations Enabled 7"
  type          = "String"
  value         = "true"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_awsregion_7" {
  name          = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/7/VendorSettings/AWSRegion"
  description   = "Theradex APP Vendor Integration Integrations VendorSettings AWSRegion 7"
  type          = "String"
  value         = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_accesskey_7" {
  name          = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/7/VendorSettings/AccessKey"
  description   = "Theradex APP Vendor Integration Integrations VendorSettings AccessKey 7"
  type          = "String"
  value         = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_secretkey_7" {
  name          = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/7/VendorSettings/SecretKey"
  description   = "Theradex APP Vendor Integration Integrations VendorSettings SecretKey 7"
  type          = "String"
  value         = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_archivesubfolder_7" {
  name          = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/7/VendorSettings/ArchiveSubFolder"
  description   = "Theradex APP Vendor Integration Integrations VendorSettings ArchiveSubFolder 7"
  type          = "String"
  value         = "External"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_url_7" {
  name          = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/7/VendorSettings/Url"
  description   = "Theradex APP Vendor Integration Integrations VendorSettings Url 7"
  type          = "String"
  value         = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_key_7" {
  name          = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/7/VendorSettings/Key"
  description   = "Theradex APP Vendor Integration Integrations VendorSettings Key 7"
  type          = "String"
  value         = "EMPTY"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_timeoutinsecs_7" {
  name          = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/7/VendorSettings/TimeoutInSecs"
  description   = "Theradex APP Vendor Integration Integrations VendorSettings TimeoutInSecs 7"
  type          = "String"
  value         = "1200"
}

resource "aws_ssm_parameter" "theradex_app_vendorintg_intgsettings_vendorsettings_endpoint_7" {
  name          = "/${var.vendorintg_env}/app/vendorintegration/IntegrationSettings/Integrations/7/VendorSettings/EndPoint"
  description   = "Theradex APP Vendor Integration Integrations VendorSettings EndPoint 7"
  type          = "String"
  value         = "EMPTY"
}