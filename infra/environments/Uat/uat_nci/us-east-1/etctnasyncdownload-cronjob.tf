############################
##### Role/Policy #####
############################

resource "aws_iam_role" "etctnasyncdownload-sf_scheduler_role" {
  name = "etctnasyncdownload-sf_scheduler_role"

  assume_role_policy = <<EOF
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Principal": {
                "Service": "scheduler.amazonaws.com"
            },
            "Action": "sts:AssumeRole",
            "Condition": {
                "StringEquals": {
                    "aws:SourceAccount": "${local.myaccount}"
                }
            }
        }
    ]
}
EOF
}

resource "aws_iam_policy" "etctnasyncdownload-sf_scheduler_role_executepolicy" {
  name = "etctnasyncdownload-sf_scheduler_role_executepolicy"

  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [
      {
        "Effect" : "Allow",
        "Action" : [
          "states:StartExecution"
        ],
        "Resource" : [
          "${aws_sfn_state_machine.etctnasyncdownload-init-sf_state_machine.arn}"
        ]
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "etctnasyncdownload-sf_scheduler_role_policy_attachment1" {
  role       = aws_iam_role.etctnasyncdownload-sf_scheduler_role.name
  policy_arn = aws_iam_policy.etctnasyncdownload-sf_scheduler_role_executepolicy.arn
}

############################
##### Amazon EventBridge Scheduler for PreAnalytics #####
############################

resource "aws_scheduler_schedule" "etctn-async_download-preanalytics_scheduler" {
  name       = "etctn_async_download_preanalytics"
  group_name = "default"

  flexible_time_window {
    mode = "OFF"
  }

  state = "ENABLED"

  start_date = "2024-08-15T06:05:00Z" //06:05am UTC to 01:05am EST

  schedule_expression = "rate(24 hours)"

  schedule_expression_timezone = "America/New_York"

  target {
    arn = aws_sfn_state_machine.etctnasyncdownload-init-sf_state_machine.arn

    role_arn = aws_iam_role.etctnasyncdownload-sf_scheduler_role.arn

    input = jsonencode({ "form_oid" : "PREANALYTIC_RESULTS", "query_id" : 16, "protocols" : [10334, 10388, 10300, 10358, 10355] })

    retry_policy {
      maximum_retry_attempts = 0
    }
  }
}

############################
##### Amazon EventBridge Scheduler for SolidTissue #####
############################

resource "aws_scheduler_schedule" "etctn-async_download-solidtissue_scheduler" {
  name       = "etctn_async_download_solidtissue"
  group_name = "default"

  flexible_time_window {
    mode = "OFF"
  }

  state = "ENABLED"

  start_date = "2024-08-15T06:15:00Z" //06:15am UTC to 01:15am EST

  schedule_expression = "rate(24 hours)"

  schedule_expression_timezone = "America/New_York"

  target {
    arn = aws_sfn_state_machine.etctnasyncdownload-init-sf_state_machine.arn

    role_arn = aws_iam_role.etctnasyncdownload-sf_scheduler_role.arn

    input = jsonencode({ "form_oid" : "SOLID_TISSUE_PATHOLOGY_VERIFICATION_AND_ASSESSMENT", "query_id" : 15, "protocols" : [10334, 10388, 10300, 10358, 10355] })

    retry_policy {
      maximum_retry_attempts = 0
    }
  }
}

############################
##### Amazon EventBridge Scheduler for Hematology #####
############################

resource "aws_scheduler_schedule" "etctn-async_download-hematology_scheduler" {
  name       = "etctn_async_download_hematology"
  group_name = "default"

  flexible_time_window {
    mode = "OFF"
  }

  state = "ENABLED"

  start_date = "2024-08-15T06:25:00Z" //06:25am UTC to 01:25am EST

  schedule_expression = "rate(24 hours)"

  schedule_expression_timezone = "America/New_York"

  target {
    arn = aws_sfn_state_machine.etctnasyncdownload-init-sf_state_machine.arn

    role_arn = aws_iam_role.etctnasyncdownload-sf_scheduler_role.arn

    input = jsonencode({ "form_oid" : "HEMATOLOGIC_MALIGNANCY_VERIFICATION_AND_ASSESSMENT", "query_id" : 15, "protocols" : [10334, 10388, 10300, 10358, 10355] })

    retry_policy {
      maximum_retry_attempts = 0
    }
  }
}

############################
##### Amazon EventBridge Scheduler for ShippingStatus #####
############################

resource "aws_scheduler_schedule" "etctn-async_download-shippingstatus_scheduler" {
  name       = "etctn_async_download_shippingstatus"
  group_name = "default"

  flexible_time_window {
    mode = "OFF"
  }

  state = "ENABLED"

  start_date = "2024-08-15T06:35:00Z" //06:35am UTC to 01:35am EST

  schedule_expression = "rate(24 hours)"

  schedule_expression_timezone = "America/New_York"

  target {
    arn = aws_sfn_state_machine.etctnasyncdownload-init-sf_state_machine.arn

    role_arn = aws_iam_role.etctnasyncdownload-sf_scheduler_role.arn

    input = jsonencode({ "form_oid" : "SHIPPING_STATUS", "query_id" : 17, "protocols" : [10334, 10388, 10300, 10358, 10355] })

    retry_policy {
      maximum_retry_attempts = 0
    }
  }
}

############################
##### Amazon EventBridge Scheduler for ReceivingStatus #####
############################

resource "aws_scheduler_schedule" "etctn-async_download-receivingstatus_scheduler" {
  name       = "etctn_async_download_receivingstatus"
  group_name = "default"

  flexible_time_window {
    mode = "OFF"
  }

  state = "ENABLED"

  start_date = "2024-08-15T06:45:00Z" //06:45am UTC to 01:45am EST

  schedule_expression = "rate(24 hours)"

  schedule_expression_timezone = "America/New_York"

  target {
    arn = aws_sfn_state_machine.etctnasyncdownload-init-sf_state_machine.arn

    role_arn = aws_iam_role.etctnasyncdownload-sf_scheduler_role.arn

    input = jsonencode({ "form_oid" : "RECEIVING_STATUS", "query_id" : 18, "protocols" : [10334, 10388, 10300, 10358, 10355] })

    retry_policy {
      maximum_retry_attempts = 0
    }
  }
}

############################
##### Amazon EventBridge Scheduler for Inventory #####
############################

resource "aws_scheduler_schedule" "etctn-async_download-inventory_scheduler" {
  name       = "etctn_async_download_inventory"
  group_name = "default"

  flexible_time_window {
    mode = "OFF"
  }

  state = "ENABLED"

  start_date = "2024-08-15T06:55:00Z" //06:55am UTC to 01:55am EST

  schedule_expression = "rate(24 hours)"

  schedule_expression_timezone = "America/New_York"

  target {
    arn = aws_sfn_state_machine.etctnasyncdownload-init-sf_state_machine.arn

    role_arn = aws_iam_role.etctnasyncdownload-sf_scheduler_role.arn

    input = jsonencode({ "form_oid" : "EET_INVENTORY", "query_id" : 14, "protocols" : [10334, 10388, 10300, 10358, 10355] })

    retry_policy {
      maximum_retry_attempts = 0
    }
  }
}


#Amazon_EventBridge_Scheduler_SFN_5df304ce27