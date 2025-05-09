# Amsterdam Coffee Shop Deployment Summary

## Deployment Information
- **Resource Group**: amsterdam-coffee-rg
- **Region**: Sweden Central
- **Deployment Date**: 2025-05-09

## Deployed Resources

### Web Application
- **Name**: amsterdamcoffee-app-prod
- **URL**: https://amsterdamcoffee-app-prod.azurewebsites.net
- **App Service Plan**: amsterdamcoffee-plan-prod (B1)

### SQL Database
- **Server**: amsterdamcoffee-sql-prod.database.windows.net
- **Database**: amsterdamcoffee-db-prod
- **Admin Username**: coffeeshopadmin
- **Note**: The SQL admin password was securely generated during deployment

### Key Vault
- **Name**: amsterdamcoffeevaultprod

### Application Insights
- **Name**: amsterdamcoffee-insights-prod

### Virtual Network
- **Name**: amsterdamcoffee-vnet-prod
- **Subnet**: web-subnet

## Next Steps
1. Store sensitive application settings in Key Vault
2. Deploy your application code using the GitHub Actions workflow
3. Set up CI/CD pipeline from your GitHub repository

For more information, refer to:
- [Setup Guide](docs/setup-guide.md)
- [CI/CD Pipeline Documentation](docs/cicd-pipeline.md)
