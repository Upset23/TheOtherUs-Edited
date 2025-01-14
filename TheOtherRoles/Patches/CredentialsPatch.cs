﻿using System.Collections.Generic;
using InnerNet;
using TMPro;
using UnityEngine;

namespace TheOtherRoles.Patches;

[HarmonyPatch]
public static class CredentialsPatch
{
    public static string fullCredentialsVersion = $"<size=130%>{getString("TouTitle")}</size> v{Main.Version + (Main.betaDays > 0 ? "-Beta" : "")}";

    public static string fullCredentials = getString("fullCredentials");

    public static string mainMenuCredentials = getString("mainMenuCredentials");

    public static string contributorsCredentials = getString("contributorsCredentials");

    [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
    internal static class PingTrackerPatch
    {
        private static float DeltaTime;

        private static void Postfix(PingTracker __instance)
        {

            DeltaTime += (Time.deltaTime - DeltaTime) * 0.1f;
            var fps = Mathf.Ceil(1f / DeltaTime);
            var PingText = $"<size=80%>Ping: {AmongUsClient.Instance.Ping}ms{(ModOption.showFPS ? $"  FPS: {fps}" : "")}</size>";
            __instance.text.SetOutlineThickness(0.1f);
            var host = $"<size=80%>{"Host".Translate()}: {GameData.Instance?.GetHost()?.PlayerName}</size>";

            __instance.text.alignment = TextAlignmentOptions.TopRight;
            var position = __instance.GetComponent<AspectPosition>();
            var gameModeText = ModOption.gameMode switch
            {
                CustomGamemodes.HideNSeek => getString("isHideNSeekGM"),
                CustomGamemodes.Guesser => getString("isGuesserGm"),
                CustomGamemodes.PropHunt => getString("isPropHuntGM"),
                _ => ""
            };
            if (ModOption.DebugMode) gameModeText += "(Debug Mode)";
            if (gameModeText != "") gameModeText = cs(Color.yellow, gameModeText) + "\n";
            if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started)
            {
                __instance.text.text = $"<size=110%>{getString("TouTitle")}</size>  v{Main.Version + "\n" + getString("inGameTitle")}\n{PingText}\n{gameModeText}";
                position.DistanceFromEdge = new Vector3(2.25f, 0.1f, 0);
            }
            else
            {
                __instance.text.text = $"{fullCredentialsVersion}\n {PingText}\n  {gameModeText + fullCredentials}\n {host}";
                position.DistanceFromEdge = new Vector3(2.85f, 0.1f, 0);
            }
            position.AdjustPosition();
        }
    }

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public static class LogoPatch
    {
        public static SpriteRenderer renderer;
        private static PingTracker instance;

        public static GameObject motdObject;
        public static TextMeshPro motdText;

        private static void Postfix(PingTracker __instance)
        {
            var torLogo = new GameObject("bannerLogo_TOR");
            torLogo.transform.SetParent(GameObject.Find("RightPanel").transform, false);
            torLogo.transform.localPosition = new Vector3(-0.4f, 1f, 5f);

            renderer = torLogo.AddComponent<SpriteRenderer>();
            if (IsCN()) renderer.sprite = new ResourceSprite("TheOtherRoles.Resources.Banner2.png", 270f);
            else renderer.sprite = new ResourceSprite("TheOtherRoles.Resources.Banner.png", 300f);
            instance = __instance;
            var credentialObject = new GameObject("credentialsTOR");
            var credentials = credentialObject.AddComponent<TextMeshPro>();
            credentials.SetText(
                $"<size=90%>TheOtherUs-Edited v{Main.Version + (Main.betaDays > 0 ? "-Beta" : "")}</size>\n<size=30%>\n</size>{mainMenuCredentials}\n<size=30%>\n</size>{contributorsCredentials}");
            credentials.alignment = TextAlignmentOptions.Center;
            credentials.fontSize *= 0.05f;

            credentials.transform.SetParent(torLogo.transform);
            credentials.transform.localPosition = Vector3.down * 1.5f;
            motdObject = new GameObject("torMOTD");
            motdText = motdObject.AddComponent<TextMeshPro>();
            motdText.alignment = TextAlignmentOptions.Center;
            motdText.fontSize *= 0.04f;

            motdText.transform.SetParent(torLogo.transform);
            motdText.enableWordWrapping = true;
            var rect = motdText.gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(5.2f, 0.25f);

            motdText.transform.localPosition = Vector3.down * 2.25f;
            motdText.color = new Color(1, 53f / 255, 31f / 255);
            var mat = motdText.fontSharedMaterial;
            mat.shaderKeywords = new[] { "OUTLINE_ON" };
            motdText.SetOutlineColor(Color.white);
            motdText.SetOutlineThickness(0.025f);
        }
    }

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.LateUpdate))]
    public static class MOTD
    {
        public static List<string> motds = new();
        private static float timer;
        private static readonly float maxTimer = 5f;
        private static int currentIndex;

        public static void Postfix()
        {
            if (motds.Count == 0)
            {
                timer = maxTimer;
                return;
            }

            if (motds.Count > currentIndex && LogoPatch.motdText != null)
                LogoPatch.motdText.SetText(motds[currentIndex]);
            else return;

            // fade in and out:
            var alpha = Mathf.Clamp01(Mathf.Min(new[] { timer, maxTimer - timer }));
            if (motds.Count == 1) alpha = 1;
            LogoPatch.motdText.color = LogoPatch.motdText.color.SetAlpha(alpha);
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = maxTimer;
                currentIndex = (currentIndex + 1) % motds.Count;
            }
        }
    }
}