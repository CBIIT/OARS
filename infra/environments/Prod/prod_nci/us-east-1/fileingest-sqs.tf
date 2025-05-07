resource "aws_sqs_queue" "fileingest-updatemedidata-queue" {
  name = "fileingest-updatemedidata-queue"
  #create_dlq                  = true
  message_retention_seconds  = 1209600 #(default — 345600 seconds[4days])
  delay_seconds              = 0
  visibility_timeout_seconds = 30
  max_message_size           = 262144 #(default — 262144 bytes [256 KiB])
  receive_wait_time_seconds  = 0      #(default — 0 seconds)
  sqs_managed_sse_enabled    = true
}

resource "aws_sqs_queue" "fileingest-updatemedidata-dlq" {
  name = "fileingest-updatemedidata-dlq"
  redrive_allow_policy = jsonencode({
    redrivePermission = "byQueue",
    sourceQueueArns   = [aws_sqs_queue.fileingest-updatemedidata-queue.arn]
  })
}

resource "aws_sqs_queue_redrive_policy" "fileingest-updatemedidata-redrive" {
  queue_url = aws_sqs_queue.fileingest-updatemedidata-queue.id
  redrive_policy = jsonencode({
    deadLetterTargetArn = aws_sqs_queue.fileingest-updatemedidata-dlq.arn
    maxReceiveCount     = 2
  })
}

resource "aws_sqs_queue" "fileingest-requestreconcile-queue" {
  name = "fileingest-requestreconcile-queue.fifo"
  #create_dlq                  = true
  fifo_queue                  = true
  message_retention_seconds   = 1209600 #(default — 345600 seconds[4days])
  delay_seconds               = 0
  visibility_timeout_seconds  = 30
  max_message_size            = 262144 #(default — 262144 bytes [256 KiB])
  receive_wait_time_seconds   = 0      #(default — 0 seconds)
  sqs_managed_sse_enabled     = true
  deduplication_scope         = "queue"
  content_based_deduplication = true
  fifo_throughput_limit       = "perQueue"
}

resource "aws_sqs_queue" "fileingest-requestreconcile-dlq" {
  name       = "fileingest-requestreconcile-dlq.fifo"
  fifo_queue = true
  redrive_allow_policy = jsonencode({
    redrivePermission = "byQueue",
    sourceQueueArns   = [aws_sqs_queue.fileingest-requestreconcile-queue.arn]
  })
}

resource "aws_sqs_queue_redrive_policy" "fileingest-requestreconcile-redrive" {
  queue_url = aws_sqs_queue.fileingest-requestreconcile-queue.id
  redrive_policy = jsonencode({
    deadLetterTargetArn = aws_sqs_queue.fileingest-requestreconcile-dlq.arn
    maxReceiveCount     = 2
  })
}