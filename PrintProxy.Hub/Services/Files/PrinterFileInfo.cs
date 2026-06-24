using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace PrintProxy.Hub.Services.Files
{
    public class PrinterFileInfo 
    {


        private FileInfo _FileInfo;

        public PrinterFileInfo(FileInfo info)
        {
            _FileInfo = info;
        }

        public FileInfo FileInfo { get => _FileInfo; }

        public async Task ExtractGcodeThumbnailAsync()
        {
            var filedata = File.ReadAllLines(_FileInfo.FullName).Where(l => l.StartsWith("; ")).ToList();

            bool started = false;

            List<string> imageData = new();

            foreach (var item in filedata)
            {
                if (item.StartsWith("; thumbnail begin"))
                {
                    started = true;
                }

                if (started)
                {
                    imageData.Add(item);
                }
            }

            imageData = imageData.Select(d => d.Replace("; ", "")).ToList();

            string base64image = string.Join(string.Empty, imageData);

            filedata.Clear();
            imageData.Clear();

            byte[] Data = Convert.FromBase64String(base64image);

            File.WriteAllBytes(_FileInfo.Name + ".bmp", Data);
        }

        public async Task<string> GetThumbnailAsync()
        {
            if (!File.Exists(_FileInfo.Name + ".bmp"))
            {
                await ExtractGcodeThumbnailAsync();
            }

            var data = File.ReadAllBytes(_FileInfo.Name + ".bmp");

            string base64 = Convert.ToBase64String(data);

            return $"data:image/bmp;base64,{base64}";
        }

    }
}
