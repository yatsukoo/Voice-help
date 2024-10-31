using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AliceHook.Engine.Modifiers.Abstract;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierRunWebhook : ModifierBase
    {
        protected override bool Check(AliceRequest request)
        {
            return request.State.Session.Step == Step.None && GetWebhook(request) != null;
        }

        protected override AliceResponse CreateResponse(AliceRequest request)
        {
            AliceResponse response = base.CreateResponse(request);
            bool isOutsideCommand = !request.Request.Command.IsNullOrEmpty() && request.Session?.New == true;
            response.Response.EndSession = request.State.Session.Step != Step.AwaitWebhookResponse && isOutsideCommand;
            return response;
        }

        protected override Phrase Respond(AliceRequest request)
        {
            Webhook webhook = GetWebhook(request);
            string[] tokens = request.Request.Command.ToLower().Split(" ");
            int skipCount = Utils.OptimalSkipLength(webhook.Phrase, tokens);
            string textToSend = tokens.Skip(skipCount).Join(" ").CapitalizeFirst();

            request.State.Session.LastResult = "";
            request.State.Session.LastError = "";

            Task.Run(() =>
            {
                DateTime localStarted = DateTime.Now;
                using var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback 
                        = (message, certificate2, arg3, arg4) => true
                };
                using var client = new HttpClient(handler);
                var data = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "value1", textToSend },
                    { "value2", request.Request.Command.CapitalizeFirst() }, // full command
                    { "value3", webhook.Phrase.ToLower() }
                });

                try
                {
                    HttpResponseMessage httpResponse = client.PostAsync(webhook.Url, data).Result;
                    string body = httpResponse.Content.ReadAsStringAsync().Result;
                    request.State.Session.LastResult = body.IsNullOrEmpty() ? "Выполнено!" : $"{body}";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    request.State.Session.LastError = e.ToString();
                }
                finally
                {
                    TimeSpan total = DateTime.Now - localStarted;
                    Console.WriteLine($"Webhook \"{webhook.Phrase}\" {webhook.Url} " +
                                      $"was finished in {total.TotalMilliseconds}ms\n");
                }
            });

            DateTime started = DateTime.Now;
            while (true)
            {
                TimeSpan diff = DateTime.Now - started;
                if (
                    !request.State.Session.LastResult.IsNullOrEmpty()
                    || !request.State.Session.LastError.IsNullOrEmpty()
                    || diff > new TimeSpan(0, 0, 0, 1200)
                )
                {
                    break;
                }

                Thread.Sleep(10);
            }
            
            if(!request.State.Session.LastResult.IsNullOrEmpty())
            {
                return new Phrase(
                    request.State.Session.LastResult,
                    new []{ "Список", "Помощь", "Выход" }
                );
            }

            if(!request.State.Session.LastError.IsNullOrEmpty())
            {
                return new Phrase(
                    "С вызовом произошла ошибка.",
                    new []{ "Список", "Помощь", "Выход" }
                );
            }

            request.State.Session.Step = Step.AwaitWebhookResponse;
            
            // weebhook too long
            return new Phrase(
                "В[screen|е][voice|э]бх[+]ук отвечает очень долго. Попробовать узнать результат сейчас?",
                new[] {"Да", "Нет"}
            );
        }

        private Webhook GetWebhook(AliceRequest request)
        {
            string requestCommand = request.Request.Command.ToLower().Trim();
            return request.State.User.FindWebhook(requestCommand.Replace(" ", ""));
        }
    }
}