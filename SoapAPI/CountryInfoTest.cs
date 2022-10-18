using Microsoft.VisualStudio.TestTools.UnitTesting;
using CountryInfoService;

[assembly: Parallelize(Workers = 10, Scope = ExecutionScope.MethodLevel)]
namespace SoapAPI
{
    [TestClass]
    public class CountryInfoTest
    {
        private readonly CountryInfoServiceSoapTypeClient countryInfo
           = new CountryInfoServiceSoapTypeClient(CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

        [TestMethod]
        public void CheckTheOrderOfCountryNamesAscending()
        {
            var listOfCountryNamesByCode = countryInfo.ListOfCountryNamesByCode();
            var isOrderedAscending = listOfCountryNamesByCode.SequenceEqual(listOfCountryNamesByCode.OrderBy(a => a.sISOCode));
            Assert.IsTrue(isOrderedAscending);
        }

        [TestMethod]
        public void PassInvalidCountryCode()
        {
            var countryCode = "phl123";
            var response = countryInfo.CountryName(countryCode);
            Assert.IsTrue(response.Contains("Country not found in the database"), $"Country code {countryCode} can be found in the database.");
        }

        [TestMethod]
        public void ValidateCountryName()
        {
            var lastCountryInList = countryInfo.ListOfCountryNamesByCode().LastOrDefault();
            var country = countryInfo.CountryName(lastCountryInList.sISOCode);

            Assert.AreEqual(lastCountryInList.sName, country);
        }
    }
}