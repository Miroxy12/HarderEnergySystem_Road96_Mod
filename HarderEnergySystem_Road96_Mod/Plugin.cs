using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BlueEyes.Interactions;
using BlueEyes.NarrativeBrain;
using HarmonyLib;
using UnityEngine;

namespace HarderEnergySystem_Road96_Mod
{
    [BepInEx.BepInPlugin(mod_guid, "Harder Energy System", version)]
    [BepInEx.BepInProcess("Road 96.exe")]
    public class HarderEnergySystem : BasePlugin
    {
        private const string mod_guid = "miroxy12.harderenergysystem";
        private const string version = "1.0";
        private readonly Harmony harmony = new Harmony(mod_guid);
        internal static new ManualLogSource Log;
        public static bool lostalready = false;

        public override void Load()
        {
            Log = base.Log;
            Log.LogInfo(mod_guid + " started, version: " + version);
            harmony.PatchAll(typeof(ImpactEnergyHook));
            harmony.PatchAll(typeof(AddSpriteConstraintUIHook));
            AddComponent<ModMain>();
        }
    }

    public class ModMain : MonoBehaviour
    {
        void Awake()
        {
            HarderEnergySystem.Log.LogInfo("loading Harder Energy System");
        }
        void OnEnable()
        {
            HarderEnergySystem.Log.LogInfo("enabled Harder Energy System");
        }
    }

    [HarmonyPatch(typeof(NarrativeContext), "ImpactEnergy", new System.Type[] { typeof(float), typeof(NarrativeSettings), typeof(bool) })]
    public class ImpactEnergyHook
    {
        static void Prefix(NarrativeContext __instance, float energyImpact, NarrativeSettings settings, bool sendEvent)
        {
            if (energyImpact < 0) {
                __instance._currentEnergy = __instance._currentEnergy - 0.1f;
            }
        }
    }

    [HarmonyPatch(typeof(InteractionUI), "AddSpriteConstraintUI", new System.Type[] { typeof(UnityEngine.Sprite), typeof(bool) })]
    public class AddSpriteConstraintUIHook
    {
        static void Prefix(InteractionUI __instance, UnityEngine.Sprite sprite, bool showOptionnalFeedback)
        {
            if (sprite.name.ToLower().Contains("energycost")) {
                __instance.AddTextConstraintUI("-1 Energy (HarderEnergySystem)", true);
            }
        }
    }
}