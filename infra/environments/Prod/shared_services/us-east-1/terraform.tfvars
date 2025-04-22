tags = {
    "CreatedBy"   = "terraform"
    "Environment" = "cicd"
}

project_name =  "nci-oars"

environment  =  [ 
    "qa1",
    "staging",
    "production"
] 

 environment2 = [
    {
      id   = 1
      name = "qa1"
    },
    {
      id   = 10
      name = "staging"
    }
    , {
      id   = 20
      name = "production"
    }
  ]



region       = "us-east-1"
account_id   = "606199607275"

environment_account = {
    qa1  = "993530973844"
    staging = "352847057549",
    production = "590811900011"    
}
