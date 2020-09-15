using System;
using System.IO;

namespace LogFileLibrary
{
    public class LogFile
    {

        string logFilePath;
        string logFileMessage;

        public LogFile(string path, string message)
        {
            this.logFilePath = path;
            this.logFileMessage = message;
        }

        public void WriteToLogFile()
        {

            FileStream outStream;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;
            try
            {
                outStream = new FileStream(this.logFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(outStream);
            }

            catch (Exception a)
            {
                Console.WriteLine(a.Message);
                return;
            }

            Console.SetOut(writer);
            Console.WriteLine(this.logFileMessage);
            Console.SetOut(oldOut);
            writer.Close();
            outStream.Close();
        }
    }
}
