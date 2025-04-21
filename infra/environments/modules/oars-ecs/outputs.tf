output "cluster_arn" {
  value = aws_ecs_cluster.main.arn
}
output "sg_id" {
  value = aws_security_group.theradex_sg.id
}

output "taskdefinition_arn" {
  value = aws_ecs_task_definition.task.arn
}
