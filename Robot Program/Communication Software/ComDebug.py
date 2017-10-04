#Debug Stuff
import ComControl

serv = ComControl.ComControl(4256)

def Main():
    ComDataTest()
    serv.onReceivedEventAdd(RemoveTest)
    serv.onReceivedEventAdd(echo)
    serv.onReceivedEventRemove(RemoveTest)
    
    while True:
        pass

def ComDataTest(): 
    try:
        #Getter and constructer tests
        testData = ComControl.ComData()
        test(0, testData[3])
        testData = ComControl.ComData("0:0")
        test(0, testData[0])
        testData = ComControl.ComData("0:2147483647")
        test(2147483647, testData[0])
        testData = ComControl.ComData("0:-2147483647")
        test(-2147483647, testData[0])
        testData = ComControl.ComData("60:34|2:56")
        test(56, testData[2])
        testData = ComControl.ComData("5:0|aa::fa:e|faef|aef|a:e|f:eagae|5:2014")
        test(2014, testData[5])
        testData = ComControl.ComData("0:0|1:2|2:3|3:3323242|4:878|5:-1234567")
        test(878, testData[4])

        #Setter and constructer tests
        testData = ComControl.ComData()
        testData[0] = 256
        test(256, testData[0])
        testData[1] = 1024
        test(1024, testData[1])
        testData[2] = 0
        test(256, testData[0])
        testData[666] = 245
        test(256, testData[0])
        testData[2] = 2147483647
        test(2147483647, testData[2])
        testData[2] = -2147483647
        test(-2147483647, testData[2])
        testData = ComControl.ComData()
        test(0, testData[2])

        #ToString tests
        testData = ComControl.ComData("0:0")
        testStr = testData.toString(filter=ComControl.Filter([ComControl.dataId(0)]))
        test("0:0|", testStr)
        testData = ComControl.ComData("0:0|1:456|2:444|3:555")
        testStr = testData.toString(filter=ComControl.Filter([ComControl.dataId(0),ComControl.dataId(2)]))
        test("0:0|2:444|", testStr)
        testData = ComControl.ComData("0:0|1:456|2:444|3:555")
        testStr = testData.toString(ComControl.ComData("0:0|1:555|2:444|3:555|4:3"))
        test("1:456|4:0|", testStr)
        testData = ComControl.ComData("0:0|1:456|2:444|3:555")
        testStr = testData.toString(prevData=ComControl.ComData("0:0|1:555|2:444|3:555|4:3"), filter=ComControl.Filter([ComControl.dataId(1)]))
        test("1:456|", testStr)
        testData = ComControl.ComData("0:0|1:456|2:444|3:555|5:333")
        testStr = testData.toString(prevData=ComControl.ComData("0:0|1:555|2:444|3:555|4:3"), filter=ComControl.Filter([ComControl.dataId(1),ComControl.dataId(5)]))
        test("1:456|5:333|", testStr)

        print("ComDataTests Didn't crash during tests")
    except Exception as e:
        print("ComDataTests Crashed")
        print(e)

def test(check, value):
    if(check != value):
        print("FAILED-------------------------------------------------")
    print(str(check) + " == " + str(value) + " " + str(check == value))

def RemoveTest(data):
    print("FAILED-------------------------------------------------")
    print("Remove test failed")

def echo(data):
    try:
        print("Received: " + data.toString())
        serv.send(data.toString(filter=ComControl.ComData.RobotAuth))
    except Exception as e:
        print(e)

Main()
