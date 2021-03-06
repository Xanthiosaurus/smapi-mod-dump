/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/Igorious/StardevValleyNewMachinesMod
**
*************************************************/

using System.Collections.Generic;

namespace Igorious.StardewValley.DynamicAPI.Extensions
{
    public static class EnumerableExtensions
    {
        public static string Serialize<T>(this IEnumerable<T> items)
        {
            return string.Join(" ", items);
        }
    }
}
