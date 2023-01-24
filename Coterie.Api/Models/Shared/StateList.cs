using System;
using System.Collections.Generic;
using System.Linq;

namespace Coterie.Api.Models.Shared
{
    public static class StateList
    {
        private static IDictionary<string, string> StateNamesDictionary =>
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Texas", "TX" },
                { "Ohio", "OH" },
                { "Florida", "FL"  }
            };

        public static States? GetState(string stateName)
        {
            if (StateNamesDictionary.TryGetValue(stateName, out var keyMatch))
            {
                return Enum.Parse<States>(keyMatch);
            }

            var valueMatch = StateNamesDictionary.Values.FirstOrDefault(x => x.Equals(stateName, StringComparison.OrdinalIgnoreCase));

            return !string.IsNullOrEmpty(valueMatch) ? Enum.Parse<States>(valueMatch, true) : null;
        }
    }
}