namespace Code4Bugs.Utils.IO.Modbus
{
    public static class Checksum
    {
        public static byte[] ComputeCRC16(byte[] message, int offset, int length)
        {
            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;

            for (int i = offset; i < length; i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }

            var crc16 = new byte[2];
            crc16[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            crc16[0] = CRCLow = (byte)(CRCFull & 0xFF);
            return crc16;
        }
    }
}