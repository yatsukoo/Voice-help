using System.Collections.Generic;
using AliceHook.Engine.Modifiers.Abstract;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierAddWebhook : ModifierBaseKeywords
    {
        protected override List<string> Keywords { get; } = new List<string>
        {
            "добав вебхук",
            "новый вебхук",
            "созда вебхук",
            "добав webhook",
            "новый webhook",
            "созда webhook",
            "добав веб хук",
            "новый веб хук",
            "созда веб хук",
        };

        protected override bool CheckState(SessionState state)
        {
            return state.Step == Step.None;
        }

        protected override Phrase Respond(AliceRequest request)
        {
            if (!request.HasScreen())
            {
                return new Phrase("Добавлять в[screen|е][voice|э]бх[+]уки можно только на устройстве с экраном");
            }
            
            if (request.State.User.Webhooks.Count >= 20)
            {
                return new Phrase(
                    "У вас очень много в[screen|е][voice|э]бх[+]уков, удалите что-нибудь сначала с помощью команды \"Список\"",
                    new []{ "Список", "Помощь", "Выход" }
                );
            }
            
            request.State.Session.Step = Step.AwaitForUrl;
            return new Phrase(
                "Введите URL в[screen|е][voice|э]бх[+]ука:",
                new []{ "Отмена", "Помощь", "Выход" }
            );
        }
    }
}