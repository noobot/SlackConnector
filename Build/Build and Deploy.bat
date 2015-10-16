nuget pack ..\src\SlackConnector\SlackConnector.csproj -Prop Configuration=Release
nuget push SlackConnector.1.0.*.nupkg
move SlackConnector.1.0.*.nupkg Archive