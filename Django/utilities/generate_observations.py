import requests, json, getpass, random
from scipy.sparse import rand

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
    token = response.json()["token"]

    users_file = open('usersdatabase.json', encoding="utf8")
    products_file = open('productsdatabase.json', encoding="utf8")
    users = json.load(users_file)
    products = json.load(products_file)

    x = rand(len(users), len(products), density=0.02, format='csr')
    x.data[:] = 1
    x = x.tocoo()
    l = [(r,c) for r,c in zip(x.row, x.col) if r>2] # elimina gli admin
    obs = [{
        "creator":users[uid]["email"],
        "product":int(pid),
        "threshold_price": str(2*float(products[pid]["price"])//3)
    } for (uid, pid) in l]
    del l

    for o in obs:
        print("Adding: " + json.dumps(o))
        django_headers = {
            'content-type': 'application/json',
            'authorization' : 'token {}'.format(token)
        }
        django_pload = o
        django_response = requests.post('http://localhost:' + port + '/create_observation', data=json.dumps(django_pload), headers=django_headers)
        if django_response.status_code == 201:
            print("Inserted")
        else:
            print("Insert failed")
    users_file.close()
    products_file.close()
    print("Observations insert is over")
else:
    print (response.json())