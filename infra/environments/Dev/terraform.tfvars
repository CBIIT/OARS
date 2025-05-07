#General values
tags = {
        "CreatedBy"   = "terraform"
         "Environment" = "development"
         "Application" = "Oars"
        }
tags_qa = {
        "CreatedBy"   = "terraform"
         "Environment" = "qa"
         "Application" = "Oars"
        }
oars_project_name     = "nci-oars"
environment_name = "dev"
region           = "us-east-1"
vpc_id           = "vpc-0b37d6149252c439d"
lb_subnet_ids   = ["subnet-0500d8f1148a6b347" , "subnet-026bc4c76ad6b3453" ]

#OARS Environments
dev_environment_name = "dev"
qa_environment_name = "qa1"
studydev_environment_name = "studydev"

dev_aspnetcore_environment_name = "DeployedDev"
qa_aspnetcore_environment_name = "DeployedQA"
studydev_aspnetcore_environment_name = "DeployedStudyDev"

dev_oars_environment = "dev"
qa_oars_environment = "qa"
studydev_oars_environment = "studydev"

dev_task_def_host_port = 8080
dev_task_def_container_port = 8080
dev_load_balancer_container_port = 8080
dev_alb_port = 8080
qa_task_def_host_port = 80
qa_task_def_container_port = 80
qa_load_balancer_container_port = 80
qa_alb_port = 80
studydev_task_def_host_port = 80
studydev_task_def_container_port = 80
studydev_load_balancer_container_port = 80
studydev_alb_port = 80

#ACM Variables
dev_domain_name = "dev.nci-oars.com"
qa_domain_name = "qa.nci-oars.com"
studydev_domain_name = "studydev.nci-oars.com"
# dev_gov_certificate_arn = "arn:aws:acm:us-east-1:993530973844:certificate/f0961039-01da-4732-8d4e-345eabc284c9"
# qa_gov_certificate_arn = "arn:aws:acm:us-east-1:993530973844:certificate/f0961039-01da-4732-8d4e-345eabc284c9"
# dev_gov_listener_arn = "arn:aws:elasticloadbalancing:us-east-1:993530973844:listener/app/nci-oars-dev-lb/33f41952879729cc/b5ce9545643773a0"
# qa_gov_listener_arn = "arn:aws:elasticloadbalancing:us-east-1:993530973844:listener/app/nci-oars-qa1-lb/22a72ecf62a4b985/e7e2b4b7035d99f6"

#BastionHost variables
bastion_subnet_id = "subnet-026bc4c76ad6b3453"
bastion_instance_type = "t3.nano"
bastion_ami = "ami-03c951bbe993ea087"

#ECS variables
ecs_subnet_ids = [ "subnet-0500d8f1148a6b347", "subnet-026bc4c76ad6b3453" ]
task_def_cpu    = 4096
task_def_memory = 8192
desired_count   = 1
