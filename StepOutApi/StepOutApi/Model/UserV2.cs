using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StepOutApi.Model
{
    class UserV2
    {
        public String Gebruik { get { return "Login"; }} 
        public String Naam { get; set; }
        public String Wachtwoord { get; set; }
        public String Email { get; set; }
        public String Land { get; set; }
        public TokenModel Token  { get; set; }


    }

    public class TokenModel
    {
        [JsonProperty(propertyName: "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(propertyName: "token_type")]
        public string TokenType { get; set; }

        [JsonProperty(propertyName: "x_user_id")]
        public int UserId { get; set; }

        public override string ToString()
        {
            return char.ToUpper(TokenType[0]) + TokenType.Substring(1) + " " + AccessToken;
        }
    }
}
