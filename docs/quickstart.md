# Quick Start Guide for Developers

This guide will help you get started with developing the Amsterdam Coffee Shop application on your local machine.

## Prerequisites

1. **Development Environment**:
   - [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
   - [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/)
   - [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)
   - [Git](https://git-scm.com/downloads)

2. **Azure Tools** (for deployment and testing):
   - [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
   - [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio) (optional)

## Clone the Repository

```bash
git clone https://github.com/yourusername/amsterdam-coffee-shop.git
cd amsterdam-coffee-shop
```

## Local Development Setup

### 1. Update the Local Connection String

Edit `AmsterdamCoffeeShop/appsettings.Development.json` and update the connection string to point to your local SQL Server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AmsterdamCoffeeShop;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 2. Create the Database

```bash
# Navigate to the project directory
cd AmsterdamCoffeeShop

# Apply database migrations
dotnet ef database update
```

### 3. Run the Application Locally

```bash
dotnet run
```

The application will be available at `https://localhost:5001` and `http://localhost:5000`

## Project Structure

### Key Components

- **Models**: Data models for customers, products, orders, etc.
- **Pages**: Razor Pages for the web application
- **Services**: Business logic and data access services
- **wwwroot**: Static files (CSS, JavaScript, images)
- **infra**: Infrastructure as Code (Bicep templates)

### Important Files

- `Program.cs`: Application startup and configuration
- `ApplicationDbContext.cs`: Entity Framework Core database context
- `_Layout.cshtml`: Main layout template
- `.github/workflows/deploy.yml`: CI/CD pipeline configuration

## Development Workflow

1. **Create a Branch**: Always work on a feature branch, not directly on `main`
   ```bash
   git checkout -b feature/new-feature-name
   ```

2. **Make Changes**: Implement your features or fixes

3. **Run Tests**: Make sure to test your changes
   ```bash
   dotnet test
   ```

4. **Commit and Push**:
   ```bash
   git add .
   git commit -m "Description of your changes"
   git push origin feature/new-feature-name
   ```

5. **Create a Pull Request**: On GitHub, create a PR from your branch to `main`

## Working with Azure Resources

If you need to test with Azure resources:

1. **Login to Azure**:
   ```bash
   az login
   ```

2. **Set the Active Subscription** (if you have multiple):
   ```bash
   az account set --subscription <subscription-id>
   ```

3. **View Deployed Resources**:
   ```bash
   az resource list --resource-group amsterdam-coffee-rg --output table
   ```

## Database Migrations

When you change the data model:

1. **Create a Migration**:
   ```bash
   dotnet ef migrations add <MigrationName>
   ```

2. **Apply the Migration**:
   ```bash
   dotnet ef database update
   ```

## Debugging Tips

1. **Visual Studio**: Press F5 to start debugging
2. **Visual Studio Code**: Use the .NET Core launch configuration
3. **Command Line**: Use `dotnet run --launch-profile "Development"`
4. **SQL Server**: Use SQL Server Management Studio or Azure Data Studio to inspect the database

## Getting Help

- Check the [official documentation](../README.md)
- Review the [architecture overview](../README.md#architecture-overview)
- Look at the [setup guide](setup-guide.md) for infrastructure details
- Refer to the [CI/CD pipeline documentation](cicd-pipeline.md) for deployment issues
