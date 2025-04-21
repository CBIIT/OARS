resource "aws_sns_topic" "codepipeline_integrationcodeowners_notification_topic" {
  name = "CodePipline-Intg-CodeOwner-Notification-Topic"

   delivery_policy = jsonencode({
    "http" : {
      "defaultHealthyRetryPolicy" : {
        "minDelayTarget" : 20,
        "maxDelayTarget" : 20,
        "numRetries" : 3,
        "numMaxDelayRetries" : 0,
        "numNoDelayRetries" : 0,
        "numMinDelayRetries" : 0,
        "backoffFunction" : "linear"
      },
      "disableSubscriptionOverrides" : false,
      "defaultThrottlePolicy" : {
        "maxReceivesPerSecond" : 1
      }
    }
  })
}

resource "aws_sns_topic_policy" "codepipeline_integrationcodeowners_notification_topic_policy" {
  arn = aws_sns_topic.codepipeline_integrationcodeowners_notification_topic.arn
  policy = jsonencode({
    Version = "2012-10-17",
    Id      = "__default_policy_ID",
    Statement = [
        {
            "Sid": "StatusNotificationsPolicy",
            "Effect": "Allow",
            "Principal": {
                "AWS": "*",
                "Service": "codestar-notifications.amazonaws.com"
            },
            "Action": "SNS:Publish",
            "Resource": aws_sns_topic.codepipeline_integrationcodeowners_notification_topic.arn
        },
        {
            "Sid": "__default_statement_ID",
            "Effect": "Allow",
            "Principal": {
                "AWS": "*"
            },
            "Action": [
                "SNS:GetTopicAttributes",
                "SNS:SetTopicAttributes",
                "SNS:AddPermission",
                "SNS:RemovePermission",
                "SNS:DeleteTopic",
                "SNS:Subscribe",
                "SNS:ListSubscriptionsByTopic",
                "SNS:Publish"
            ],
            "Resource": aws_sns_topic.codepipeline_integrationcodeowners_notification_topic.arn,
            "Condition": {
                "StringEquals": {
                    "AWS:SourceOwner": "${var.aws_accounts.theradex-shared-service.account_id}"
                }         
           }
        }
    ]
  })
}

locals {
  intg_codeowner_emails = ["uvarada@theradex.com", "mrathi@theradex.com"]
}

resource "aws_sns_topic_subscription" "codepipeline_integrationcodeowners_notification_subscription" {
  count     = length(local.intg_codeowner_emails)
  topic_arn = aws_sns_topic.codepipeline_integrationcodeowners_notification_topic.arn
  protocol  = "email"
  endpoint  = local.intg_codeowner_emails[count.index] 
}