

using StarWars.Characters;
using StarWars.Repositories;
using StarWars.Reviews;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    // In order to use the ASP.NET Core routing we need to add the routing services.
    .AddRouting()
    
    // We will add some in-memory repositories that hold the in-memory data for this GraphQL server.
    .AddSingleton<ICharacterRepository, CharacterRepository>()
    .AddSingleton<IReviewRepository, ReviewRepository>()
    
    // Next we are adding our GraphQL server configuration. 
    // We can host multiple named GraphQL server configurations
    // that can be exposed on different routes.
    .AddGraphQLServer()
    
    // The query types are split into two classes,
    // by splitting the types into several class we can organize 
    // our query fields by topics and also am able to test
    // them separately.
    .AddQueryType()
    .AddTypeExtension<CharacterQueries>()
    .AddTypeExtension<ReviewQueries>()
    .AddMutationType()
    .AddTypeExtension<ReviewMutations>()
    .AddSubscriptionType()
    .AddTypeExtension<ReviewSubscriptions>()
    
    // The type discover is very good in exploring the types that we are using 
    // but sometimes when a GraphQL field for instance only exposes an interface 
    // we need to provide we need to provide the implementation types that we want
    // to host in our GraphQL schema.
    .AddType<Human>()
    .AddType<Droid>()
    .AddType<Starship>()
    .AddTypeExtension<CharacterResolvers>()

    // Add filtering and sorting capabilities.
    .AddFiltering()
    .AddSorting()

    // if you wanted to controll the pagination settings globally you could
    // do so by setting the paging options.
    // .SetPagingOptions()

    // Since we are exposing a subscription type we also need a pub/sub system 
    // handling the subscription events. For our little demo here we will use 
    // an in-memory pub/sub system.
    .AddInMemorySubscriptions()

    // Last we will add apollo tracing to our server which by default is 
    // only activated through the X-APOLLO-TRACING:1 header.
    .AddApolloTracing();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
// in order to expose our GraphQL schema we need to map the GraphQL server 
// to a specific route. By default it is mapped onto /graphql.
app
    .UseWebSockets()
    .UseRouting()
    .UseEndpoints(endpoint => endpoint.MapGraphQL());

app.Run();