﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherRoles.Roles.Impostor;

public class Miner
{
    public static readonly List<Vent> Vents = new();
    public static PlayerControl miner;
    public static DateTime LastMined;
    public static ResourceSprite buttonSprite = new("Mine.png");

    public static float cooldown = 30f;
    public static Color color = Palette.ImpostorRed;
    public KillButton _mineButton;

    public bool CanPlace { get; set; }
    public static Vector2 VentSize { get; set; }

    public static void clearAndReload()
    {
        miner = null;
        cooldown = CustomOptionHolder.minerCooldown.getFloat();
    }
}
