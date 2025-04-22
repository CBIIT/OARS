#S3 Bucket for UAT OARS
resource "aws_s3_bucket" "nci-oars-uat" {
   bucket = var.nci-oars-uat-bucket-name
   tags = var.tags
}

resource "aws_s3_bucket_public_access_block" "nci-oars-uat" {
   bucket = aws_s3_bucket.nci-oars-uat.id
   block_public_acls       = false
   block_public_policy     = false
   ignore_public_acls      = false
   restrict_public_buckets = false
}

resource "aws_s3_bucket_policy" "nci-oars-uat-bucketpolicy" {
   bucket = aws_s3_bucket.nci-oars-uat.id
   depends_on = [ aws_s3_object.dashboardhelp_object ]
   policy = <<EOF
{ 
   "Version": "2012-10-17",
   "Statement": [
     {
       "Effect": "Allow",
       "Principal": {"AWS": "*"},
       "Action": ["s3:GetObject"],
       "Resource": ["arn:aws:s3:::nci-oars-uat/DashboardHelp/*"]
     }
   ]
}
 EOF
}

resource "aws_s3_object" "dashboardhelp_object" {
   bucket = aws_s3_bucket.nci-oars-uat.id
   key = var.nci-oars-uat-object-name
   content = ""
   tags = var.tags
}