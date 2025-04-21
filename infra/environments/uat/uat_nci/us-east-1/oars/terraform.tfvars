#General values
tags = {
  "CreatedBy"   = "terraform"
  "Environment" = "staging"
  "Application" = "Oars"
}
oars_project_name = "nci-oars"
environment_name  = "staging"
region            = "us-east-1"
vpc_id            = "vpc-0c095f75d81698ebe"
lb_subnet_ids     = ["subnet-06ff978d5788116e4", "subnet-0f184d5ad884d453b"]

#OARS Environments
uat_environment_name            = "staging"
uat_aspnetcore_environment_name = "DeployedUAT"
uat_oars_environment            = "UAT"
uat_task_def_host_port = 80
uat_task_def_container_port = 80
uat_load_balancer_container_port = 80
uat_alb_port = 80

#ACM Variables
uat_domain_name = "uat.nci-oars.com"
uat_gov_certificate_arn = "arn:aws:acm:us-east-1:352847057549:certificate/e378251e-ea79-43b4-9329-377c519803fa"
uat_gov_listener_arn = "arn:aws:elasticloadbalancing:us-east-1:352847057549:listener/app/nci-oars-staging-lb/9062a339afbf8ce2/e8c1cfc21d91f9a3"

#BastionHost variables
bastion_subnet_id     = "subnet-01493ad6d9e419854"
bastion_instance_type = "t3.nano"
bastion_ami           = "ami-03c951bbe993ea087"

#ECS variables
task_def_cpu    = 4096
task_def_memory = 8192
desired_count   = 1
ecs_subnet_ids  = ["subnet-0f0a15a1de9dd3f66", "subnet-01493ad6d9e419854"]
