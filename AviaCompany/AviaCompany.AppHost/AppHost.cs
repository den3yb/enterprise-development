var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("postgres")
	.WithPgAdmin();

var api = builder.AddProject<Projects.AviaCompany_Api_Host>("apihost")
	.WithReference(db)
	.WaitFor(db);

builder.Build().Run();