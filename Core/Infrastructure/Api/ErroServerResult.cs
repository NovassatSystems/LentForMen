using System;
using Newtonsoft.Json;

namespace Core
{
    public class ErroServerResult
    {      
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
