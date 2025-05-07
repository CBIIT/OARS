#Iam Role to create Bastion Host 
data "aws_iam_policy_document" "bastion" {
  statement {
    actions = ["sts:AssumeRole"]
    principals {
      type        = "Service"
      identifiers = ["ec2.amazonaws.com"]
    }
  }

}
data "aws_iam_policy_document" "bastion_role_policies" {

  statement {
    actions   = ["s3:GetObject"]
    resources = ["arn:aws:s3:::aws-quicksetup-patchpolicy-*"]
  }

  statement {
    actions   = ["cloudformation:DescribeStackResource", "cloudformation:SignalResource"] 
    resources = ["arn:aws:cloudformation:us-east-1:993530973844:stack/nci-web-reporting-dev/072b7330-e42e-11ed-b852-0ede2622022f"]
  }

  statement {
    actions   = ["ssmmessages:*", "ssm:UpdateInstanceInformation", "ec2messages:*"]
    resources = ["*"] 
  }
}

resource "aws_iam_role" "bastion_role" {
  name               = "BastionHostInstanceRole"
  assume_role_policy = data.aws_iam_policy_document.bastion.json
  managed_policy_arns= ["arn:aws:iam::aws:policy/AmazonSSMManagedInstanceCore"]
  inline_policy {
    name   = "${var.project_name}-${var.environment_name}-bastion-policy"
    policy = data.aws_iam_policy_document.bastion_role_policies.json
  }
  tags = merge({"ProjectName"= var.project_name},{"EnvironmentName"= var.environment_name},var.tags)

  lifecycle {
    ignore_changes = [tags ["QSConfigId-079hl"]]
  }
}

resource "aws_iam_instance_profile" "profile" {
  name = "bastionprofile"
  role = aws_iam_role.bastion_role.name
}