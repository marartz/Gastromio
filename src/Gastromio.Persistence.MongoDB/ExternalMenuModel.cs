using System;

namespace Gastromio.Persistence.MongoDB
{
    public class ExternalMenuModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string Url { get; set; }
    }
}