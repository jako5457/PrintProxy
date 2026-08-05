using System.IO.Compression;
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

        public async Task ExtractGcodeThumbnailAsync(string filepath)
        {
            var filedata = File.ReadAllLines(_FileInfo.FullName).Where(l => l.StartsWith("; ")).ToList();

            bool started = false;

            List<string> imageData = new();

            foreach (var item in filedata)
            {
                if (item.StartsWith("; thumbnail end"))
                {
                    break;
                }

                if (started)
                {
                    imageData.Add(item);
                }

                if (item.StartsWith("; thumbnail begin"))
                {
                    started = true;
                }

            }

            imageData = imageData.Select(d => d.Replace("; ", "")).ToList();

            string base64image = string.Join(string.Empty, imageData);

            filedata.Clear();
            imageData.Clear();

            byte[] Data = Convert.FromBase64String(base64image);

            File.WriteAllBytes(Path.Combine(filepath,_FileInfo.Name) + ".bmp", Data);
        }

        public async Task Extract3mfThumbnailAsync(string filePath)
        {
            ZipArchive arch = ZipFile.OpenRead(FileInfo.FullName);

            int entryId = 1;

            for (int i = 0; i < 15; i++)
            {
                ZipArchiveEntry? GcodeEntry = arch.GetEntry(Path.Combine("Metadata", $"plate_{i}.gcode"));

                if (GcodeEntry != null)
                {
                    entryId = i;
                    break;
                }
            }

            ZipArchiveEntry? entry = arch.GetEntry(Path.Combine("Metadata",$"plate_{entryId}.png"));

            if (entry != null)
            {
                entry.ExtractToFile(Path.Combine(filePath, _FileInfo.Name) + ".png");
            }
        }

        public async Task<string> GetThumbnailAsync(string filepath)
        {

            if (FileInfo.Extension.Contains("gcode"))
            {
                if (!File.Exists(Path.Combine(filepath, _FileInfo.Name) + ".bmp"))
                {
                    await ExtractGcodeThumbnailAsync(filepath);
                }

                var data = File.ReadAllBytes(Path.Combine(filepath, _FileInfo.Name) + ".bmp");

                string base64 = Convert.ToBase64String(data);

                return $"data:image/bmp;base64,{base64}";
            }
            else if (FileInfo.Extension.Contains("3mf"))
            {
                if (!File.Exists(Path.Combine(filepath, _FileInfo.Name) + ".png"))
                {
                    await Extract3mfThumbnailAsync(filepath);
                }

                var data = File.ReadAllBytes(Path.Combine(filepath, _FileInfo.Name) + ".png");

                string base64 = Convert.ToBase64String(data);

                return $"data:image/png;base64,{base64}";
            }

            return string.Empty;
        }

    }
}
