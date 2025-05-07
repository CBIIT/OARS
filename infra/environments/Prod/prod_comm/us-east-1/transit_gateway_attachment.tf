#####################################
##### Transit Gateway Resources #####
#####################################

resource "aws_subnet" "prod-comm-vpc-us-east-1a-tgw" {
    vpc_id = module.prod_comm_vpc.id
    availability_zone = "us-east-1a"
    cidr_block = "10.15.30.0/28"
    tags = {
        Name        = "prod-comm-vpc-us-east-1a-tgw"
        environment = "Shared Services"
        owner       = "terraform"
        managed-by  = "terraform"
    }
}

resource "aws_subnet" "prod-comm-vpc-us-east-1b-tgw" {
    vpc_id = module.prod_comm_vpc.id
    availability_zone = "us-east-1b"
    cidr_block = "10.15.31.0/28"
    tags = {
        Name        = "prod-comm-vpc-us-east-1b-tgw"
        environment = "Shared Services"
        owner       = "terraform"
        managed-by  = "terraform"
    }
}

resource "aws_route" "tgw_route_private" {
  for_each               = {
    for region, id in module.prod_comm_vpc.private_route_tables:
    region => region
  }
  route_table_id         = module.prod_comm_vpc.private_route_tables[each.key]
  destination_cidr_block = "0.0.0.0/0"
  transit_gateway_id     = var.secure-vpc-tgw-1-id
}

# resource "aws_route" "tgw_route_public" {
#   for_each               = {
#     for region, id in module.prod_comm_vpc.public_route_tables:
#     region => region
#   }
#   route_table_id         = module.prod_comm_vpc.public_route_tables[each.key]
#   destination_cidr_block = "0.0.0.0/0"
#   transit_gateway_id     = var.secure-vpc-tgw-1-id
# }

resource "aws_ec2_transit_gateway_vpc_attachment" "secure-vpc-tgwa-1" {
    subnet_ids = [ "${aws_subnet.prod-comm-vpc-us-east-1a-tgw.id}", "${aws_subnet.prod-comm-vpc-us-east-1b-tgw.id}" ]
    transit_gateway_id = var.secure-vpc-tgw-1-id
    vpc_id = module.prod_comm_vpc.id
}

