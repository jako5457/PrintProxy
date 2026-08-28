namespace PrintProxy.Hub.Data.Entities
{
    public class Tag
    {

        public int TagId { get; set; }

        public string TagName { get; set; } = string.Empty;

        public bool IsSystemTag { get; set; } = false;

        public List<PrinterFile> PrinterFiles { get; set; } = null!;

        public List<Printer> Printers { get; set; } = null!;

    }
}
