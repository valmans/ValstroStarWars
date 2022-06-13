# ValstroStarWars

The Star Wars API is a public REST API for making queries about the first 6 Star Wars films. 
The purpose of this assessment is to show that you can interact with a simple Websocket API(implementrued with Socket.io v4.

Requirements
- Produce a console/CLI application (the "Client App") that acts as a Socket.io client to the test server as described here
- The Client App must allow a user to search for arbitrary strings against the person/character search API
- The Client App works with the prepared `clonardo/socketio-backend` Docker image (representing the server)
- When search results are received, the names of the character that was matched ("name" field)+ their filmography ("films" field) should be printed to the console
- When any errors are received, they should be logged to the console
- The console should reset on completion of a search (on receipt of the last message in case of success, or on any error) to allow the user to make another search without restarting the application

Stack used:
- c#
- .net 6.0

Enviroment:
- Linux 
- VS Code
- Docker Deslktop (run Socket.IO Server)

Libraries:
  - SocketIO Client: https://github.com/doghappy/socket.io-client-csharp

Comments:
- Used main Socket.IO functions:
  - onConnected, .on, .onError, Dispose, onDisconnected
- I used a Queue to store the server response async.
- Then out of the listener I check if the transmition is ended and process the data in the Queue.
- If exists an error show it.

Improvements to do:
- Reconnection flow in case the server connection lost. 
- Process to validate data if connection interrupted

Compile and Run
- start server: Run the following command at a terminal: `docker run -p 3000:3000 clonardo/socketio-backend`
- Requirements .net runtime 6.0
- Command /valstroStarWars$ dotnet run [enter]

Video Demo:
ValstroStarWarsDemo.webm

Author: Manuel Valdes
