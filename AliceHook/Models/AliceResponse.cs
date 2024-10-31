using AliceHook.Models.Abstract;

namespace AliceHook.Models
{
    public class AliceResponse : AliceResponseBase<UserState, SessionState>
    {
        public AliceResponse(
            AliceRequest request,
            SessionState sessionState = default,
            UserState userState = default
        ) : base(request, sessionState, userState) { }
    }
}