using System;
using System.Diagnostics;
using System.Threading;

namespace Code4Bugs.Utils.IO.Modbus
{
    public static partial class Modbus
    {
        private static byte[] BuildFunc16Message(int slaveAddress, int dataAddress, byte[] writeValue)
        {
            //Function 16 request is always 9+ bytes
            var message = new byte[9 + writeValue.Length];
            var registerCount = writeValue.Length / 2;

            message[0] = (byte)slaveAddress;

            message[1] = 16;

            message[2] = (byte)(dataAddress >> 8);
            message[3] = (byte)dataAddress;

            message[4] = (byte)(registerCount >> 8);
            message[5] = (byte)registerCount;

            message[6] = (byte)(registerCount * 2);

            Array.Copy(writeValue, 0, message, 7, registerCount * 2);

            var crc16 = ComputeChecksum(message);
            message[message.Length - 2] = crc16[0];
            message[message.Length - 1] = crc16[1];

            return message;
        }

        private static byte[] BuildFunc6Message(int slaveAddress, int dataAddress, int writeValue)
        {
            return BuildFunc3And4Message(slaveAddress, 6, dataAddress, writeValue);
        }

        private static byte[] BuildFunc3And4Message(int slaveAddress, int funcCode, int dataAddress, int registerCount)
        {
            var message = new byte[8];

            message[0] = (byte)slaveAddress;
            message[1] = (byte)funcCode;
            message[2] = (byte)(dataAddress >> 8);
            message[3] = (byte)dataAddress;
            message[4] = (byte)(registerCount >> 8);
            message[5] = (byte)registerCount;

            var crc16 = ComputeChecksum(message);
            message[message.Length - 2] = crc16[0];
            message[message.Length - 1] = crc16[1];

            return message;
        }

        private static byte[] ComputeChecksum(byte[] message)
        {
            return Checksum.ComputeCRC16(message, 0, message.Length - 2);
        }

        private static void VerifyEmptyResponse(byte[] response)
        {
            if (response == null || response.Length == 0)
                throw new EmptyResponsedException();
        }

        private static void VerifyChecksum(byte[] response)
        {
            var crc16 = ComputeChecksum(response);
            if (crc16[0] != response[response.Length - 2] || crc16[1] != response[response.Length - 1])
                throw new DataCorruptedException("checksum failed.");
        }

        private static void VerifyGeneralStructure(byte[] response, int slaveAddress, int funcCode)
        {
            if (response[0] != slaveAddress)
                throw new DataCorruptedException($"Wrong response slave address. Expected {slaveAddress}, actual {response[0]}");

            if (response[1] != funcCode)
                throw new DataCorruptedException($"Wrong response function code. Expected {funcCode}, actual {response[1]}");
        }

        private static byte[] ReceiveMessage(ICommStream stream, int length)
        {
            var timeout = stream.ReadTimeout;

            if (!WaitUntilAvailable(stream, timeout))
                return new byte[] { };

            var stopwatch = Stopwatch.StartNew();
            var totalReceivedBytes = 0;
            var response = new byte[length];

            do
            {
                var receivedBytes = stream.Read(response, totalReceivedBytes, response.Length - totalReceivedBytes);
                totalReceivedBytes += receivedBytes;

                if (totalReceivedBytes >= response.Length)
                    break;

                while ((stream.Available < 1) && (timeout > stopwatch.ElapsedMilliseconds))
                {
                    Thread.Sleep(1);
                }
            } while (timeout > stopwatch.ElapsedMilliseconds);

            if (totalReceivedBytes == response.Length)
                return response;
            else
                throw new MissingDataException($"Required {response.Length} bytes but received {totalReceivedBytes} bytes.");
        }

        private static bool WaitUntilAvailable(ICommStream stream, int timeout)
        {
            var stopwatch = Stopwatch.StartNew();
            while ((stream.Available < 1) && (timeout > stopwatch.ElapsedMilliseconds))
            {
                Thread.Sleep(50);
            }
            return stream.Available > 0;
        }
    }
}