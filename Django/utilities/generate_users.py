import requests, json, getpass, random

prod_num = int(input('Enter the number of current products: '))
user_num = (2*prod_num)//3
email = input('Enter your admin email: ')
password = getpass.getpass('Enter your password: ')
port = input('Enter Django server port number: ')
login_headers = {'content-type': 'application/json'}
login_pload = {
    "username" : email,
    "password" : password
}
response = requests.post('http://localhost:'+port+'/login', data=json.dumps(login_pload), headers=login_headers)
if response.status_code == 200:
    print("Login: success")
    #token = response.json()["token"]
    users_file = open('users.json', encoding="utf8")
    ##################################################
    # Credits: https://github.com/dominictarr/random-name
    names = open('first-names.json', encoding="utf8")
    surnames = open('names.json', encoding="utf8")
    ##################################################
    users_dataset = json.load(users_file)
    users_dataset["names"] = json.load(names)
    users_dataset["surnames"] = json.load(surnames)

    users_set = set()
    users_list = list()
    while len(users_set)<user_num:
        _name = random.choice(users_dataset["names"])
        _surname = random.choice(users_dataset["surnames"])
        _host = random.choice(users_dataset["hosts"])
        mail = _name.lower()+"."+_surname.lower()+"@"+_host
        if mail not in users_set:
            users_set.add(mail)
            users_list.append({
                "email" : mail,
                "name" : _name,
                "surname" : _surname,
                "nickname" : _name+_surname,
                "password" : "password",
                "password_confirm" : "password"
            })
    del users_set

    for body in users_list:
        print("Adding: " + json.dumps(body))
        django_headers = {
            'content-type': 'application/json'
        }
        django_pload = body
        django_response = requests.post('http://localhost:' + port + '/register', data=json.dumps(django_pload), headers=django_headers)
        if django_response.status_code == 201:
            print("Inserted")
        else:
            print("Insert failed")

    users_file.close()
    names.close()
    surnames.close()
    print("Users insert is over")
else:
    print (response.json())

