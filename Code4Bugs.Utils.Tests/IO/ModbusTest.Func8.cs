using Code4Bugs.Utils.IO;
using Code4Bugs.Utils.IO.Modbus;
using NUnit.Framework;

namespace Code4Bugs.Utils.Tests.IO
{
    [TestFixture]
    internal partial class ModbusTest
    {
        [Test]
        public void func8_should_throw_exception_if_no_responsed()
        {
            var stream = new DummyStream();
            Assert.Throws<EmptyResponsedException>(() =>
            {
                stream.RequestFunc8(0x11, 0x006B, 0x0003);
            });
        }

        [Test]
        public void func8_should_throw_exception_if_missing_responsed_data()
        {
            var initResponse = new byte[] { 0x11, 0x08, 0x00, 0x01, 0x00, 0x03 };
            var stream = new DummyStream(initResponse);
            stream.Available = initResponse.Length;

            Assert.Throws<MissingDataException>(() =>
            {
                stream.RequestFunc8(0x11, 0x0001, 0x0003);
            });
        }

        [Test]
        public void func8_should_throw_exception_if_wrong_responsed_slave_address()
        {
            var initResponse = new byte[] { 0x11, 0x08, 0x00, 0x01, 0x00, 0x03, 0x9A, 0x9B };
            var stream = new DummyStream(initResponse);
            stream.Available = initResponse.Length;

            Assert.Throws<DataCorruptedException>(() =>
            {
                stream.RequestFunc8(0x12, 0x0001, 0x0003);
            });
        }

        [Test]
        public void func8_should_throw_exception_if_wrong_responsed_func_code()
        {
            var initResponse = new byte[] { 0x11, 0x05, 0x00, 0x01, 0x00, 0x03, 0x9A, 0x9B };
            var stream = new DummyStream(initResponse);
            stream.Available = initResponse.Length;

            Assert.Throws<DataCorruptedException>(() =>
            {
                stream.RequestFunc8(0x11, 0x0001, 0x0003);
            });
        }

        [Test]
        public void func8_should_throw_exception_if_wrong_responsed_checksum()
        {
            var initResponse = new byte[] { 0x11, 0x08, 0x00, 0x01, 0x00, 0x03, 0x9A, 0x00 };
            var stream = new DummyStream(initResponse);
            stream.Available = initResponse.Length;

            Assert.Throws<DataCorruptedException>(() =>
            {
                stream.RequestFunc8(0x11, 0x0001, 0x0003);
            });
        }

        [Test]
        public void func8_should_success_if_valid_responsed_data()
        {
            var initResponse = new byte[] { 0x0B, 0x08, 0x00, 0x00, 0x02, 0x03, 0xA1, 0xC0 };
            var stream = new DummyStream(initResponse);
            stream.Available = initResponse.Length;

            Assert.DoesNotThrow(() =>
            {
                stream.RequestFunc8(0x0B, 0x0000, 0x0203);
            });
        }
    }
}