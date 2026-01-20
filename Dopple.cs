using System;

using HarmonyLib;

using FFSNet;
using ScenarioRuleLibrary;

namespace BugFixes;

// Make sure actor is set
[HarmonyPatch(typeof(UIUseItemsBar), "ProxyUseItemBonus", new Type[] { typeof(GameAction) })]
public class Patch_MultiplayerDopplegangerDesynch1
{
    static bool Prefix(UIUseItemsBar __instance, GameAction action)
    {
        if (__instance.actor == null)
        {
            __instance.actor = Choreographer.s_Choreographer.FindPlayerActor(action.ActorID);
        }
        return true;
    }
}

// Find the item to use either on the current actor or it's summoner
[HarmonyPatch(typeof(UIUseItemsBar), "ProxyUseItemBonus", new Type[] { typeof(uint), typeof(ItemToken) })]
public class Patch_MultiplayerDopplegangerDesynch
{
    static bool Prefix(UIUseItemsBar __instance, uint itemNetworkID, ItemToken itemToken)
    {
        if (__instance.actor != null)
        {
            if (!__instance.isShown) // if the item bar is not shown we have to find the item
            {
                CItem citem = __instance.actor.Inventory.AllItems.Find((CItem item) => item.NetworkID == itemToken.ItemNetworkID);
                if (citem != null) // searched the actor's inventory, if all is good return
                {
                    return true;
                }
                if (__instance.actor is CHeroSummonActor heroSummonActor) // If didn't find item and the actor is a summons, check the summoner
                { 
                    __instance.actor = Choreographer.s_Choreographer.FindPlayerActor(heroSummonActor.Summoner.ID);
                    citem = __instance.actor.Inventory.AllItems.Find((CItem item) => item.NetworkID == itemToken.ItemNetworkID);
                    if (citem != null) // If summoner has item we are good
                    {
                        return true;
                    }
                }
            }
        }
        return true; // everything may not be perfect, but let the original method die if needed
    }
}
