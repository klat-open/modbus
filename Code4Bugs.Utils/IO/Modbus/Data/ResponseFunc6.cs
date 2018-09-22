namespace Code4Bugs.Utils.IO.Modbus.Data
{
    public class ResponseFunc6 : Response
    {
        public int DataAddress { get; set; }
        public int WrittenValue { get; set; }
    }
}