using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class ExternalMenu
    {
        public ExternalMenu(ExternalMenuId id, string name, string description, string url)
        {
            ValidateName(name);
            ValidateDescription(description);
            ValidateUrl(url);

            Id = id;
            Name = name;
            Description = description;
            Url = url;
        }

        public ExternalMenuId Id { get; }

        public string Name { get; }

        public string Description { get; }

        public string Url { get; }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw DomainException.CreateFrom(new ExternalMenuHasNoNameFailure());
        }

        private static void ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw DomainException.CreateFrom(new ExternalMenuHasNoDescriptionFailure());

        }

        private static void ValidateUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw DomainException.CreateFrom(new ExternalMenuHasNoUrlFailure());
        }
    }
}
