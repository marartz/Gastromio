using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class ExternalMenus : IReadOnlyCollection<ExternalMenu>
    {
        private readonly IReadOnlyDictionary<ExternalMenuId, ExternalMenu> externalMenuDict;

        public ExternalMenus(IEnumerable<ExternalMenu> externalMenus)
        {
            var tempDict = new Dictionary<ExternalMenuId, ExternalMenu>();
            foreach (var externalMenu in externalMenus)
            {
                if (tempDict.ContainsKey(externalMenu.Id))
                    throw new InvalidOperationException("external menu is already registered");
                tempDict.Add(externalMenu.Id, externalMenu);
            }

            externalMenuDict = new ReadOnlyDictionary<ExternalMenuId, ExternalMenu>(tempDict);
        }

        public bool TryGetExternalMenu(ExternalMenuId id, out ExternalMenu externalMenu)
        {
            return externalMenuDict.TryGetValue(id, out externalMenu);
        }

        public ExternalMenus AddOrReplace(ExternalMenu externalMenu)
        {
            var newExternalMenus = externalMenuDict
                .Where(en => en.Key != externalMenu.Id)
                .Select(en => en.Value)
                .Append(externalMenu);
            return new ExternalMenus(newExternalMenus);
        }

        public ExternalMenus Remove(ExternalMenuId externalMenuId)
        {
            if (!TryGetExternalMenu(externalMenuId, out _))
                throw DomainException.CreateFrom(new ExternalMenuDoesNotExistFailure());
            var newExternalMenus = externalMenuDict
                .Where(en => en.Key != externalMenuId)
                .Select(en => en.Value);
            return new ExternalMenus(newExternalMenus);
        }

        public IEnumerator<ExternalMenu> GetEnumerator()
        {
            return externalMenuDict
                .OrderBy(en => en.Value.Name)
                .Select(en => en.Value)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return externalMenuDict.Count; }
        }
    }
}
