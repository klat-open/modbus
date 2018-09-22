using Code4Bugs.Utils.IO;
using Code4Bugs.Utils.IO.Modbus;
using NUnit.Framework;

namespace Code4Bugs.Utils.Tests.IO
{
    [TestFixture]
    internal partial class ModbusTest
    {
        [Test]
        public void func16_should_throw_exception_if_no_responsed()
        {
            var stream = new DummyStream();
            Assert.Throws<EmptyResponsedException>(() =>
            {
                stream.RequestFunc16(0x11, 0x006B, new byte[] { 0x00, 0x0A, 0x01, 0x02 });
            });
        }

        [Test]
        public void func16_should_throw_exception_if_missing_responsed_data()
        {
            var initResponse = new byte[] { 0x11, 0x10, 0x00, 0x01, 0x00, 0x02 };
            var stream = new DummyStream(initResponse);
            stream.Available = initResponse.Length;

            Assert.Throws<MissingDataException>(() =>
            {
                stream.RequestFunc16(0x11, 0x0001, new byte[] { 0x00, 0x0A, 0x01, 0x02 });
            });
        }

        [Test]
        public void func16_should_throw_exception_if_wrong_responsed_slave_address()
        {
            var initResponse = new byte[] { 0x11, 0x10, 0x00, 0x01, 0x00, 0x02, 0x12, 0x98 };
            var stream = new DummyStream(initResponse);
            stream.Available = initResponse.Length;

            Assert.Throws<DataCorruptedException>(() =>
            {
                stream.RequestFunc16(0x12, 0x0001, new byte[] { 0x00, 0x0A, 0x01, 0x02 });
            });
        }

        [Test]
        public void func16_should_throw_exception_if_wrong_responsed_func_code()
        {
            var initResponse = new byte[] { 0x11, 0x11, 0x00, 0x01, 0x00, 0x02, 0x12, 0x98 };
            var stream = new DummyStream(initResponse);
            stream.Available = initResponse.Length;

            Assert.Throws<DataCorruptedException>(() =>
            {
                stream.RequestFunc16(0x11, 0x0001, new byte[] { 0x00, 0x0A, 0x01, 0x02 });
            });
        }

        [Test]
        public void func16_should_throw_exception_if_wrong_responsed_checksum()
        {
            var initResponse = new byte[] { 0x11, 0x10, 0x00, 0x01, 0x00, 0x02, 0x12, 0x00 };
            var stream = new DummyStream(initResponse);
            stream.Available = initResponse.Length;

            Assert.Throws<DataCorruptedException>(() =>
            {
                stream.RequestFunc16(0x11, 0x0001, new byte[] { 0x00, 0x0A, 0x01, 0x02 });
            });
        }

        [Test]
        public void func16_should_success_if_valid_responsed_data()
        {
            var initResponse = new byte[] { 0x11, 0x10, 0x00, 0x01, 0x00, 0x02, 0x12, 0x98 };
            var stream = new DummyStream(initResponse);
            stream.Available = initResponse.Length;

            Assert.DoesNotThrow(() =>
            {
                stream.RequestFunc16(0x11, 0x0001, new byte[] { 0x00, 0x0A, 0x01, 0x02 });
            });
        }
    }
}