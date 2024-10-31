using System.Collections.Generic;
using AliceHook.Engine.Modifiers.Abstract;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierCancel : ModifierBaseKeywords
    {
        protected override List<string> Keywords { get; } = new List<string>
        {
            "отмен"
        };

        protected override bool CheckState(SessionState state)
        {
            return state.Step != Step.None;
        }

        protected override Phrase Respond(AliceRequest request)
        {
            request.State.Session.Clear();
            return new Phrase(
                "Отменено. Что теперь?",
                new []{ "Добавить вебхук", "Примеры", "Список", "Выход" }
            );
        }
    }
}