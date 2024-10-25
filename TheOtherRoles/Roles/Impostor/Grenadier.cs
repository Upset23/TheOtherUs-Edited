﻿using System;
using Il2CppSystem.Collections.Generic;
using TheOtherRoles.Utilities;
using UnityEngine;

namespace TheOtherRoles.Roles.Impostor;
public class Grenadier
{
    public static PlayerControl grenadier;
    public static Color color = Palette.ImpostorRed;
    public static Color flash = new Color32(153, 153, 153, byte.MaxValue);
    public static List<PlayerControl> controls = new();

    public static float cooldown;
    public static float duration;
    public static float radius;
    public static int indicatorsMode;

    public static ResourceSprite ButtonSprite = new("FlashButton.png");


    public static void showFlash(Color color, float duration = 10f, float alpha = 1f)
    {
        if (FastDestroyableSingleton<HudManager>.Instance == null ||
            FastDestroyableSingleton<HudManager>.Instance.FullScreen == null) return;

        FastDestroyableSingleton<HudManager>.Instance.FullScreen.gameObject.SetActive(true);
        FastDestroyableSingleton<HudManager>.Instance.FullScreen.enabled = true;

        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(duration, new Action<float>(p =>
        {
            var renderer = FastDestroyableSingleton<HudManager>.Instance.FullScreen;
            var fadeFraction = 0.25f / duration;

            if (IsMeeting)
            {
                renderer.enabled = false;
                controls = new();
                return;
            }

            if (p < (float)0.25f / duration)
            {
                float fadeInProgress = p / fadeFraction;
                if (renderer != null) renderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01(fadeInProgress * alpha));
            }
            else if (p > 1 - ((float)0.25f / duration))
            {
                float fadeOutProgress = (p - (1 - fadeFraction)) / fadeFraction;
                if (renderer != null) renderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01((1 - fadeOutProgress) * alpha));
                controls = new();
            }
            else
            {
                if (renderer != null) renderer.color = new Color(color.r, color.g, color.b, alpha);
            }

            if (p == 1f && renderer != null) renderer.enabled = false;
        })));
    }


    public static void clearAndReload()
    {
        grenadier = null;
        controls.Clear();
        cooldown = CustomOptionHolder.grenadierCooldown.getFloat();
        duration = CustomOptionHolder.grenadierDuration.getFloat() + 0.5f;
        radius = CustomOptionHolder.grenadierFlashRadius.getFloat();
        indicatorsMode = CustomOptionHolder.grenadierTeamIndicators.getSelection();
    }
}
