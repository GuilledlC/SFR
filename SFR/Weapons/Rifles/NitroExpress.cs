using Microsoft.Xna.Framework;
using SFD;
using SFD.Sounds;
using SFD.Weapons;

namespace SFR.Weapons.Rifles;

internal sealed class NitroExpress : RWeapon
{
    private int _consumedShells;
    internal NitroExpress()
    {
        RWeaponProperties weaponProperties = new(106, "NitroExpress", "WpnNitroExpress", false, WeaponCategory.Primary)
        {
            MaxMagsInWeapon = 2,
            MaxRoundsInMag = 1,
            MaxCarriedSpareMags = 6,
            StartMags = 4,
            MuzzleEffectTextureID = "MuzzleFlashShotgun",
            DrawSoundID = "SawedOffDraw",
            GrabAmmoSoundID = "SawedOffReload",
            OutOfAmmoSoundID = "OutOfAmmoHeavy",
            CooldownBeforePostAction = 150,
            MuzzlePosition = new Vector2(16f, -2.5f),
            LazerPosition = new Vector2(16f, -0.5f),
            CursorAimOffset = new Vector2(4f, 2.5f),
            AimStartSoundID = "PistolAim",
            ProjectilesEachBlast = 1,
            ClearRoundsOnReloadStart = false,
            ProjectileID = 103,
            ReloadPostCooldown = 600f,
            BreakDebris = new[] { "ItemDebrisStockWood00", "ItemDebrisWood00", "MetalDebris00C", "ItemDebrisShiny00" },
            SpecialAmmoBulletsRefill = 4,
            AI_DamageOutput = DamageOutputType.High
        };

        RWeaponVisuals weaponVisuals = new();
        weaponVisuals.SetModelTexture("NitroExpressM");
        weaponVisuals.SetDrawnTexture("NitroExpressD");
        weaponVisuals.SetSheathedTexture("NitroExpressS");
        weaponVisuals.SetThrowingTexture("NitroExpressThrowing");
        weaponVisuals.AnimIdleUpper = "UpperIdleRifle";
        weaponVisuals.AnimCrouchUpper = "UpperCrouchRifle";
        weaponVisuals.AnimJumpKickUpper = "UpperJumpKickRifle";
        weaponVisuals.AnimJumpUpper = "UpperJumpRifle";
        weaponVisuals.AnimJumpUpperFalling = "UpperJumpFallingRifle";
        weaponVisuals.AnimKickUpper = "UpperKickRifle";
        weaponVisuals.AnimStaggerUpper = "UpperStaggerHandgun";
        weaponVisuals.AnimRunUpper = "UpperRunRifle";
        weaponVisuals.AnimWalkUpper = "UpperWalkRifle";
        weaponVisuals.AnimUpperHipfire = "UpperHipfireRifle";
        weaponVisuals.AnimFireArmLength = 2f;
        weaponVisuals.AnimDraw = "UpperDrawRifle";
        weaponVisuals.AnimManualAim = "ManualAimRifle";
        weaponVisuals.AnimManualAimStart = "ManualAimRifleStart";
        weaponVisuals.AnimReloadUpper = "UpperReloadShell";
        weaponVisuals.AnimFullLand = "FullLandHandgun";
        weaponVisuals.AnimToggleThrowingMode = "UpperToggleThrowing";
        weaponProperties.VisualText = "Nitro Express Rifle";

        SetPropertiesAndVisuals(weaponProperties, weaponVisuals);
        CacheDrawnTextures(new[] { "Reload" });
    }
    
    private NitroExpress(RWeaponProperties weaponProperties, RWeaponVisuals weaponVisuals)
    {
        SetPropertiesAndVisuals(weaponProperties, weaponVisuals);
    }

    public override void OnReloadAnimationEvent(Player player, AnimationEvent animEvent, SubAnimationPlayer subAnim)
    {
        if (player.GameOwner != GameOwnerEnum.Server && animEvent == AnimationEvent.EnterFrame)
        {
            if (subAnim.GetCurrentFrameIndex() == 1)
            {
                SpawnUnsyncedShell(player, "ShellNitro");
                _consumedShells -= 1;
                SoundHandler.PlaySound("MagnumReloadStart", player.Position, player.GameWorld);
            }
            else if (subAnim.GetCurrentFrameIndex() == 3)
            {
                SoundHandler.PlaySound("ShotgunReload", player.Position, player.GameWorld);
            }
        }
    }

    public override void GrabAmmo(Player player)
    {
        base.GrabAmmo(player);
        _consumedShells = MaxRoundsInWeapon - CurrentRoundsInWeapon;
    }

    public override void OnSubAnimationEvent(Player player, AnimationEvent animationEvent, AnimationData animationData, int currentFrameIndex)
    {
        if (player.GameOwner != GameOwnerEnum.Server && animationEvent == AnimationEvent.EnterFrame)
        {
            if (animationData.Name == "UpperDrawRifle")
            {
                switch (currentFrameIndex)
                {
                    case 1:
                        SoundHandler.PlaySound("Draw1", player.GameWorld);
                        break;
                    case 6:
                        SoundHandler.PlaySound("SawedOffDraw", player.GameWorld);
                        break;
                }
            }
        }
    }
    
    public override bool CheckDrawLazerAttachment(string subAnimation, int subFrame) => subAnimation is not "UpperReloadShell";
    
    public override void ConsumeAmmoFromFire(Player player)
    {
        _consumedShells += 1;
        base.ConsumeAmmoFromFire(player);
        SoundHandler.PlaySound("Magnum", player.Position, player.GameWorld);
    }

    public override RWeapon Copy()
    {
        NitroExpress nitroExpress = new NitroExpress(Properties, Visuals);
        nitroExpress.CopyStatsFrom(this);
        return nitroExpress;
    }
}