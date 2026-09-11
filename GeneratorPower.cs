using System;
using System.Linq;

namespace CustomizableEnergy {
	internal static class GeneratorPower {
		internal const float MinimumWattage = 0f;
		internal const float MaximumWattage = 100000f;

		internal static float Clamp(float wattage) {
			return Math.Max(MinimumWattage, Math.Min(MaximumWattage, wattage));
		}

		internal static void Apply(BuildingDef definition) {
			var settings = EnergyOptions.Instance.Values.Generators.FirstOrDefault(setting => setting.BuildingId == definition.PrefabID);
			if (settings == null) {
				settings = new GeneratorSetting { BuildingId = definition.PrefabID, Wattage = definition.GeneratorWattageRating };
				EnergyOptions.Instance.Values.Generators = EnergyOptions.Instance.Values.Generators.Concat(new[] { settings }).ToArray();
			} else {
				settings.Wattage = Clamp(settings.Wattage);
			}
			if (settings.EnableOverwrite)
				definition.GeneratorWattageRating = settings.Wattage;
			definition.GeneratorBaseCapacity = definition.GeneratorWattageRating;
		}
	}
}
