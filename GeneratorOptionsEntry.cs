using System;
using System.Linq;
using PeterHan.PLib.Options;
using PeterHan.PLib.UI;
using TMPro;
using UnityEngine;

namespace CustomizableEnergy {
	public sealed class GeneratorOptionsEntry : IOptionsEntry {
		private GeneratorOptionsValue value;
		private GeneratorSetting[] orderedSettings;
		private GameObject[] toggles;
		private GameObject[] fields;

		public string Category { get; set; }
		public string Field => nameof(EnergyOptions.Values);
		public string Format => null;
		public string Name => nameof(GeneratorOptionsEntry);
		public string Title => "Generators";
		public string Tooltip => "Configure generator output power.";
		public bool RestartRequired { get; set; }

		public void CreateUIEntry(PGridPanel parent, ref int row) {
			if (value == null)
				ReadFrom(EnergyOptions.Instance);
			orderedSettings = value.Generators.OrderBy(setting => setting.BuildingId, StringComparer.Ordinal).ToArray();
			toggles = new GameObject[orderedSettings.Length];
			fields = new GameObject[orderedSettings.Length];
			parent.AddColumn(new GridColumnSpec(flex: 1.0f)).AddColumn(new GridColumnSpec()).AddColumn(new GridColumnSpec());
			for (var i = 0; i < orderedSettings.Length; i++) {
				var index = i;
				var setting = orderedSettings[index];
				parent.AddRow(new GridRowSpec());
				parent.AddChild(new PLabel { Text = setting.BuildingId, TextStyle = PUITuning.Fonts.TextLightStyle }, new GridComponentSpec(row, 0) { Alignment = TextAnchor.MiddleLeft });
				var toggle = new PButton { Text = setting.EnableOverwrite ? "ON" : "OFF", ToolTip = "Enable overwrite", Color = setting.EnableOverwrite ? PUITuning.Colors.ButtonPinkStyle : PUITuning.Colors.ButtonBlueStyle, OnClick = realized => { setting.EnableOverwrite = !setting.EnableOverwrite; var label = realized.GetComponentInChildren<TextMeshProUGUI>(); if (label != null) label.text = setting.EnableOverwrite ? "ON" : "OFF"; } }.AddOnRealize(realized => toggles[index] = realized);
				parent.AddChild(toggle, new GridComponentSpec(row, 1) { Alignment = TextAnchor.MiddleCenter });
				var field = new PTextField { Text = Clamp(setting.Wattage).ToString("0"), Type = PTextField.FieldType.Integer, MinWidth = 72, MaxLength = 6, ToolTip = "Output wattage", OnTextChanged = (_, text) => { if (int.TryParse(text, out var wattage)) setting.Wattage = Clamp(wattage); } }.AddOnRealize(realized => fields[index] = realized);
				parent.AddChild(field, new GridComponentSpec(row, 2) { Alignment = TextAnchor.MiddleRight });
				row++;
			}
		}

		public void ReadFrom(object settings) {
			value = (settings as EnergyOptions)?.Values ?? new GeneratorOptionsValue();
			var discovered = GeneratorDiscovery.Definitions.Values.OrderBy(definition => definition.PrefabID, StringComparer.Ordinal).ToArray();
			var existing = value.Generators.ToDictionary(setting => setting.BuildingId, StringComparer.Ordinal);
			value.Generators = discovered.Select(definition => existing.TryGetValue(definition.PrefabID, out var setting) ? setting : new GeneratorSetting { BuildingId = definition.PrefabID, Wattage = GeneratorDiscovery.OriginalWattages[definition.PrefabID] }).ToArray();
			if (orderedSettings != null && toggles != null && fields != null) {
				orderedSettings = value.Generators.OrderBy(setting => setting.BuildingId, StringComparer.Ordinal).ToArray();
				for (var i = 0; i < orderedSettings.Length && i < toggles.Length && i < fields.Length; i++) {
					var buttonLabel = toggles[i]?.GetComponentInChildren<TextMeshProUGUI>();
					if (buttonLabel != null)
						buttonLabel.text = orderedSettings[i].EnableOverwrite ? "ON" : "OFF";
					var input = fields[i]?.GetComponentInChildren<TMP_InputField>();
					if (input != null)
						input.text = Clamp(orderedSettings[i].Wattage).ToString("0");
				}
			}
		}

		public bool WriteTo(object settings) {
			if (!(settings is EnergyOptions options) || orderedSettings == null)
				return false;
			for (var i = 0; i < orderedSettings.Length; i++) {
				var input = fields[i].GetComponentInChildren<TMP_InputField>();
				if (input != null && int.TryParse(input.text, out var wattage))
					orderedSettings[i].Wattage = Clamp(wattage);
			}
			options.Values = new GeneratorOptionsValue { Generators = orderedSettings };
			value = options.Values;
			return true;
		}

		private static float Clamp(float wattage) {
			return Math.Max(0f, Math.Min(100000f, wattage));
		}
	}
}
