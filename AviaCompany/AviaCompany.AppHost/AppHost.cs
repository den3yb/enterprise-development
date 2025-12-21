using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres-aviacompany")
    .AddDatabase("AviaCompanyDb");

var kafka = builder.AddKafka("avia-kafka")
    .WithKafkaUI();

var apiHost = builder.AddProject<Projects.AviaCompany_Api_Host>("apihost")
    .WithReference(postgres, "postgres")
    .WithReference(kafka)
    .WithEnvironment("KAFKA_BOOTSTRAP_SERVERS", kafka.GetEndpoint("tcp"))
    .WithEnvironment("Kafka:TopicName", "flights-topic")
    .WaitFor(postgres)
    .WaitFor(kafka);

var generator = builder.AddProject<Projects.AviaCompany_Generator_Kafka_Host>("flightgenerator")
    .WithReference(kafka)
    .WithEnvironment("KAFKA_BOOTSTRAP_SERVERS", kafka.GetEndpoint("tcp"))
    .WithEnvironment("Kafka:TopicName", "flights-topic")
    .WaitFor(kafka)
    .WaitFor(apiHost); 
builder.Build().Run();