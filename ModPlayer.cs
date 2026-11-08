using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AlternateScaling;

public class MPlayer : ModPlayer
{
	private float lifeDifference = 1f;
	private float manaDifference = 1f;

	public override void PostUpdate()
	{
		int oldLife = Player.statLifeMax2;
		Player.statLifeMax2 = (int)(Player.statLifeMax2 * Config.Server.PlayerHealthMultiplier);
		Player.statLife = Math.Min(Player.statLife, Player.statLifeMax2);
		lifeDifference = (float)Player.statLifeMax2 / (float)oldLife;
		int oldMana = Player.statManaMax2;
		Player.statManaMax2 = (int)(Player.statManaMax2 * Config.Server.PlayerManaMultiplier);
		Player.statMana = Math.Min(Player.statMana, Player.statManaMax2);
		manaDifference = Player.statManaMax2 / oldMana;
		Player.UpdateImmunity(); // Extra call, reduce immunity by half.
	}

	public override void UpdateEquips()
	{
		Player.manaCost /= Config.Server.PlayerManaMultiplier; // For halved mana
	}

	public override void ModifyHurt(ref Player.HurtModifiers modifiers)
	{
		if (Config.Server.Enabled && Config.Server.DefenseRework)
		{
			Player.DefenseEffectiveness = MultipliableFloat.One * 0f; // Handle defense calculations during damage.
		}
	}
}

public class MPlayerSystem : ModSystem
{
	public override void Load()
	{
		// Show damage changes on tooltips
		Terraria.On_Player.GetWeaponDamage += (GetWeaponDamage, self, sItem, forTooltip) =>
		{
			int damage = GetWeaponDamage(self, sItem, forTooltip);
			if (Config.Server.Enabled)
			{
				if (forTooltip)
				{
					damage = Functions.ScaleDamage(damage);
				}
			}
			return damage;
		};

		// Apply damage change on player damage
		Terraria.On_Player.Hurt_HurtInfo_bool += (Hurt, self, info, quiet) =>
		{
			if (Config.Server.Enabled)
			{
				info.Damage = Functions.ScaleDamage(info.Damage);
				info.Damage = Functions.ReduceDamage(info.Damage, self.statDefense);
			}
			Hurt(self, info, quiet);
			return;
		};
	}
}