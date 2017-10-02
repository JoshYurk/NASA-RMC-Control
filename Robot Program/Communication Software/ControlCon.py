import socket
import asyncio
import ComData

class Server:
    """
    This establishes and maintains a connection to the controler
    """
    _recDeles = []

    def __init__(self, port):
        self._serv = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self._serv.setblocking(1)
        self._serv.bind(('',port))
        self._serv.listen(1)
        self._loop = asyncio.get_event_loop()
        self._loop.run_in_executor(None, self._accept)

    def send(self, data):
        """
        Sends data to the controler
        :param data: String from ComData
        """
        self._loop.run_in_executor(None, self._send, str.encode(data))

    def onReceivedEvent(self, delegate):
        """
        Event that is called when data is received
        :param delegate: The method(s) to be called when data is received, format is foo(ComData) 
        """
        self._recDeles.append(delegate)

    def _accept(self):
        self._conn, address = self._serv.accept()
        print("Connection established")
        self._loop.run_in_executor(None, self._receive)
    
    def _receive(self):
        try:
            data = self._conn.recv(1024)
            self._onReceived(data)
            self._loop.run_in_executor(None, self._receive)
        except socket.error as e:
            print("Connection lost")
            self._loop.run_in_executor(None, self._accept)
    
    def _send(self, data):
        try:
            self._conn.sendall(data)
        except socket.error as e:
            print("Connection lost")
            self._loop.run_in_executor(None, self._accept)

    def _onReceived(self, data):
        data = ComData.ComData(data.decode("ascii"))
        for dele in self._recDeles:
            dele(data)



#Debug Stuff
"""
def thing(data):
    try:
        print("Received: " + data.toString())
    except Exception as e:
        print(e)

serv = Server(4256)
serv.onReceivedEvent(thing)
while True:
    pass
"""