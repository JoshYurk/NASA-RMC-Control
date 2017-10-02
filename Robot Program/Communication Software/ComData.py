from enum import Enum, unique
class ComData:
    """
    This stores and formats the data used to communicate with the controler
    """

    @unique
    class dataId(Enum):
        """
        Modify this to match the one on the controler
        """
        Moter1 = 0
        Moter2 = 1
        Moter3 = 2
        Moter4 = 3
        Error1 = 4
        Sensor = 5

    _dataLength = len(dataId)
    _dataArray = [0] * _dataLength

    def __init__(self, data=None):
        """
        :param data: String received from the toString function or from the controler
        """
        if data != None:
            for item in data.split('|', len(data)):
                pair = item.split(':', 2)
                if int(pair[0]) < self._dataLength:
                    self._dataArray[int(pair[0])] = int(pair[1])
    def data(self, idenifier=None, data=None):
        """
        Gets or sets data

        :param idenifier: A dataId that specifies the data being get / set
        :param data: The value to be set
        """
        if idenifier < self._dataLength and idenifier >= 0:
            if data != None:
                self._dataArray[idenifier] = data
            else:
                return self._dataArray[idenifier]

    def toString(self, prevData=None):
        """
        Turns data into string to be sent to controler

        :param prevData: Previous data sent or received to controler for filtering
        :returns: string to send to controler
        """
        filtering = prevData != None
        outData = ""
        for i in range(0, self._dataLength):
            if (not filtering) or (prevData.data(i) == self.data(i)):
                outData += str(i) + ":" + str(self.data(i)) + "|"
        return outData
