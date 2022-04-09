using BepInEx;
using BepInEx.Bootstrap;
using FasterLoading.Patches;
using HarmonyLib;

namespace FasterLoading {
    [BepInPlugin(ModGuid, ModName, ModVersion)]
    public class Plugin : BaseUnityPlugin {
        public const string ModName = "FasterLoading";
        public const string ModGuid = "com.maxsch.valheim.FasterLoading";
        public const string ModVersion = "0.0.0";

        public static Plugin Instance { get; private set; }
        private Harmony harmony;

        private void Awake() {
            Instance = this;

            Log.Init(Logger);
            harmony = new Harmony(ModGuid);
        }

        private void Start() {
            bool vpo = Chainloader.PluginInfos.ContainsKey("dev.ontrigger.vpo");

            Log.LogDebug($"VPO active: {vpo}");

            if (!vpo) {
                harmony.PatchAll(typeof(MinimapGenerationPatch));
                harmony.PatchAll(typeof(ThreadedWorldLoadingPatch));
            }
        }
    }
}
