using Microsoft.EntityFrameworkCore;
using PIHLSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTests
{
    public class DbSetup : IDisposable
    {
        protected DbContextOptions<PIHLDBContext> ContextOptions { get; }
        public PIHLDBContext PIHLDBContext { get; set; }

        public DbSetup()
        {
            ContextOptions = new DbContextOptionsBuilder<PIHLDBContext>()
                .UseInMemoryDatabase(databaseName: "testing")
                .EnableSensitiveDataLogging()
                .Options;

            PIHLDBContext = new PIHLDBContext(ContextOptions);

            PIHLDBContext.Database.EnsureDeleted();
            PIHLDBContext.Database.EnsureCreated();

            PIHLDBContext.SaveChanges();
        }

        public void Dispose()
        {
            // clean-up code
        }
    }
}
