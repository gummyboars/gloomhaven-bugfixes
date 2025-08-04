using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HarmonyLib.Tools;

using FFSNet;
using ScenarioRuleLibrary;

namespace BugFixes;

[BepInPlugin(pluginGUID, pluginName, pluginVersion)]
public class BugFixesPlugin : BaseUnityPlugin
{
    const string pluginGUID = "com.gummyboars.gloomhaven.bugfixes";
    const string pluginName = "Bug Fixes";
    const string pluginVersion = "1.0.0";

    private Harmony HarmonyInstance = null;

    public static ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource(pluginName);

    private void Awake()
    {
        BugFixesPlugin.logger.LogInfo($"Loading plugin {pluginName}.");
        try
        {
            HarmonyInstance = new Harmony(pluginGUID);
            Assembly assembly = Assembly.GetExecutingAssembly();
            HarmonyInstance.PatchAll(assembly);
            var majorMinorVersion = pluginVersion.Substring(0, pluginVersion.LastIndexOf('.'));
            NetworkVersion.Current = NetworkVersion.Current + "B" + majorMinorVersion;
            BugFixesPlugin.logger.LogInfo($"Plugin {pluginName} version {majorMinorVersion} loaded.");
        }
        catch (Exception e)
        {
            BugFixesPlugin.logger.LogError($"Could not load plugin {pluginName}: {e}");
        }
    }
}

// An extra turn of type TopAction or BottomAction will disable one half of all cards
// in the player's hand. Before SetPhase() is called again, we set both cards to
// interactable and allow SetPhase() to fix their interactability state when called.
[HarmonyPatch(typeof(CardsActionControlller), "RestorePhase")]
public static class Patch_RestorePhase
{
    private static void Prefix(CardsActionControlller.Phase ___CachedPhase, FullAbilityCard ___cachedTopCard, FullAbilityCard ___cachedBottomCard, ref CAbilityExtraTurn.EExtraTurnType ___extraTurnType)
    {
        if (___CachedPhase != CardsActionControlller.Phase.None)
        {
            ___cachedTopCard?.SetInteractable(true);
            ___cachedBottomCard?.SetInteractable(true);
            ___extraTurnType = CardsHandManager.Instance.CurrentHand.PlayerActor.TakingExtraTurnOfType;
        }
    }
}

// This function creates an unnecessary extra ExtraTurnType on the player's ExtraTurnOFTypeStack
// When we detect that this has happened, we will remove the extra ExtraTurnType from the stack.
// We only remove this in the case that the top item is CurrentAction and the second item is
// either TopAction or BottomAction.
[HarmonyPatch(typeof(GameState), "StartActorExtraTurnImmediately")]
public static class Patch_StartActorExtraTurnImmediately
{
    private static void Postfix(CPlayerActor playerActor)
    {
        if (playerActor.TakingExtraTurnOfTypeStack.Count < 2)
        {
            return;
        }
        CAbilityExtraTurn.EExtraTurnType turnType = playerActor.TakingExtraTurnOfTypeStack.Peek();
        if (turnType != CAbilityExtraTurn.EExtraTurnType.CurrentAction)
        {
            return;
        }
        playerActor.TakingExtraTurnOfTypeStack.Pop();
        CAbilityExtraTurn.EExtraTurnType prevType = playerActor.TakingExtraTurnOfTypeStack.Peek();
        if (prevType != CAbilityExtraTurn.EExtraTurnType.TopAction && prevType != CAbilityExtraTurn.EExtraTurnType.BottomAction)
        {
            playerActor.TakingExtraTurnOfTypeStack.Push(turnType);
            return;
        }
    }
}

public static class PrintHelper
{
    public static void PrintActor(CPlayerActor actor)
    {
        BugFixesPlugin.logger.LogInfo($"Actor {actor.CharacterName}");
        BugFixesPlugin.logger.LogInfo($"  ID {actor.ID}");
        BugFixesPlugin.logger.LogInfo($"  SkipTopCardAction {actor.SkipTopCardAction}");
        BugFixesPlugin.logger.LogInfo($"  SkipBottomCardAction {actor.SkipBottomCardAction}");
        BugFixesPlugin.logger.LogInfo($"  Health {actor.Health}");
        BugFixesPlugin.logger.LogInfo($"  MaxHealth {actor.MaxHealth}");
        BugFixesPlugin.logger.LogInfo($"  PlayedThisRound {actor.PlayedThisRound}");
        BugFixesPlugin.logger.LogInfo($"  LastAbilityPerformed {actor.LastAbilityPerformed}");
        BugFixesPlugin.logger.LogInfo($"  ActorActionHasHappened {actor.ActorActionHasHappened}");
        BugFixesPlugin.logger.LogInfo($"  ExhaustAfterAction {actor.ExhaustAfterAction}");
        BugFixesPlugin.logger.LogInfo($"  PendingExtraTurnOfType {actor.PendingExtraTurnOfType}");
        BugFixesPlugin.logger.LogInfo($"  TakingExtraTurnOfType {actor.TakingExtraTurnOfType}");
        BugFixesPlugin.logger.LogInfo($"  IsTakingExtraTurn {actor.IsTakingExtraTurn}");
        BugFixesPlugin.logger.LogInfo($"  HasPendingExtraTurn {actor.HasPendingExtraTurn}");
    }
}
