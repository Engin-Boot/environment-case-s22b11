using System.Threading;
using Xunit;
using ConditionsAnalyzer;
using NSubstitute;

namespace ConditionsAnalyzerTest
{
    public class ConditionAnalyzerTest
    {
        [Fact]
        public void TestForGetConditionValues()
        {
            const string conditionData = "25.7 50";
            var conditions = Analyzer.GetConditions(conditionData);
            Assert.True(conditions.Temperature.Equals(25.7F));
            Assert.True(conditions.Humidity.Equals(50F));
        }

        [Fact]
        public void WhenConditionsIsNotInHigherRange()
        {
            var temperatureRangeResult = Analyzer.CheckConditionIsInRange(45, 37, 4);
            var humidityRangeResult = Analyzer.CheckConditionIsInRange(75, 70, 0);
            Assert.False(temperatureRangeResult.IsInRange);
            Assert.False(humidityRangeResult.IsInRange);

        }
        [Fact]
        public void WhenConditionIsNotInLowerRange()
        {
            var temperatureRangeResult = Analyzer.CheckConditionIsInRange(2, 37, 4);
            var humidityRangeResult = Analyzer.CheckConditionIsInRange(-1, 70, 0);
            Assert.False(temperatureRangeResult.IsInRange);
            Assert.False(humidityRangeResult.IsInRange);

        }
        [Fact]
        public void WhenConditionIsInRange()
        {
            var temperatureRangeResult = Analyzer.CheckConditionIsInRange(35, 37, 4);
            var humidityRangeResult = Analyzer.CheckConditionIsInRange(68, 70, 0);
            Assert.True(temperatureRangeResult.IsInRange);
            Assert.True(humidityRangeResult.IsInRange);
        }
        [Fact]
        public void WhenConditionIsInHighWarningLevels()
        {
            var temperatureMessage = Analyzer.CheckForBreachedCondition(39, "Temperature", 40, 0);
            var humidityMessage = Analyzer.CheckForBreachedCondition(72, "Humidity", 90, 0);
            var temperatureResult = temperatureMessage.Contains("Temperature value = 39 is at WARNING level");
            var humidityResult = humidityMessage.Contains("Humidity value = 72 is at WARNING level");
            Assert.True(temperatureResult);
            Assert.True(humidityResult);
        }
        [Fact]
        public void WhenConditionIsInLowWarningLevels()
        {
            var temperatureMessage = Analyzer.CheckForBreachedCondition(2, "Temperature", 40, 0);
            var humidityMessage = Analyzer.CheckForBreachedCondition(2, "Humidity", 90, 0);
            var temperatureResult = temperatureMessage.Contains("Temperature value = 2 is at WARNING level");
            var humidityResult = humidityMessage.Contains("Humidity value = 2 is at WARNING level");
            Assert.True(temperatureResult);
            Assert.True(humidityResult);
        }
        [Fact]
        public void WhenConditionIsInHigherErrorLevels()
        {
            var temperatureMessage = Analyzer.CheckForBreachedCondition(42, "Temperature", 40, 0);
            var humidityMessage = Analyzer.CheckForBreachedCondition(95, "Humidity", 90, 0);
            var temperatureResult = temperatureMessage.Contains("Temperature value = 42 is at ERROR level");
            var humidityResult = humidityMessage.Contains("Humidity value = 95 is at ERROR level");
            Assert.True(temperatureResult);
            Assert.True(humidityResult);
      
        }
        [Fact]
        public void WhenConditionIsInLowerErrorLevels()
        {
            var temperatureMessage = Analyzer.CheckForBreachedCondition(-2, "Temperature", 40, 0);
            var humidityMessage = Analyzer.CheckForBreachedCondition(-1, "Humidity", 90, 0);
            var temperatureResult = temperatureMessage.Contains("Temperature value = -2 is at ERROR level");
            var humidityResult = humidityMessage.Contains("Humidity value = -1 is at ERROR level");
            Assert.True(temperatureResult);
            Assert.True(humidityResult);
            
        }

        [Fact]
        public void TestAnalyzeFunctionIfConditionIsNotBreached()
        {
            IReporter reporter = new SmsReporter();
            var analyzer = new Analyzer(reporter);
            var result = analyzer.Analyze(60, "Humidity", 70, 0, 90, 0);
            Assert.True(result);
            
        }
        [Fact]
        public void TestAnalyzeFunctionIfConditionIsBreachedUsingEmailReporter()
        {
            IReporter reporter = new EmailReporter();
            var analyzer = new Analyzer(reporter);
            var result = analyzer.Analyze(39, "Temperature", 37, 4, 40, 0);
            Assert.False(result);
        }
        [Fact]
        public void TestAnalyzeFunctionIfConditionIsBreachedUsingSmsReporter()
        {
            IReporter reporter = new SmsReporter();
            var analyzer = new Analyzer(reporter);
            var result = analyzer.Analyze(39, "Temperature", 37, 4, 40, 0);
            Assert.False(result);
        }
    }
}
