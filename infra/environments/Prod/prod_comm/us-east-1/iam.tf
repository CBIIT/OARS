#########################
##### IAM Resources #####
#########################


#############################
# EC2 Instance Profile Role #
#############################

module "ec2_iam_profile_role" {
  source  = "../../../../modules/iam/general"
}