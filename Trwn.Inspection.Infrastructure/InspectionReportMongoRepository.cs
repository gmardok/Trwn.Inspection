﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Trwn.Inspection.Configuration;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Infrastructure
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

        public async Task<PhotoDocumentation?> AddInspectionFoto(string id, PhotoDocumentation photoDocumentation)
        {
            await _collection.UpdateOneAsync(r => r.Id == id, Builders<InspectionReport>.Update.Push(r => r.PhotoDocumentation, photoDocumentation));
            return photoDocumentation;
        }

        public async Task<InspectionReport> AddInspectionReport(InspectionReport report)
        {
            report.Id = report.Id ?? Guid.NewGuid().ToString();
            await _collection.InsertOneAsync(report);
            return report;
        }

        public async Task DeleteInspectionFoto(string id, int fotoCode)
        {
            await _collection.UpdateOneAsync(r => r.Id == id, 
                Builders<InspectionReport>.Update.PullFilter(r => r.PhotoDocumentation, f => f.Code == fotoCode));
        }

        public async Task DeleteInspectionReport(string id)
        {
            await _collection.DeleteOneAsync(r => r.Id == id);
        }

        public async Task<List<PhotoDocumentation>> GetAllInspectionFoto(string id)
        {
            var report = await _collection.Find(r => r.Id == id).FirstOrDefaultAsync();
            return report?.PhotoDocumentation ?? new List<PhotoDocumentation>();
        }

        public async Task<PhotoDocumentation?> GetInspectionFoto(string id, int fotoCode)
        {
            var report = await _collection.Find(r => r.Id == id).FirstOrDefaultAsync();
            return report?.PhotoDocumentation.FirstOrDefault(f => f.Code == fotoCode);
        }

        public async Task<InspectionReport?> GetInspectionReport(string id)
        {
            return await _collection.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<InspectionReport>> GetInspectionReports()
        {
            return await _collection.Find(Builders<InspectionReport>.Filter.Empty).ToListAsync();
        }

        public async Task<InspectionReport?> UpdateInspectionReport(string id, InspectionReport report)
        {
            var result = await _collection.FindOneAndUpdateAsync(r => r.Id == id, 
                Builders<InspectionReport>.Update.Set(r => r, report),
                new FindOneAndUpdateOptions<InspectionReport, InspectionReport?> { ReturnDocument = ReturnDocument.After});
            return result;
        }
    }
}
