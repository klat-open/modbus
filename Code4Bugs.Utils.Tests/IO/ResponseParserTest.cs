using Code4Bugs.Utils.IO.Modbus;
using Code4Bugs.Utils.IO.Modbus.Data;
using Code4Bugs.Utils.Types;
using NUnit.Framework;

namespace Code4Bugs.Utils.Tests.IO
{
    [TestFixture]
    internal class ResponseParserTest
    {
        [Test]
        public void parse_func3_success()
        {
            var response = new byte[] { 0x11, 0x03, 0x06, 0xAE, 0x41, 0x56, 0x52, 0x43, 0x40, 0x49, 0xAD };
            ResponseFunc3 result = null;
            Assert.DoesNotThrow(() =>
            {
                result = response.ToResponseFunc3();
            });
            Assert.NotNull(result);
            Assert.True(result.SlaveId == response[0]);
            Assert.True(result.Data.AreEqual(new byte[] { 0xAE, 0x41, 0x56, 0x52, 0x43, 0x40 }));
        }

        [Test]
        public void parse_func4_success()
        {
            var response = new byte[] { 0x11, 0x04, 0x06, 0xAE, 0x41, 0x56, 0x52, 0x43, 0x40, 0x08, 0x4B };
            ResponseFunc4 result = null;
            Assert.DoesNotThrow(() =>
            {
                result = response.ToResponseFunc4();
            });
            Assert.NotNull(result);
            Assert.True(result.SlaveId == response[0]);
            Assert.True(result.Data.AreEqual(new byte[] { 0xAE, 0x41, 0x56, 0x52, 0x43, 0x40 }));
        }

        [Test]
        public void parse_func6_success()
        {
            var response = new byte[] { 0x11, 0x06, 0x00, 0x01, 0x00, 0x03, 0x9A, 0x9B };
            ResponseFunc6 result = null;
            Assert.DoesNotThrow(() =>
            {
                result = response.ToResponseFunc6();
            });
            Assert.NotNull(result);
            Assert.True(result.SlaveId == response[0]);
            Assert.True(result.DataAddress == 1);
            Assert.True(result.WrittenValue == 3);
        }

        [Test]
        public void parse_func16_success()
        {
            var response = new byte[] { 0x11, 0x10, 0x00, 0x01, 0x00, 0x02, 0x12, 0x98 };
            ResponseFunc16 result = null;
            Assert.DoesNotThrow(() =>
            {
                result = response.ToResponseFunc16();
            });
            Assert.NotNull(result);
            Assert.True(result.SlaveId == response[0]);
            Assert.True(result.DataAddress == 1);
            Assert.True(result.WrittenRegisterCount == 2);
        }
    }
}