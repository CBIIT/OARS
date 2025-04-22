 /*
Deploy fortigate resources to current security VPC
Current configuration:

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

*/

// Creating Public EIP address
resource "aws_eip" "ClusterPublicIP" {
  depends_on        = [aws_instance.fgtpassive]
  domain = "vpc"
  network_interface = aws_network_interface.eth0.id
}

resource "aws_eip" "MGMTPublicIP" {
  depends_on        = [aws_instance.fgtactive]
  domain = "vpc"
  network_interface = aws_network_interface.eth3.id
}

resource "aws_eip" "PassiveMGMTPublicIP" {
  depends_on        = [aws_instance.fgtpassive]
  domain = "vpc"  
  network_interface = aws_network_interface.passiveeth3.id
}

#HA Subnets

resource "aws_subnet" "security-vpc-us-east-1a-ftg-ha" {
    vpc_id = module.security_vpc.id
    availability_zone = "us-east-1a"
    cidr_block = "10.10.32.0/24"
    tags = {
        Name        = "security-vpc-us-east-1a-ftg-ha"
        environment = "Shared Services"
        owner       = "terraform"
        managed-by  = "terraform"
    }
}

resource "aws_subnet" "security-vpc-us-east-1b-ftg-ha" {
    vpc_id = module.security_vpc.id
    availability_zone = "us-east-1b"
    cidr_block = "10.10.33.0/24"
    tags = {
        Name        = "security-vpc-us-east-1b-ftg-ha"
        environment = "Shared Services"
        owner       = "terraform"
        managed-by  = "terraform"
    }
}

# Mgmt Subnets

resource "aws_subnet" "security-vpc-us-east-1a-ftg-mgmt" {
    vpc_id = module.security_vpc.id
    availability_zone = "us-east-1a"
    cidr_block = "10.10.34.0/24"
    tags = {
        Name        = "security-vpc-us-east-1a-ftg-mgmt"
        environment = "Shared Services"
        owner       = "terraform"
        managed-by  = "terraform"
    }
}

resource "aws_subnet" "security-vpc-us-east-1b-ftg-mgmt" {
    vpc_id = module.security_vpc.id
    availability_zone = "us-east-1b"
    cidr_block = "10.10.35.0/24"
    tags = {
        Name        = "security-vpc-us-east-1b-ftg-mgmt"
        environment = "Shared Services"
        owner       = "terraform"
        managed-by  = "terraform"
    }
}