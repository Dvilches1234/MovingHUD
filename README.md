# MovingHUD
This Unity package was made to move elements from the HUD to a web browser in a smartphone. This is achived by a creating an HTTP server to host a website and a WebSocket Server and a Websocket Client to send data in real time to the website. Both server are made to be hosted in the local network.

## Installing instruction.

### Necessarie installations

1. First you need to install the NuGetForUnity package, this is will be used to install some packages for C#. This can be found here:  https://github.com/GlitchEnzo/NuGetForUnity.
2. With the NuGet manager, install the following packages:
   - Newtonsft.Json: this will be used to send the data in JSON format.
   - WebSocketSharp-NonPreRelease: to create the WebSocket Server and Client. It also can be found here: https://github.com/sta/websocket-sharp
   - QRCoder and QRCoder.Unity: This will allow us to create a QR code that can be scan for the player to open the HUD in their phone. It also can be found here: https://github.com/codebude/QRCoder.Unity/

### Installation

You cand download the unity package from here.
Or clone the repository.
```
git clone https://github.com/Dvilches1234/MovingHUD.0.0.1.git
```
## How to use it

First you need to add the object called Servers in the prefab folder to the scene where you are going to make available the option to move the HUD. This object have all the components necessaries to manage the process.

![image](https://user-images.githubusercontent.com/14845457/137754106-752066e9-58af-4356-a3a2-864ca37c0a69.png)

In the HTTP Server Controller component you can define the port you are going to use and the location of the files needed by the website. This folder has to be in the Asset folder directly. In this case the files are in the /root/web with root being in the asset folder. 
Then in the WS Server Controller component you can edit the port used by the WebSocket Server and also modify the path that is going to use. I recommend to leave it like "HUD". Here you need to add the object that which is going to hold the QR code. This object needs to have an Image component.  

In your code you need to add this simple lines of code to your project in the Update methods of the scripts where the elements of UI are updated. 
```c#
 if (ServersController.ServersOn)
            WSClientController.AddValueToDictionary("Points", points.ToString());
```
In this case, we have a variable called points and if the servers are running, it add the value to a dictionary. This dictionary is constantly checked for changes and if a value is updated it is send from the client to the server in a JSON.  

Then you can call the StarServers method in the ServersController component from a button or from wherever you prefer to start running both the servers and the client.


## Example

In this link https://github.com/Dvilches1234/root is an example a web page created for the HUD of a game called Artifact that you also can find here: https://github.com/Svartskogen/Artifact
