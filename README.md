# SlackLibrary

[![Build status](https://ci.appveyor.com/api/projects/status/m92929hjx6ab3jpl?svg=true)](https://ci.appveyor.com/project/Workshop2/slackconnector-glqir) [![Test status](http://teststatusbadge.azurewebsites.net/api/status/Workshop2/slackconnector-glqir)](https://ci.appveyor.com/project/Workshop2/slackconnector-glqir)  [![Nuget.org](https://img.shields.io/nuget/v/SlackConnector.svg?style=flat)](https://www.nuget.org/packages/SlackConnector)




SlackLibrary is a C# client to initiate and manage an open connection between you and the Slack servers. SlackLibrary uses web-sockets to allow real-time messages to be received and handled within your application.
You can also use the API methods with classic HTTP requests.

This library implement slack block !

## History
This library is a fork of SlackConnector but with better support for modern slack feature (like blocks) and a more complete API methods implementation.

## Installation
 
```
Install-Package SlackLibrary
```


## Usage

``` cs
ISlackConnector connector = new SlackConnector.SlackConnector();
ISlackConnection connection = await connector.Connect(botAccessToken);
connection.OnMessageReceived += MessageReceived;
connection.OnDisconnect += Disconnected;
```

##Features

 - Async by default
 - Easy setup
 - Real-time communication
 - Supports default proxies
 - Unit tested
 - Slack mock server
