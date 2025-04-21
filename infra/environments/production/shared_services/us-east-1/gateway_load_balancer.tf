#####################################
##### Transit Gateway Resources #####
#####################################

/*
No Longer needed - Not using GLBP.

resource "aws_subnet" "security-vpc-us-east-1a-gwlb" {
    vpc_id = module.security_vpc.id
    availability_zone = "us-east-1a"
    cidr_block = "10.10.30.16/28"
    tags = {
        Name        = "security-vpc-us-east-1a-gwlb"
        environment = "Shared Services"
        owner       = "terraform"
        managed-by  = "terraform"
    }
}

resource "aws_subnet" "security-vpc-us-east-1b-gwlb" {
    vpc_id = module.security_vpc.id
    availability_zone = "us-east-1b"
    cidr_block = "10.10.31.16/28"
    tags = {
        Name        = "security-vpc-us-east-1b-gwlb"
        environment = "Shared Services"
        owner       = "terraform"
        managed-by  = "terraform"
    }
}

resource "aws_route_table" "security-vpc-us-east-1a-gwlb-route-1" {
    vpc_id = module.security_vpc.id
    route = []
    tags = {
        Name        = "security-vpc-us-east-1a-gwlb-route-1"
        environment = "Shared Services"
        owner       = "terraform"
        managed-by  = "terraform"
    }
}

resource "aws_route_table" "security-vpc-us-east-1b-gwlb-route-1" {
    vpc_id = module.security_vpc.id
    route = []
    tags = {
        Name        = "security-vpc-us-east-1b-gwlb-route-1"
        environment = "Shared Services"
        owner       = "terraform"
        managed-by  = "terraform"
    }
}

resource "aws_route_table_association" "security-vpc-us-east-1a-gwlb-rta-1" {
    subnet_id = aws_subnet.security-vpc-us-east-1a-gwlb.id
    route_table_id = aws_route_table.security-vpc-us-east-1a-gwlb-route-1.id
}

resource "aws_route_table_association" "security-vpc-us-east-1b-gwlb-rta-1" {
    subnet_id = aws_subnet.security-vpc-us-east-1b-gwlb.id
    route_table_id = aws_route_table.security-vpc-us-east-1b-gwlb-route-1.id
}

*/





