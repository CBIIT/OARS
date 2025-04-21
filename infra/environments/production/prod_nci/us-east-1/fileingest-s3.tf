resource "aws_s3_bucket" "s3bucket_fileingest_files" {
  bucket = var.fileingest_files_s3_bucket
}