using Maikelvdb.Xprtz.Assessment.Tests.Setup;

namespace Maikelvdb.Xprtz.Assessment.Tests
{
    [TestClass]
    public class MappingTests : TestFixture
    {
        /// <summary>
        /// Test om te controleren of alle AutoMapper profiles rekening houden met de benodigde properties die aanwezig zijn
        /// </summary>
        [TestMethod]
        public void TestValidMappings()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}