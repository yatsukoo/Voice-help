using System.Collections.Generic;
using System.Linq;

namespace AliceHook.Models
{
    public class UserState
    {
        public List<Webhook> Webhooks { get; } = new List<Webhook>();
        public Webhook FindWebhook(string phrase)
        {
            return Webhooks.FirstOrDefault(w =>
            {
                string shorten = w.Phrase.Replace(" ", "");
                string startPhrase = phrase.SafeSubstring(shorten.Length);
                return Utils.LevenshteinRatio(startPhrase, shorten) < Utils.PossibleRatio(shorten.Length);
            });
        }
    }
}