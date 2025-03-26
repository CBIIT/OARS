<html><head></head><body><h1>CI/CD Pipeline Documentation</h1>
<p>This document explains how to use the CI/CD pipeline for automated building, testing, and deployment of your application.</p>
<h2>Overview</h2>
<p>The CI/CD pipeline automates the following processes:</p>
<ul>
<li>Building and testing your application</li>
<li>Semantic versioning based on commit messages</li>
<li>Creating Docker images</li>
<li>Pushing images to Amazon ECR</li>
<li>Deploying to Amazon ECS</li>
<li>Creating GitHub releases</li>
<li>Triggering approval workflows for promotion to higher environments</li>
</ul>
<h2>Environments</h2>
<p>The pipeline is designed to support multiple environments:</p>
<ul>
<li><strong>DEV</strong>: Automatic deployment on changes to the main branch</li>
<li><strong>QA</strong>: Requires approval after successful DEV deployment</li>
<li><strong>UAT</strong>: Requires approval after successful QA deployment</li>
<li><strong>PROD</strong>: Requires approval after successful UAT deployment</li>
</ul>
<h2>How to Use</h2>
<h3>Commit Message Format</h3>
<p>The pipeline uses commit messages to determine version bumps. Use the following prefixes:</p>

Prefix | Version Bump | Example
-- | -- | --
feat!: | Major | feat!: completely redesign user authentication
feat: | Minor | feat: add new reporting feature
fix: | Patch | fix: resolve null reference in dashboard
wip: | Patch | wip: initial implementation of feature
chore:, docs:, ci:, perf:, refactor:, style:, test: | Patch | chore: update dependencies


<p>Alternatively, you can explicitly specify the version bump type in your commit:</p>
<ul>
<li><code>(major) your commit message</code></li>
<li><code>(minor) your commit message</code></li>
<li><code>(patch) your commit message</code></li>
</ul>
<h3>Workflow Process</h3>
<ol>
<li>
<p><strong>Push to Main Branch</strong>:</p>
<ul>
<li>When code is pushed to the main branch, the pipeline automatically triggers</li>
<li>The application is built and tested</li>
<li>A new version is determined based on commit messages</li>
<li>A Docker image is created and pushed to ECR</li>
<li>The application is deployed to the DEV environment</li>
<li>A GitHub release is created with the new version</li>
<li>An approval workflow is triggered for promotion to QA</li>
</ul>
</li>
<li>
<p><strong>Promotion to QA</strong>:</p>
<ul>
<li>After DEV deployment, approvers will receive a notification</li>
<li>Upon approval, the same version will be deployed to QA</li>
</ul>
</li>
<li>
<p><strong>Promotion to UAT</strong>:</p>
<ul>
<li>After QA deployment, approvers will receive a notification</li>
<li>Upon approval, the same version will be deployed to UAT</li>
</ul>
</li>
<li>
<p><strong>Promotion to PROD</strong>:</p>
<ul>
<li>After UAT deployment, approvers will receive a notification</li>
<li>Upon approval, the same version will be deployed to PROD</li>
</ul>
</li>
</ol>
<h2>Prerequisites</h2>
<p>To use this pipeline, ensure:</p>
<ol>
<li>
<p>Required secrets are configured in GitHub:</p>
<ul>
<li><code>AWS_ARN_DEV</code>: AWS role ARN for DEV environment</li>
<li><code>AWS_DEFAULT_REGION</code>: AWS region</li>
<li><code>ECR_URI_DEV</code>: ECR repository URI for DEV environment</li>
</ul>
</li>
<li>
<p>Required variables are configured in GitHub:</p>
<ul>
<li><code>CONTAINER_REPOSITORY_NAME_DEV</code>: ECR repository name</li>
</ul>
</li>
</ol>
<h2>Monitoring Deployments</h2>
<p>You can monitor deployments:</p>
<ol>
<li>In the GitHub Actions tab of your repository</li>
<li>In the workflow summary that's generated after each run</li>
<li>In the GitHub Releases section, where new versions are published</li>
</ol>
<h2>Troubleshooting</h2>
<p>If a deployment fails:</p>
<ol>
<li>Check the GitHub Actions logs for detailed error information</li>
<li>Verify AWS permissions are correctly configured</li>
<li>Ensure Docker build is successful locally before pushing</li>
</ol>
<h2>Best Practices</h2>
<ol>
<li>Always use semantic commit messages to ensure proper versioning</li>
<li>Write comprehensive tests to avoid deployment of broken code</li>
<li>Keep Docker images as small as possible</li>
<li>Document any changes to the pipeline configuration</li>
</ol>
<h2>Technical Details</h2>
<p>The pipeline uses:</p>
<ul>
<li>GitHub Actions for orchestration</li>
<li>.NET 9.0 for building and testing</li>
<li>Docker for containerization</li>
<li>Amazon ECR for container registry</li>
<li>Amazon ECS for deployment</li>
<li>Semantic versioning based on <a href="https://github.com/paulhatch/semantic-version">paulhatch/semantic-version</a></li>
</ul></body></html># CI/CD Pipeline Documentation

This document explains how to use the CI/CD pipeline for automated building, testing, and deployment of your application.

## Overview

The CI/CD pipeline automates the following processes:
- Building and testing your application
- Semantic versioning based on commit messages
- Creating Docker images
- Pushing images to Amazon ECR
- Deploying to Amazon ECS
- Creating GitHub releases
- Triggering approval workflows for promotion to higher environments

## Environments

The pipeline is designed to support multiple environments:
- **DEV**: Automatic deployment on changes to the main branch
- **QA**: Requires approval after successful DEV deployment
- **UAT**: Requires approval after successful QA deployment
- **PROD**: Requires approval after successful UAT deployment

## How to Use

### Commit Message Format

The pipeline uses commit messages to determine version bumps. Use the following prefixes:

| Prefix | Version Bump | Example |
|--------|--------------|---------|
| `feat!:` | Major | `feat!: completely redesign user authentication` |
| `feat:` | Minor | `feat: add new reporting feature` |
| `fix:` | Patch | `fix: resolve null reference in dashboard` |
| `wip:` | Patch | `wip: initial implementation of feature` |
| `chore:`, `docs:`, `ci:`, `perf:`, `refactor:`, `style:`, `test:` | Patch | `chore: update dependencies` |

Alternatively, you can explicitly specify the version bump type in your commit:
- `(major) your commit message`
- `(minor) your commit message`
- `(patch) your commit message`

### Workflow Process

1. **Push to Main Branch**:
   - When code is pushed to the main branch, the pipeline automatically triggers
   - The application is built and tested
   - A new version is determined based on commit messages
   - A Docker image is created and pushed to ECR
   - The application is deployed to the DEV environment
   - A GitHub release is created with the new version
   - An approval workflow is triggered for promotion to QA

2. **Promotion to QA**:
   - After DEV deployment, approvers will receive a notification
   - Upon approval, the same version will be deployed to QA

3. **Promotion to UAT**:
   - After QA deployment, approvers will receive a notification
   - Upon approval, the same version will be deployed to UAT

4. **Promotion to PROD**:
   - After UAT deployment, approvers will receive a notification
   - Upon approval, the same version will be deployed to PROD

## Prerequisites

To use this pipeline, ensure:
1. Required secrets are configured in GitHub:
   - `AWS_ARN_DEV`: AWS role ARN for DEV environment
   - `AWS_DEFAULT_REGION`: AWS region
   - `ECR_URI_DEV`: ECR repository URI for DEV environment

2. Required variables are configured in GitHub:
   - `CONTAINER_REPOSITORY_NAME_DEV`: ECR repository name

## Monitoring Deployments

You can monitor deployments:
1. In the GitHub Actions tab of your repository
2. In the workflow summary that's generated after each run
3. In the GitHub Releases section, where new versions are published

## Troubleshooting

If a deployment fails:
1. Check the GitHub Actions logs for detailed error information
2. Verify AWS permissions are correctly configured
3. Ensure Docker build is successful locally before pushing

## Best Practices

1. Always use semantic commit messages to ensure proper versioning
2. Write comprehensive tests to avoid deployment of broken code
3. Keep Docker images as small as possible
4. Document any changes to the pipeline configuration

## Technical Details

The pipeline uses:
- GitHub Actions for orchestration
- .NET 9.0 for building and testing
- Docker for containerization
- Amazon ECR for container registry
- Amazon ECS for deployment
- Semantic versioning based on [[paulhatch/semantic-version](https://github.com/paulhatch/semantic-version)](https://github.com/paulhatch/semantic-version)