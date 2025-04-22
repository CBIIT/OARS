#S3 Bucket for OARS-QA
resource "aws_s3_bucket" "nci-oars-qa" {
   bucket = var.nci-oars-qa-bucket-name
   tags             = var.tags_qa

}

resource "aws_s3_bucket_public_access_block" "nci-oars-qa" {
   bucket = aws_s3_bucket.nci-oars-qa.id
   block_public_acls       = false
   block_public_policy     = false
   ignore_public_acls      = false
   restrict_public_buckets = false
}

resource "aws_s3_bucket_policy" "nci-oars-qa-bucketpolicy" {
   bucket = aws_s3_bucket.nci-oars-qa.id
   depends_on = [ aws_s3_object.dashboardhelp_object ]
   policy = <<EOF
{ 
   "Version": "2012-10-17",
   "Statement": [
     {
       "Effect": "Allow",
       "Principal": {"AWS": "*"},
       "Action": ["s3:GetObject"],
       "Resource": ["arn:aws:s3:::nci-oars-qa/DashboardHelp/*"]
     }
   ]
}
 EOF
}

resource "aws_s3_object" "dashboardhelp_object_qa" {
   bucket = aws_s3_bucket.nci-oars-qa.id
   key = var.nci-oars-qa-object-name
   tags             = var.tags_qa
   content = ""
}