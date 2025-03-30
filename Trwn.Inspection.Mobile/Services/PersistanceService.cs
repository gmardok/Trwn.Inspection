using System.Text.Json;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Mobile.Services
{
    public class PersistanceService
    {
        private string _fileName;

        public PersistanceService()
        {
            _fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}.report.txt";
        }
        public Task Save(InspectionReport report)
        {
            var data = JsonSerializer.Serialize(report);
            return File.WriteAllTextAsync(System.IO.Path.Combine(FileSystem.AppDataDirectory, _fileName), data);
        }

        public async Task<InspectionReport> Load(string filename)
        {
            filename = Path.Combine(FileSystem.AppDataDirectory, filename);

            if (!File.Exists(filename))
                throw new FileNotFoundException("Unable to find file on local storage.", filename);

            var data = await File.ReadAllTextAsync(filename);
            var result = JsonSerializer.Deserialize<InspectionReport>(data);

            if (result == null)
            {
                File.Delete(filename);
                throw new InvalidOperationException("Unable to deserialize report data.");
            }

            return result;
        }
    }
}
