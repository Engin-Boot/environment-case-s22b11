using Xunit;
using ConditionsAnalyzer;

namespace ConditionsAnalyzerTest
{
    public class ConditionsAnalyerTest
    {
        [Fact]
        public void TestforGetConditionValues()
        {
            string Conditiondata = "25.7 50";
            Analyzer.Conditions conditions = Analyzer.getConditions(Conditiondata);
            Assert.True(conditions.temperature.Equals(25.7F));
            Assert.True(conditions.humidity.Equals(50F));
        }

        [Fact]
        public void WhenConditionsisNotInHigherRange()
        {
            RangeResult TemperatureRangeResult = Analyzer.CheckConditionsisInRange(45, "Temperature", 37, 4);
            RangeResult HumidityRangeResult = Analyzer.CheckConditionsisInRange(75, "Humidity", 70, 0);
            Assert.False(TemperatureRangeResult.isInRange);
            Assert.False(HumidityRangeResult.isInRange);

        }
        [Fact]
        public void WhenConditionsisNotInLowerRange()
        {
            RangeResult TemperatureRangeResult = Analyzer.CheckConditionsisInRange(2, "Temperature", 37, 4);
            RangeResult HumidityRangeResult = Analyzer.CheckConditionsisInRange(-1, "Humidity", 70, 0);
            Assert.False(TemperatureRangeResult.isInRange);
            Assert.False(HumidityRangeResult.isInRange);

        }
        [Fact]
        public void WhenConditionsisInRange()
        {
            RangeResult TemperatureRangeResult = Analyzer.CheckConditionsisInRange(35, "Temperature", 37, 4);
            RangeResult HumidityRangeResult = Analyzer.CheckConditionsisInRange(68, "Humidity", 70, 0);
            Assert.True(TemperatureRangeResult.isInRange);
            Assert.True(HumidityRangeResult.isInRange);
        }
        [Fact]
        public void WhenConditionisInHighWarningLevels()
        {
            string TemperatureMessage = Analyzer.checkForBreachedCondition(39, "Temperature", 40, 0);
            string HumidityMessage = Analyzer.checkForBreachedCondition(72, "Humidity", 90, 0);
            Assert.True(TemperatureMessage.Equals("Temperature value = 39 is at WARNING level"));
            Assert.True(HumidityMessage.Equals("Humidity value = 72 is at WARNING level"));
        }
        [Fact]
        public void WhenConditionisInLowWarningLevels()
        {
            string TemperatureMessage = Analyzer.checkForBreachedCondition(2, "Temperature", 40, 0);
            string HumidityMessage = Analyzer.checkForBreachedCondition(2, "Humidity", 90, 0);
            Assert.True(TemperatureMessage.Equals("Temperature value = 2 is at WARNING level"));
            Assert.True(HumidityMessage.Equals("Humidity value = 2 is at WARNING level"));
        }
        [Fact]
        public void WhenConditionisInHigherErrorLevels()
        {
            string TemperatureMessage = Analyzer.checkForBreachedCondition(42, "Temperature", 40, 0);
            string HumidityMessage = Analyzer.checkForBreachedCondition(95, "Humidity", 90, 0);
            Assert.True(TemperatureMessage.Equals("Temperature value = 42 is at ERROR level"));
            Assert.True(HumidityMessage.Equals("Humidity value = 95 is at ERROR level"));
        }
        [Fact]
        public void WhenConditionisInLowerErrorLevels()
        {
            string TemperatureMessage = Analyzer.checkForBreachedCondition(-2, "Temperature", 40, 0);
            string HumidityMessage = Analyzer.checkForBreachedCondition(-1, "Humidity", 90, 0);
            Assert.True(TemperatureMessage.Equals("Temperature value = -2 is at ERROR level"));
            Assert.True(HumidityMessage.Equals("Humidity value = -1 is at ERROR level"));
        }

        [Fact]
        public void TestAnalyzeFunctionIfConditionIsNotBreached()
        {
            IReporter reporter = new SMSReporter();
            Analyzer analyzer = new Analyzer(reporter);
            bool result = analyzer.Analyze(60, "Humidity", 70, 0, 90, 0);
            Assert.True(result);
            
        }
        [Fact]
        public void TestAnalyzeFunctionIfConditionIsBreached()
        {
            IReporter reporter = new SMSReporter();
            Analyzer analyzer = new Analyzer(reporter);
            bool result = analyzer.Analyze(39, "Temperature", 37, 4, 40, 0);
            Assert.False(result);
        }
    }
}
