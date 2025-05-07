resource "aws_s3_bucket" "s3bucket_etctnasyncdownload_files" {
  bucket = var.etctnasyncdownload_files_s3_bucket
}
resource "aws_s3_bucket_server_side_encryption_configuration" "s3bucket_etctnasyncdownload_files_encryption" {
  bucket = var.etctnasyncdownload_files_s3_bucket
  rule {
    apply_server_side_encryption_by_default {
      kms_master_key_id = module.kms.s3_key_arn
      sse_algorithm     = "aws:kms"
    }
  }
}