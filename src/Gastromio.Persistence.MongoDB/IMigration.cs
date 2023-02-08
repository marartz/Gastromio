using System;
using MongoDB.Driver;

namespace Gastromio.Persistence.MongoDB
{
    public interface IMigration
    {
        Version Version { get; }
        
        string Name { get; }
        
        void Up(IMongoDatabase database);

        void Down(IMongoDatabase database);
    }
}
