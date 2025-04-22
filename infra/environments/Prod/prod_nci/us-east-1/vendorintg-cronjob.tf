resource "aws_iam_role" "vendorintg_ecs_events_task_role" {
  name = "vendorintg-ecsEventsTaskRole"

  assume_role_policy = <<EOF
{
   "Version":"2012-10-17",
   "Statement":[
      {
         "Effect":"Allow",
         "Principal":{
            "Service":[
               "ecs-tasks.amazonaws.com"
            ]
         },
         "Action":"sts:AssumeRole",
         "Condition":{
            "ArnLike":{
            "aws:SourceArn":"arn:aws:ecs:us-east-1:${local.myaccount}:*"
            },
            "StringEquals":{
               "aws:SourceAccount":"${local.myaccount}"
            }
         }
      }
   ]
}
EOF
}

resource "aws_iam_role_policy_attachment" "vendorintg_ecs_events_task_role_policy_attachment" {
  role       = aws_iam_role.vendorintg_ecs_events_task_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonEC2ContainerServiceEventsRole"
}

# Define the ECS scheduled task
resource "aws_cloudwatch_event_rule" "preanalytics_eetbiobank_1" {
  name                = "preanalytics_eetbiobank_1"
  description         = "PreAnalytics ETCTN job"
  state               = "ENABLED"
  schedule_expression = "cron(45 07 * * ? *)" //7:45am utc --> 2:45am est
}

# Grant permissions to CloudWatch Events to run the ECS task
resource "aws_cloudwatch_event_target" "preanalytics_eetbiobank_1" {
  rule      = aws_cloudwatch_event_rule.preanalytics_eetbiobank_1.name
  target_id = "PreAnalytics_EETBiobank_1"
  role_arn  = "arn:aws:iam::${local.myaccount}:role/ecsEventsRole"
  arn       = aws_ecs_cluster.theradex_prod_nci_cluster_vendorintg.id
  count     = 1
  ecs_target {
    task_count          = 1
    task_definition_arn = aws_ecs_task_definition.vendorintg.arn
    launch_type         = "FARGATE"
    platform_version    = "LATEST"
    network_configuration {
      assign_public_ip = false
      security_groups  = [aws_security_group.theradex_app_vendorintg_sg.id]
      subnets          = local.private_subnet_ids
    }
  }
  input = jsonencode(
    {
      containerOverrides = [
        {
          command          = ["PREANALYTIC_RESULTS", "EETBioBank", "1", ]
          environmentFiles = []
          name             = "${var.vendorintg_container_name}"
        },
      ]
  })
}


# Define the ECS scheduled task
resource "aws_cloudwatch_event_rule" "hematologic_eetbiobank_1" {
  name                = "hematologic_eetbiobank_1"
  description         = "Hematologic ETCTN job"
  state               = "ENABLED"
  schedule_expression = "cron(30 07 * * ? *)" //7:30am utc --> 2:30am est
}

# Grant permissions to CloudWatch Events to run the ECS task
resource "aws_cloudwatch_event_target" "hematologic_eetbiobank_1" {
  rule      = aws_cloudwatch_event_rule.hematologic_eetbiobank_1.name
  target_id = "Hematologic_EETBiobank_1"
  role_arn  = "arn:aws:iam::${local.myaccount}:role/ecsEventsRole"
  arn       = aws_ecs_cluster.theradex_prod_nci_cluster_vendorintg.id
  count     = 1
  ecs_target {
    task_count          = 1
    task_definition_arn = aws_ecs_task_definition.vendorintg.arn
    launch_type         = "FARGATE"
    platform_version    = "LATEST"
    network_configuration {
      assign_public_ip = false
      security_groups  = [aws_security_group.theradex_app_vendorintg_sg.id]
      subnets          = local.private_subnet_ids
    }
  }
  input = jsonencode(
    {
      containerOverrides = [
        {
          command          = ["HEMATOLOGIC_MALIGNANCY_VERIFICATION_AND_ASSESSMENT", "EETBioBank", "1", ]
          environmentFiles = []
          name             = "${var.vendorintg_container_name}"
        },
      ]
  })
}


# Define the ECS scheduled task
resource "aws_cloudwatch_event_rule" "solidtissue_eetbiobank_1" {
  name                = "solidtissue_eetbiobank_1"
  description         = "Solidtissue ETCTN job"
  state               = "ENABLED"
  schedule_expression = "cron(15 07 * * ? *)" //7:15am utc --> 2:15am est
}

# Grant permissions to CloudWatch Events to run the ECS task
resource "aws_cloudwatch_event_target" "solidtissue_eetbiobank_1" {
  rule      = aws_cloudwatch_event_rule.solidtissue_eetbiobank_1.name
  target_id = "Solidtissue_EETBiobank_1"
  role_arn  = "arn:aws:iam::${local.myaccount}:role/ecsEventsRole"
  arn       = aws_ecs_cluster.theradex_prod_nci_cluster_vendorintg.id
  count     = 1
  ecs_target {
    task_count          = 1
    task_definition_arn = aws_ecs_task_definition.vendorintg.arn
    launch_type         = "FARGATE"
    platform_version    = "LATEST"
    network_configuration {
      assign_public_ip = false
      security_groups  = [aws_security_group.theradex_app_vendorintg_sg.id]
      subnets          = local.private_subnet_ids
    }
  }
  input = jsonencode(
    {
      containerOverrides = [
        {
          command          = ["SOLID_TISSUE_PATHOLOGY_VERIFICATION_AND_ASSESSMENT", "EETBioBank", "1", ]
          environmentFiles = []
          name             = "${var.vendorintg_container_name}"
        },
      ]
  })
}

# Define the ECS scheduled task
resource "aws_cloudwatch_event_rule" "shippingstatus_eetbiobank_1" {
  name                = "shippingstatus_eetbiobank_1"
  description         = "ShippingStatus ETCTN job"
  state               = "ENABLED"
  schedule_expression = "cron(45 06 * * ? *)" //6:45am utc --> 1:45am est
}

# Grant permissions to CloudWatch Events to run the ECS task
resource "aws_cloudwatch_event_target" "shippingstatus_eetbiobank_1" {
  rule      = aws_cloudwatch_event_rule.shippingstatus_eetbiobank_1.name
  target_id = "ShippingStatus_EETBiobank_1"
  role_arn  = "arn:aws:iam::${local.myaccount}:role/ecsEventsRole"
  arn       = aws_ecs_cluster.theradex_prod_nci_cluster_vendorintg.id
  count     = 1
  ecs_target {
    task_count          = 1
    task_definition_arn = aws_ecs_task_definition.vendorintg.arn
    launch_type         = "FARGATE"
    platform_version    = "LATEST"
    network_configuration {
      assign_public_ip = false
      security_groups  = [aws_security_group.theradex_app_vendorintg_sg.id]
      subnets          = local.private_subnet_ids
    }
  }
  input = jsonencode(
    {
      containerOverrides = [
        {
          command          = ["SHIPPING_STATUS", "EETBioBank", "1", ]
          environmentFiles = []
          name             = "${var.vendorintg_container_name}"
        },
      ]
  })
}

# Define the ECS scheduled task
resource "aws_cloudwatch_event_rule" "receivingstatus_eetbiobank_1" {
  name                = "receivingstatus_eetbiobank_1"
  description         = "ReceivingStatus ETCTN job"
  state               = "ENABLED"
  schedule_expression = "cron(15 06 * * ? *)" //6:15am utc --> 1:15am est
}

# Grant permissions to CloudWatch Events to run the ECS task
resource "aws_cloudwatch_event_target" "receivingstatus_eetbiobank_1" {
  rule      = aws_cloudwatch_event_rule.receivingstatus_eetbiobank_1.name
  target_id = "ReceivingStatus_EETBiobank_1"
  role_arn  = "arn:aws:iam::${local.myaccount}:role/ecsEventsRole"
  arn       = aws_ecs_cluster.theradex_prod_nci_cluster_vendorintg.id
  count     = 1
  ecs_target {
    task_count          = 1
    task_definition_arn = aws_ecs_task_definition.vendorintg.arn
    launch_type         = "FARGATE"
    platform_version    = "LATEST"
    network_configuration {
      assign_public_ip = false
      security_groups  = [aws_security_group.theradex_app_vendorintg_sg.id]
      subnets          = local.private_subnet_ids
    }
  }
  input = jsonencode(
    {
      containerOverrides = [
        {
          command          = ["RECEIVING_STATUS", "EETBioBank", "1", ]
          environmentFiles = []
          name             = "${var.vendorintg_container_name}"
        },
      ]
  })
}

# Define the ECS scheduled task
resource "aws_cloudwatch_event_rule" "biospecimen_roadmap_assay_theradex_1" {
  name                = "biospecimen_roadmap_assay_theradex_1"
  description         = "Biospecimen Roadmap Assay ETCTN job"
  state               = "ENABLED"
  schedule_expression = "cron(00 08 * * ? *)" //8:00am utc --> 3:00am est
}

# Grant permissions to CloudWatch Events to run the ECS task
resource "aws_cloudwatch_event_target" "biospecimen_roadmap_assay_theradex_1" {
  rule      = aws_cloudwatch_event_rule.biospecimen_roadmap_assay_theradex_1.name
  target_id = "biospecimen_roadmap_assay_theradex_1"
  role_arn  = "arn:aws:iam::${local.myaccount}:role/ecsEventsRole"
  arn       = aws_ecs_cluster.theradex_prod_nci_cluster_vendorintg.id
  count     = 1
  ecs_target {
    task_count          = 1
    task_definition_arn = aws_ecs_task_definition.vendorintg.arn
    launch_type         = "FARGATE"
    platform_version    = "LATEST"
    network_configuration {
      assign_public_ip = false
      security_groups  = [aws_security_group.theradex_app_vendorintg_sg.id]
      subnets          = local.private_subnet_ids
    }
  }
  input = jsonencode(
    {
      containerOverrides = [
        {
          command          = ["BIOSPECIMEN_ROADMAP_ASSAY", "Theradex", "1", ]
          environmentFiles = []
          name             = "${var.vendorintg_container_name}"
        },
      ]
  })
}

# Define the ECS scheduled task
resource "aws_cloudwatch_event_rule" "reconciliation_datashare_nationwidechildrens_1" {
  name                = "reconciliation_datashare_nationwidechildrens_1"
  description         = "Biobank Reconciliation Datashare Nationwide Childerns job"
  state               = "ENABLED"
  schedule_expression = "cron(00 08 * * ? *)" //8:00am utc --> 3:00am est
}

# Grant permissions to CloudWatch Events to run the ECS task
resource "aws_cloudwatch_event_target" "reconciliation_datashare_nationwidechildrens_1" {
  rule      = aws_cloudwatch_event_rule.reconciliation_datashare_nationwidechildrens_1.name
  target_id = "reconciliation_datashare_nationwidechildrens_1"
  role_arn  = "arn:aws:iam::${local.myaccount}:role/ecsEventsRole"
  arn       = aws_ecs_cluster.theradex_prod_nci_cluster_vendorintg.id
  count     = 1
  ecs_target {
    task_count          = 1
    task_definition_arn = aws_ecs_task_definition.vendorintg.arn
    launch_type         = "FARGATE"
    platform_version    = "LATEST"
    network_configuration {
      assign_public_ip = false
      security_groups  = [aws_security_group.theradex_app_vendorintg_sg.id]
      subnets          = local.private_subnet_ids
    }
  }
  input = jsonencode(
    {
      containerOverrides = [
        {
          command          = ["RECONCILIATION_DATASHARE", "NationwideChildrens", "1", ]
          environmentFiles = []
          name             = "${var.vendorintg_container_name}"
        },
      ]
  })
}

resource "aws_iam_role" "vendorintg_ecsEventsRole" {
  name = "ecsEventsRole"

  assume_role_policy = <<EOF
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Sid": "",
            "Effect": "Allow",
            "Principal": {
                "Service": "events.amazonaws.com"
            },
            "Action": "sts:AssumeRole"
        }
    ]
}
EOF
}

resource "aws_iam_role_policy_attachment" "vendorintg_ecsEventsRole_policy_attachment" {
  role       = aws_iam_role.vendorintg_ecsEventsRole.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonEC2ContainerServiceEventsRole"
}


# Define the ECS scheduled task
resource "aws_cloudwatch_event_rule" "eetinventory_eetbiobank_1" {
  name        = "eetinventory_eetbiobank_1"
  description = "EETInventory ETCTN job"
  state = "DISABLED"
  schedule_expression = "cron(30 08 * * ? *)"  //08:30am utc --> 3:30am est
}

# Grant permissions to CloudWatch Events to run the ECS task
resource "aws_cloudwatch_event_target" "eetinventory_eetbiobank_1" {
  rule      = aws_cloudwatch_event_rule.eetinventory_eetbiobank_1.name
  target_id = "eetinventory_eetbiobank_1"
  role_arn = "arn:aws:iam::${local.myaccount}:role/ecsEventsRole"
  arn      = aws_ecs_cluster.theradex_prod_nci_cluster_vendorintg.id 
  count = 1
  ecs_target {
    task_count        = 1
    task_definition_arn = aws_ecs_task_definition.vendorintg.arn
    launch_type       = "FARGATE"
    platform_version  = "LATEST"
    network_configuration {
      assign_public_ip = false
      security_groups  = [ aws_security_group.theradex_app_vendorintg_sg.id ]
      subnets          = local.private_subnet_ids
    }
  }
   input  = jsonencode(
        {
            containerOverrides = [
             {
                command          = ["EET_INVENTORY", "EETBioBank", "1", ]
                environmentFiles = []
                name             = "${var.vendorintg_container_name}"
             },
            ]
        })
}