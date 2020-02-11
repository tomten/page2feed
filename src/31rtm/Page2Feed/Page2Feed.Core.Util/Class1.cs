using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32.SafeHandles;

namespace Page2Feed.Core.Util
{

    public static class HexUtil
    {

        public static string Hex(this IEnumerable<byte> bytes)
        {
            var s = BitConverter.ToString(bytes.ToArray());
            var replace = s.Replace("-", "", StringComparison.InvariantCulture);
            var lowerInvariant = replace.ToLowerInvariant();
            return lowerInvariant;
        }

    }

}
