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
            var testFile = "C:/Users/Aayush/Desktop/testFileForFileHavingContent.csv";
            var testFile2 = "C:/Users/Aayush/Desktop/testLogFile.txt";
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            var x = obj.ReadingData(testFile, testFile2);
            Assert.True(x == 1);
        }

        [Fact]
        public void TestForFileNotHavingContent()
        {
            var testFile = "C:/Users/Aayush/Desktop/testFileForEmptyFile.csv";
            var testFile2 = "C:/Users/Aayush/Desktop/testLogFile.txt";
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            var x = obj.ReadingData(testFile, testFile2);
            Assert.True(x == 0);
        }

        [Fact]
        public void testForWhenFileInCorrectFormat()
        {
            var testFile = "C:/Users/Aayush/Desktop/CorrectFileFormat.csv";
            var testFile1 = "C:/Users/Aayush/Desktop/testLogFile.txt";
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            obj.ReadingData(testFile, testFile1);
            var x = obj.FormatChecker(testFile1);
            Assert.True(x == 1);
        }

        [Fact]
        public void testForWhenFileinIncorrectFormat()
        {
            var testFile = "C:/Users/Aayush/Desktop/IncorrectFormatFile.xlsx";
            var testFile1 = "C:/Users/Aayush/Desktop/testLogFile.txt";

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
            var testFile = "C:/Users/Aayush/Desktop/MissingValue.csv";
            var testFile1 = "C:/Users/Aayush/Desktop/testLogFile.txt";

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
            string testFile = "C:/Users/Aayush/Desktop/TestFileForLogFile.txt";

            LogFileLibrary.LogFile obj = new LogFileLibrary.LogFile(testFile, "abcd");
            obj.WriteToLogFile();

            Assert.True(File.ReadAllText(testFile) != "");
        }

        [Fact]
        public void TestForWhenSenderDoesNotSendData ()
        {
           
            var logFile = "C:/Users/Aayush/Desktop/timeIssue.txt";

            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            MonitoringDevice.MonitoringDevice.SetTimer();
            
            var x = obj.SendingData(logFile);

            Assert.True(x.Date == DateTime.Now.Date); 

        }

        [Fact]
        public void TestForSendingIncorrectData()
        {
            MonitoringDevice.MonitoringDevice.SetTimer();
            var testFile = "C:/Users/Aayush/Desktop/MissingValue.csv";
            var testFile1 = "C:/Users/Aayush/Desktop/testLogFileForSenderFunction.txt";
            MonitoringDevice.MonitoringDevice obj = new MonitoringDevice.MonitoringDevice();
            obj.ReadingData(testFile, testFile1);

            var x = obj.SendingData(testFile1);
            Assert.True((File.ReadAllText(testFile1) == ""));

        }

    }
}
