using BepInEx;
using BepInEx.Logging;
using FFSNet;
using HarmonyLib;
using ScenarioRuleLibrary;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BugFixes;

[HarmonyPatch(typeof(CHeroSummonActor), "Summoner", MethodType.Getter)]
public class ReplaceSummonerGetter
{
    static bool Prefix(CHeroSummonActor __instance, ref CPlayerActor __result)
    {
        CPlayerActor m_SummonerCached = __instance.m_SummonerCached;
        if (__instance.m_SummonerCached == null)
        {
            m_SummonerCached = ScenarioManager.Scenario.AllPlayers.SingleOrDefault(s => s.ActorGuid == __instance.m_SummonerGuid);
            if (m_SummonerCached == null)
            {
                var m_HeroSummoner = ScenarioManager.Scenario.AllHeroSummons.SingleOrDefault(s => s.ActorGuid == __instance.m_SummonerGuid);
                if (m_HeroSummoner != null)
                {
                    m_SummonerCached = m_HeroSummoner.m_SummonerCached;
                }
            }
        }
        __instance.m_SummonerCached = m_SummonerCached;
        __result = m_SummonerCached;
        return false;
    }
}

[HarmonyPatch(typeof(DebugMenuProvider), "Awake")]
public class Patch_DebugMenu
{
    static bool Prefix(DebugMenuProvider __instance)
    {
        __instance.SetInstance(__instance);
        return false;
    }
}

[HarmonyPatch(typeof(PartyAdventureData), "PartySaveDir", MethodType.Getter)]
public class FixModSaveName
{
    static bool Prefix(PartyAdventureData __instance, ref string __result)
    {
        string text = Path.Combine(__instance.PartySaveRoot, __instance.PartySaveName);
        if (text.Contains("_[MOD]_"))
        {
            text = text.Replace("_[MOD]_", "_[MOD]");
        }
        __result=text;
        return false;
    }
}
