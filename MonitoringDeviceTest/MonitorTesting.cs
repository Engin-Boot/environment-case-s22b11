using System;
using Xunit;
using MonitoringDevice;

namespace MonitoringDeviceTest
{
    public class MonitorTesting
    {
        [Fact]
        public void WhenFileEmptyThenShowError()
        {
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            var x = obj.ReadingData("C:/Users/Aayush/Desktop/test.csv");
            Assert.True(x == 1);
        }

        
        [Fact]

        public void WhenFileNotInCorrectFormatThenDontAcceptTheFile()
        {
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            var x = obj.FormatChecker();
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
        public void WhenSenderDoesNotSendDataOnTimeThenShowMessage()
        {

        }
       
    }
}
