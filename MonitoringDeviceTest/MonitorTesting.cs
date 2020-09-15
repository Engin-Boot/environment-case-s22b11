using System;
using Xunit;
using MonitoringDevice;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
using System.IO;

namespace MonitoringDeviceTest
{
    public class MonitorTesting
    {
        [Fact]
        public void WhenFileEmptyThenShowError()
        {
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            var x = obj.ReadingData("C:/Users/Aayush/Desktop/test.csv", "C:/Users/Aayush/Desktop/abc.txt");
            Assert.True(x == 1);
        }

        
        [Fact]

        public void WhenFileNotInCorrectFormatThenDontAcceptTheFile()
        {
            
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            var x = obj.FormatChecker("C:/Users/Aayush/Desktop/abc.txt");
            Assert.True(x == 1);
        }
        
        [Fact]
        public void WhenMissingValueinFileThenSkipTheLine()
        {
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();

            var x = obj.ProcessingData();
            Assert.True(x == 0);
        }

        [Fact]

        public void testToHandleMissingValue()
        {
            string[] testString = {"", "Test String"};
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            var x = obj.HandleMissingValues(testString);
            Assert.True(x == 1);
        }
        [Fact]

        public void testToCheckWorkingOfLogFile()
        {
            string testFile = "C:/Users/Aayush/Desktop/testfile.txt";
            

            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            obj.WriteToLogFile(testFile,"abcd");

            Assert.True(File.ReadAllText(testFile) != "");
        }

        //[Fact]
        //public void WhenSenderDoesNotSendDataThenShowMessage()
        //{
        //    string logFile = "C:/Users/Aayush/Desktop/timeIssue.txt";
        //    MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
           
        //    var x = obj.SendingData(logFile);

        //    Assert.True(x.Date == DateTime.Now.Date); 
            
        //}
       
    }
}
