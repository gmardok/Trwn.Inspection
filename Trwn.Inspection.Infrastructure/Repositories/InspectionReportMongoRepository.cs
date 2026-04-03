using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Trwn.Inspection.Configuration;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Infrastructure.Repositories
{
    public class InspectionReportMongoRepository : IInspectionReportRepository
    {
        private readonly IMongoCollection<InspectionReport> _collection;

        public InspectionReportMongoRepository(IOptions<AppSettings> options)
        {
            IMongoClient client;
            try
            {
                client = new MongoClient(options.Value.MongoDb.ConnectionString);
            }
            catch (Exception e)
            {
                Console.WriteLine("There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {e.Message}");
                Console.WriteLine(e);
                Console.WriteLine();
                throw;
            }

            try
            {
                _collection = client.GetDatabase(options.Value.MongoDb.DatabaseName)
                    .GetCollection<InspectionReport>("InspectionReport");
            }
            catch (Exception e)
            {
                Console.WriteLine("There was a problem accessing your " +
                    "Atlas cluster. Check that the database name is " +
                    $"valid. Message: {e.Message}");
                Console.WriteLine(e);
                Console.WriteLine();
                throw;
            }
        }

        public async Task<InspectionReport> AddInspectionReport(InspectionReport report, int userId)
        {
            report.UserId = userId;
            report.CreatedAtUtc = DateTime.UtcNow;
            await _collection.InsertOneAsync(report);
            return report;
        }

        public async Task DeleteInspectionReport(int id)
        {
            await _collection.DeleteOneAsync(r => r.Id == id);
        }

        public async Task<InspectionReport?> GetInspectionReport(int id)
        {
            return await _collection.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<InspectionReport>> GetInspectionReports()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<InspectionReport?> UpdateInspectionReport(int id, InspectionReport report, int userId)
        {
            report.Id = id;
            report.UpdatedByUserId = userId;
            report.UpdatedAtUtc = DateTime.UtcNow;
            var result = await _collection.FindOneAndReplaceAsync(
                r => r.Id == id,
                report,
                new FindOneAndReplaceOptions<InspectionReport, InspectionReport?>
                {
                    ReturnDocument = ReturnDocument.After,
                });
            return result;
        }
    }
}
