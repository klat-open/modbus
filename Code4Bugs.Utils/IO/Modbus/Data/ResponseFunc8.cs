namespace Code4Bugs.Utils.IO.Modbus.Data
{
    public class ResponseFunc8 : Response
    {
        public int SubFunction { get; set; }
        public int Data { get; set; }
    }
}