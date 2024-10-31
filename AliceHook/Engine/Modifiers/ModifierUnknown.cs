using AliceHook.Engine.Modifiers.Abstract;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierUnknown : ModifierBase
    {
        protected override bool Check(AliceRequest request)
        {
            return true;
        }

        protected override Phrase Respond(AliceRequest request)
        {
            if (request.State.Session.Step == Step.None)
            {
                if (request.HasScreen())
                {
                    return new Phrase(
                        "Команда не распознана. Вы можете добавить в[screen|е][voice|э]бх[+]ук, " +
                        "или посмотреть список ключевых фраз. Что хотите сделать?",
                        new []{ "Добавить вебхук", "Список", "Помощь", "Выход" }
                    );
                }
                
                // no screen
                return new Phrase(
                    "Команда не распознана. Вы можете прослушать список ключевых фраз либо выйти. " +
                    "Что хотите сделать?"
                );
            }
            return ModifierHelp.GetHelp(request.State.Session.Step, request.HasScreen());
        }
    }
}