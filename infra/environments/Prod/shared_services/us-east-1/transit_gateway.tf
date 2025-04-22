#####################################
##### Transit Gateway Resources #####
#####################################

resource "aws_subnet" "security-vpc-us-east-1a-tgw" {
    vpc_id = module.security_vpc.id
    availability_zone = "us-east-1a"
    cidr_block = "10.10.30.0/28"
    tags = {
        Name        = "security-vpc-us-east-1a-tgw"
        environment = "Shared Services"
        owner       = "terraform"
        managed-by  = "terraform"
    }
}

resource "aws_subnet" "security-vpc-us-east-1b-tgw" {
    vpc_id = module.security_vpc.id
    availability_zone = "us-east-1b"
    cidr_block = "10.10.31.0/28"
    tags = {
        Name        = "security-vpc-us-east-1b-tgw"
        environment = "Shared Services"
        owner       = "terraform"
        managed-by  = "terraform"
    }
}

resource "aws_route_table" "security-vpc-us-east-tgw-rt" {
  vpc_id = module.security_vpc.id

  tags = {
    Name = "security-vpc-us-east-tgw-rt"
  }
}

resource "aws_route_table_association" "security-vpc-us-east-tgw-rta-a" {
  subnet_id      = aws_subnet.security-vpc-us-east-1a-tgw.id
  route_table_id = aws_route_table.security-vpc-us-east-tgw-rt.id
}

resource "aws_route_table_association" "security-vpc-us-east-tgw-rta-b" {
  subnet_id      = aws_subnet.security-vpc-us-east-1b-tgw.id
  route_table_id = aws_route_table.security-vpc-us-east-tgw-rt.id
}


module "secure-vpc-tgw-1" {
    source = "../../../../modules/transit-gateway"
    allow_external_principals = true
    name = "secure-vpc-tgw-1"
    target_share_accounts = [for k, v in var.aws_accounts : v["account_id"] if k != "theradex-shared-service"]
}

resource "aws_ec2_transit_gateway_vpc_attachment" "secure-vpc-tgwa-1" {
    subnet_ids = [ "${aws_subnet.security-vpc-us-east-1a-tgw.id}", "${aws_subnet.security-vpc-us-east-1b-tgw.id}" ]
    transit_gateway_id = module.secure-vpc-tgw-1.id
    vpc_id = module.security_vpc.id
    appliance_mode_support = "enable"
    tags = {
        Name = "security-vpc"
    }
}

resource "aws_ec2_transit_gateway_route" "secure-vpc-tgw-default-route" {
  destination_cidr_block         = "0.0.0.0/0"
  transit_gateway_attachment_id  = aws_ec2_transit_gateway_vpc_attachment.secure-vpc-tgwa-1.id
  transit_gateway_route_table_id = module.secure-vpc-tgw-1.association_default_route_table_id
}

resource "aws_route" "secure-vpc-tgw-subnet-default-route" {
  route_table_id         = aws_route_table.security-vpc-us-east-tgw-rt.id
  destination_cidr_block = "0.0.0.0/0"
  network_interface_id     = aws_network_interface.eth1.id
}

resource "aws_route" "tgw_route_private_dev_comm" {
  for_each               = {
    for region, id in module.security_vpc.private_route_tables:
    region => region
  }
  route_table_id         = module.security_vpc.private_route_tables[each.key]
  destination_cidr_block = "10.11.0.0/16"
  transit_gateway_id     = module.secure-vpc-tgw-1.id
}

resource "aws_route" "tgw_route_public_dev_comm" {
  for_each               = {
    for region, id in module.security_vpc.public_route_tables:
    region => region
  }
  route_table_id         = module.security_vpc.public_route_tables[each.key]
  destination_cidr_block = "10.11.0.0/16"
  transit_gateway_id     = module.secure-vpc-tgw-1.id
}

resource "aws_route" "tgw_route_private_dev_nci" {
  for_each               = {
    for region, id in module.security_vpc.private_route_tables:
    region => region
  }
  route_table_id         = module.security_vpc.private_route_tables[each.key]
  destination_cidr_block = "10.12.0.0/16"
  transit_gateway_id     = module.secure-vpc-tgw-1.id
}

resource "aws_route" "tgw_route_public_dev_nci" {
  for_each               = {
    for region, id in module.security_vpc.public_route_tables:
    region => region
  }
  route_table_id         = module.security_vpc.public_route_tables[each.key]
  destination_cidr_block = "10.12.0.0/16"
  transit_gateway_id     = module.secure-vpc-tgw-1.id
}
resource "aws_route" "tgw_route_private_uat_comm" {
  for_each               = {
    for region, id in module.security_vpc.private_route_tables:
    region => region
  }
  route_table_id         = module.security_vpc.private_route_tables[each.key]
  destination_cidr_block = "10.13.0.0/16"
  transit_gateway_id     = module.secure-vpc-tgw-1.id
}

resource "aws_route" "tgw_route_public_uat_comm" {
  for_each               = {
    for region, id in module.security_vpc.public_route_tables:
    region => region
  }
  route_table_id         = module.security_vpc.public_route_tables[each.key]
  destination_cidr_block = "10.13.0.0/16"
  transit_gateway_id     = module.secure-vpc-tgw-1.id
}
resource "aws_route" "tgw_route_private_uat_nci" {
  for_each               = {
    for region, id in module.security_vpc.private_route_tables:
    region => region
  }
  route_table_id         = module.security_vpc.private_route_tables[each.key]
  destination_cidr_block = "10.14.0.0/16"
  transit_gateway_id     = module.secure-vpc-tgw-1.id
}

resource "aws_route" "tgw_route_public_uat_nci" {
  for_each               = {
    for region, id in module.security_vpc.public_route_tables:
    region => region
  }
  route_table_id         = module.security_vpc.public_route_tables[each.key]
  destination_cidr_block = "10.14.0.0/16"
  transit_gateway_id     = module.secure-vpc-tgw-1.id
}
resource "aws_route" "tgw_route_private_prod_comm" {
  for_each               = {
    for region, id in module.security_vpc.private_route_tables:
    region => region
  }
  route_table_id         = module.security_vpc.private_route_tables[each.key]
  destination_cidr_block = "10.15.0.0/16"
  transit_gateway_id     = module.secure-vpc-tgw-1.id
}

resource "aws_route" "tgw_route_public_prod_comm" {
  for_each               = {
    for region, id in module.security_vpc.public_route_tables:
    region => region
  }
  route_table_id         = module.security_vpc.public_route_tables[each.key]
  destination_cidr_block = "10.15.0.0/16"
  transit_gateway_id     = module.secure-vpc-tgw-1.id
}
resource "aws_route" "tgw_route_private_prod_nci" {
  for_each               = {
    for region, id in module.security_vpc.private_route_tables:
    region => region
  }
  route_table_id         = module.security_vpc.private_route_tables[each.key]
  destination_cidr_block = "10.16.0.0/16"
  transit_gateway_id     = module.secure-vpc-tgw-1.id
}

resource "aws_route" "tgw_route_public_prod_nci" {
  for_each               = {
    for region, id in module.security_vpc.public_route_tables:
    region => region
  }
  route_table_id         = module.security_vpc.public_route_tables[each.key]
  destination_cidr_block = "10.16.0.0/16"
  transit_gateway_id     = module.secure-vpc-tgw-1.id
}
