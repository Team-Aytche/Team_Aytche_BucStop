# Pong
### Team_Aytche_BucStop
#### CSCI 4350-001
#### Spring 2025, East Tennessee State University

### Overview:
This is a microservice that contains game information for Pong

### Project Structure:
* The application handles HTTP calls in the PongController.cs file in the /Pong/Controllers directory.
* It only handles an HTTP Get call to the path /Pong. So if the application was running locally, you would call [http://localhost/Pong](http://localhost/Pong).
* This application is deployed alongside the BucStop project with docker compose, see [BOBBY Project](https://github.com/chrisseals98/BOBBY) for more details.
