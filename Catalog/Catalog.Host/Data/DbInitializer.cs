using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!context.CatalogBrands.Any())
        {
            await context.CatalogBrands.AddRangeAsync(GetPreconfiguredCatalogBrands());

            await context.SaveChangesAsync();
        }

        if (!context.CatalogTypes.Any())
        {
            await context.CatalogTypes.AddRangeAsync(GetPreconfiguredCatalogTypes());

            await context.SaveChangesAsync();
        }

        if (!context.CatalogRadiuses.Any())
        {
            await context.CatalogRadiuses.AddRangeAsync(GetPreconfiguredCatalogRadius());

            await context.SaveChangesAsync();
        }

        if (!context.CatalogItems.Any())
        {
            await context.CatalogItems.AddRangeAsync(GetPreconfiguredItems());

            await context.SaveChangesAsync();
        }
    }

    private static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
    {
        return new List<CatalogBrand>()
        {
            new CatalogBrand() { Brand = "Michelin" },
            new CatalogBrand() { Brand = "Bridgestone" },
            new CatalogBrand() { Brand = "Goodyear" },
            new CatalogBrand() { Brand = "Continental" }
        };
    }

    private static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
    {
        return new List<CatalogType>()
        {
            new CatalogType() { Type = "Summer Tires" },
            new CatalogType() { Type = "Winter Tires" },
            new CatalogType() { Type = "All-Season Tires" }
        };
    }

    private static IEnumerable<CatalogRadius> GetPreconfiguredCatalogRadius()
    {
        return new List<CatalogRadius>()
        {
            new CatalogRadius() { Radius = "R14" },
            new CatalogRadius() { Radius = "R17" },
            new CatalogRadius() { Radius = "R19" }
        };
    }

    private static IEnumerable<CatalogItem> GetPreconfiguredItems()
    {
        return new List<CatalogItem>()
        {
            new CatalogItem
            {
                CatalogTypeId = 1,
                CatalogBrandId = 1,
                CatalogRadiusId = 2,
                AvailableStock = 100,
                Description = "High-performance summer tire designed for sporty driving.",
                Name = "Michelin Pilot Sport 4",
                Price = 200.00M,
                PictureFileName = "1.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 2,
                CatalogBrandId = 2,
                CatalogRadiusId = 1,
                AvailableStock = 100,
                Description = "Winter tire optimized for icy and snowy conditions.",
                Name = "Bridgestone Blizzak WS80",
                Price = 180.00M,
                PictureFileName = "2.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 3,
                CatalogBrandId = 3,
                CatalogRadiusId = 3,
                AvailableStock = 100,
                Description = "Versatile all-season tire with three unique tread zones for various conditions.",
                Name = "Goodyear Assurance TripleTred",
                Price = 160.00M,
                PictureFileName = "3.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 3,
                CatalogBrandId = 4,
                CatalogRadiusId = 2,
                AvailableStock = 100,
                Description = "High-performance all-season tire with excellent wet and dry traction.",
                Name = "Continental ExtremeContact DWS06",
                Price = 190.00M,
                PictureFileName = "4.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 2,
                CatalogBrandId = 1,
                CatalogRadiusId = 3,
                AvailableStock = 100,
                Description = "Premium winter tire designed for ice and snow performance.",
                Name = "Michelin X-Ice Xi3",
                Price = 220.00M,
                PictureFileName = "5.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 1,
                CatalogBrandId = 2,
                CatalogRadiusId = 1,
                AvailableStock = 100,
                Description = "Ultra-high-performance summer tire for responsive handling.",
                Name = "Bridgestone Potenza S-04 Pole Position",
                Price = 210.00M,
                PictureFileName = "6.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 3,
                CatalogBrandId = 3,
                CatalogRadiusId = 2,
                AvailableStock = 100,
                Description = "Sporty all-season tire with strong grip and responsive handling.",
                Name = "Goodyear Eagle Sport All-Season",
                Price = 175.00M,
                PictureFileName = "7.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 2,
                CatalogBrandId = 4,
                CatalogRadiusId = 1,
                AvailableStock = 100,
                Description = "Winter tire designed for improved traction and braking in snowy conditions.",
                Name = "Continental WinterContact SI",
                Price = 185.00M,
                PictureFileName = "8.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 3,
                CatalogBrandId = 1,
                CatalogRadiusId = 2,
                AvailableStock = 100,
                Description = "All-season tire with enhanced wet traction and braking capabilities.",
                Name = "Michelin Premier A/S",
                Price = 195.00M,
                PictureFileName = "9.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 3,
                CatalogBrandId = 2,
                CatalogRadiusId = 3,
                AvailableStock = 100,
                Description = "All-season tire for SUVs and crossovers with comfortable ride.",
                Name = "Bridgestone Dueler H/L Alenza Plus",
                Price = 205.00M,
                PictureFileName = "10.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 2,
                CatalogBrandId = 3,
                CatalogRadiusId = 3,
                AvailableStock = 100,
                Description = "Studdable winter tire designed for icy and snowy roads.",
                Name = "Goodyear Ultra Grip Ice WRT",
                Price = 180.00M,
                PictureFileName = "11.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 3,
                CatalogBrandId = 4,
                CatalogRadiusId = 2,
                AvailableStock = 100,
                Description = "All-terrain tire for trucks and SUVs with off-road capabilities.",
                Name = "Continental TerrainContact A/T",
                Price = 220.00M,
                PictureFileName = "12.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 3,
                CatalogBrandId = 1,
                CatalogRadiusId = 1,
                AvailableStock = 100,
                Description = "All-season tire with long-lasting tread life and comfortable ride.",
                Name = "Michelin Defender T+H",
                Price = 195.00M,
                PictureFileName = "13.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 3,
                CatalogBrandId = 2,
                CatalogRadiusId = 2,
                AvailableStock = 100,
                Description = "Run-flat all-season tire with reinforced sidewalls for continued driving even with a puncture.",
                Name = "Bridgestone DriveGuard",
                Price = 210.00M,
                PictureFileName = "14.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 3,
                CatalogBrandId = 3,
                CatalogRadiusId = 3,
                AvailableStock = 100,
                Description = "Versatile all-terrain tire for trucks and SUVs with strong off-road performance.",
                Name = "Goodyear Ultra Grip All-Terrain Adventure",
                Price = 225.00M,
                PictureFileName = "15.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 3,
                CatalogBrandId = 4,
                CatalogRadiusId = 1,
                AvailableStock = 100,
                Description = "All-season tire with improved fuel efficiency and comfortable ride.",
                Name = "Continental PureContact LS",
                Price = 190.00M,
                PictureFileName = "16.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 3,
                CatalogBrandId = 1,
                CatalogRadiusId = 2,
                AvailableStock = 100,
                Description = "High-performance all-season tire for SUVs with precise handling and grip.",
                Name = "Michelin Latitude Tour HP",
                Price = 205.00M,
                PictureFileName = "17.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 3,
                CatalogBrandId = 2,
                CatalogRadiusId = 1,
                AvailableStock = 100,
                Description = "Ultra-high-performance all-season tire for improved wet and dry performance.",
                Name = "Bridgestone Potenza RE980AS",
                Price = 215.00M,
                PictureFileName = "18.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 2,
                CatalogBrandId = 3,
                CatalogRadiusId = 1,
                AvailableStock = 100,
                Description = "Studdable winter tire designed for cold and snowy conditions.",
                Name = "Goodyear Ultra Grip Winter",
                Price = 175.00M,
                PictureFileName = "19.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 3,
                CatalogBrandId = 4,
                CatalogRadiusId = 3,
                AvailableStock = 100,
                Description = "All-season tire with excellent wet and dry traction and long-lasting tread life.",
                Name = "Continental ContiProContact",
                Price = 180.00M,
                PictureFileName = "20.png"
            }
        };
    }
}