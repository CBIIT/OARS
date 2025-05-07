# # This resource creates an AWS WAFv2 Web ACL (Web Access Control List) to manage web traffic filtering for the load balancer.
# resource "aws_wafv2_web_acl" "lb-waf" {
#   # Defines the name and description of the Web ACL.
#   name        = "${var.oars_project_name}-${var.environment_name}-waf"
#   description = "WAF"
#   scope       = "REGIONAL" # Specifies the scope, which can be "REGIONAL" for regional resources or "CLOUDFRONT" for CloudFront distributions.

#   # Sets the default action for requests that don't match any rules (in this case, allow).
#   default_action {
#     allow {}
#   }
#   visibility_config {
#     cloudwatch_metrics_enabled = true
#     metric_name                = "${var.oars_project_name}-${var.environment_name}-waf-metrics" # Metric name for monitoring
#     sampled_requests_enabled   = true
#   }
#   # First rule: AWS Managed Rules Common Rule Set
#   rule {
#     name     = "my-lb-aws-managed-rules-common-rule-set" # The name for the rule
#     priority = 0                                         # Priority determines the order of rule evaluation, starting from 0.

#     # Override action to count requests matching this rule instead of blocking or allowing them.
#     override_action {
#       count {}
#     }

#     # Specifies a managed rule group statement to use AWS's common rule set for web security.
#     statement {
#       managed_rule_group_statement {
#         name        = "AWSManagedRulesCommonRuleSet" # Name of the AWS Managed Rule Group
#         vendor_name = "AWS"                          # Vendor providing the rule set, here it's AWS.
      
#       }
#     }

#     # Configures visibility settings for monitoring through CloudWatch.
#     visibility_config {
#       cloudwatch_metrics_enabled = true
#       metric_name                = "${var.oars_project_name}-${var.environment_name}-waf" # Metric name for monitoring
#       sampled_requests_enabled   = true
#     }
#   }

#   # Second rule: AWS Managed Rules IP Reputation List
#   rule {
#     name     = "my-lb-aws-managed-rules-ip-reputation-list" # Name for IP reputation rule
#     priority = 1                                            # Priority of this rule in the ACL evaluation order.

#     # Override action set to none, meaning no explicit action, just allows matching requests.
#     override_action {
#       none {}
#     }

#     # Uses AWS's managed IP reputation rule list to filter based on known malicious IPs.
#     statement {
#       managed_rule_group_statement {
#         name        = "AWSManagedRulesAmazonIpReputationList"
#         vendor_name = "AWS"
#       }
#     }

#     # Enables visibility settings for CloudWatch metrics.
#     visibility_config {
#       cloudwatch_metrics_enabled = true
#       metric_name                = "my-lb-aws-managed-rules-ip-reputation-list" # Metric name for this specific rule.
#       sampled_requests_enabled   = true
#     }
#   }

#   # Third rule: Blanket Rate Limit Rule
#   rule {
#     name     = "my-lb-blanket-rate-limit" # Name for the blanket rate-limiting rule
#     priority = 2                          # Priority of this rule in the ACL evaluation order.


#     # Action to block requests exceeding the rate limit.
#     action {
#       block {}
#     }

#     # Defines a rate-based rule that blocks IPs sending more than 10,000 requests within a 5-minute window.
#     statement {
#       rate_based_statement {
#         limit              = 10000 # Rate limit threshold.
#         aggregate_key_type = "IP"  # Aggregate requests by IP address.
#       }
#     }

#     # Enables visibility settings for this rate-limiting rule.
#     visibility_config {
#       cloudwatch_metrics_enabled = true
#       metric_name                = "my-lb-blanket-rate-limit" # Metric name for this rate-limit rule.
#       sampled_requests_enabled   = true
#     }
#   }
#   rule {
#     name     = "allow-International-traffic-only"
#     priority = 3

#     action {
#       block {}
#     }

#     statement {
#       not_statement {
#         statement {
#           geo_match_statement {
#             country_codes = ["US", "CA", "IN", "MX", "GB"]
#           }
#         }
#       }
#     }

#     visibility_config {
#       cloudwatch_metrics_enabled = true
#       metric_name                = "AllowINTERNATIONALTrafficOnly"
#       sampled_requests_enabled   = true
#     }
#   }
# }

# # This resource associates the WAF Web ACL with a specific Load Balancer ARN or other AWS resource.

# # resource "aws_wafv2_web_acl_association" "lb-waf" {
# #   resource_arn = module.oars-dev-alb.target_group_arn
# #   web_acl_arn  = aws_wafv2_web_acl.lb-waf.arn   # ARN of the Web ACL created above.
# # }
# # Get the ALB ARN
# data "aws_lb" "existing_lb" {
#   name = "${var.oars_project_name}-${var.environment_name}-lb"
# }

# # Associate the Web ACL with the ALB
# resource "aws_wafv2_web_acl_association" "waf_alb" {
#   resource_arn = data.aws_lb.existing_lb.arn
#   web_acl_arn  = aws_wafv2_web_acl.lb-waf.arn
# }

# # Optional: Create CloudWatch logging for WAF
# resource "aws_cloudwatch_log_group" "waf_log_group" {
#   name              = "/aws/waf/us-traffic-only"
#   retention_in_days = 30
# }

# # resource "aws_wafv2_web_acl_logging_configuration" "waf_logging" {
# #   log_destination_configs = ["${aws_cloudwatch_log_group.waf_log_group.arn}:*"]
# #   resource_arn            = aws_wafv2_web_acl.lb-waf.arn
# # }

