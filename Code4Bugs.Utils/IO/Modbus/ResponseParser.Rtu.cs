using Code4Bugs.Utils.IO.Modbus.Data;
using Code4Bugs.Utils.Types;

namespace Code4Bugs.Utils.IO.Modbus
{
    public static partial class ResponseParser
    {
        public static ResponseFunc3 ToResponseFunc3(this byte[] response)
        {
            var data = new ResponseFunc3
            {
                SlaveId = response[0],
                Data = response.CopyRange(3, response[2])
            };
            return data;
        }

        public static ResponseFunc4 ToResponseFunc4(this byte[] response)
        {
            var data = new ResponseFunc4
            {
                SlaveId = response[0],
                Data = response.CopyRange(3, response[2])
            };
            return data;
        }

        public static ResponseFunc6 ToResponseFunc6(this byte[] response)
        {
            var data = new ResponseFunc6
            {
                SlaveId = response[0],
                DataAddress = response.CopyRange(2, 2).ToInt16(),
                WrittenValue = response.CopyRange(4, 2).ToInt16()
            };
            return data;
        }

        public static ResponseFunc8 ToResponseFunc8(this byte[] response)
        {
            var data = new ResponseFunc8
            {
                SlaveId = response[0],
                SubFunction = response.CopyRange(2, 2).ToInt16(),
                Data = response.CopyRange(4, 2).ToInt16()
            };
            return data;
        }

        public static ResponseFunc16 ToResponseFunc16(this byte[] response)
        {
            var data = new ResponseFunc16
            {
                SlaveId = response[0],
                DataAddress = response.CopyRange(2, 2).ToInt16(),
                WrittenRegisterCount = response.CopyRange(4, 2).ToInt16()
            };
            return data;
        }
    }
}