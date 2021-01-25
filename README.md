# Battleship
 Battleship game

This is a sample ASP.NET Core Web App deployed to Azure.

Swagger API documentation - https://battleshipgamewebapp.azurewebsites.net/swagger/index.html
(Please note the above URL without "/swagger/index.html" will give a 404 error as there is no default page set up. Please ignore this)

Sample GET call to test the API via command line -
	curl -i https://battleshipgamewebapp.azurewebsites.net/api/game/board

Dependencies -
	.Net Core 3.1
	Swashbuckle.AspNetCore (5.6.3) - This is to help provide the swagger page for API documentation