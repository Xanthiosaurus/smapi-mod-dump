/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/Dem1se/CustomReminders
**
*************************************************/

using StardewModdingAPI;

namespace Dem1se.CustomReminders
{
    /// <summary> Mod config.json data model </summary>
    class ModConfig
    {
        public SButton CustomRemindersButton { get; set; } = SButton.F2;
        public bool SubtlerReminderSound { get; set; } = false;
        public SButton FarmhandInventoryButton { get; set; } = SButton.E;
        public bool EnableMobilePhoneApp { set; get; } = true;
    }

    /// <summary> Data model for reminders </summary>
    class ReminderModel
    {
        public ReminderModel(string reminderMessage, int daysSinceStart, int time24hrs)
        {
            ReminderMessage = reminderMessage;
            DaysSinceStart = daysSinceStart;
            Time = time24hrs;
        }
        public string ReminderMessage { get; set; }
        public int DaysSinceStart { get; set; }
        public int Time { get; set; }
    }
}