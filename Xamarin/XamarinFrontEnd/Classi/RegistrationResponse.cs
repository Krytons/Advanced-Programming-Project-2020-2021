using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinFrontEnd.Classi
{
    public class RegistrationResponse
    {

        public string response { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string nickname { get; set; }
        public string token { get; set; }

        public RegistrationResponse(string response, string email, string name, string surname, string nickname, string token)
        {
            this.response = response;
            this.email = email;
            this.name = name;
            this.surname = surname;
            this.nickname = nickname;
            this.token = token;
        }
    }
}
