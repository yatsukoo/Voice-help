using System.Collections.Generic;
using System.Linq;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers.Abstract
{
    public abstract class ModifierBaseKeywords : ModifierBase
    {
        protected abstract List<string> Keywords { get; }
        
        protected abstract bool CheckState(SessionState state);
        
        protected override bool Check(AliceRequest request)
        {
            return CheckState(request.State.Session) && CheckTokens(request);
        }
        
        protected bool CheckTokens(AliceRequest request)
        {
            return CheckTokens(request.Request.Nlu.Tokens, Keywords.ToArray());
        }

        private bool CheckTokens(IEnumerable<string> tokens, params string[] expected)
        {
            return expected.Any(expectedString =>
            {
                string[] expectedTokens = expectedString.Split(" ");
                return expectedTokens.All(tokens.ContainsStartWith);
            });
        }
    }
}