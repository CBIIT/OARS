resource "aws_iam_role_policy_attachment" "dlm_worker" {
  policy_arn = aws_iam_policy.dlm_worker.arn
  role       = split("role/", var.dlm_lifecycle_role_arn)[1]
}

resource "aws_iam_policy" "dlm_worker" {
  name = "${var.name}-dlm-backup"

  policy = <<POLICY
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Action": [
        "iam:GetRole",
        "iam:GetUser",
        "iam:ListRoles",
        "iam:PassRole"
      ],
      "Resource": "*",
      "Effect": "Allow"
    },
    {
      "Action": "dlm:*",
      "Resource": "*",
      "Effect": "Allow"
    },
    {
      "Action": [
        "ec2:CreateSnapshot",
        "ec2:CreateSnapshots",
        "ec2:CreateTags",
        "ec2:DeleteSnapshot",
        "ec2:DescribeInstances",
        "ec2:DescribeSnapshots",
        "ec2:DescribeVolumes",
        "ec2:ModifySnapshotAttribute",
        "ec2:ResetSnapshotAttribute"
      ],
      "Resource": "*",
      "Effect": "Allow"
    },
    {
      "Action": "logs:*",
      "Resource": "arn:aws:logs:*:*:*",
      "Effect": "Allow"
    }
  ]
}
POLICY
}
