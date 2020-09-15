using System;
using System.Timers;

namespace ConditionsAnalyzer
{

    public struct RangeResult
    {
        public bool IsInRange;
        public string Message;
    }

    public class Analyzer
    {
        private static Timer _aTimer;
        private static IReporter _reporter;
        private static float _temperatureLowerWarning;
        private static float _temperatureHigherWarning;
        private static float _temperatureLowerError;
        private static float _temperatureHigherError;

        private static float _humidityLowerWarning;
        private static float _humidityHigherWarning;
        private static float _humidityLowerError;
        private static float _humidityHigherError;

        public struct Conditions
        {
            public float Temperature;
            public float Humidity;
        }

        public Analyzer(IReporter reporter)
        {
            _reporter = reporter;
            _temperatureLowerWarning = 4;
            _temperatureHigherWarning = 37;
            _temperatureLowerError = 0;
            _temperatureHigherError = 40;

            _humidityLowerWarning = 4;
            _humidityLowerError = 0;
            _humidityHigherWarning = 70;
            _humidityHigherError = 90;
            

        }

        public static Conditions GetConditions(string data)
        {
            var condition = new Conditions();
            var conditionArray = data.Split(' ');
            condition.Temperature = float.Parse(conditionArray[0]);
            condition.Humidity = float.Parse(conditionArray[1]);
            return condition;
        }

        public bool Analyze(float value, string conditionName, float warningHighLevel, float warningLowLevel, float errorHighLevel, float errorLowLevel)
        {
            var rangeResult = CheckConditionIsInRange(value, warningHighLevel, warningLowLevel);
            if (rangeResult.IsInRange) return true;
            rangeResult.Message = CheckForBreachedCondition(value, conditionName, errorHighLevel, errorLowLevel);
            _reporter.SendMessage(rangeResult.Message);
            return false;
        }
       
        public static RangeResult CheckConditionIsInRange(float value, float warningHighLevel, float warningLowLevel)
        {
            var rangeResult = new RangeResult {IsInRange = true};
            rangeResult.IsInRange = !(value < warningLowLevel || value > warningHighLevel);
            return rangeResult;
        }
      
        public static string CheckForBreachedCondition(float value, string conditionName, float higherExtremeCondition, float lowerExtremeCondition)
        {
            var extremeConditionCheck = (value < lowerExtremeCondition || value > higherExtremeCondition);
            var alertType = extremeConditionCheck ? "ERROR" : "WARNING";
            var message = GenerateAnAlertMessage(value, conditionName, alertType);
            return message;
        }

        private static string GenerateAnAlertMessage(float value, string conditionName,string alertType)
        {
            var message = $"{conditionName} value = {value} is at {alertType} level at {DateTime.Now}";
            return message;   
        }
        
        public void SetTimer(int interval)
        { 
            _aTimer = new Timer(interval); 
            _aTimer.Elapsed += OnTimedEvent;
            _aTimer.AutoReset = true;
            _aTimer.Enabled = true;
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            _reporter.SendMessage($"Sender has not sent any data since {DateTime.Now.AddMinutes(-30)}");
        }
       
        public static void Main()
        {
            var reporter = new SMSReporter();
            var analyzer = new Analyzer(reporter);
            string line;
            analyzer.SetTimer(10000);
          
            while ((line = Console.ReadLine()) != null)
            {
                _aTimer.Stop();
                var conditions = GetConditions(line);
                analyzer.Analyze(conditions.Temperature,"Temperature",_temperatureHigherWarning,_temperatureLowerWarning,_temperatureHigherError,_temperatureLowerError);
                analyzer.Analyze(conditions.Humidity, "Humidity", _humidityHigherWarning,_humidityLowerWarning,_humidityHigherError,_humidityLowerError);
                _aTimer.Start();
            }
        }
    }
}
