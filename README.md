# Twitch Backend C# Take-Home Project

The purpose of this project is to evaluate your skill as a developer. It should take you about 4-6 hours to complete though you may take as long as you wish.

## Getting Started

To ensure consistency, specific tools and technologies must be used to complete this project.

### Data Contract Format

All HTTP and Websocket communication should be in JSON format.

### Hosting

The project is set up to launch as a console app running a web host on http://localhost:5000 which can be changed in ```Program.cs``` as desired.

### API Interface

The API interface must be created using ASP.NET Web API controllers.

### Websocket Interface

The project is already set up to use this via System.Net.Sockets and the built in AspNetCore WebSocketManager. 

Scaffolding code to use web sockets is included in the project. An instance of the ```Models/ClientSocket.cs``` class will be created for every web socket request. This class has ```OnCloseAsync```, ```OnConnectedAsync```, and ```HandleMessageAsync``` stubs for you to implement as needed. 
The ```SendMessageAsync``` function sends a string as a web socket frame to the connected client. 

The default buffer size for the Websocket code is ```1024``` and is a constant that can be changed in the ```ClientSocket``` class. The code as written assumes that complete data is received over the websocket and does not handle partial messages out of the box.

### Data Storage

Persistent data must be stored in Redis. This is available via Docker+Kitematic (https://www.docker.com). You are free to choose any client library for communicating with Redis.

## Spec

The goal of this project is to create a simple chat system with a NoSQL database for persistence and Websockets for real-time notifications triggered by data changes.

The basic model of the system is as follows:
- Clients can create users
  - Users are persisted between service restarts
  - Clients can connect via Websocket and identify as a specific users
- Clients can create and delete chat rooms
  - Chat rooms are persisted between service restarts
  - All connected web sockets representing a user with membership to a chat room should receive a message when the room is deleted
- Clients can add and remove members from chat rooms
  - Chat room membership is persisted between service restarts
  - All connected clients representing a user with membership to a chat room should receive a message that a member was added/removed
  - All connected clients representing the added/removed member should receive a message that they have been added/removed
- Clients can send messages to chat rooms
  - Users with no membership to a chat room should not be able to send a message to that room
  - All connected clients representing a user with membership to a chat room should receive chat messages for that room
  - Messages do not need to be persisted

### Code Organization
 
The majority of work is expected to be done in the typical Web API folders:
- Contracts - Classes representing the request and response objects serialized for API/Websocket interfaces
- Controllers - API Interface/Service Behavior
- Models - Data Access/Representation

Additional or different folders/subfolders can be created to better organize the code however you see fit. 

Feel free to make any changes to the provided code files as you desire.

## Bonus, Not Required
- Authentication
- Authorize only people with membership to a chat room to add/remove members
- Authorize only the creator of a chat room to delete the room
- Generate/Create documentation for the API/Websocket interfaces

