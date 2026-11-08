using System;
using System.ComponentModel;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace AlternateScaling;

public class ServerConfig : ModConfig
{
	public override ConfigScope Mode => ConfigScope.ServerSide;
	public static ServerConfig Instance;

	[Header("Main")]

	[DefaultValue(true)]
	public bool Enabled;

	[Header("Damage")]

	[DefaultValue(true)]
	public bool DamageRework;

	[DefaultValue(true)]
	public bool DefenseRework;

	[DefaultValue(5f)]
	[Range(1f, 20f)]
	[Increment(0.25f)]
	[Slider]
	public float GlobalDamageReworkVar;

	[Header("Enemy")]

	[DefaultValue(true)]
	public bool EnemyHealthRework;

	[DefaultValue(10f)]
	[Range(1f, 20f)]
	[Increment(0.25f)]
	[Slider]
	public float EnemyHealthReworkVar;

	[DefaultValue(100f)]
	[Range(10f, 200f)]
	[Increment(2.5f)]
	[Slider]
	public float EnemyBossHealthReworkVar;

	[DefaultValue(true)]
	public bool AggressiveEnemies;

	[DefaultValue(true)]
	public bool AggressiveBosses;

	[DefaultValue(false)]
	public bool DisableVanillaDifficultyEnemyScaling;

	[DefaultValue(true)]
	public bool AdjustEnemyValue;

	[Header("PlayerHelathMana")]

	[DefaultValue(0.5f)]
	[Range(0.25f, 10f)]
	[Increment(0.25f)]
	[Slider]
	public float PlayerHealthMultiplier;

	[DefaultValue(0.5f)]
	[Range(0.25f, 10f)]
	[Increment(0.25f)]
	[Slider]
	public float PlayerManaMultiplier;

	[Header("Projectile")]

	[DefaultValue(false)]
	public bool ForceProjectileTileCollide;

	[DefaultValue(false)]
	public bool ProjectileTileCollideExtraCheck;

	public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message)
	{
		return true;
	}
}

public static class Config
{
	public static ServerConfig Server => ServerConfig.Instance;
}

