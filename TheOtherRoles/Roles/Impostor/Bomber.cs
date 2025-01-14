﻿using UnityEngine;

namespace TheOtherRoles.Roles.Impostor;

public static class Bomber
{
    public static PlayerControl bomber;
    public static Color color = Palette.ImpostorRed;

    public static float cooldown = 30f;
    public static float bombDelay = 10f;
    public static float bombTimer = 10f;
    public static bool triggerBothCooldowns;
    public static bool canGiveToBomber;
    public static bool hotPotatoMode;

    public static bool bombActive;

    public static bool hasAlerted;
    public static int timeLeft;
    public static PlayerControl currentTarget;
    public static PlayerControl currentBombTarget;
    public static PlayerControl hasBombPlayer;


    public static ResourceSprite buttonSprite = new("Bomber2.png");

    public static void clearAndReload()
    {
        bomber = null;
        currentTarget = null;
        currentBombTarget = null;
        hasBombPlayer = null;
        bombActive = false;
        cooldown = CustomOptionHolder.bomberBombCooldown.getFloat();
        bombDelay = CustomOptionHolder.bomberDelay.getFloat();
        bombTimer = CustomOptionHolder.bomberTimer.getFloat();
        triggerBothCooldowns = CustomOptionHolder.bomberTriggerBothCooldowns.getBool();
        canGiveToBomber = CustomOptionHolder.bomberCanGiveToBomber.getBool();
        hotPotatoMode = CustomOptionHolder.bomberHotPotatoMode.getBool();
    }
}
