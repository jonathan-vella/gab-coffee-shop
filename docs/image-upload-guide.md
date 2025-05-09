# Adding Images to Your Coffee Shop Website

This guide provides instructions on how to find, download, and add relevant images to your Amsterdam Coffee Shop website.

## Required Images

Your coffee shop website needs the following images:

1. Logo image (`coffee-logo.png`)
2. Hero/banner image (`coffee-hero.jpg`)
3. Coffee shop image (`coffee-shop.jpg`) 
4. Product images:
   - `espresso.jpg`
   - `cappuccino.jpg`
   - `latte.jpg`
   - `iced-coffee.jpg`
   - `americano.jpg`

## Finding Free Images

Here are some sources for free, high-quality images:

1. [Unsplash](https://unsplash.com/): Search for "coffee", "coffee shop", "espresso", etc.
2. [Pexels](https://www.pexels.com/): Another great source for free stock photos
3. [Pixabay](https://pixabay.com/): Free stock photos and vectors

## Preparing Images

Once you've found suitable images:

1. Download the images to your computer
2. Resize them to appropriate dimensions:
   - Logo: 200px × 80px
   - Hero image: 1920px × 600px
   - Coffee shop image: 800px × 600px
   - Product images: 800px × 600px or square (600px × 600px)
3. Optimize them for web (compress them using tools like [TinyPNG](https://tinypng.com/))

## Adding Images to Your Website

### Method 1: Direct Upload to App Service

1. Open the Azure Portal
2. Navigate to your App Service
3. Select "Advanced Tools" or "Kudu"
4. Click on "Debug Console" > "CMD" or "PowerShell"
5. Navigate to `site/wwwroot/wwwroot/images/`
6. Use the upload button to upload all your images

### Method 2: Include in Deployment

1. Add the images to the `/AmsterdamCoffeeShop/wwwroot/images/` directory in your local project
2. Ensure the images are included in your git repository
3. Push the changes to GitHub
4. The CI/CD pipeline will automatically deploy them with your application

### Method 3: Using Azure Blob Storage (for better performance)

For a more scalable solution, especially for many images:

1. Create an Azure Storage account
2. Create a blob container (set to "public" access level)
3. Upload your images to the container
4. Use the direct URLs to the images in your website code

Example configuration for adding Azure Storage:

```csharp
// Program.cs - Add this to your service configuration
builder.Services.AddSingleton<IAzureStorageService>(provider =>
    new AzureStorageService(
        builder.Configuration["Storage:ConnectionString"],
        builder.Configuration["Storage:ContainerName"]
    )
);
```

## Testing Images

After adding your images:

1. Deploy your website (if using Method 2 or 3)
2. Navigate to your website URL
3. Verify that all images load correctly
4. Check for any broken image links
5. Test on different devices to ensure responsive behavior

## Troubleshooting Image Issues

If images aren't displaying:

1. **Check paths**: Make sure the paths in your HTML match the actual file locations
2. **File permissions**: Ensure proper permissions are set for the image files
3. **Case sensitivity**: URLs are case-sensitive, so `Coffee.jpg` is different from `coffee.jpg`
4. **MIME types**: Ensure the web server is configured to serve the correct MIME types for your images
5. **Browser cache**: Try hard-refreshing your browser (Ctrl+F5) to clear the cache

## Image Naming Best Practices

1. Use lowercase letters
2. Use hyphens instead of spaces or underscores
3. Use descriptive names
4. Include relevant keywords
5. Be consistent with naming conventions

## Adding New Products

When adding new coffee products to your shop:

1. Acquire a suitable image for the new product
2. Follow the same naming convention (e.g., `product-name.jpg`)
3. Upload the image using one of the methods above
4. Add the new product to your database, including the image path
5. The new product will automatically appear in the product catalog
