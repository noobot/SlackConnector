# SlackConnector 

[![Build status](https://ci.appveyor.com/api/projects/status/m92929hjx6ab3jpl?svg=true)](https://ci.appveyor.com/project/Workshop2/slackconnector-glqir) [![Test status](http://teststatusbadge.azurewebsites.net/api/status/Workshop2/slackconnector-glqir)]
(https://ci.appveyor.com/project/Workshop2/slackconnector-glqir)  [![Nuget.org](https://img.shields.io/nuget/v/SlackConnector.svg?style=flat)](https://www.nuget.org/packages/SlackConnector)




SlackConnector is a C# client to initiate and manage an open connection between you and the Slack servers. SlackConnector uses web-sockets to allow real-time messages to be received and handled within your application.

## History
This library was originally extracted MargieBot and has iterated on it's own to become testable and progress without being coupled to any one implementation. This library has been built for [noobot](http://github.com/noobot/noobot), however it can easily be used in any project due to it's decoupling.


## Installation
 
```
Install-Package SlackConnector
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
