

# CI/CD Pipeline Usage Guide

This guide provides instructions on how to use the CI/CD pipeline for the .NET application. The pipeline automates building, testing, deploying, and promoting the application across multiple environments: Dev, QA, UAT, and Production.

## Table of Contents

- [Overview](#overview)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
  - [1. Continuous Integration and Deployment on Dev](#1-continuous-integration-and-deployment-on-dev)
  - [2. Promoting to QA](#2-promoting-to-qa)
  - [3. Promoting to UAT](#3-promoting-to-uat)
  - [4. Promoting to Production](#4-promoting-to-production)
- [Manual Approvals](#manual-approvals)
- [Environment Variables and Secrets](#environment-variables-and-secrets)
- [Notes](#notes)
- [Support](#support)

## Overview

The CI/CD pipeline automates the process of building, testing, deploying, and promoting the application through various environments:

1. **Dev**: Automatic build, test, and deployment upon code changes.
2. **QA**: Deployments can be triggered manually, but requires approval.
3. **UAT**: Deployments require manual approval.
4. **Production**: Deployments require manual approval.

## Prerequisites

- **GitHub Repository Access**: Ensure you have access to the repository containing the application's code.
- **GitHub Actions Enabled**: Actions must be enabled in the repository.
- **AWS Accounts and Permissions**: AWS accounts are set up for each environment with appropriate permissions and IAM roles.
- **GitHub Secrets Configured**: Necessary AWS credentials, ECR URIs, and other secrets are stored securely in GitHub Secrets.

## Manual Approvals
For QA, UAT and Production deployments, manual approvals are required to proceed.

### Approval Process:

After triggering the deployment, the workflow will pause and request approval.
Approvers (specified in the workflow) will receive a notification.
Approvers can approve the deployment via Pull Request triggered by the pipeline.
Note: Only users specified as approvers can approve the deployment.

## Getting Started

### 1. Continuous Integration and Deployment on Dev

- **Trigger**: The CI/CD pipeline runs automatically on every push to the `main` branch.
- **Process**:
  - **Build and Test**: The application is built and unit tests are executed.
  - **Semantic Versioning**: A new version is generated based on commit messages. SemVers are created based on certain commit messages, for example **`patch/<commit-message>`** triggers a `0.0.1` increment, **`minor/<commit-message>`** triggers a `0.1.0` increment and **`major/<commit-message> `** triggers a `1.0.0` increment
  - **Docker Image Build**: A Docker image is built and tagged with the new version and `latest`.
  - **Push to ECR**: The Docker image is pushed to the Dev ECR repository.
  - **Deployment to ECS**: The ECS service in the Dev environment is updated with the new image.
  - **Artifact Creation**: The Docker image is saved as an artifact for use in other environments.
  - **Automatic Promotion**: The pipeline triggers the QA deployment workflow.

**No action is required from the user during this stage unless you need to push code to the `main` branch.**

### 2. Promoting to QA

- **Trigger**: The QA deployment is automatically triggered by the Dev pipeline after a successful deployment.
- **Process**:
  - **Manual Trigger (Optional)**: If needed, deployments to QA can be manually initiated.
  - **Docker Image Retrieval**: The Docker image artifact is downloaded.
  - **Push to ECR**: The image is pushed to the QA ECR repository.
  - **Deployment to ECS**: The ECS service in the QA environment is updated with the new image.

**To manually trigger a deployment to QA:**

1. Navigate to the **Actions** tab in the GitHub repository.
2. Select the **"Promote Image to QA"** workflow.
3. Click **"Run workflow"**.
4. Enter the version number you wish to deploy (e.g., `1.0.0`).
5. Enter the RunID of the Dev workflow located at the 'Trigger Approval for Next Environment' step in the workflow.
   ![image](https://github.com/user-attachments/assets/7bfed5a7-b052-4cc1-9dad-904dd86fa63c)


### 3. Promoting to UAT

- **Trigger**: Deployment to UAT must be manually initiated.
- **Process**:
  - **Manual Approval**: Required before the deployment proceeds.
  - **Docker Image Retrieval**: The Docker image artifact is downloaded.
  - **Push to ECR**: The image is pushed to the UAT ECR repository.
  - **Deployment to ECS**: The ECS service in the UAT environment is updated with the new image.

**To promote to UAT:**

1. Navigate to the **Actions** tab in the GitHub repository.
2. Select the **"Promote Image to UAT"** workflow.
3. Click **"Run workflow"**.
4. Enter the version number you wish to deploy.
5. Wait for the approval request and approve it as per [Manual Approvals](#manual-approvals).

### 4. Promoting to Production

- **Trigger**: Deployment to Production must be manually initiated.
- **Process**:
  - **Manual Approval**: Required before the deployment proceeds.
  - **Docker Image Retrieval**: The Docker image artifact is downloaded.
  - **Push to ECR**: The image is pushed to the Production ECR repository.
  - **Deployment to ECS**: The ECS service in the Production environment is updated with the new image.

**To promote to Production:**

1. Navigate to the **Actions** tab in the GitHub repository.
2. Select the **"Promote Image to Production"** workflow.
3. Click **"Run workflow"**.
4. Enter the version number you wish to deploy.
5. Wait for the approval request and approve it as per [Manual Approvals](#manual-approvals).

## Manual Approvals

For UAT and Production deployments, manual approvals are required to proceed.

**Approval Process:**

1. After triggering the deployment, the workflow will pause and request approval.
2. Approvers (specified in the workflow) will receive a notification.
3. Approvers can approve the deployment via:
   - The **"Actions"** tab in the repository.
   - Finding the pending workflow run.
   - Clicking **"Review deployments"** and approving the deployment.

**Note**: Only users specified as approvers can approve the deployment.

## Environment Variables and Secrets

The pipeline uses GitHub Secrets to store sensitive information. Ensure the following secrets are configured:

- **AWS Credentials and Regions**:
  - `AWS_ROLE_ARN_DEV`, `AWS_REGION_DEV`
  - `AWS_ROLE_ARN_QA`, `AWS_REGION_QA`
  - `AWS_ROLE_ARN_UAT`, `AWS_REGION_UAT`
  - `AWS_ROLE_ARN_PROD`, `AWS_REGION_PROD`
- **ECR URIs**:
  - `ECR_URI_DEV`
  - `ECR_URI_QA`
  - `ECR_URI_UAT`
  - `ECR_URI_PROD`
- **ECS Cluster and Service Names**:
  - `ECS_CLUSTER_DEV`, `ECS_SERVICE_DEV`
  - `ECS_CLUSTER_QA`, `ECS_SERVICE_QA`
  - `ECS_CLUSTER_UAT`, `ECS_SERVICE_UAT`
  - `ECS_CLUSTER_PROD`, `ECS_SERVICE_PROD`
- **GitHub Token**:
  - `GITHUB_TOKEN` is automatically provided by GitHub Actions.

**Note**: Access to secrets is required for the workflows to function properly. Ensure you have the necessary permissions.

## Notes

- **Version Numbers**: Use the semantic version generated during the Dev build when promoting to other environments (e.g., `1.0.0`).
- **Consistency**: Always ensure the same version is used across environments to maintain consistency.
- **Approvals**: Be mindful of who is designated as an approver in the workflows. Only those users can approve deployments.
- **Monitoring**: Keep an eye on the workflow runs in the **Actions** tab to monitor progress and identify any issues.
- **ECS Deployment**: The deployment steps update the ECS services with the new task definitions that reference the latest Docker image.
- **Terraform Integration**: If you manage infrastructure with Terraform, ensure configurations are updated accordingly.

## Support

If you encounter issues or have questions about the CI/CD pipeline:

- **Review Workflow Logs**: Check the logs in the **Actions** tab for error messages.
- **Contact the Team**: Reach out to the development team or repository maintainers for assistance.
- **Documentation**: Refer to any additional documentation provided in the repository.

---
