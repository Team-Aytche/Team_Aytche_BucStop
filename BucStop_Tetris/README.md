# Tetris
### Team_Aytche_BucStop
#### CSCI 4350
#### Spring 2025, East Tennessee State University

### Overview:
This is a microservice that returns Tetris game info to Bucstop

### Project Structure:
* The application handles HTTP calls in the Tetris.cs file in the /Tetris/Controllers directory.
* It only handles an HTTP Get call to the path /Tetris. So if the application was running locally, you would call [http://localhost/Tetris](http://localhost/Tetris).
* This application is deployed alongside the BucStop project with docker compose, see [BOBBY Project](https://github.com/chrisseals98/BOBBY) for more details.
