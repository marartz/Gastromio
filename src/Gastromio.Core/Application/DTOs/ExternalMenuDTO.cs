using System;

namespace Gastromio.Core.Application.DTOs
{
    public class ExternalMenuDTO
    {
        public ExternalMenuDTO(Guid id, string name, string description, string url)
        {
            Id = id;
            Name = name;
            Description = description;
            Url = url;
        }

        public Guid Id { get; }
        
        public string Name { get; }

        public string Description { get; }
        
        public string Url { get; }
    }
}