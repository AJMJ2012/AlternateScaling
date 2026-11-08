using System;
using Terraria;
using Terraria.ID;

namespace AlternateScaling;

internal static class Functions
{
	internal static bool IsBoss(NPC npc) => (npc.boss || npc.GetBossHeadTextureIndex() > 0 || NPCID.Sets.ShouldBeCountedAsBoss[npc.type]) || (npc.realLife >= 0 && (Main.npc[npc.realLife].boss || Main.npc[npc.realLife].GetBossHeadTextureIndex() > 0 || NPCID.Sets.ShouldBeCountedAsBoss[Main.npc[npc.realLife].type]));

	internal static int ScaleLife(int life, bool boss)
	{
		if (Config.Server.EnemyHealthRework && life > 0)
		{
			life = (int)(Math.Sqrt(life) * (boss ? Config.Server.EnemyBossHealthReworkVar : Config.Server.EnemyHealthReworkVar));
		}
		return Math.Max(1, life);
	}
	internal static int ScaleDamage(int damage)
	{
		if (Config.Server.DamageRework && damage > 0)
		{
			damage = (int)(Math.Sqrt(damage) * Config.Server.GlobalDamageReworkVar);
		}
		return Math.Max(1, damage);
	}
	internal static int UnScaleDamage(int damage)
	{
		if (Config.Server.DamageRework && damage > 0)
		{
			damage = (int)((damage * damage) / Config.Server.GlobalDamageReworkVar);
		}
		return Math.Max(1, damage);
	}

	internal static int ReduceDamage(int damage, int defense)
	{
		damage = Math.Max(1, damage);
		if (Config.Server.DefenseRework && defense > 0)
		{
			damage -= (int)(((defense / 2f) * damage) / 100f);
		}
		return Math.Max(1, damage);
	}
}
