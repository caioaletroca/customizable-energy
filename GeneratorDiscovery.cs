using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace CustomizableEnergy {
	internal static class GeneratorDiscovery {
		internal static readonly Dictionary<string, BuildingDef> Definitions = new Dictionary<string, BuildingDef>();
		internal static readonly Dictionary<string, float> OriginalWattages = new Dictionary<string, float>();

		internal static void RegisterPatches(Harmony harmony) {
			var register = AccessTools.Method(typeof(BuildingConfigManager), nameof(BuildingConfigManager.RegisterBuilding));
			if (register != null)
				harmony.Patch(register, postfix: new HarmonyMethod(typeof(GeneratorDiscovery), nameof(OnRegisterBuilding)));
			var load = AccessTools.Method(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings));
			if (load != null)
				harmony.Patch(load, postfix: new HarmonyMethod(typeof(GeneratorDiscovery), nameof(OnBuildingsLoaded)));
		}

		private static void OnBuildingsLoaded() {
			var options = EnergyOptions.Instance;
			var existing = options.Values.Generators.ToDictionary(setting => setting.BuildingId, StringComparer.Ordinal);
			options.Values.Generators = Definitions.Values.Select(definition => {
				if (!existing.TryGetValue(definition.PrefabID, out var setting))
					setting = new GeneratorSetting { BuildingId = definition.PrefabID, Wattage = OriginalWattages[definition.PrefabID] };
				return setting;
			}).ToArray();
		}

		private static void OnRegisterBuilding(IBuildingConfig config) {
			if (config == null)
				return;
			var definition = AccessTools.Field(typeof(BuildingConfigManager), "configTable")?.GetValue(BuildingConfigManager.Instance) as Dictionary<IBuildingConfig, BuildingDef>;
			if (definition != null && definition.TryGetValue(config, out var buildingDef) && IsGenerator(buildingDef)) {
				Definitions[buildingDef.PrefabID] = buildingDef;
				OriginalWattages[buildingDef.PrefabID] = buildingDef.GeneratorWattageRating;
				GeneratorPower.Apply(buildingDef);
			}
		}

		private static bool IsGenerator(BuildingDef definition) {
			return definition != null && definition.GeneratorWattageRating > 0f && definition.RequiresPowerOutput;
		}
	}
}
