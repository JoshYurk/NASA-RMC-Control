#Debug Stuff
import ComControl

serv = ComControl.ComControl(4256)

def Main():
    serv.onReceivedEvent(thing)
    while True:
        pass

def thing(data):
    try:
        print("Received: " + data.toString())
        serv.send(data.toString(filter=ComControl.ComData.RobotAuth))
    except Exception as e:
        print(e)

Main()
