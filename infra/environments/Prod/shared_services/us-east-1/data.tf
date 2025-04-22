#search all private route tables in shared services account by Name tag key and value of "shared-services-vpc*"
#used in vpc.tf file to apply route to multiple route tables simulteneously
data "aws_route_tables" "private_route_tables" {
  vpc_id = module.security_vpc.id

  filter {
    name   = "tag:Name"
    values = ["security-vpc-us-east-1a-private","security-vpc-us-east-1b-private"]
  }
}