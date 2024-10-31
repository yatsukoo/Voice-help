using System.Collections.Generic;
using AliceHook.Engine.Modifiers.Abstract;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierExit : ModifierBaseKeywords
    {
        protected override List<string> Keywords { get; } = new List<string>
        {
            "выход",
            "выйти",
            "пока"
        };

        protected override bool CheckState(SessionState state)
        {
            return true; // any state
        }

        protected override AliceResponse CreateResponse(AliceRequest request)
        {
            AliceResponse resp = base.CreateResponse(request);
            resp.Response.EndSession = true;
            return resp;
        }

        protected override Phrase Respond(AliceRequest request)
        {
            return new Phrase("Выхож[+]у. Хорошего дня.");
        }
    }
}