/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/demiacle/QualityOfLifeMods
**
*************************************************/

using System;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace Demiacle.ImprovedQualityOfLife {

    /// <summary>
    /// Handles the creation of the menu for AlterSpeedtime
    /// </summary>
    internal class QualityOfLifeModOptionHandler {

        QualityOfLifeModOptions modOption = new QualityOfLifeModOptions();

        public QualityOfLifeModOptionHandler() {
            ControlEvents.KeyPressed += onKeyPress;
        }

        private void onKeyPress( object sender, EventArgsKeyPressed e ) {

            if( Game1.activeClickableMenu is GameMenu ) {
                return;
            }

            if( e.KeyPressed.ToString() == ModEntry.modConfig.alterTenMinuteKey ) {

                if( Game1.activeClickableMenu == modOption ) {
                    Game1.activeClickableMenu = null;
                    
                } else {
                    modOption.xPositionOnScreen = Game1.viewport.Width / 2 - modOption.width / 2;
                    modOption.yPositionOnScreen = Game1.viewport.Height / 2 - modOption.height / 2;
                    modOption.resetPosition();
                    Game1.activeClickableMenu = modOption;
                }
            }
        }

    }
}