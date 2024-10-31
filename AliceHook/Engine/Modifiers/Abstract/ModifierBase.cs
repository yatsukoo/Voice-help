using AliceHook.Models;

namespace AliceHook.Engine.Modifiers.Abstract
{
    public abstract class ModifierBase
    {
        public bool Run(AliceRequest request, out AliceResponse response)
        {
            if (!Check(request))
            {
                response = null;
                return false;
            }
            
            response = CreateResponse(request);
            return true;
        }

        protected virtual AliceResponse CreateResponse(AliceRequest request)
        {
            Phrase phrase = Respond(request);
            return phrase.Generate(request);
        }

        protected abstract bool Check(AliceRequest request);
        protected abstract Phrase Respond(AliceRequest request);
    }
}