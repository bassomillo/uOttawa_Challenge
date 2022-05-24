# uOttawa_Challenge
### The deployment link is https://uottawachallenge.ruilincai.repl.co/swagger/index.html
### If you want to get connect and see my MongoDB Atlas database, you can download MongoDB Compass and connect my database by using this connection string: mongodb+srv://rcai006:ShopifyChallenge@shopifychallenge.ucrmo.mongodb.net/test
-Instructions on how to connect MongoDB Atlas with MongoDB Compass can be found here: https://www.mongodb.com/docs/atlas/compass-connection/
-It is a ASP.NET REST API with MongoDB as the database.
-It has 3 APIs which are getKey, guess and checkHistory. As for database, it has two collections which are word and history. 
-When the user use getKey API, the API will generate a word with 5 letters randomly and it will also use the MD5 encrypt method to generate a unique key for the user. 
-When the user use guess API, the API will store the guess word into the history database and return the result to the user.
-When the user use checkHistory API, the user need to input the key and the API will return all the records.
