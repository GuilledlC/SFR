using Microsoft.Xna.Framework;
using SFD;
using SFD.Objects;
using SFD.Sounds;
using SFD.Weapons;

namespace SFR.Weapons.Handguns;

internal sealed class ColtNavy : RWeapon
{
    internal ColtNavy()
    {
        RWeaponProperties weaponProperties = new(107, "ColtNavy", "WpnColtNavy", false, WeaponCategory.Secondary)
        {
            MaxMagsInWeapon = 6,
            MaxRoundsInMag = 1,
            MaxCarriedSpareMags = 24,
            StartMags = 12,
            CooldownBeforePostAction = 500,
            CooldownAfterPostAction = 0,
            ExtraAutomaticCooldown = 100,
            MuzzleEffectTextureID = "MuzzleFlashM",
            DrawSoundID = "RevolverDraw",
            BlastSoundID = "Pistol45",
            GrabAmmoSoundID = "RevolverReload",
            OutOfAmmoSoundID = "OutOfAmmoLight",
            MuzzlePosition = new Vector2(6f, -2f),
            CursorAimOffset = new Vector2(0f, 3.5f),
            LazerPosition = new Vector2(7f, -0.5f),
            AimStartSoundID = "PistolAim",
            ProjectilesEachBlast = 1,
            ClearRoundsOnReloadStart = false,
            ProjectileID = 1,
            ReloadPostCooldown = 600f,
            BreakDebris = new[] {"ItemDebrisWood00", "MetalDebris00C"},
            SpecialAmmoBulletsRefill = 18,
            AI_DamageOutput = DamageOutputType.High
        };

        RWeaponVisuals weaponVisuals = new();
        weaponVisuals.SetModelTexture("ColtNavyM");
        weaponVisuals.SetDrawnTexture("ColtNavyD");
        weaponVisuals.SetThrowingTexture("ColtNavyThrowing");
        weaponVisuals.AnimIdleUpper = "UpperIdleHandgun";
        weaponVisuals.AnimCrouchUpper = "UpperCrouchHandgun";
        weaponVisuals.AnimJumpKickUpper = "UpperJumpKickHandgun";
        weaponVisuals.AnimJumpUpper = "UpperJumpHandgun";
        weaponVisuals.AnimJumpUpperFalling = "UpperJumpFallingHandgun";
        weaponVisuals.AnimKickUpper = "UpperKickHandgun";
        weaponVisuals.AnimStaggerUpper = "UpperStaggerHandgun";
        weaponVisuals.AnimRunUpper = "UpperRunHandgun";
        weaponVisuals.AnimWalkUpper = "UpperWalkHandgun";
        weaponVisuals.AnimUpperHipfire = "UpperHipfireHandgun";
        weaponVisuals.AnimFireArmLength = 7f;
        weaponVisuals.AnimDraw = "UpperDrawMagnum";
        weaponVisuals.AnimManualAim = "ManualAimHandgun";
        weaponVisuals.AnimManualAimStart = "ManualAimHandgunStart";
        weaponVisuals.AnimReloadUpper = "UpperReload";
        weaponVisuals.AnimFullLand = "FullLandHandgun";
        weaponVisuals.AnimToggleThrowingMode = "UpperToggleThrowing";
        weaponProperties.VisualText = "Colt Navy Revolver";

        SetPropertiesAndVisuals(weaponProperties, weaponVisuals);
        CacheDrawnTextures(new[] { "Reload" });
    }
    
    private ColtNavy(RWeaponProperties weaponProperties, RWeaponVisuals weaponVisuals)
    {
        SetPropertiesAndVisuals(weaponProperties, weaponVisuals);
    }
    
    public override void OnReloadAnimationEvent(Player player, AnimationEvent animEvent, SubAnimationPlayer subAnim)
    {
        if (animEvent == AnimationEvent.EnterFrame && subAnim.GetCurrentFrameIndex() == 1)
        {
            if (player.GameOwner != GameOwnerEnum.Server)
            {
                base.SpawnUnsyncedShell(player, "ShellSmall");
                /*int num = 6;
                for (int i = 0; i < num; i++)
                { 
                }*/
            }
            SoundHandler.PlaySound("MagnumReloadStart", player.Position, player.GameWorld);
        }
    }
    
    public override void OnSubAnimationEvent(Player player, AnimationEvent animationEvent, AnimationData animationData, int currentFrameIndex)
    {
        if (player.GameOwner != GameOwnerEnum.Server && animationEvent == AnimationEvent.EnterFrame && animationData.Name == "UpperDrawMagnum")
        {
            if (currentFrameIndex == 1)
            {
                SoundHandler.PlaySound("Draw1", player.GameWorld);
            }
            if (currentFrameIndex == 6)
            {
                SoundHandler.PlaySound("MagnumDraw", player.GameWorld);
            }
        }
    }
    
    public override bool CheckDrawLazerAttachment(string subAnimation, int subFrame)
    {
        return subAnimation == null || !(subAnimation == "UpperReload");
    }
    
    public override void OnReloadAnimationFinished(Player player)
    {
        if (player.GameOwner != GameOwnerEnum.Server)
        {
            SoundHandler.PlaySound("MagnumReloadEnd", player.Position, player.GameWorld);
        }
    }
    
    public override void OnThrowWeaponItem(Player player, ObjectWeaponItem thrownWeaponItem)
    {
        Vector2 linearVelocity = thrownWeaponItem.Body.GetLinearVelocity();
        linearVelocity.X *= 1.2f;
        linearVelocity.Y *= 1f;
        thrownWeaponItem.Body.SetLinearVelocity(linearVelocity);
    }
    
    public override RWeapon Copy()
    {
        ColtNavy nitroExpress = new ColtNavy(Properties, Visuals);
        nitroExpress.CopyStatsFrom(this);
        return nitroExpress;
    }
}