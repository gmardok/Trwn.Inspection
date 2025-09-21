using System.Text.Json;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Mobile.Services
{
    public class PersistanceService
    {
        private const string FileSufix = ".report.txt";

        private string _fileName;

        public PersistanceService()
        {
            _fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}{FileSufix}";
        }

        public string Save(InspectionReport report)
        {
            var data = JsonSerializer.Serialize(report);
            File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, _fileName), data);
            return _fileName;
        }

        public InspectionReport Load(string fileName)
        {
            _fileName = fileName.EndsWith(FileSufix, StringComparison.InvariantCultureIgnoreCase) ? fileName : $"{fileName}{FileSufix}";

            fileName = Path.Combine(FileSystem.AppDataDirectory, _fileName);

            if (!File.Exists(fileName))
                throw new FileNotFoundException("Unable to find file on local storage.", fileName);

            var data = File.ReadAllText(fileName);
            var result = JsonSerializer.Deserialize<InspectionReport>(data);

            if (result == null)
            {
                File.Delete(fileName);
                throw new InvalidOperationException("Unable to deserialize report data.");
            }

            return result;
        }

        public List<string> GetLocalReports()
        {
            var files = Directory.GetFiles(FileSystem.AppDataDirectory, $"*{FileSufix}");
            return files
                .OrderByDescending(f => new FileInfo(f).LastWriteTime)
                .Select(f => Path.GetFileName(f).TrimEnd(FileSufix.ToCharArray()))
                .ToList();
        }
    }
}
