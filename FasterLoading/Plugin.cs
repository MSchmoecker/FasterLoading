using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using FasterLoading.Patches;
using HarmonyLib;

namespace FasterLoading {
    [BepInPlugin(ModGuid, ModName, ModVersion)]
    [BepInIncompatibility("dev.ontrigger.vpo")]
    public class Plugin : BaseUnityPlugin {
        public const string ModName = "FasterLoading";
        public const string ModGuid = "com.maxsch.valheim.FasterLoading";
        public const string ModVersion = "0.0.0";

        public static Plugin Instance { get; private set; }
        private Harmony harmony;

        private ConfigEntry<bool> PatchMinimap { get; set; }
        private ConfigEntry<bool> PatchWorldGenThreading { get; set; }

        private void Awake() {
            Instance = this;

            Log.Init(Logger);
            harmony = new Harmony(ModGuid);

            PatchMinimap = Config.Bind("Patches", "Minimap Caching", true, "Writes the minimap texture to disk and reuse it on subsequent world loads. This significantly speeds up loading times. Requires a restart to take effect.");
            PatchWorldGenThreading = Config.Bind("Patches", "Load World Threaded", true, "Patches the world generation to load partially threaded. This speeds up loading times a bit on small bases and more on large bases. Requires a restart to take effect.");

            if (PatchMinimap.Value) {
                harmony.PatchAll(typeof(MinimapGenerationPatch));
            }

            if (PatchWorldGenThreading.Value) {
                harmony.PatchAll(typeof(ThreadedWorldLoadingPatch));
            }
        }
    }
}
