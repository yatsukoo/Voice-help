using System;
using System.IO;
using System.Threading.Tasks;
using AliceHook.Engine;
using AliceHook.Models;
using AliceHook.Models.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AliceHook.Controllers
{
    [ApiController]
    [Route("/")]
    public class AliceController : ControllerBase
    {
        private readonly AliceService _aliceService;

        private static readonly JsonSerializerSettings ConverterSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            NullValueHandling = NullValueHandling.Ignore
        };

        public AliceController(AliceService aliceService)
        {
            _aliceService = aliceService;
        }
        
        [HttpGet]
        public string Get()
        {
            return "It works!";
        }

        [HttpPost]
        public Task Post()
        {
            using var reader = new StreamReader(Request.Body);
            string body = reader.ReadToEnd();

            var aliceRequest = JsonConvert.DeserializeObject<AliceRequest>(body, ConverterSettings);
            if (aliceRequest?.IsPing() == true)
            {
                AliceResponseBase<UserState, SessionState> pongResponse = new AliceResponse(aliceRequest).ToPong();
                string stringPong = JsonConvert.SerializeObject(pongResponse, ConverterSettings);
                return Response.WriteAsync(stringPong);
            }
            
            string userId = aliceRequest.Session.UserId;

            Console.WriteLine($"REQUEST FROM {userId}:\n{body}\n");

            AliceResponse aliceResponse = _aliceService.HandleRequest(aliceRequest);
            string stringResponse = JsonConvert.SerializeObject(aliceResponse, ConverterSettings);

            Console.WriteLine($"RESPONSE:\n{stringResponse}\n");
            
            return Response.WriteAsync(stringResponse);
        }
    }
}