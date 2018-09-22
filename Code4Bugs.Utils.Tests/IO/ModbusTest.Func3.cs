using Code4Bugs.Utils.IO;
using NUnit.Framework;

namespace Code4Bugs.Utils.Tests.IO
{
    [TestFixture]
    internal partial class ModbusTest
    {
        [Test]
        public void func3_should_throw_exception_if_no_responsed()
        {
            var stream = new DummyStream();
            Assert.Throws<EmptyResponsedException>(() =>
            {
                stream.RequestFunc3(0x11, 0x006B, 0x0003);
            });
        }

        [Test]
        public void func3_should_throw_exception_if_missing_responsed_data()
        {
            var initResponse = new byte[] { 0x11, 0x03, 0x06, 0xAE, 0x41, 0x56, 0x52, 0x43, 0x40 };
            var stream = new DummyStream(initResponse);
            stream.Available = initResponse.Length;

            Assert.Throws<MissingDataException>(() =>
            {
                stream.RequestFunc3(0x11, 0x006B, 0x0003);
            });
        }

        [Test]
        public void func3_should_throw_exception_if_wrong_responsed_slave_address()
        {
            var initResponse = new byte[] { 0x11, 0x03, 0x06, 0xAE, 0x41, 0x56, 0x52, 0x43, 0x40, 0x49, 0xAD };
            var stream = new DummyStream(initResponse);
            stream.Available = initResponse.Length;

            Assert.Throws<DataCorruptedException>(() =>
            {
                stream.RequestFunc3(0x12, 0x006B, 0x0003);
            });
        }

        [Test]
        public void func3_should_throw_exception_if_wrong_responsed_func_code()
        {
            var initResponse = new byte[] { 0x11, 0x04, 0x06, 0xAE, 0x41, 0x56, 0x52, 0x43, 0x40, 0x49, 0xAD };
            var stream = new DummyStream(initResponse);
            stream.Available = initResponse.Length;

            Assert.Throws<DataCorruptedException>(() =>
            {
                stream.RequestFunc3(0x11, 0x006B, 0x0003);
            });
        }

        [Test]
        public void func3_should_throw_exception_if_wrong_responsed_checksum()
        {
            var initResponse = new byte[] { 0x11, 0x03, 0x06, 0xAE, 0x41, 0x56, 0x52, 0x43, 0x40, 0x49, 0x00 };
            var stream = new DummyStream(initResponse);
            stream.Available = initResponse.Length;

            Assert.Throws<DataCorruptedException>(() =>
            {
                stream.RequestFunc3(0x11, 0x006B, 0x0003);
            });
        }

        [Test]
        public void func3_should_success_if_valid_responsed_data()
        {
            var initResponse = new byte[] { 0x11, 0x03, 0x06, 0xAE, 0x41, 0x56, 0x52, 0x43, 0x40, 0x49, 0xAD };
            var stream = new DummyStream(initResponse);
            stream.Available = initResponse.Length;

            Assert.DoesNotThrow(() =>
            {
                stream.RequestFunc3(0x11, 0x006B, 0x0003);
            });
        }
    }
}