/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/Dawilly/MTN2
**
*************************************************/

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTN2.MapData
{
    public struct Interaction
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Interaction(int X, int Y) {
            this.X = X;
            this.Y = Y;
        }

        public Point ToPoint() {
            return new Point(X, Y);
        }
    }
}
