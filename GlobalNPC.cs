using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AlternateScaling;

public class GNPC : GlobalNPC
{
	public override bool InstancePerEntity => true;

	internal float lifeDifference = 1f;

	// Get the base scaled life difference and adjust value. 
	public override void SetDefaults(NPC npc)
	{
		if (Config.Server.Enabled && npc.lifeMax > 5)
		{
			// Get the base scaled life difference, but don't apply. That's handled elsewhere due to stat scaling.
			int oldLife = npc.lifeMax;
			int newLife = Functions.ScaleLife(npc.lifeMax, Functions.IsBoss(npc));
			lifeDifference = (float)newLife / (float)oldLife;
			if (Config.Server.AdjustEnemyValue)
			{
				// Adjust NPC value based on life difference.
				npc.value *= lifeDifference;
			}
		}
	}

	// Set defense virtually to zero as it's handled directly in the damage received calculations.
	public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
	{
		if (Config.Server.Enabled && Config.Server.DefenseRework)
		{
			modifiers.DefenseEffectiveness = MultipliableFloat.One * 0f;
		}
	}

	// Make NPCs more aggressive.
	private uint AITime = 0;
	public override void AI(NPC npc)
	{
		if ((Functions.IsBoss(npc) ? Config.Server.AggressiveBosses : Config.Server.AggressiveEnemies) && !NPCID.Sets.ProjectileNPC[npc.type] && AITime++ % 2 > 0)
		{
			npc.AI();
		}
	}
}

public class GNPCS : ModSystem
{
	public override void Load()
	{
		// Apply life changes after NPC stat scaling.
		Terraria.On_NPC.ScaleStats += (ScaleStats, npc, activePlayersCount, gameModeData, strengthOverride) =>
		{
			ScaleStats(npc, activePlayersCount, gameModeData, strengthOverride);
			if (Config.Server.Enabled && npc.lifeMax > 5)
			{
				npc.life = Functions.ScaleLife(npc.lifeMax, Functions.IsBoss(npc));
				npc.life = npc.lifeMax;
				return;
			}
		};

		// Apply damage change on NPC damage.
		Terraria.On_NPC.StrikeNPC_HitInfo_bool_bool += (StrikeNPC, self, info, fromNet, noPlayerInteraction) =>
		{
			if (Config.Server.Enabled)
			{
				info.Damage = Functions.ScaleDamage(info.Damage);
				info.Damage = Functions.ReduceDamage(info.Damage, self.defense);
			}
			return StrikeNPC(self, info, fromNet, noPlayerInteraction);
		};
	}
}
