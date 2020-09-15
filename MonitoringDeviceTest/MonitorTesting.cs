using System;
using Xunit;
using System.IO;

namespace MonitoringDeviceTest
{
    public class MonitorTesting
    {
        [Fact]
        public void TestForFileHavingContent()
        {
            var testFile = "../../../../testFileForFileHavingContent.csv";
            var testFile2 = "../../../../testLogFile.txt";
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            var x = obj.ReadingData(testFile, testFile2);
            Assert.True(x == 1);
        }

        [Fact]
        public void TestForFileNotHavingContent()
        {
            var testFile = "../../../../testFileForEmptyFile.csv";
            var testFile2 = "../../../../testLogFile.txt";
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            var x = obj.ReadingData(testFile, testFile2);
            Assert.True(x == 0);
        }

        [Fact]
        public void testForWhenFileInCorrectFormat()
        {
            var testFile = "../../../../CorrectFileFormat.csv";
            var testFile1 = "../../../../testLogFile.txt";
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            obj.ReadingData(testFile, testFile1);
            var x = obj.FormatChecker(testFile1);
            Assert.True(x == 1);
        }

        [Fact]
        public void testForWhenFileinIncorrectFormat()
        {
            var testFile = "../../../../IncorrectFormatFile.xlsx";
            var testFile1 = "../../../../testLogFile.txt";

            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            obj.ReadingData(testFile, testFile1);

            var x = obj.FormatChecker(testFile1);
            Assert.True(x == 0);

        }

        [Fact]
        public void WhenMissingValueinFileThenSkipTheLine()
        {
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();

            var x = obj.ProcessingData();
            Assert.True(x == 0);
        }

        [Fact]
        public void TestForMissingValueSkip()
        {
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            var testFile = "../../../../MissingValue.csv";
            var testFile1 = "../../../../testLogFile.txt";

            obj.ReadingData(testFile, testFile1);

            var x = obj.ProcessingData();
            Assert.True(x == 1);
        }

        [Fact]

        public void TestForNoMissingValue()
        {
            string[] testString = {"", "Test String"};
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            var x = obj.HandleMissingValues(testString);
            Assert.True(x == 1);
        }
        [Fact]
        public void TestForMissingValue()
        {
           string[] testString = { "abcd", "123" };
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();

            var x = obj.HandleMissingValues(testString);

            Assert.True(x == 0);

        }
        [Fact]

        public void TestToCheckWorkingOfLogFile()
        {
            string testFile = "../../../../TestFileForLogFile.txt";

            LogFileLibrary.LogFile obj = new LogFileLibrary.LogFile(testFile, "abcd");
            obj.WriteToLogFile();

            Assert.True(File.ReadAllText(testFile) != "");
        }

        [Fact]
        public void TestForWhenSenderDoesNotSendData ()
        {
           
            var logFile = "../../../../timeIssue.txt";

            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            MonitoringDevice.MonitoringDevice.SetTimer();
            
            var x = obj.SendingData(logFile);

            Assert.True(x.Date == DateTime.Now.Date); 

        }
    }
}
