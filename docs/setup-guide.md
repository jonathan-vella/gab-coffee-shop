# Setting Up Your Azure Infrastructure and CI/CD Pipeline

This guide provides step-by-step instructions for setting up the Azure infrastructure for the Amsterdam Coffee Shop application and configuring the GitHub CI/CD pipeline.

## Prerequisites

Before you begin, make sure you have:

1. **Azure CLI** installed on your local machine
2. **Git** installed on your local machine
3. **GitHub account** with access to create repositories
4. **Azure subscription**

## Step 1: Create a GitHub Repository

1. Go to GitHub and create a new repository named `amsterdam-coffee-shop`
2. Clone the repository to your local machine:

```bash
git clone https://github.com/yourusername/amsterdam-coffee-shop.git
cd amsterdam-coffee-shop
```

3. Copy all the project files into this directory

## Step 2: Set Up Azure Environment

### Create Azure Resource Group

```bash
# Login to Azure
az login

# Create a resource group
az group create --name amsterdam-coffee-rg --location westeurope
```

### Create a Service Principal for GitHub Actions

```bash
# Create a service principal with contributor access to the resource group
az ad sp create-for-rbac --name "AmsterdamCoffeeGitHubActions" \
  --role contributor \
  --scopes /subscriptions/$(az account show --query id -o tsv)/resourceGroups/amsterdam-coffee-rg \
  --sdk-auth
```

The command will output a JSON object like this:

```json
{
  "clientId": "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
  "clientSecret": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
  "subscriptionId": "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
  "tenantId": "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
  "activeDirectoryEndpointUrl": "https://login.microsoftonline.com",
  "resourceManagerEndpointUrl": "https://management.azure.com/",
  "activeDirectoryGraphResourceId": "https://graph.windows.net/",
  "sqlManagementEndpointUrl": "https://management.core.windows.net:8443/",
  "galleryEndpointUrl": "https://gallery.azure.com/",
  "managementEndpointUrl": "https://management.core.windows.net/"
}
```

**Save this JSON**, as you'll need it in the next step.

## Step 3: Configure GitHub Secrets

Go to your GitHub repository settings to add the necessary secrets:

1. Navigate to your repository on GitHub
2. Click on "Settings" > "Secrets and variables" > "Actions"
3. Add the following secrets:

| Secret Name | Value |
|-------------|-------|
| `AZURE_CREDENTIALS` | The entire JSON output from the service principal creation |
| `SQL_ADMIN_USERNAME` | A username for your SQL server administrator (e.g., `coffeeshopadmin`) |
| `SQL_ADMIN_PASSWORD` | A strong password for your SQL server administrator |

## Step 4: Commit and Push Your Code

```bash
# Add all files to git
git add .

# Commit the files
git commit -m "Initial commit with project files"

# Push to GitHub
git push -u origin main
```

## Step 5: Trigger the CI/CD Pipeline

1. Go to your GitHub repository
2. Click on the "Actions" tab
3. You should see the "Build and Deploy" workflow running
4. If not, you can manually trigger it by clicking "Run workflow"

## Step 6: Verify the Deployment

Once the workflow completes successfully:

1. Log in to the Azure Portal (https://portal.azure.com)
2. Navigate to the resource group `amsterdam-coffee-rg`
3. Verify that all the resources are created:
   - App Service Plan
   - App Service
   - Virtual Network
   - SQL Server
   - SQL Database
   - Key Vault
   - Application Insights
4. Click on the App Service to find the URL of your deployed application
5. Visit the URL to verify that the website is up and running

## Troubleshooting

### Common Issues and Solutions

#### Authentication Failures

If you see authentication errors in your GitHub Actions workflow:

1. Verify that the `AZURE_CREDENTIALS` secret is correctly formatted
2. Check that the service principal still has the required permissions
3. Recreate the service principal if necessary

#### Deployment Failures

If the Bicep deployment fails:

1. Check the error message in the GitHub Actions log
2. Verify that there are no naming conflicts in Azure
3. Make sure the resource names don't exceed Azure's character limits
4. Check for any policy restrictions in your Azure subscription

#### Application Errors

If the application is deployed but not working:

1. Check the App Service logs in the Azure Portal
2. Verify that the connection string is correctly configured
3. Make sure the Key Vault access policies are properly set up
4. Check that Application Insights is properly connected

## Next Steps

After successful deployment, you may want to:

1. Set up custom domain and SSL certificate for your web application
2. Configure additional security features like Azure Web Application Firewall
3. Set up monitoring alerts in Application Insights
4. Implement database backup and restore procedures
5. Implement automated testing in your CI/CD pipeline

## Resources

- [Azure App Service Documentation](https://docs.microsoft.com/en-us/azure/app-service/)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Azure Bicep Documentation](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/)
- [Azure Key Vault Documentation](https://docs.microsoft.com/en-us/azure/key-vault/)
- [Azure SQL Database Documentation](https://docs.microsoft.com/en-us/azure/azure-sql/)
- [Application Insights Documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview)
