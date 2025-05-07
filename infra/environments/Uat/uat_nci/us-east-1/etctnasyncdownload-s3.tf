resource "aws_s3_bucket" "s3bucket_etctnasyncdownload_files" {
  bucket = var.etctnasyncdownload_files_s3_bucket
}