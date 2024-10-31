using System.Collections.Generic;
using AliceHook.Engine.Modifiers.Abstract;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierExampleList : ModifierBaseKeywords
    {
        protected override List<string> Keywords { get; } = new List<string>
        {
            "пример",
            "например"
        };
        
        protected override Phrase Respond(AliceRequest request)
        {
            return new Phrase(
                "Вот список некоторых пользовательских сценариев, которые возможны с помощью этого навыка.",
                new []{ "Добавить вебхук", "Список вебхуков", "Выход" }
            );
        }

        protected override AliceResponse CreateResponse(AliceRequest request)
        {
            AliceResponse response = base.CreateResponse(request);
            ModifierHelp.AddExamplesTo(response);

            return response;
        }

        protected override bool CheckState(SessionState state)
        {
            return state.Step == Step.None;
        }

        protected override bool Check(AliceRequest request)
        {
            return request.HasScreen() && base.Check(request);
        }
    }
}