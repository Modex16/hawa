# README #

This is a server API to find the less polluted route from one point to another in a particular city with a client application on windows universal app platform


* server : contains source code files for the server application
* client : contains source code for client side application on UWA
* Version : 1.0

## Installing and setting up server ##

* Install requirements first
    - Django >= 1.9.0
    - Pillow >=2.0.0
    - BeautifulSoup >= 4.4.0
    - MySQL-python >=1.2.3
    - requests >= 2.9.1
    - geopy >= 1.11.0

* Creating database for the application
    - Create a database in your server's mysql DBMS named "hawa"

* Deploying server in development mode
    - Run following command in the server directory:
        ```
            $ ./manage.py makemigrations  
        ```
        ```
            $ ./manage.py migrate
        ```
        ```
            $ sudo ./manage.py runserver (address):(port)  
        ```

Now your API server will be listening the HTTP requests at your mentioned address and port
