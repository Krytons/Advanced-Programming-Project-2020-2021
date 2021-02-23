# Xamarin Frontend

<p align="center" style="font-size: 24px">
  <span> English </span> |
  <a href="#">Italiano</a>
</p>

## Advanced Programming Languages 2020/2021
### Developed by:
- **Bartolomeo Caruso**
- **Giuseppe Fallica**
- **Gabriele Costanzo**

---

## 1. What we used
In order to develop this frontend we have used the following elements:
- **Xamarin.Forms:** is an open source cross-platform framework from Microsoft for building iOS, Android, & Windows apps with .NET from a single shared codebase.
- **Xamarin.Essentials:** A kit of essential API's for Xamarin.Forms.
- **Newtonsoft.Json:** is a popular high-performance JSON framework for .NET.
- **OxyPlot.Xamarin.Forms:** is a plotting library for .NET. This package includes a portable library for Xamarin.Forms apps and platform-specific libraries for IOS and Android.
- **System.Net.Http:** provides a programming interface for modern HTTP applications, including HTTP client components that allow applications to consume web services over HTTP and HTTP componenets that can be used by both clients and servers for parsing HTTP headers.
- **NETStandard.Library:** a set of standard .NET API's that are prescribed to be used and supported together.
- **Microsoft.Net.Http:Headers:** HTTP header parser implementations.

---

## 2. Frontend setup
Our application uses ngrok to allow communication between our backend and the frontend, it constitutes a service that allows to create a local tunnel between a local port and a public URLs and inspect traffic. For this reason it is necessary to modify the secrets.json file in the Configuration folder of our project by setting the URL provided by ngrok as in the following example.  
- **Configuration/secrets.json:**
  ```JSON
    {
      "ngrok": "http://65160599d543.ngrok.io"
    }
  ```

---

## 3. Pages

Xamarin generates two files when creating a new page:
File .xaml: the actual aesthetics of the page are implemented in it.
File .xaml.cs: all the operating logic behind the page it is associated with is implemented in it.

- **Login Page:** This page allows a registered user to request access to the services provided by the application by correctly providing the email and password chosen at registration time.

  ![Controller](./Images/Login/Login.jpg)

  At the bottom of this page there are two buttons, as shown in the previous image:
  - `Login`: used to confirm the data inserted inside the form, and attempt login.
  - `Sign in`: used to navigate to registration page.

  If login procedure is successful, a login token is returned and saved inside a secure storage.

- **Registration Page:** This page allows a user to sign in our application by providing informations such as email and password.

  ![Controller](./Images/Login/Registration.jpg)

  At the bottom of this page there are two buttons, as shown in the previous image:
    - `Sign in`: used to confirm the data inserted inside the form, and attempt sign in.
    - `Login`: used to navigate to login page.

  If registration procedure is successful, a login token is returned and saved inside a secure storage.

- **Observations Page:** This page is used to show every product observed by the logged user.
    Every observed product is inserted inside the page as a grid element as shown below:

  ![Controller](./Images/Observations/Page.png)

  Thanks to this page our app user is able to delete or modify it's own observations pressing the modify button.
  When the button is pressed, a display prompt appears, as shown below:

  ![Controller](./Images/Observations/Buttonpressed.png)

  The display prompt permits two actions:

  - `Update Observation`: to change the observation price threshold. If pressed an input box is showed to the user.

  - `Delete Observation`: to delete the observation

  Price threshold update input box is showed down below: 

  ![Controller](./Images/Observations/Pricechange.png)


- **Recommendation page:** This page is used to show recommended product for the user, that are gathered from our recommendation system.
Every recommended product is inserted inside the page as a grid element as shown below:

  ![Controller](./Images/Recommended/Page.jpg)

  Every recommended product is associated with an info button, which if pressed navigates the user to a proper product info page.


- **Notification page:** This page is used to show every notification sent to the user, even the old ones.

  ![Controller](./Images/Notifications/Page.jpg)

  Thanks to this page the user is able to read all the received notifications, and he is able to delete the old ones.


- **Search page:** This page is used to search a product using ebay API. When the search input box is filled with a keyword, the space down below the box is filled with the products that matched the query.  

  ![Controller](./Images/Search/Page.jpg)

  Each received product is associated with an info button, which if pressed navigates the user to a proper product info page.

  ![Controller](./Images/Search/Filledpage.jpg)


- **Product info page:** This page is used to show all the useful details of a product and to show a price history graph to the user.

  ![Controller](./Images/Info/Page.jpg)

  At the bottom of this page there is a button, which if pressed shows to the user a desired price input box.

  ![Controller](./Images/Info/Input.jpg)

  If a valid price is inserted inside the input box, and if the current user hasn't already observed the product, a new observation will be created.
  

---

## 4. Http Requests
Our frontend communicates with our Django backend using four kind of http requests:

- `Products requests`: this requests are used to retrieve products info.
  There are four different products requests:
    - `GetProductById`: this request is used to obtain product info by using product id value.
    - `GetProducts`: this request is used to obtain products using a search keyword.
    - `GetProductsByRecommendations`: this request is used to obtain product info of a specific recommendation.
    - `GetProductPriceHistory`: this request is used to get the price history of a specific product, using an ebay product id.

- `Auth requests`: this requests are used to retrieve an auth token for the current user.
  There are two different auth requests:
    - `TryLogin`: this request is used to attempt login, using user's login data as body.
    - `SignIn`: this request is used to attempt sign in, using user's sign in data as body.

- `Observation requests`: this requests are used to handle observations info.
  There are five different observation requests:
    - `Insert Observation`: this request is used to add an observation for the current user.
    - `GetObservationById`: this request is used to obtain all observation info using it's id.
    - `GetAllUserObservation`: this request is used to obtain all observations of the current user.
    - `DeleteObservation`: this request is used to delete the current user's observation.
    - `UpdateObservation`: this request is used to update the threshold price for the current user's observation.

- `Notification requests`: this requests are used to handle user's notifications.
  There three different notification requests:
    - `GetNotPulledNotifications`: this request is used to obtain all the not pulled notifications for the current user. 
    - `GetAllNotifications`: this request is used to obtain all notifications for the current user. 
    - `DeleteNotification`: this request is used to delete an already pulled notification for the current user.


---

## 5. Notifications


---
