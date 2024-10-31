using System.Collections.Generic;
using System.Linq;
using AliceHook.Engine.Modifiers.Abstract;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierList : ModifierBaseKeywords
    {
        protected override List<string> Keywords { get; } = new List<string>
        {
            "список",
            "все вебхуки",
            "все веб хуки",
            "все вебхук и",
            "все webhook и",
            "все вебхук",
            "все webhook"
        };
        
        protected override Phrase Respond(AliceRequest request)
        {
            if (request.State.Session.Step == Step.AwaitForKeyword)
            {
                return new Phrase(
                    "Эта ключевая фраза пересекается с одной из команд данного навыка. Выберите, пожалуйста, другую.",
                    new []{ "Отмена", "Помощь", "Выход" }
                );
            }
            
            if (request.State.User.Webhooks.Count == 0)
            {
                return new Phrase(
                    "У вас пока нет в[screen|е][voice|э]бх[+]уков. Самое время добавить. Можете посмотреть примеры сценариев.",
                    new []{ "Добавить вебхук", "Примеры", "Выход" }
                );
            }

            var removeFirst = $"Удалить {request.State.User.Webhooks.First().Phrase.CapitalizeFirst()}";
            IEnumerable<string> list = request.State.User.Webhooks.Select(w => $"[screen|• {w.Phrase}: {w.Url}]");

            if (request.HasScreen())
            {
                return new Phrase(
                    $"Вывела на экран ваши в[screen|е][voice|э]бх[+]уки:\n\n {list.Join("\n")}\n\nДля удаления скажите" +
                           $" \"Удалить\" и ключевую фразу. Например: \"{removeFirst}\"",
                    new[] {removeFirst, "Помощь", "Выход"}
                );
            }
            
            // no screen
            string wCount = request.State.User.Webhooks.Count.ToPhrase(
                "ключевая фраза",
                "ключевые фразы",
                "ключевых фраз"
            );
            string tts = $"У вас {wCount}: " + request.State.User.Webhooks.Select(w => w.Phrase).Join(" - - ");
            return new Phrase(tts);
        }
        
        protected override bool CheckState(SessionState state)
        {
            return state.Step == Step.None || state.Step == Step.AwaitForKeyword;
        }
    }
}