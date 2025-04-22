#General values
tags = {
  "CreatedBy"   = "terraform"
  "Environment" = "production"
}
oars_project_name = "nci-oars"
environment_name  = "production"
region            = "us-east-1"
vpc_id            = "vpc-03a0268bacc2aecc3"
lb_subnet_ids     = ["subnet-01a2b118ccf7b977b", "subnet-035acbce2ebe9a9d5"]

#OARS Environments
production_environment_name            = "production"
production_aspnetcore_environment_name = "DeployedProd"
production_oars_environment            = "PROD"

#ACM Variables
production_domain_name                  = "nci-oars.com"
production_gov_certificate_arn          = "arn:aws:acm:us-east-1:590811900011:certificate/c7730ed6-8a99-456f-bbcb-320aca7cc5d7"
production_gov_listener_arn             = "arn:aws:elasticloadbalancing:us-east-1:590811900011:listener/app/nci-oars-production-lb/bb72c5777eec42e9/7c5d5f5dccbb8c28"
production_task_def_host_port           = 80
production_task_def_container_port      = 80
production_load_balancer_container_port = 80
production_alb_port                     = 80

#BastionHost variables
bastion_subnet_id     = "subnet-01986166d4f9a1f14"
bastion_instance_type = "t3.nano"
bastion_ami           = "ami-03c951bbe993ea087"

#ECS variables
task_def_cpu    = 4096
task_def_memory = 16384
desired_count   = 1
ecs_subnet_ids  = ["subnet-027a4f5d738cbab6e", "subnet-01986166d4f9a1f14"]

# ECS Autoscaling Variables
ecs_service_resource_id  = "service/nci-oars-production-cluster/nci-oars-production-service"
ecs_autoscaling_role_arn = "arn:aws:iam::590811900011:role/aws-service-role/ecs.application-autoscaling.amazonaws.com/AWSServiceRoleForApplicationAutoScaling_ECSService"
ecs_scalable_dimension   = "ecs:service:DesiredCount"
ecs_service_namespace    = "ecs"

# ECS Autoscaling Policy Variables
ecs_service_name        = "nci-oars-production-service"
ecs_scale_in_cooldown   = 60
ecs_scale_out_cooldown  = 60
ecs_memory_target_value = 60
ecs_cpu_target_value    = 50
