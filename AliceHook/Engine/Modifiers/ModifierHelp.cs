using System;
using System.Collections.Generic;
using System.Linq;
using AliceHook.Engine.Modifiers.Abstract;
using AliceHook.Models;
using AliceHook.Models.Abstract;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierHelp : ModifierBaseKeywords
    {
        protected override List<string> Keywords { get; } = new List<string>
        {
            "помощ",
            "помог",
            "что ты умеешь",
            "что делать",
        };

        protected override bool CheckState(SessionState state)
        {
            return true;
        }

        protected override AliceResponse CreateResponse(AliceRequest request)
        {
            AliceResponse response = base.CreateResponse(request);
            if (request.State.Session.Step == Step.None && request.HasScreen())
            {
                AddExamplesTo(response);
            }

            return response;
        }

        public static void AddExamplesTo(AliceResponse response)
        {
            response.Response.Buttons ??= new List<Button>();
            response.Response.Buttons.AddRange(
                Example.List().Select(
                    x => new Button
                    {
                        Title = x.Title,
                        Hide = false
                    }
                )
            );
        }

        protected override Phrase Respond(AliceRequest request)
        {
            return GetHelp(request.State.Session.Step, request.HasScreen());
        }

        public static Phrase GetHelp(Step step, bool hasScreen)
        {
            if (!hasScreen)
            {
                return new Phrase(
                    "В этом навыке вы можете добавлять URL-адрес[+]а, на которые я буду отправлять " +
                    "[screen|POST-запросы][voice|пост запросы]. Добавление новых в[screen|е][voice|э]бх[+]уков " +
                    "возможно только на устройстве с экраном, для этого вам нужно войти на таком " +
                    "устройстве под тем же аккаунтом, что и здесь, и добавить команды. " +
                    "После этого сможете называть колонке ключевые слова.\n\n" +
                    "Для запроса всех в[screen|е][voice|э]бх[+]уков скаж[+]ите \"Список\"."
                );
            }
            
            return step switch
            {
                Step.None => new Phrase(
                    "В этом навыке вы можете добавлять URL с ключевыми фразами. Сказали фразу — вызвался " + 
                    "[screen|POST-запрос][voice|пост-запрос] на этот адрес. Это позволит интегрироваться с сервисами " +
                    "автоматизации и интернета вещей, например [screen|IFTTT][voice|иф три тэ], " +
                    "[screen|Zapier][voice|з+эпиер] и [screen|Integromat][voice|интегром+ат].\n\n" +
                    "Сейчас вы можете добавить в[screen|е][voice|э]бх[+]ук или посмотреть список уже добавленных. " +
                    "Что хотите сделать?",
                    new []{ "Добавить вебхук", "Список вебхуков", "Выход" }
                ),
    
                Step.AwaitForUrl => new Phrase(
                    "Отправьте мне корректный адрес URL, и я его запомню. Можно отменить это действие.",
                    new []{ "Отмена", "Выход" }
                ),
                
                Step.AwaitForKeyword => new Phrase(
                    "Отправьте мне ключевую фразу, в дальнейшем я буду вызывать указанный URL, когда " +
                    "вы произнесёте эту фразу.",
                    new []{ "Отмена", "Выход" }
                ),
                
                _ => throw new ArgumentException("Unknown Step")
            };
        }
    }
}