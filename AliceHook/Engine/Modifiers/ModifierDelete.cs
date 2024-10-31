using System.Linq;
using System.Text.RegularExpressions;
using AliceHook.Engine.Modifiers.Abstract;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierDelete : ModifierBase
    {
        protected override bool Check(AliceRequest request)
        {
            return request.State.Session.Step == Step.None 
                   && request.Request.Command.ToLower().StartsWith("удали") 
                   && request.Request.Nlu.Tokens.Count > 1
                   && GetWebhook(request) != null;
        }

        protected override Phrase Respond(AliceRequest request)
        {
            Webhook w = GetWebhook(request);

            if (w == null)
            {
                return new Phrase(
                    "Не могу найти в[screen|е][voice|э]бх[+]ук по вашему запросу. Что хотите сделать сейчас?",
                    new []{ "Добавить вебхук", "Примеры", "Список", "Выход" }
                );
            }
            
            request.State.User.Webhooks.Remove(w);

            return new Phrase(
                $"Удален в[screen|е][voice|э]бх[+]ук: {w.Phrase.CapitalizeFirst()}. Что теперь?", 
                new []{ "Добавить вебхук", "Список", "Примеры", "Выход" }
            );
        }

        private Webhook GetWebhook(AliceRequest request)
        {
            string[] tokens = Regex.Split(request.Request.Command.ToLower(), "\\s+");
            if (tokens.Length <= 1)
            {
                return null;
            }
            string trimmedCommand = tokens.Skip(1).Join("").Trim();
            return request.State.User.FindWebhook(trimmedCommand);
        }
    }
}