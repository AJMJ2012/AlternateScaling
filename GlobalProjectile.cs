using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AlternateScaling;

public class GProjectile : GlobalProjectile
{
	public override void SetDefaults(Projectile projectile)
	{
		if (Config.Server.ForceProjectileTileCollide)
		{
			// Workarounds?
			if (projectile.hostile) { return; }
			if (projectile.damage <= 0) { return; }
			if (Config.Server.ProjectileTileCollideExtraCheck && projectile.ownerHitCheck) { return; }
			if (projectile.minion) { return; }
			if (projectile.sentry) { return; }
			if (projectile.aiStyle == ProjAIStyleID.Yoyo) { return; } // TODO: Detect only returns
			if (projectile.aiStyle == ProjAIStyleID.Boomerang) { return; } // TODO: Detect only returns
			if (projectile.aiStyle == ProjAIStyleID.HeldProjectile) { return; }
			if (projectile.aiStyle == ProjAIStyleID.Whip) { return; }
			if (projectile.aiStyle == ProjAIStyleID.HorsemanPumpkin) { return; }
			if (projectile.aiStyle == ProjAIStyleID.Powder) { return; }
			projectile.tileCollide = true;
		}
	}
}