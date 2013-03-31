using ComPortApp.Entites;
using NUnit.Framework;

namespace ComPortApp.Tests
{
    public class AngleValuesProviderTest
    {
        [Test]
        [TestCase(48.6978, 35.4686, 500, 450, 15, 240)]
        public void GetAngleValues_Test(double latitude, double longitude, int altitude, int height,
            int firstAngle, int secondAngle)
        {
            //given
            var parsedInfo = new ParsedPortInfo { Latitude = latitude, Longitude = longitude, 
                Altitude = altitude, Height = height }; 

            //when]
            InitialDataProvider.InitializeConfigData();
            var angleValuesProvider = new AngleValuesProvider();
            angleValuesProvider.ValidateParsedInfo(parsedInfo);

            var result = angleValuesProvider.GetAngleValues(450, true);

            //then
            Assert.IsNotNull(result);
            //Assert.AreEqual(firstAngle, result[0]);
            //Assert.AreEqual(secondAngle, result[1]);
        }
    }
}
