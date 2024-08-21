using System.Text.Json;
using System.Text.Json.Serialization;

namespace MagicSystem.BFF
{
    internal static class Api
    {
        public static readonly JsonSerializerOptions JsonParsingOptions = new(JsonSerializerDefaults.Web)
            { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
        
        internal static RouteGroupBuilder BuildApiEndpoints(this RouteGroupBuilder builder)
        {
            Tomes.Configure(builder);
            Spells.Configure(builder);
            builder.WithOpenApi();
            
            return builder;
        }

        private static class Tomes
        {
            private static class Endpoint // TODO HATEOAS endpoints generator for a tome's spells
            {
                internal const string Cast = "tomes-cast";
                internal const string History = "tomes-history";
            }

            internal static void Configure(RouteGroupBuilder builder)
            {
                var tomes = builder.MapGroup("tomes");
                
                tomes.MapGet("{tomeName}", Tome.History)
                    .WithName(Endpoint.History)
                    .Produces(StatusCodes.Status200OK)
                    .ProducesProblem(StatusCodes.Status404NotFound)
                    .WithSummary("The history of spells cast by the tome");
                
                tomes.MapPost("{tomeName}/cast", Tome.Cast)
                    .WithName(Endpoint.Cast)
                    .Produces(StatusCodes.Status200OK)
                    .WithSummary("Cast a spell");
            }
        }
        
        private static class Spells
        {
            private static class Endpoint
            {
                internal const string All = "spells-all";
            }

            internal static void Configure(RouteGroupBuilder builder)
            {
                var tomes = builder.MapGroup("spells");
                
                tomes.MapGet("", () => MagicSystem.Spells.SpellType.All)
                    .WithName(Endpoint.All)
                    .Produces(StatusCodes.Status200OK)
                    .WithSummary("List all spells");
            }
        }
    }
}

