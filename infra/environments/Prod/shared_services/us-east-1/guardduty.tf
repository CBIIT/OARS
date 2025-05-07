###############################
##### Guardduty Resources #####
###############################


module "guardduty" {
  source = "../../../../modules/guardduty/master"
  enable = true
}