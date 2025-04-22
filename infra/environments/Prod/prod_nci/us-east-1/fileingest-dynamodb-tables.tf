
###############################
##### FileIngestRequest Table #####
###############################

resource "aws_dynamodb_table" "FileIngestRequest-dynamodb-table" {
  name             = "FileIngestRequest"
  billing_mode     = "PROVISIONED"
  read_capacity    = 2
  write_capacity   = 2
  hash_key         = "RequestId"
  stream_enabled   = true
  stream_view_type = "NEW_AND_OLD_IMAGES"

  attribute {
    name = "RequestId"
    type = "S"
  }

  attribute {
    name = "UserId"
    type = "S"
  }

  ttl {
    attribute_name = ""
    enabled        = false
  }

  global_secondary_index {
    name               = "UserId-index"
    hash_key           = "UserId"
    write_capacity     = 2
    read_capacity      = 2
    projection_type    = "INCLUDE"
    non_key_attributes = ["UserId"]
  }
}

# Auto-scaling for FileIngestRequest Table
resource "aws_appautoscaling_target" "FileIngestRequest_read_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.FileIngestRequest-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:ReadCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "FileIngestRequest_read_policy" {
  name               = "FileIngestRequest-read-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.FileIngestRequest_read_target.resource_id
  scalable_dimension = aws_appautoscaling_target.FileIngestRequest_read_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.FileIngestRequest_read_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBReadCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}

resource "aws_appautoscaling_target" "FileIngestRequest_write_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.FileIngestRequest-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:WriteCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "FileIngestRequest_write_policy" {
  name               = "FileIngestRequest-write-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.FileIngestRequest_write_target.resource_id
  scalable_dimension = aws_appautoscaling_target.FileIngestRequest_write_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.FileIngestRequest_write_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBWriteCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}

###############################
##### ReceivingStatusFileData Table #####
###############################
resource "aws_dynamodb_table" "ReceivingStatusFileData-dynamodb-table" {
  name           = "ReceivingStatusFileData"
  billing_mode   = "PROVISIONED"
  read_capacity  = 2
  write_capacity = 2
  hash_key       = "RequestItemId"
  range_key      = "RequestId"

  attribute {
    name = "RequestItemId"
    type = "S"
  }

  attribute {
    name = "RequestId"
    type = "S"
  }

  ttl {
    attribute_name = ""
    enabled        = false
  }

  global_secondary_index {
    name               = "RequestId-index"
    hash_key           = "RequestId"
    write_capacity     = 2
    read_capacity      = 2
    projection_type    = "INCLUDE"
    non_key_attributes = ["RequestId"]
  }
}

# Auto-scaling for ReceivingStatusFileData Table
resource "aws_appautoscaling_target" "ReceivingStatusFileData_read_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.ReceivingStatusFileData-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:ReadCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "ReceivingStatusFileData_read_policy" {
  name               = "ReceivingStatusFileData-read-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.ReceivingStatusFileData_read_target.resource_id
  scalable_dimension = aws_appautoscaling_target.ReceivingStatusFileData_read_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.ReceivingStatusFileData_read_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBReadCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}

resource "aws_appautoscaling_target" "ReceivingStatusFileData_write_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.ReceivingStatusFileData-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:WriteCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "ReceivingStatusFileData_write_policy" {
  name               = "ReceivingStatusFileData-write-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.ReceivingStatusFileData_write_target.resource_id
  scalable_dimension = aws_appautoscaling_target.ReceivingStatusFileData_write_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.ReceivingStatusFileData_write_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBWriteCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}

###############################
##### BiospecimenRoadmapFileData Table #####
###############################

resource "aws_dynamodb_table" "BiospecimenRoadmapFileData-dynamodb-table" {
  name           = "BiospecimenRoadmapFileData"
  billing_mode   = "PROVISIONED"
  read_capacity  = 2
  write_capacity = 2
  hash_key       = "RequestItemId"
  range_key      = "RequestId"

  attribute {
    name = "RequestItemId"
    type = "S"
  }

  attribute {
    name = "RequestId"
    type = "S"
  }

  ttl {
    attribute_name = ""
    enabled        = false
  }

  global_secondary_index {
    name               = "RequestId-index"
    hash_key           = "RequestId"
    write_capacity     = 2
    read_capacity      = 2
    projection_type    = "INCLUDE"
    non_key_attributes = ["RequestId"]
  }
}

# Auto-scaling for BiospecimenRoadmapFileData Table
resource "aws_appautoscaling_target" "BiospecimenRoadmapFileData_read_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.BiospecimenRoadmapFileData-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:ReadCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "BiospecimenRoadmapFileData_read_policy" {
  name               = "BiospecimenRoadmapFileData-read-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.BiospecimenRoadmapFileData_read_target.resource_id
  scalable_dimension = aws_appautoscaling_target.BiospecimenRoadmapFileData_read_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.BiospecimenRoadmapFileData_read_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBReadCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}

resource "aws_appautoscaling_target" "BiospecimenRoadmapFileData_write_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.BiospecimenRoadmapFileData-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:WriteCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "BiospecimenRoadmapFileData_write_policy" {
  name               = "BiospecimenRoadmapFileData-write-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.BiospecimenRoadmapFileData_write_target.resource_id
  scalable_dimension = aws_appautoscaling_target.BiospecimenRoadmapFileData_write_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.BiospecimenRoadmapFileData_write_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBWriteCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}


###############################
##### ShippingStatusFileData Table #####
###############################

resource "aws_dynamodb_table" "ShippingStatusFileData-dynamodb-table" {
  name           = "ShippingStatusFileData"
  billing_mode   = "PROVISIONED"
  read_capacity  = 2
  write_capacity = 2
  hash_key       = "RequestItemId"
  range_key      = "RequestId"

  attribute {
    name = "RequestItemId"
    type = "S"
  }

  attribute {
    name = "RequestId"
    type = "S"
  }

  ttl {
    attribute_name = ""
    enabled        = false
  }

  global_secondary_index {
    name               = "RequestId-index"
    hash_key           = "RequestId"
    write_capacity     = 2
    read_capacity      = 2
    projection_type    = "INCLUDE"
    non_key_attributes = ["RequestId"]
  }
}

# Auto-scaling for ShippingStatusFileData Table
resource "aws_appautoscaling_target" "ShippingStatusFileData_read_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.ShippingStatusFileData-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:ReadCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "ShippingStatusFileData_read_policy" {
  name               = "ShippingStatusFileData-read-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.ShippingStatusFileData_read_target.resource_id
  scalable_dimension = aws_appautoscaling_target.ShippingStatusFileData_read_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.ShippingStatusFileData_read_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBReadCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}

resource "aws_appautoscaling_target" "ShippingStatusFileData_write_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.ShippingStatusFileData-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:WriteCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "ShippingStatusFileData_write_policy" {
  name               = "ShippingStatusFileData-write-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.ShippingStatusFileData_write_target.resource_id
  scalable_dimension = aws_appautoscaling_target.ShippingStatusFileData_write_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.ShippingStatusFileData_write_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBWriteCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}


###############################
##### TSO500LibraryQCFileData Table #####
###############################

resource "aws_dynamodb_table" "TSO500LibraryQCFileData-dynamodb-table" {
  name           = "TSO500LibraryQCFileData"
  billing_mode   = "PROVISIONED"
  read_capacity  = 2
  write_capacity = 2
  hash_key       = "RequestItemId"
  range_key      = "RequestId"

  attribute {
    name = "RequestItemId"
    type = "S"
  }

  attribute {
    name = "RequestId"
    type = "S"
  }

  ttl {
    attribute_name = ""
    enabled        = false
  }

  global_secondary_index {
    name               = "RequestId-index"
    hash_key           = "RequestId"
    write_capacity     = 2
    read_capacity      = 2
    projection_type    = "INCLUDE"
    non_key_attributes = ["RequestId"]
  }
}

# Auto-scaling for TSO500LibraryQCFileData Table
resource "aws_appautoscaling_target" "TSO500LibraryQCFileData_read_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.TSO500LibraryQCFileData-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:ReadCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "TSO500LibraryQCFileData_read_policy" {
  name               = "TSO500LibraryQCFileData-read-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.TSO500LibraryQCFileData_read_target.resource_id
  scalable_dimension = aws_appautoscaling_target.TSO500LibraryQCFileData_read_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.TSO500LibraryQCFileData_read_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBReadCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}

resource "aws_appautoscaling_target" "TSO500LibraryQCFileData_write_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.TSO500LibraryQCFileData-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:WriteCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "TSO500LibraryQCFileData_write_policy" {
  name               = "TSO500LibraryQCFileData-write-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.TSO500LibraryQCFileData_write_target.resource_id
  scalable_dimension = aws_appautoscaling_target.TSO500LibraryQCFileData_write_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.TSO500LibraryQCFileData_write_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBWriteCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}


###############################
##### TSO500SequencingQCFileData Table #####
###############################

resource "aws_dynamodb_table" "TSO500SequencingQCFileData-dynamodb-table" {
  name           = "TSO500SequencingQCFileData"
  billing_mode   = "PROVISIONED"
  read_capacity  = 2
  write_capacity = 2
  hash_key       = "RequestItemId"
  range_key      = "RequestId"

  attribute {
    name = "RequestItemId"
    type = "S"
  }

  attribute {
    name = "RequestId"
    type = "S"
  }

  ttl {
    attribute_name = ""
    enabled        = false
  }

  global_secondary_index {
    name               = "RequestId-index"
    hash_key           = "RequestId"
    write_capacity     = 2
    read_capacity      = 2
    projection_type    = "INCLUDE"
    non_key_attributes = ["RequestId"]
  }
}

# Auto-scaling for TSO500SequencingQCFileData Table
resource "aws_appautoscaling_target" "TSO500SequencingQCFileData_read_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.TSO500SequencingQCFileData-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:ReadCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "TSO500SequencingQCFileData_read_policy" {
  name               = "TSO500SequencingQCFileData-read-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.TSO500SequencingQCFileData_read_target.resource_id
  scalable_dimension = aws_appautoscaling_target.TSO500SequencingQCFileData_read_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.TSO500SequencingQCFileData_read_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBReadCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}

resource "aws_appautoscaling_target" "TSO500SequencingQCFileData_write_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.TSO500SequencingQCFileData-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:WriteCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "TSO500SequencingQCFileData_write_policy" {
  name               = "TSO500SequencingQCFileData-write-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.TSO500SequencingQCFileData_write_target.resource_id
  scalable_dimension = aws_appautoscaling_target.TSO500SequencingQCFileData_write_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.TSO500SequencingQCFileData_write_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBWriteCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}


###############################
##### IFAFileData Table #####
###############################

resource "aws_dynamodb_table" "IFAFileData-dynamodb-table" {
  name           = "IFAFileData"
  billing_mode   = "PROVISIONED"
  read_capacity  = 2
  write_capacity = 2
  hash_key       = "RequestItemId"
  range_key      = "RequestId"

  attribute {
    name = "RequestItemId"
    type = "S"
  }

  attribute {
    name = "RequestId"
    type = "S"
  }

  ttl {
    attribute_name = ""
    enabled        = false
  }

  global_secondary_index {
    name               = "RequestId-index"
    hash_key           = "RequestId"
    write_capacity     = 2
    read_capacity      = 2
    projection_type    = "INCLUDE"
    non_key_attributes = ["RequestId"]
  }
}

# Auto-scaling for IFAFileData Table
resource "aws_appautoscaling_target" "IFAFileData_read_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.IFAFileData-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:ReadCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "IFAFileData_read_policy" {
  name               = "IFAFileData-read-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.IFAFileData_read_target.resource_id
  scalable_dimension = aws_appautoscaling_target.IFAFileData_read_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.IFAFileData_read_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBReadCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}

resource "aws_appautoscaling_target" "IFAFileData_write_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.IFAFileData-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:WriteCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "IFAFileData_write_policy" {
  name               = "IFAFileData-write-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.IFAFileData_write_target.resource_id
  scalable_dimension = aws_appautoscaling_target.IFAFileData_write_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.IFAFileData_write_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBWriteCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}


###############################
##### IFAResultSummaryFileData Table #####
###############################

resource "aws_dynamodb_table" "IFAResultSummaryFileData-dynamodb-table" {
  name           = "IFAResultSummaryFileData"
  billing_mode   = "PROVISIONED"
  read_capacity  = 2
  write_capacity = 2
  hash_key       = "RequestItemId"
  range_key      = "RequestId"

  attribute {
    name = "RequestItemId"
    type = "S"
  }

  attribute {
    name = "RequestId"
    type = "S"
  }

  ttl {
    attribute_name = ""
    enabled        = false
  }

  global_secondary_index {
    name               = "RequestId-index"
    hash_key           = "RequestId"
    write_capacity     = 2
    read_capacity      = 2
    projection_type    = "INCLUDE"
    non_key_attributes = ["RequestId"]
  }
}

# Auto-scaling for IFAResultSummaryFileData Table
resource "aws_appautoscaling_target" "IFAResultSummaryFileData_read_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.IFAResultSummaryFileData-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:ReadCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "IFAResultSummaryFileData_read_policy" {
  name               = "IFAResultSummaryFileData-read-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.IFAResultSummaryFileData_read_target.resource_id
  scalable_dimension = aws_appautoscaling_target.IFAResultSummaryFileData_read_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.IFAResultSummaryFileData_read_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBReadCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}

resource "aws_appautoscaling_target" "IFAResultSummaryFileData_write_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.IFAResultSummaryFileData-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:WriteCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "IFAResultSummaryFileData_write_policy" {
  name               = "IFAResultSummaryFileData-write-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.IFAResultSummaryFileData_write_target.resource_id
  scalable_dimension = aws_appautoscaling_target.IFAResultSummaryFileData_write_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.IFAResultSummaryFileData_write_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBWriteCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}


###############################
##### PathologyEvaluationReportFileData Table #####
###############################

resource "aws_dynamodb_table" "PathologyEvaluationReportFileData-dynamodb-table" {
  name           = "PathologyEvaluationReportFileData"
  billing_mode   = "PROVISIONED"
  read_capacity  = 2
  write_capacity = 2
  hash_key       = "RequestItemId"
  range_key      = "RequestId"

  attribute {
    name = "RequestItemId"
    type = "S"
  }

  attribute {
    name = "RequestId"
    type = "S"
  }

  ttl {
    attribute_name = ""
    enabled        = false
  }

  global_secondary_index {
    name               = "RequestId-index"
    hash_key           = "RequestId"
    write_capacity     = 2
    read_capacity      = 2
    projection_type    = "INCLUDE"
    non_key_attributes = ["RequestId"]
  }
}

# Auto-scaling for PathologyEvaluationReportFileData Table
resource "aws_appautoscaling_target" "PathologyEvaluationReportFileData_read_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.PathologyEvaluationReportFileData-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:ReadCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "PathologyEvaluationReportFileData_read_policy" {
  name               = "PathologyEvaluationReportFileData-read-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.PathologyEvaluationReportFileData_read_target.resource_id
  scalable_dimension = aws_appautoscaling_target.PathologyEvaluationReportFileData_read_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.PathologyEvaluationReportFileData_read_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBReadCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}

resource "aws_appautoscaling_target" "PathologyEvaluationReportFileData_write_target" {
  max_capacity       = 100
  min_capacity       = 2
  resource_id        = "table/${aws_dynamodb_table.PathologyEvaluationReportFileData-dynamodb-table.name}"
  scalable_dimension = "dynamodb:table:WriteCapacityUnits"
  service_namespace  = "dynamodb"
}

resource "aws_appautoscaling_policy" "PathologyEvaluationReportFileData_write_policy" {
  name               = "PathologyEvaluationReportFileData-write-scaling-policy"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.PathologyEvaluationReportFileData_write_target.resource_id
  scalable_dimension = aws_appautoscaling_target.PathologyEvaluationReportFileData_write_target.scalable_dimension
  service_namespace  = aws_appautoscaling_target.PathologyEvaluationReportFileData_write_target.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 70.0
    predefined_metric_specification {
      predefined_metric_type = "DynamoDBWriteCapacityUtilization"
    }
    scale_in_cooldown  = 60
    scale_out_cooldown = 60
  }
}
