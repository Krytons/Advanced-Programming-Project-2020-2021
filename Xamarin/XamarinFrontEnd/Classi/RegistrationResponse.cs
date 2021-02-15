using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinFrontEnd.Classi
{
    public class RegistrationResponse
    {

        public string Response { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }
        public string Token { get; set; }

        public RegistrationResponse(string response, string email, string name, string surname, string nickname, string token)
        {
            Response = response;
            Email = email;
            Name = name;
            Surname = surname;
            Nickname = nickname;
            Token = token;
        }
    }
}
