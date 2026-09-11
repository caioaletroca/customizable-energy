using Newtonsoft.Json;
using PeterHan.PLib;
using PeterHan.PLib.Options;

namespace CustomizableEnergy {
	[JsonObject(MemberSerialization.OptIn)]
	[RestartRequired]
	public sealed class EnergyOptions : SingletonOptions<EnergyOptions> {
		[JsonProperty]
		[DynamicOption(typeof(GeneratorOptionsEntry), "Generators")]
		public GeneratorOptionsValue Values { get; set; }

		public EnergyOptions() {
			Values = new GeneratorOptionsValue();
		}
	}

	[JsonObject(MemberSerialization.OptIn)]
	public sealed class GeneratorOptionsValue {
		[JsonProperty]
		public GeneratorSetting[] Generators { get; set; }

		public GeneratorOptionsValue() {
			Generators = new GeneratorSetting[0];
		}
	}

	[JsonObject(MemberSerialization.OptIn)]
	public sealed class GeneratorSetting {
		[JsonProperty]
		public string BuildingId { get; set; }

		[JsonProperty]
		public bool EnableOverwrite { get; set; }

		[JsonProperty]
		public float Wattage { get; set; }
	}
}
