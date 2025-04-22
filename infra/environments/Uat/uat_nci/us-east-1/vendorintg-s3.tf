resource "aws_s3_bucket" "s3bucket_integration_files" {
  bucket = var.vendorintg_outputfile_s3_bucket
}