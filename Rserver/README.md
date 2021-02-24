# R : Recommendation System Backend

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
In order to develop the Recommendation System backup, we used the following CRAN packages:
- **here** - A simple package for path traversing, starting from the CWD, useful for calling Rscript calls automation
- **rjson** - Implements *fromJSON* and *toJSON* functions to translate lists and dataframes in JSON format, and vice versa
- **httr** - General-purpose HTTP library with built-in JSON parsing and headers support, used for the WebClient
- **recommenderlab** - Suite of tools for building recommendation systems, supporting different formats and methods
- **cronR** - For scheduling tasks in UNIX environments
- **taskscheduleR** - For scheduling tasks in Windows environments

---

## 2. Server startup
The Rserver supposes a Django backend already up and running in the background, at startup.
It works by executing a `main.R` script that generates the job schedule on the user's OS.
The scheduled task already includes a method to empty the job schedule if the Django web server is closed.

  ### 0. If on a UNIX system (Ubuntu, macOS), make sure that the `cron` process is running:
  - **Start the cron process:**
    ```bash
      $ sudo cron
    ```
    If on **macOS**, it may be necessary to add the **Terminal** app and/or the **cron** binary to the apps that can control the computer under *"Security & Privacy"* 
    
    ![Where to setup the cron/terminal permissions](./images/cron-macos.png)
    - **cron location:**
    ```bash
      $ which cron
      /usr/sbin/cron
    ```
    - **Terminal location:**
    ```bash
      /Applications/Utilities/Terminal.app
    ```

  ### 1. Start the Django backend:
  -  **Start Django (view Django's README):**
    ```bash
      $ cd Django
      $ python3 manage.py runserver 0.0.0.0:8000 --noreload
    ```

  ### 2. Initialize the Rserver:
  -  **Start main.R:**
    ```bash
      $ cd Rserver
      $ sudo Rscript main.R $PWD
    ```

  ### 3. (Optional) Manually stop the R server:
  -  **Start terminate.R:**
    ```bash
      $ cd Rserver
      $ sudo Rscript terminate.R $PWD
    ```

  For steps 2. and 3. on Windows it is advisable to use `Rscript`:
  - **Use Rscript:**
    ```
    cd Rserver
    Rscript main.R
    Rscript terminate.R
    ```
If running the whole project with the script included in the root folder, `startR.sh/cmd` is used on a separate shell/command prompt to start the Recommendation System separately, and `stop.sh/cmd` to stop it, along with the other processes.

---

## 3. Classes

The OOP paradigm was followed to separate concerns in the project, by identifying 4 specific classes, one of which serves as a base for two separate implemented classes, for Unix and Windows systems.

Each class was declared in a separate file by using R's `S4` Classes' structure, defining its `prototype`, a `constructor` and various `methods`.

- **RecSys:** 
  
  This class represents the `Recommendation System` in its main components:
    - **m**: a starting `data.frame`, obtained from the Django backend to initialize the whole system
    - **model**: an object produced from `"recommenderlab"` and the **m** data.frame, that implements the methods to calculate the recommendations, given the setup parameters that define the tipology of recommendation system
      - this field is updated at every update of **m**, that is - every time new `observations` are received
    - **recs**: the object containing all recommendations, produced by the **model**
    - **seq_num**: the `SequenceNumber` of the first data.frame received, for setup and logging purposes

  It uses the following methods:
    - **train:** re-generates *model* and *recs* based on the updated *m* data.frame
    - **removeEmptyEntries:** utility to remove empty rows and columns from the data.frame
    - **updateRS:** takes in input a list of new observations and updates the *m* data.frame

- **RecDB:**
  
  This class is a database of `Recommendations`, and is used to calculate the difference between previous and new lists of recommendations, in order to send just and only the updates to the Django server.

  It contains the following fields:
    - **currList:** represents the current list of recommendations, obtained from `RecSys`' **recs** field
    - **seqNum:** contains the last received `SequenceNumber` and gets incremented when a new set of observations is correctly received
    - **reqs:** a list of recommendation updates to send, already formatted in JSON and indexed by the `seqNum` to be used for the request
      - guarantees *synchronization* with the Django backend's responses with new observations

  It uses the following methods:
    - **updateRDB:** takes a new list of all recommendations to take the place of `currList`, calculate the difference and prepare the new entry in `reqs`
    - **increaseCounter:** simply increments the `seqNum`, taking into consideration the max value reachable
    - **getNewRecs:** simply returns the next request to be sent

- **WebClient:**

  This class represents a basic representation of a web-client, written using the `httr` package, to connect to the Django backend, send HTTP requests, receive and manipulate responses, and also catch exceptions by checking the response's HTTP status.

  It's basic workflow is that of - using provided credentials of an admin user - `login` into Django, obtain an `user Token`, and act as a middle-man to `get new observations` and `send new recommendations`.

  In order to do so, it uses the following fields:
    - **url:** represents the base url of Django
      - defaults to `localhost`
    - **credentials:** a character array of `username` and `password`, defaults to a couple used by an admin
    - **endpoints:** a list of the endpoints to be used by the WebClient
      - defaults to:
        ```
        "login" = "login",
        "init" = "communication/send_all_observations",
        "send_rec" = "communication/add_recommendation",
        "retrieve_obs" = "communication/send_all_new_observations"
        ```
    - **token:** the `Token` of an authenticated admin, to be used for all requests
    - **attempts:** a counter, starting from 0, that counts the number of `failed attempts` to connect to the Django backend
      - after 10 attempts it closes itself and removes the remaining cron jobs

  The methods represent:

    - Four routes to the Django backend:

      - **login**
      - **getDataframe** corresponding to **"communication/send_all_observations"**
      - **getObs** corresponding to **"communication/send_all_new_observations"**
      - **sendRecs** corresponding to **"communication/add_recommendation"**

    - Two utils function to `increment` and `reset` the attempts to connect

- **TaskScheduler:**

  **TaskScheduler** as two principal uses:
  - as a container for the `RecSys`, `RecDB` and `WebClient` objects, which represent the class' field
  - as a manager of `scheduled jobs`, by `creating` and `destroying` them

  It uses two separate libraries depending on the OS environment and has two separate derived classes:
  - **UnixTaskScheduler**:

    To be used on Unix systems (i.e. macOS, Ubuntu), with the `cronR` package.
    It's based on the `cron` process and can manage jobs as a single entry, recognized by its ID `recsys_cron`, set to be scheduled i.e. every minute.
    The job is set to be the execution of a `task.R` file, and the execution of `terminate.R` removes it.

  - **WindowsTaskScheduler**:

    Very similar, but uses the package `taskscheduleR`, which has the particular limit of having the job's command be defined by a string of **maximum 261 characters**.
      - **If adding the jobs fails when executing "main.R", a workaround is to change the name of the root folder of the project to something shorter, like "apl2021"**

  Two more fields are defined for future use:
  - **seconds** to define a custom number of seconds for the job scheduling
    - in `cronR`, it defaults to a minute, while in `taskscheduleR` the amount can be changed with a modifer
  - **loop** to signal whether the next job can be executed or if the cron jobs need to be deleted
    - currently, there is no control that requires this mechanism


---

## 4. How the Recommendation System works

The `recommenderlab` package supports different tipologies of Recommendation Systems, so we implemented the most simple type, that can be upgraded at any time by changing just the `model` implementation.

- **Collaborative Filtering:**

  The data.frame **m** is represented by a binary matrix (1/0, True/False) where:
  - the rows are represented by `users` and use their `user_id` as name
  - the cols are represented by `products` and use their `product_id` as name
  - each cell is put to `1` if the given user has an observation for the given product

  So, viewing each row as a `vector`, a `distance` measure between two users can be calculated, i.e.
  - `Jaccard distance` calculated as the number of exclusive items between the two users
  - `cosine distance` calculated as a cosine, where the angle is the one between the two users seen as vectors

  The **Collaborative Filtering** uses this matrix as a `training set`, and then recommends items to users coming from a `test set` of similar shape following this patterns:
  ```
  for each user A of test set:
    while needs more recommendations:
      calculate the least distant user B of training set
      insert B's exclusive products into the A's recommendations
      delete B from the candidates
  ```

For a simple matter of having a minimal example working, it was chosen to make the `training` and `test` set match, which introduces some form of bias into the recommended items.

Although R makes implementing this kind of calculations very simple, it has been deemed best to use an actual package that makes it possible to upgrade the model at any time and in a scalable way, i.e. when there is a dataset large enough to separate training and test sets.

---

## 5. Data Structuring
One of the main reasons to choose R for the job is the ease of structuring and re-structuring data with its built-in types and methods.

Some operations were implemented to provide an interface between the incoming data structure from the backend and the expected structure format for the R packages.

- **Init: all observations:**
    
  When initializing the RecSys data.frame **m** we expect the following kind of format from Django:
  ```JSON
  {
    "index":["1", "2", "3"],
    "columns":["1", "2", "3"],
    "data":[
      [1, 0, 0],
      [1, 1, 0],
      [0, 1, 1]
    ]
  }
  ```
  The reason for this specific kind of structure is that it is produced with a built-in method in `pandas`, by parsing a Dataframe.

  The content of `"data"` can be automatically included in a R `data.frame`, while the `index` and `columns` array can be assigned to the data.frame by using `row.names` and `colnames`.

- **Get new observations:**

  The route to ask for new observations returns each object as a JSON representing the original Django model, and as such it is not optimal to process the response directly.

  Instead, just three fields are of interest, and can be re-structured as such:
  ```
  c(as.logical(x$operation), toString(x$id), toString(x$user_id))
  ```
  Using this expression as the return value of a function used in `lapply`, we can map each NewObservation object to an array of this kind:
  ```JSON
  [ true, "product_id", "user_id"]
  ```
  With this simplified form, it's easy to compare the new observations with the existing **m** data.frame to add or remove entries, by also creating new rows and columns if necessary.

- **Send new recommendations**

  Django makes use of Python's built-in `set` data structures to make set operations on the recommendation elements, such as making set differences to know the old recommendations that need to be deleted, the new ones that need to be added, and discarding the set intersection that don't need to be updated.

  A similar operation is needed in RecDB, by comparing previous and new list of recommendations to get the differences that need to be communicated to Django: new, removed, and updated recs.

  ```
  removedList <- Map(function(x){return(character(0))}, oldList[!(oldList %in% filtList)])
  newList <- filtList[!(filtList %in% oldList)]
  changedList <- filtList[(filtList %in% oldList)]
  changedList <- changedList[unlist(sapply(names(changedList), function(x) !identical(changedList[[x]], oldList[[x]])))]
  ```

  While the same simplicity can't be achieved, R provides powerful methods, such as the `Map` and `Filter` functions, the `-apply` family, and the subsetting via the `%in%` operator.
