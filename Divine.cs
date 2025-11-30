using System;

using HarmonyLib;

using ScenarioRuleLibrary;

namespace BugFixes;

// When OnPreventDamageTriggered happens, only PreventandApplytoActiveBonusCaster if it is toggled.  Fixes Divine Intervention applying at wrong time
[HarmonyPatch(typeof(CPreventDamageActiveBonus_PreventAndApplyToActiveBonusCaster), "OnPreventDamageTriggered")]
public class Patch_DivineIntervention
{
    static bool Prefix(CPreventDamageActiveBonus_PreventAndApplyToActiveBonusCaster __instance, int damagePrevented, CActor damageSource, CActor damagedActor, CAbility damagingAbility)
    {
        int reducedDamage = (Math.Max(0, damagePrevented - __instance.m_Ability.Strength));
        if (__instance.m_ActiveBonus.IsActiveBonusToggledAndNotRestricted(damagedActor))
        {
            if (reducedDamage > 0)
            {
                GameState.RedirectedDamageToActor = new Tuple<CActor, int>(__instance.m_ActiveBonus.Caster, reducedDamage);
            }

            __instance.OnBehaviourTriggered();
        }
        return false;
    }
}
