﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace PlatoWarpMenu
{
    public class PlatoWarpMenuMod : Mod
    {
        public Config config;
        internal ITranslationHelper i18n => Helper.Translation;

        internal static bool intercept = false;

        internal static IModHelper _helper;

        internal static GameLocation CurrentLocation;

        internal static Action Callback;

        public static Texture2D LastScreen = null;

        internal static PlatoWarpMenuMod instance;

        internal static Queue<GameLocation> locations = new Queue<GameLocation>();
        public override void Entry(IModHelper helper)
        {
            instance = this;
            _helper = helper;
            WarpMenu.Helper = Helper;

            config = helper.ReadConfig<Config>();

            helper.Events.Input.ButtonPressed += Input_ButtonPressed;

            helper.Events.GameLoop.GameLaunched += (s, e) => SetUpConfigMenu();

            var harmony = Harmony.HarmonyInstance.Create("Platonymous.PlatoWarpMenu");
            harmony.Patch(typeof(Image).GetMethod("Save", new Type[] { typeof(string), typeof(ImageFormat) }), prefix: new Harmony.HarmonyMethod(this.GetType().GetMethod("InterceptScreenshot",System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)));
        }

        private void StartExportSundial()
        {
            instance.config.UseTempFolder = true;

            foreach(var location in Game1.locations)
                location.characters.ToList().ForEach(c => c.IsInvisible = true);

            locations = new Queue<GameLocation>(Game1.locations.Where(l => !l.isStructure.Value && !l.IsFarm && !l.IsGreenhouse));
            ExportSundial();
        }

        public static void ExportSundial()
        {
            if (locations.Count > 0)
            {
                instance.config.UseTempFolder = true;
                GameLocation loc = locations.Dequeue();
                instance.Monitor.Log(loc.Name);
                PlatoWarpMenuMod.GetLocationShotSundial(loc, ExportSundial);
            }
        }

        public static void GetLocationShot(GameLocation location, Action callback)
        {
            CurrentLocation = location;
            Callback = callback;
            PyTK.PyUtils.setDelayedAction(1, () => _helper.Events.Display.RenderedActiveMenu += Display_Rendered);
        }

        public static void GetLocationShotSundial(GameLocation location, Action callback)
        {
            if (location == null)
                return;

            CurrentLocation = location;
            Callback = callback;
            PyTK.PyUtils.setDelayedAction(1, () => _helper.Events.Display.RenderedWorld += Display_RenderedWorld);
        }

        private static void Display_RenderedWorld(object sender, RenderedWorldEventArgs e)
        {
            _helper.Events.Display.RenderedWorld -= Display_RenderedWorld;
            intercept = true;
            var g = Game1.currentLocation;
            var priorColor = Game1.ambientLight;
            PyTK.PyUtils.setDelayedAction(2, () => Game1.ambientLight = priorColor);
            Game1.ambientLight = Microsoft.Xna.Framework.Color.White;
            Game1.currentLocation = CurrentLocation;
            try
            {
                Game1.spriteBatch.End();
                Game1.game1.takeMapScreenshot(0.25f, CurrentLocation.isStructure.Value ? CurrentLocation.uniqueName.Value : CurrentLocation.Name);
                Game1.spriteBatch.Begin();
            }
            catch
            {

            }
            Game1.currentLocation = g;
            intercept = false;
            Callback?.Invoke();
        }

        public static void Display_Rendered(object sender, RenderedActiveMenuEventArgs e)
        {
            intercept = true;
            var g = Game1.currentLocation;
            var priorColor = Game1.ambientLight;
            PyTK.PyUtils.setDelayedAction(2, () => Game1.ambientLight = priorColor);
            Game1.ambientLight = Microsoft.Xna.Framework.Color.White;
            Game1.currentLocation = CurrentLocation;
            try
            {
                Game1.spriteBatch.End();
                Game1.game1.takeMapScreenshot(0.25f, CurrentLocation.isStructure.Value ? CurrentLocation.uniqueName.Value : CurrentLocation.Name);
                Game1.spriteBatch.Begin();
            }
            catch
            {

            }
            Game1.currentLocation = g;
            _helper.Events.Display.RenderedActiveMenu -= Display_Rendered;
            intercept = false;
            Callback?.Invoke();
        }

        public static bool InterceptScreenshot(Image __instance, ref string filename)
        {
            if (!intercept)
                return true;

            if (!Directory.Exists(Path.Combine(_helper.DirectoryPath, "Temp")))
                Directory.CreateDirectory(Path.Combine(_helper.DirectoryPath, "Temp"));

            filename = Path.Combine(_helper.DirectoryPath, "Temp", Path.GetFileName(filename));

            if (instance.config.UseTempFolder)
                return true;

            using (var mem = new MemoryStream())
            {
                (__instance as Bitmap).Save(mem, ImageFormat.Png);
                LastScreen = Texture2D.FromStream(Game1.graphics.GraphicsDevice, mem);
            }

            return false;
        }

        private void Input_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            e.Button.TryGetKeyboard(out Keys keyPressed);

            if (e.Button == SButton.NumPad8)
                StartExportSundial();

            if (e.Button != config.MenuButton)
                return;

            if(Context.IsWorldReady)
                WarpMenu.Open();
        }

        private void SetUpConfigMenu()
        {
            if (!Helper.ModRegistry.IsLoaded("spacechase0.GenericModConfigMenu"))
                return;

            var api = Helper.ModRegistry.GetApi<IGMCMAPI>("spacechase0.GenericModConfigMenu");


            api.RegisterModConfig(ModManifest, () =>
            {
                config.MenuButton = SButton.J;
                config.UseTempFolder = false;
            }, () =>
            {
                Helper.WriteConfig<Config>(config);
            });

            var fonts = new string[] { "vanilla", "opensans", "escrita" };

            api.RegisterLabel(ModManifest, ModManifest.Name, ModManifest.Description);
            api.RegisterSimpleOption(ModManifest, i18n.Get("MenuButton"), "", () => config.MenuButton, (SButton b) => config.MenuButton = b);
            api.RegisterSimpleOption(ModManifest, i18n.Get("UseTempFolder"), "", () => config.UseTempFolder, (bool b) => config.UseTempFolder = b);
            if (LocalizedContentManager.CurrentLanguageLatin)
            {
                api.RegisterChoiceOption(ModManifest, i18n.Get("MenuFont1"), "", () => config.MenuFont1, (string m) => config.MenuFont1 = m == "vanilla" ? "" : m, fonts);
                api.RegisterChoiceOption(ModManifest, i18n.Get("MenuFont2"), "", () => config.MenuFont2, (string m) => config.MenuFont2 = m == "vanilla" ? "" : m, fonts);
            }
        }
    }
}
