namespace AliceHook.Models
{
    public class SessionState
    {
        public Step Step { get; set; } = Step.None;
        public string TempUrl { get; set; } = "";
        public string LastResult { get; set; } = "";
        public string LastError { get; set; } = "";

        public void Clear()
        {
            Step = Step.None;
            TempUrl = "";
            LastError = "";
            LastResult = "";
        }
    }

    public enum Step
    {
        None,
        AwaitForUrl,
        AwaitForKeyword,
        AwaitWebhookResponse
    }
}