using SFD.Projectiles;
using SFD.Tiles;

namespace SFR.Projectiles;

internal sealed class ProjectileNitroExpress : Projectile
{
    internal ProjectileNitroExpress()
    {
        Visuals = new ProjectileVisuals(Textures.GetTexture("BulletNitro"), Textures.GetTexture("BulletNitroSlowmo"));
        Properties = new ProjectileProperties(103, 1500f, 1000f, 45f, 500f, 0.25f, 65f, 100f, 1f)
        {
            
            
        };
    }
    
    private ProjectileNitroExpress(ProjectileProperties projectileProperties, ProjectileVisuals projectileVisuals) : base(projectileProperties, projectileVisuals) { }
    
    public override Projectile Copy()
    {
        ProjectileNitroExpress projectile = new ProjectileNitroExpress(Properties, Visuals);
        projectile.CopyBaseValuesFrom(this);
        return projectile;
    }
}