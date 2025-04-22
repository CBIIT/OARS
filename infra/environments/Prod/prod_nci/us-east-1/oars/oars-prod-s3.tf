#S3 Bucket for Production OARS
resource "aws_s3_bucket" "nci-oars" {
   bucket = var.nci-oars-bucket-name
}

resource "aws_s3_bucket_public_access_block" "nci-oars" {
   bucket = aws_s3_bucket.nci-oars.id
   block_public_acls       = false
   block_public_policy     = false
   ignore_public_acls      = false
   restrict_public_buckets = false
}

resource "aws_s3_bucket_policy" "nci-oars-bucketpolicy" {
   bucket = aws_s3_bucket.nci-oars.id
   depends_on = [ aws_s3_object.dashboardhelp_object ]
   policy = <<EOF
{ 
   "Version": "2012-10-17",
   "Statement": [
     {
       "Effect": "Allow",
       "Principal": {"AWS": "*"},
       "Action": ["s3:GetObject"],
       "Resource": ["arn:aws:s3:::nci-oars/DashboardHelp/*"]
     }
   ]
}
 EOF
}

resource "aws_s3_object" "dashboardhelp_object" {
   bucket = aws_s3_bucket.nci-oars.id
   key = var.nci-oars-object-name
   content = ""

   lifecycle {
      ignore_changes = [
         tags,
         tags_all
      ]
   }
}