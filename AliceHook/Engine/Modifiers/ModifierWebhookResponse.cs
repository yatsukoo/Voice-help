using System.Linq;
using AliceHook.Engine.Modifiers.Abstract;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierWebhookResponse : ModifierBase
    {
        private readonly string[] Yes = {
            "да",
            "ага",
            "давай",
            "узнай",
            "ок",
            "хорошо",
            "угу"
        };
        
        protected override bool Check(AliceRequest request)
        {
            return request.State.Session.Step == Step.AwaitWebhookResponse;
        }

        protected override Phrase Respond(AliceRequest request)
        {
            request.State.Session.Step = Step.None;
            
            string command = request.Request.Command;
            if (Yes.Any(x => command.StartsWith(x)))
            {
                // yes
                if (!request.State.Session.LastResult.IsNullOrEmpty())
                {
                    return new Phrase(
                        "Вебхук ответил:\n" + request.State.Session.LastResult,
                        new []{ "Список", "Помощь", "Выход" }
                    );
                }

                if (!request.State.Session.LastError.IsNullOrEmpty())
                {
                    return new Phrase(
                        "К сожалению, с вызовом произошла ошибка. Что теперь?",
                        new []{ "Список", "Помощь", "Выход" }
                    );
                }

                return new Phrase(
                    "К сожалению, получить ответ от вебхука не удалось. Что теперь?",
                    new[] {"Список", "Помощь", "Выход"}
                );
            }

            return new Phrase(
                "Ожидание ответа от вебхука прекращено. Что дальше?",
                new []{ "Добавить вебхук", "Список", "Примеры", "Выход" }
            );
        }
    }
}