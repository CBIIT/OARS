#########################
##### VPC Resources #####
#########################

#################################
# Non-Commercial Production VPC #
#################################

module "prod_noncomm_vpc" {
  source             = "../../../../modules/vpc"
  availability_zones = ["us-east-1a", "us-east-1b"]
  no_natgw           = true
  flowlog_retention  = 365
  settings = {
    main = {
      name   = "production-noncomm-vpc"
      cidr   = "10.16.0.0/16"
      region = "us-east-1"
    }
    us-east-1a = {
      cidr_public  = "10.16.0.0/24"
      cidr_private = "10.16.10.0/24"
      cidr_data    = "10.16.20.0/24"
      cidr_tgw     = "10.16.30.0/28"
    }
    us-east-1b = {
      cidr_public  = "10.16.1.0/24"
      cidr_private = "10.16.11.0/24"
      cidr_data    = "10.16.21.0/24"
      cidr_tgw     = "10.16.31.0/28"
    }
  }
  tags = {
    environment = "production-nci"
    owner       = "terraform"
    managed-by  = "terraform"
  }
}

resource "aws_network_acl_rule" "lan_ingress105" {
  for_each = {
    for region, id in module.prod_noncomm_vpc.private_network_acl :
    region => region
  }
  network_acl_id = module.prod_noncomm_vpc.private_network_acl[each.key]
  egress         = false
  protocol       = "-1"
  rule_number    = 101
  rule_action    = "allow"
  cidr_block     = "10.0.0.0/10"
  from_port      = 0
  to_port        = 0
}
