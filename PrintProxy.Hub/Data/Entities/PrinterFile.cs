namespace PrintProxy.Hub.Data.Entities
{
    public class PrinterFile
    {

        public int PrinterFileId { get; set; }

        public string PrinterFileName { get; set; }

        public List<Tag> Tags { get; set; } = null;
    }
}
