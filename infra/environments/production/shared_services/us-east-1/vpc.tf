#########################
##### VPC Resources #####
#########################

######################
# Security VPC #
######################

module "security_vpc" {
  source = "../../../../modules/vpc"
  availability_zones = ["us-east-1a", "us-east-1b"]
  settings = {
    main = {
      name   = "security-vpc"
      cidr   = "10.10.0.0/16"
      region = "us-east-1"
    }
    us-east-1a = {
      cidr_public  = "10.10.0.0/24"
      cidr_private = "10.10.10.0/24"
      cidr_data    = "10.10.20.0/24"
    }
    us-east-1b = {
      cidr_public  = "10.10.1.0/24"
      cidr_private = "10.10.11.0/24"
      cidr_data    = "10.10.21.0/24"
    }
  }
  tags = {
    environment = "Shared Services"
    owner       = "terraform"
    managed-by  = "terraform"
  }
}

resource "aws_route" "security_vpc_additional_routes_to_tgw-1" {
  count                  = length(data.aws_route_tables.private_route_tables.ids)
  route_table_id         = tolist(data.aws_route_tables.private_route_tables.ids)[count.index]
  destination_cidr_block = "10.0.0.0/11"
  transit_gateway_id     = module.secure-vpc-tgw-1.id
}

resource "aws_route" "security_vpc_additional_routes_to_tgw-2" {
  count                  = length(data.aws_route_tables.private_route_tables.ids)
  route_table_id         = tolist(data.aws_route_tables.private_route_tables.ids)[count.index]
  destination_cidr_block = "172.16.32.0/19"
  transit_gateway_id     = module.secure-vpc-tgw-1.id
}