using System.Text.RegularExpressions;
using AliceHook.Engine.Modifiers.Abstract;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierAddPhrase : ModifierBase
    {
        public static readonly Regex UrlRegex 
            = new Regex(@"^(?:http(s)?:\/\/)[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");
        
        protected override bool Check(AliceRequest request)
        {
            return request.State.Session.Step == Step.AwaitForUrl && UrlRegex.IsMatch(request.Request.OriginalUtterance);
        }

        protected override Phrase Respond(AliceRequest request)
        {
            request.State.Session.Step = Step.AwaitForKeyword;
            request.State.Session.TempUrl = request.Request.OriginalUtterance;

            return new Phrase(
                "А теперь назовите фразу активации:",
                new []{ "Отмена", "Помощь", "Выход"} 
            );
        }
    }
}