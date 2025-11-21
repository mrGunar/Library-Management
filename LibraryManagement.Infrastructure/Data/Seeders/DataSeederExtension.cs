using SimpleInjector;

namespace LibraryManagement.Infrastructure.Data.Seeders
{
    public static class DataSeederExtension
    {
        public static void SeedDatabaseAsync( this Container container)
        {
            using (var ctx = new ApplicationDbContext())
            {
                DataBaseSeeder.SeedData(ctx);
            }

        }
    }
}
