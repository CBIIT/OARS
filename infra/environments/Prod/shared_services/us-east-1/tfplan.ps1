#You can set TF_LOG to one of the log levels (in order of decreasing verbosity) TRACE, DEBUG, INFO, WARN or ERROR to change the verbosity of the logs.
param (
  [string]$TF_LOG = "INFO" # Default value for TF_LOG
)

# Define parameters for AWS SSO configuration
$params = @{
  ProfileName = 'theradex-shared-service'
  AccountId = 606199607275
  RoleName = 'AWSAdministratorAccess'
  SessionName = 'theradex-shared-service'
  StartUrl = 'https://theradex.awsapps.com/start#'
  SSORegion = 'us-east-1'
  RegistrationScopes = 'sso:account:access'
}

# Initialize AWS SSO configuration
Initialize-AWSSSOConfiguration @params
Invoke-AWSSSOLogin -ProfileName theradex-shared-service -Force

# Generate timestamp
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"

# Set environment variables
$env:TF_LOG_PATH = "logs/${timestamp}_shared_services_plan.logs"
$env:TF_LOG = $TF_LOG

# Run terraform plan and redirect output to the file
terraform plan -out "logs/${timestamp}_shared_services.plan" -no-color

# Print the command to apply the plan
echo "terraform apply 'logs/${timestamp}_shared_services.plan'"
