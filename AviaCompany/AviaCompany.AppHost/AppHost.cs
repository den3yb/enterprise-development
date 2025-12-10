var builder = DistributedApplication.CreateBuilder(args);

var realEstateAgencyDb = builder
    .AddPostgres("avia-company-db")
    .AddDatabase("avia-company");

builder.AddProject<Projects.RealEstateAgency_Api_Host>("realestateagency-api-host")
    .WithReference(realEstateAgencyDb, "DatabaseConnection")
    .WaitFor(realEstateAgencyDb);

builder.Build().Run();