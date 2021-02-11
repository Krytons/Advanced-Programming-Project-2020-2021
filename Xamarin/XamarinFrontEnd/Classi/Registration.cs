using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinFrontEnd.Classi
{
    public class Registration
    {

        public string email { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string nickname { get; set; }
        public string password { get; set; }
        public string password_confirm { get; set; }

        public Registration(string email, string name, string surname, string nickname, string password, string password_confirm)
        {
            this.email = email;
            this.name = name;
            this.surname = surname;
            this.nickname = nickname;
            this.password = password;
            this.password_confirm = password_confirm;
        }

    }
}
