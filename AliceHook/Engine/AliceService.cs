using System;
using System.Collections.Generic;
using System.Linq;
using AliceHook.Engine.Modifiers;
using AliceHook.Engine.Modifiers.Abstract;
using AliceHook.Models;
using AliceHook.Models.Abstract;

namespace AliceHook.Engine
{
    public class AliceService
    {
        private static readonly List<ModifierBase> Modifiers = new List<ModifierBase>
        {
            new ModifierEnter(),
            new ModifierTestRequest(),
            new ModifierExample(),
            new ModifierExampleList(),
            new ModifierWebhookResponse(),
            new ModifierHelp(),
            new ModifierExit(),
            new ModifierList(),
            new ModifierCancel(),
            new ModifierDelete(),
            new ModifierAddWebhook(),
            new ModifierAddPhrase(),
            new ModifierFinalWebhook(),
            new ModifierRunWebhook(),
            new ModifierUnknown()
        };
        
        public AliceResponse HandleRequest(AliceRequest aliceRequest)
        {
            // respond
            AliceResponse response = null;
            try
            {
                if (!Modifiers.Any(modifier => modifier.Run(aliceRequest, out response))) {
                    throw new NotSupportedException("No default modifier");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ERROR");
                Console.WriteLine(e);
                response = new AliceResponse(aliceRequest)
                {
                    Response = new Response
                    {
                        Text = "Произошла какая-то ошибка на сервере навыка, разработчик уже уведомлён. " +
                               "Приносим извинения."
                    }
                };
                Console.WriteLine("");
            }

            return response;
        }
    }
}