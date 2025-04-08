# Team_Aytche_BucStop

### CSCI 4350-001
### Spring 2025, East Tennessee State University

### Overview:
This project is a game website made by and for ETSU students. It
is a place to put games created by ETSU students.
This project also communicates to a microservice with HTTP calls for the game information, the repository is hosted in the links below. BucStop also interacts with this Microservice through an API Gateway whose repository is also listed below. It is deployed with the microservice using docker compose, see the scripts folder for the docker-compose.yml file.

### Backlog Information:
[Backlog](https://aytche.atlassian.net/jira/software/projects/SCRUM/summary)

[Running Locally](https://github.com/Team-Aytche/Team_Aytche_BucStop/blob/justin-dev/BucStop_Webapp/Docs/Running%20Locally.pdf)

[Deploying] (https://github.com/Team-Aytche/Team_Aytche_BucStop/blob/justin-dev/BucStop_Webapp/Docs/Akamai%20Deployment%20Doc.pdf)

[Microservices Document](https://github.com/Team-Aytche/Team_Aytche_BucStop/blob/justin-dev/BucStop_Webapp/Docs/Microservices.pdf)

### Project Structure: 
To understand the project structure, familiarize yourself with the
MVC (Model View Controller) structure. When clicking on a game, 
a value will be passed to the controller, which will decide which 
game to load. This is divided between the MVC folders in the main
BucStop folder.

* Bucstop
	* Controllers
		* This folder has the controllers, which allow pages to 
			link together and pass information between them.
	* Models
		* This folder has the basis for the Game class.
	* Views
		* Games
			* This folder has the pages related to games, such as
				the index page and the default game page.
		* Home
			* This folder contains the main pages used by the site, 				
				such as the home page, admin page, and privacy page.
		* Shared 
			* This contains other important pages and/or resources 
				that aren't in the other two folders, including the
				default layout and the error page.
	* wwwroot
		* This folder contains the resources used by the project, 
			including images, the javascript games, the icons, etc.
