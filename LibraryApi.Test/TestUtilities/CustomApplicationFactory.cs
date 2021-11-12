using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Models.Entities;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using LibraryApi.Services.Interfaces;

namespace LibraryApi.Test.TestUtilities
{
    public class CustomApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        HttpClient TestClient = null;

        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // find the db registered in our startup class
                ServiceDescriptor registeredApplicationDb = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<IdentityContext>));

                // remove it
                if (registeredApplicationDb != null) services.Remove(registeredApplicationDb);

                // find the authorization registered in our startup class
                ServiceDescriptor registeredApplicationAuth = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(AuthenticationOptions));

                // remove it
                if (registeredApplicationAuth != null) services.Remove(registeredApplicationAuth);

                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // add in-memory db
                services.AddDbContext<IdentityContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryKomradsLibraryTest");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                // creating a ServiceProvider Scope
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;

                    var db = scopedServices.GetRequiredService<IdentityContext>();
                    
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomApplicationFactory<TStartup>>>();

                    try
                    {
                        db.Database.EnsureCreated();
                        Seeding.InitializeData(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "an error occurred while creating database. Error {Message}:",
                            ex.Message);
                    }
                }
            });
        }

        // Creating a client
        public HttpClient GetTestClient()
        {
            TestClient = CreateClient();
            TestClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            base.ConfigureClient(TestClient);
            return TestClient;
        }
    }
}
