using HarmonyLib;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

namespace CustomizableEnergy {
	public sealed class CustomizableEnergyMod : KMod.UserMod2 {
		public override void OnLoad(Harmony harmony) {
			base.OnLoad(harmony);
			PUtil.InitLibrary(false);
			new POptions().RegisterOptions(this, typeof(EnergyOptions));
			GeneratorDiscovery.RegisterPatches(harmony);
		}
	}
}
