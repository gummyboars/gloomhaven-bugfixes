using HarmonyLib;

using MapRuleLibrary;
using MapRuleLibrary.Adventure;
using MapRuleLibrary.MapState;

namespace BugFixes;

// When the scenario ends in Guildmaster have to check achievements here
// This used to be true, but the code was removed, possibly to fix a different achievement bug
[HarmonyPatch(typeof(UINewAdventureResultsManager), "ShowNewAdventureModeResults")]
public class Patch_CheckNonTrophyAchievements
{
    static void Prefix(UINewAdventureResultsManager __instance)
    {
        if (AdventureState.MapState == null)
        {
            return;
        }
        // In Guildmaster we check our achievements here to make sure we show the progression in the results screen
        // and the rewards are only given after claiming the reward at the map anyway
        CQuestState questState = AdventureState.MapState?.InProgressQuestState;
        string soloID = "";
        if (questState?.Quest?.QuestCharacterRequirements?.Count > 0)
        {
            foreach (var creq in questState.Quest.QuestCharacterRequirements)
            {
                if (creq.RequiredCharacterCount == 1)
                {
                    soloID = creq.RequiredCharacterID;
                }
            }
        }
        AdventureState.MapState.SoloID = soloID;
        AdventureState.MapState.CheckNonTrophyAchievements(soloID: soloID);
    }
}
