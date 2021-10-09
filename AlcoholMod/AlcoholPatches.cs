using HarmonyLib;
using ModComponentAPI;
using System;
using UnityEngine;

namespace AlcoholMod
{
	internal static class AlcoholPatches
	{
		[HarmonyPatch(typeof(GearItem), "ApplyBuffs")]//positive caller count
		internal static class AlcoholComponentHook
		{
			public static void Postfix(GearItem __instance, float normalizedValue)
			{
				AlcoholComponent alcoholComponent = ModComponentUtils.ComponentUtils.GetComponent<AlcoholComponent>(__instance);
				if (alcoholComponent != null)
				{
					alcoholComponent.Apply(normalizedValue);
				}
			}
		}

		[HarmonyPatch(typeof(FoodItem), "Deserialize")]//Exists
		internal static class FoodItem_Deserialize
		{
			public static void Postfix(FoodItem __instance)
			{
				AlcoholExtensions.UpdateAlcoholValues(__instance);
			}
		}

		[HarmonyPatch(typeof(FoodItem), "Awake")]//Exists
		internal static class FoodItem_Awake
		{
			public static void Postfix(FoodItem __instance)
			{
				AlcoholExtensions.UpdateAlcoholValues(__instance);
			}
		}

		[HarmonyPatch(typeof(SaveGameSystem), "RestoreGlobalData")]
		internal static class SaveGameSystem_RestoreGlobalData
		{
			public static void Postfix(string name)
			{
				string serializedProxy = SaveGameSlots.LoadDataFromSlot(name, "ModHealthManager");
				SaveProxy proxy = new SaveProxy();
				if (!string.IsNullOrEmpty(serializedProxy))
				{
					proxy = SaveProxy.ParseJson(serializedProxy);
				}
				ModHealthManager.SetData(GetData(proxy));
			}

			private static ModHealthManagerData GetData(SaveProxy proxy)
			{
				if (proxy == null || string.IsNullOrEmpty(proxy.data)) return null;

				return ModHealthManagerData.ParseJson(proxy.data);
			}
		}

		[HarmonyPatch(typeof(SaveGameSystem), "SaveGlobalData")]//Exists
		internal static class SaveGameSystem_SaveGlobalData
		{
			public static void Postfix(SaveSlotType gameMode, string name)
			{
				SaveProxy proxy = new SaveProxy();
				proxy.data = ModHealthManager.GetData().DumpJson();
				SaveGameSlots.SaveDataToSlot(gameMode, SaveGameSystem.m_CurrentEpisode, SaveGameSystem.m_CurrentGameId, name, "ModHealthManager", proxy.DumpJson());
			}
		}

		[HarmonyPatch(typeof(StatusBar), "GetRateOfChange")]//positive caller count
		internal static class StatusBar_GetRateOfChange
		{
			private static void Postfix(StatusBar __instance, ref float __result)
			{
				if (__instance.m_StatusBarType == StatusBar.StatusBarType.Fatigue)
				{
					var fatigueMonitor = ModHealthManager.GetFatigueMonitor();
					__result = fatigueMonitor.GetRateOfChange();
				}
				else if (__instance.m_StatusBarType == StatusBar.StatusBarType.Thirst)
				{
					var thirstMonitor = ModHealthManager.GetThirstMonitor();
					__result = thirstMonitor.GetRateOfChange();
				}
			}
		}

		[HarmonyPatch(typeof(Condition), "UpdateBlurEffect")]//positive caller count
		internal static class Condition_UpdateBlurEffect
		{
			public static void Prefix(Condition __instance, ref float percentCondition, ref bool lowHealthStagger)
			{
				lowHealthStagger = percentCondition <= __instance.m_HPToStartBlur || ModHealthManager.ShouldStagger();
				percentCondition = Math.Min(percentCondition, __instance.m_HPToStartBlur * (1 - ModHealthManager.GetAlcoholBlurValue()) + 0.01f);

				if (!lowHealthStagger)
				{
					GameManager.GetVpFPSCamera().m_BasePitch = Mathf.Lerp(GameManager.GetVpFPSCamera().m_BasePitch, 0.0f, 0.01f);
					GameManager.GetVpFPSCamera().m_BaseRoll = Mathf.Lerp(GameManager.GetVpFPSCamera().m_BaseRoll, 0.0f, 0.01f);
				}
			}
		}

		[HarmonyPatch(typeof(Freezing), "CalculateBodyTemperature")]//positive caller count
		internal static class Freezing_CalculateBodyTemperature
		{
			public static void Postfix(ref float __result)
			{
				__result += ModHealthManager.GetBodyTempBonus();
			}
		}

		[HarmonyPatch(typeof(Frostbite), "CalculateBodyTemperatureWithoutClothing")]//positive caller count
		internal static class Frostbite_CalculateBodyTemperatureWithoutClothing
		{
			public static void Postfix(ref float __result)
			{
				__result += ModHealthManager.GetFrostbiteTempBonus();
			}
		}

		[HarmonyPatch(typeof(GameManager), "Start")]//runs
		internal static class GameManagerStartPatch
		{
			public static void Postfix(PlayerManager __instance)
			{
				ModHealthManager.instance = __instance.gameObject.AddComponent<ModHealthManager>();
			}
		}
	}
}
