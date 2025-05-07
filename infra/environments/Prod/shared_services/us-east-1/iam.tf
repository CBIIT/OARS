#########################
##### IAM Resources #####
#########################


#############################
# EC2 Instance Profile Role #
#############################

module "ec2_iam_profile_role" {
  source  = "../../../../modules/iam/general"
}

#FTG EC2 IAM Role and Policy
resource "aws_iam_policy" "FTG_EC2_Policy" {
  name        = "FTG_EC2_Policy"
  description = "Fortigate EC2 API Access"
  path        = "/"

  policy = <<EOF
{

  "Version": "2012-10-17",

  "Statement": [

  {

  "Action": [

  "ec2:Describe*",

  "ec2:AssociateAddress",

  "ec2:AssignPrivateIpAddresses",

  "ec2:UnassignPrivateIpAddresses",

  "ec2:ReplaceRoute"

  ],

  "Resource": "*",

  "Effect": "Allow"

  }

  ]

}
EOF
}

resource "aws_iam_role" "FTG_EC2_Role" {
  name        = "FTG_EC2_Role"
  description = "Fortigate EC2 API Access"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Sid    = ""
        Principal = {
          Service = "ec2.amazonaws.com"
        }
      },
    ]
  })
}

resource "aws_iam_role_policy_attachment" "FTG_EC2_Role" {
  role       = aws_iam_role.FTG_EC2_Role.name
  policy_arn = aws_iam_policy.FTG_EC2_Policy.arn
}

resource "aws_iam_instance_profile" "FTG_EC2_Role" {
  name  = "FTG_EC2_Role"
  role  = aws_iam_role.FTG_EC2_Role.name
}