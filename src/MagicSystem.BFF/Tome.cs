using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Toolbox.Infrastructure;

namespace MagicSystem.BFF;

internal static class Tome
{
    internal record CastRequest(string SpellType, JsonDocument Spell);
    
    internal static async Task<IResult> Cast(   
        [FromServices] Loom loom,
        string tomeTitle,
        CastRequest request,
        CancellationToken cancellation = default)
        {
            var parsed = request.Spell.Deserialize(Spells.SpellType.For(request.SpellType), Api.JsonParsingOptions);
            if (parsed == null) return TypedResults.BadRequest("Spell not recognized");
            
            await loom.Cast(new AuditTrail.CorrelationIdentifier(tomeTitle), (Spell)parsed, cancellation).ConfigureAwait(false);
            return TypedResults.Ok();
        }
    
    internal static async Task<IResult> History(
        [FromServices] Loom loom,
        string tomeTitle,
        CancellationToken cancellationToken = default)
        {
            var history = await loom.History(new AuditTrail.CorrelationIdentifier(tomeTitle), cancellationToken).ConfigureAwait(false);
            if (history.Count < 1) return TypedResults.NotFound();
            return TypedResults.Ok(history);
        }
}