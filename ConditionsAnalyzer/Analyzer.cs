using System;
using System.Timers;

namespace ConditionsAnalyzer
{

    public struct RangeResult
    {
        public bool isInRange;
        public string breachType;
        public string message;
    }

    public class Analyzer
    {
        public static Timer aTimer;
        static IReporter reporter;
        static float temperatureLowerWarning;
        static float temperatureHigherWarning;
        static float temperatureLowerError;
        static float temperatureHigherError;

        static float humidityLowerWarning;
        static float humidityHigherWarning;
        static float humidityLowerError;
        static float humidityHigherError;

        public struct Conditions
        {
            public float temperature;
            public float humidity;
        }

        public Analyzer(IReporter reporterobj)
        {
            reporter = reporterobj;
            temperatureLowerWarning = 4;
            temperatureHigherWarning = 37;
            temperatureLowerError = 0;
            temperatureHigherError = 40;

            humidityLowerWarning = 4;
            humidityLowerError = 0;
            humidityHigherWarning = 70;
            humidityHigherError = 90;
            

        }

        public static Conditions getConditions(string Data)
        {
            Conditions condition = new Conditions();
            string[] ConditionArray = Data.Split(' ');
            condition.temperature = float.Parse(ConditionArray[0]);
            condition.humidity = float.Parse(ConditionArray[1]);
            return condition;
        }

        public bool Analyze(float value, string ConditionName, float WarningHighLevel, float WarningLowLevel, float ErrorHighLevel, float ErrorLowLevel)
        {
            RangeResult rangeResult = new RangeResult();
            rangeResult = CheckConditionsisInRange(value,ConditionName, WarningHighLevel, WarningLowLevel);
            if(!rangeResult.isInRange)
            {
                rangeResult.message = checkForBreachedCondition(value, ConditionName, ErrorHighLevel, ErrorLowLevel);
                reporter.sendMessage(rangeResult.message);
                return rangeResult.isInRange;
            }
            return rangeResult.isInRange;
        }
       
        public static RangeResult CheckConditionsisInRange(float value, string ConditionName, float WarningHighLevel, float WarningLowLevel)
        {
            RangeResult rangeResult = new RangeResult();
            rangeResult.isInRange = true;
            rangeResult.isInRange = !(value < WarningLowLevel || value > WarningHighLevel);
            return rangeResult;
        }
      
        public static string checkForBreachedCondition(float value, string ConditionName, float HigherExtremeCondition, float LowerextremeCondition)
        {
            bool ExtremeConditionCheck = (value < LowerextremeCondition || value > HigherExtremeCondition);
            string AlertType = ExtremeConditionCheck ? "ERROR" : "WARNING";
            string message = GenerateAnAlertMessage(value, ConditionName, AlertType);
            return message;
        }

        public static string GenerateAnAlertMessage(float value, string ConditionName,string AlertType)
        {
            string message = $"{ConditionName} value = {value} is at {AlertType} level";
            return message;   
        }
        
        public void SetTimer()
        {
            
            aTimer = new System.Timers.Timer(10000); 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        public static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
            //              e.SignalTime);
            reporter.sendMessage("Sender has not sent data since 30 mins");
        }
       
        public static void Main(string[] args)
        {
            SMSReporter reporter = new SMSReporter();
            Analyzer analyzer = new Analyzer(reporter);
            Conditions conditions = new Conditions();
            string line;
            analyzer.SetTimer();
            bool result;
            while ((line = Console.ReadLine()) != null)
            {
                aTimer.Stop();
                conditions = getConditions(line);
                result = analyzer.Analyze(conditions.temperature,"Temperature",temperatureHigherWarning,temperatureLowerWarning,temperatureHigherError,temperatureLowerError);
                result = analyzer.Analyze(conditions.humidity, "Humidity", humidityHigherWarning,humidityLowerWarning,humidityHigherError,humidityLowerError);
                aTimer.Start();
            }
        }
    }
}
