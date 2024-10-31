using System;
using System.Collections.Generic;
using System.Linq;
using AliceHook.Engine.Modifiers.Abstract;
using AliceHook.Models;
using AliceHook.Models.Abstract;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierExample : ModifierBase
    {
        protected override bool Check(AliceRequest request)
        {
            if (request.State.Session.Step != Step.None)
            {
                return false;
            }
            
            Example example = FindExample(request.Request.Command);
            return example != null;
        }

        protected override Phrase Respond(AliceRequest request)
        {
            throw new NotSupportedException("Method should not be invoked");
        }

        protected override AliceResponse CreateResponse(AliceRequest request)
        {
            var response = new AliceResponse(request);
            Example example = FindExample(request.Request.Command);
            request.State.Session.Step = Step.None;

            response.Response.Text = example.Description + " По кнопке подробная видеоинструкция о том, как это сделать.";
            response.Response.Tts = example.DescriptionTts + " - По кнопке подробная видеоинструкция о том, как это сделать.";
            response.Response.Buttons = new List<Button>
            {
                new Button
                {
                    Title = "Открыть видео",
                    Hide = false,
                    Url = example.Link
                }
            };
            
            var suggests = new [] {"Добавить вебхук", "Примеры", "Список", "Выход"};
            response.Response.Buttons.AddRange(suggests.Select(x => new Button{Title = x}));
            
            return response;
        }

        private Example FindExample(string input)
        {
            return Example.List().FirstOrDefault(example => example.Check(input));
        } 
    }
}