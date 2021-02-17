using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinFrontEnd.Classi
{
    public class Registration
    {


        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("password_confirm")]
        public string Password_confirm { get; set; }

        public Registration(string email, string name, string surname, string nickname, string password, string password_confirm)
        {
            Email = email;
            Name = name;
            Surname = surname;
            Nickname = nickname;
            Password = password;
            Password_confirm = password_confirm;
        }

    }
}
