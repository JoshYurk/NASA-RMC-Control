import socket
import asyncio
from enum import IntEnum, unique

@unique
class dataId(IntEnum):
    """
    Modify this to match the one on the controler
    """
    Moter1 = 0
    Moter2 = 1
    Moter3 = 2
    Moter4 = 3
    Error1 = 4
    Sensor = 5

class Filter:
        """
        A bitSet based filter for use in the toString function
        """
        _length = len(dataId)
        _bitSet = [False] * _length

        def __init__(self, points=None):
            if points != None:
                for point in points:
                    self._bitSet[point] = True
        
        def __getitem__(self, key):
            if(key < self._length and key >= 0):
                return self._bitSet[key]

        def __setitem__(self, key, value):
            if(key < self._length and key >= 0):
                self._bitSet[key] = value

class ComControl:
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
        data = ComData(data.decode("ascii"))
        for dele in self._recDeles:
            dele(data)

class ComData:
    """
    This stores and formats the data used to communicate with the controler
    """
    #Example filter
    RobotAuth = Filter([dataId.Sensor])

    _dataLength = len(dataId)

    def __init__(self, data=None):
        """
        :param data: String received from the toString function or from the controler
        """
        self._dataArray = [0] * self._dataLength
        if data != None:
            for item in data.split('|', len(data)):
                    pair = item.split(':', 2)
                    if len(pair) == 2:
                        print("split")
                        try:
                            if int(pair[0]) < self._dataLength:
                                print("assign")
                                self._dataArray[int(pair[0])] = int(pair[1])
                        except ValueError as e:
                            print(e)

    def __getitem__(self, key):
            if(key < self._dataLength and key >= 0):
                return self._dataArray[key]

    def __setitem__(self, key, value):
        if(key < self._dataLength and key >= 0):
            self._dataArray[key] = value

    def toString(self, prevData=None, filter=None):
        """
        Turns data into string to be sent to controler

        :param prevData: Previous data sent or received to controler for filtering, is nullable
        :param filter: A filter that will remove all data not included in the filter, is nullable
        :returns: string to send to controler
        """
        outData = ""
        for i in range(0, self._dataLength):
            if (prevData == None or prevData[i] == self[i]) and (filter == None or filter[i]):
                outData += str(i) + ":" + str(self[i]) + "|"
        return outData

