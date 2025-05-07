#S3 Bucket for Development OARS-Development
resource "aws_s3_bucket" "nci-oars-dev" {
   bucket = var.nci-oars-dev-bucket-name
   tags             = var.tags

}

resource "aws_s3_bucket_public_access_block" "nci-oars-dev" {
   bucket = aws_s3_bucket.nci-oars-dev.id
   block_public_acls       = false
   block_public_policy     = false
   ignore_public_acls      = false
   restrict_public_buckets = false
}

resource "aws_s3_bucket_policy" "nci-oars-dev-bucketpolicy" {
   bucket = aws_s3_bucket.nci-oars-dev.id
   depends_on = [ aws_s3_object.dashboardhelp_object ]
   policy = <<EOF
{ 
   "Version": "2012-10-17",
   "Statement": [
     {
       "Effect": "Allow",
       "Principal": {"AWS": "*"},
       "Action": ["s3:GetObject"],
       "Resource": ["arn:aws:s3:::nci-oars-dev/DashboardHelp/*"]
     }
   ]
}
 EOF
}

resource "aws_s3_object" "dashboardhelp_object" {
   bucket = aws_s3_bucket.nci-oars-dev.id
   key = var.nci-oars-dev-object-name
   content = ""
   tags             = var.tags

}