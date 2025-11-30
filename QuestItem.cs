using System;

using HarmonyLib;

using ScenarioRuleLibrary;

namespace BugFixes;

// IsItemInteracable tries to access this.actor, so this.actor has to be set first otherwise crashes with null error
[HarmonyPatch(typeof(UIUseItemsBar), "ShowUsableItems")]
public class Patch_MultiplayerActionSelectionCrash
{
    static void Prefix(UIUseItemsBar __instance, CActor inventoryOwner)
    {
        __instance.actor = inventoryOwner;
    }
}
