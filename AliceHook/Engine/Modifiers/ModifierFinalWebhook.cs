using AliceHook.Engine.Modifiers.Abstract;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierFinalWebhook : ModifierBase
    {
        protected override bool Check(AliceRequest request)
        {
            return request.State.Session.Step == Step.AwaitForKeyword && request.HasScreen();
        }

        protected override Phrase Respond(AliceRequest request)
        {
            Webhook exists = request.State.User.FindWebhook(
                request.Request.Command
                    .ToLower()
                    .Trim()
                    .Replace(" ", "")
            );
            
            if (exists != null)
            {
                return new Phrase(
                    $"У вас уже есть вебхук с похожей ключевой фразой: {exists.Phrase.CapitalizeFirst()}. " +
                    "Назовите другую фразу.",
                    new []{ "Отмена", "Помощь", "Выход" }
                );
            }

            var webhook = new Webhook
            {
                Phrase = request.Request.Command,
                Url = request.State.Session.TempUrl
            };
            
            request.State.User.Webhooks.Add(webhook);
            request.State.Session.Clear();

            return new Phrase(
                $"Теперь, когда вы скажете фразу, которая начинается на \"{webhook.Phrase.CapitalizeFirst()}" +
                "\", я вызову этот адрес и передам туда весь ваш текст в параметр [screen|value1][voice|вэлью 1]" +
                ".[p|1000]\n\nЧто делаем дальше?", 
                new []{ "Добавить вебхук", "Список", "Примеры", "Выход" }
            );
        }
    }
}