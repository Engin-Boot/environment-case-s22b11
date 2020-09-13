using System;
using Xunit;
using MonitoringDevice;

namespace MonitoringDeviceTest
{
    public class MonitorTesting
    {
        [Fact]
        public void WhenFileNotFoundThenShowError()
        {
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            int x = obj.ReadingData();
            Assert.True(x.Equals(1));
        }

        [Fact]
        public void WhenTempratureValueMissingThenSkipTheValue()
        {
           MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
                obj.ReadingData();
                var x = obj.SendingData();
                Assert.True(x == 1);
            
        }
        [Fact]
        public void  WhenHumidityValueMissingThenSkipTheValue()
        {
            
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            
            
                var x = obj.SendingData();
                Assert.True(x == 0);
            
        }

        [Fact]

        public void WhenFileNotInCorrectFormatThenDontAcceptTheFile()
        {
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();

            var x = obj.ProcessingData();
            Assert.True(x == 1);
        }
        
    }
}
