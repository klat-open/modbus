namespace Code4Bugs.Utils.IO.Modbus.Data
{
    public class ResponseFunc16 : Response
    {
        public int DataAddress { get; set; }
        public int WrittenRegisterCount { get; set; }
    }
}