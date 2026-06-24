using PrintProxy.Hub.Data;

namespace PrintProxy.Hub.Services.Files
{
    public class PrinterfileService : IPrinterfileService
    {

        private readonly string? _printerFilePath;

        public PrinterfileService(IConfiguration configuration) 
        {
            _printerFilePath = configuration.GetValue<string>("GCODE_FILE_DIR","GcodeFiles");

            if (!Directory.Exists(_printerFilePath))
            {
                Directory.CreateDirectory(_printerFilePath);
            }
        }

        public List<PrinterFileInfo> GetFiles()
        {
            if (_printerFilePath == null)
            {
                return null!;
            }

            return Directory.GetFiles(_printerFilePath)
                            .Where(fp => !fp.EndsWith(".bmp"))
                            .Select(fp => new PrinterFileInfo(new FileInfo(fp)))
                            .ToList();
        }

        public async Task SaveFileAsync(FileStream stream,string filename)
        {
            byte[] data = new byte[stream.Length];

            await stream.ReadExactlyAsync(data, 0, data.Length);

            File.WriteAllBytes(filename, data);
        }

        public async Task DeleteFileAsync(string filename)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            if (File.Exists(filename + ".bmp"))
            {
                File.Delete(filename + ".bmp");
            }
        }
    }


}
