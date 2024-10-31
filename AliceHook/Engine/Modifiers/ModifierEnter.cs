using AliceHook.Engine.Modifiers.Abstract;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierEnter : ModifierBase
    {
        protected override bool Check(AliceRequest request)
        {
            return request.Request.Command == "";
        }

        protected override Phrase Respond(AliceRequest request)
        {
            if (request.State.User.Webhooks.Count > 0)
            {
                return new Phrase(
                    "Слушаю",
                    new []{ "Добавить вебхук", "Список", "Примеры", "Выход" }
                );
            }
            
            return ModifierHelp.GetHelp(Step.None, request.HasScreen());
        }

        protected override AliceResponse CreateResponse(AliceRequest request)
        {
            AliceResponse response = base.CreateResponse(request);
            if (request.State.User.Webhooks.Count == 0)
            {
                ModifierHelp.AddExamplesTo(response);
            }

            return response;
        }
    }
}