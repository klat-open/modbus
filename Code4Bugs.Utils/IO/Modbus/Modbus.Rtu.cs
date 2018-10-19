using Code4Bugs.Utils.Types;

namespace Code4Bugs.Utils.IO.Modbus
{
    public static partial class Modbus
    {
        public static byte[] RequestFunc3(this ICommStream stream, int slaveId, int dataAddress, int registerCount)
        {
            var message = BuildFunc3And4Message(slaveId, 3, dataAddress, registerCount);
            lock (stream)
            {
                stream.Write(message, 0, message.Length);

                var responseLength = 5 + 2 * registerCount;
                var response = ReceiveMessage(stream, responseLength);

                VerifyEmptyResponse(response);
                VerifyChecksum(response);
                VerifyGeneralStructure(response, slaveId, 3);

                return response;
            }
        }

        public static byte[] RequestFunc4(this ICommStream stream, int slaveId, int dataAddress, int registerCount)
        {
            var message = BuildFunc3And4Message(slaveId, 4, dataAddress, registerCount);
            lock (stream)
            {
                stream.Write(message, 0, message.Length);

                var responseLength = 5 + 2 * registerCount;
                var response = ReceiveMessage(stream, responseLength);

                VerifyEmptyResponse(response);
                VerifyChecksum(response);
                VerifyGeneralStructure(response, slaveId, 4);

                return response;
            }
        }

        public static byte[] RequestFunc6(this ICommStream stream, int slaveId, int dataAddress, int writeValue)
        {
            var message = BuildFunc6Message(slaveId, dataAddress, writeValue);
            lock (stream)
            {
                stream.Write(message, 0, message.Length);

                var response = ReceiveMessage(stream, 8);

                VerifyEmptyResponse(response);
                // Response of the func6 is always an echo.
                if (!response.AreEqual(message))
                    throw new DataCorruptedException("Response of function 6 should be an echo.");

                return response;
            }
        }

        public static byte[] RequestFunc8(this ICommStream stream, int slaveId, int subFunction, int data)
        {
            var message = BuildFunc8Message(slaveId, subFunction, data);
            lock (stream)
            {
                stream.Write(message, 0, message.Length);

                var response = ReceiveMessage(stream, 8);

                VerifyEmptyResponse(response);
                VerifyChecksum(response);
                VerifyGeneralStructure(response, slaveId, 8);

                return response;
            }
        }

        public static byte[] RequestFunc16(this ICommStream stream, int slaveId, int dataAddress, byte[] writeValue)
        {
            var message = BuildFunc16Message(slaveId, dataAddress, writeValue);
            lock (stream)
            {
                stream.Write(message, 0, message.Length);

                var response = ReceiveMessage(stream, 8);

                VerifyEmptyResponse(response);
                VerifyChecksum(response);
                VerifyGeneralStructure(response, slaveId, 16);

                return response;
            }
        }
    }
}