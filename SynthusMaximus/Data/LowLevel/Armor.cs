﻿#region

using System.Collections.Generic;
using Newtonsoft.Json;

#endregion

namespace SynthusMaximus.Data.LowLevel
{

    public class ArmorSettings
    {
        [JsonProperty("armorFactorBody")]
        public float ArmorFactorBody { get; set; }

        [JsonProperty("armorFactorFeet")]
        public float ArmorFactorFeet { get; set; }

        [JsonProperty("armorFactorHands")]
        public float ArmorFactorHands { get; set; }

        [JsonProperty("armorFactorHead")]
        public float ArmorFactorHead { get; set; }

        [JsonProperty("armorFactorShield")]
        public float ArmorFactorShield { get; set; }

        [JsonProperty("meltdownOutputBody")]
        public int MeltdownOutputBody { get; set; }

        [JsonProperty("meltdownOutputFeet")]
        public int MeltdownOutputFeet { get; set; }

        [JsonProperty("meltdownOutputHands")]
        public int MeltdownOutputHands { get; set; }

        [JsonProperty("meltdownOutputHead")]
        public int MeltdownOutputHead { get; set; }

        [JsonProperty("meltdownOutputShield")]
        public int MeltdownOutputShield { get; set; }

        [JsonProperty("maxProtection")]
        public float MaxProtection { get; set; }

        [JsonProperty("protectionPerArmor")]
        public float ProtectionPerArmor { get; set; }

        [JsonProperty("armorRatingPCMax")]
        public float ArmorRatingPCMax { get; set; } 

        [JsonProperty("armorRatingMax")]
        public float ArmorRatingMax { get; set; }
    }
    
    public class ArmorBinding : IBindable
    {
        [JsonProperty("substring")] public string Substring { get; set; } = "";

        [JsonProperty("identifier")] public string Identifier { get; set; } = "";
    }
    
    public class ArmorModifierBindings
    {
        [JsonProperty("binding")] public List<ArmorBinding> Binding { get; set; } = new();
    }
    
    public class ArmorModifier
    {
        [JsonProperty("identifier")] public string Identifier { get; set; } = "";

        [JsonProperty("factorArmor")] public string FactorArmor { get; set; } = "";

        [JsonProperty("factorValue")] public string FactorValue { get; set; } = "";

        [JsonProperty("factorWeight")] public string FactorWeight { get; set; } = "";
    }
    
    public class ArmorModifiers
    {
        [JsonProperty("armor_modifier")] public List<ArmorModifier> ArmorModifier { get; set; } = new();
    }
    
    public class ArmorMaterialBindings
    {
        [JsonProperty("binding")] public List<ArmorBinding> Binding { get; set; } = new();
    }
    
    public class ArmorMaterial : IBindable
    {
        [JsonProperty("armorBase")] public string ArmorBase { get; set; } = "";

        [JsonProperty("identifier")]
        public string Identifier { get; set; } = "";

        [JsonProperty("type")] public ArmorType? Type { get; set; } = ArmorType.UNDEFINED;

        [JsonProperty("materialMeltdown")] public string MaterialMeltdown { get; set; } = "";

        [JsonProperty("materialTemper")]
        public string MaterialTemper { get; set; } = "";

        public enum ArmorType
        {
            UNDEFINED,
            LIGHT,
            HEAVY,
            BOTH,
        }
    }
    
    
    public class ArmorMaterials
    {
        [JsonProperty("armor_material")] public List<ArmorMaterial> ArmorMaterial { get; set; } = new();
    }
    
    public class ArmorMasqueradeBinding
    {
        [JsonProperty("substringArmor")]
        public string SubstringArmor { get; set; } = "";

        [JsonProperty("masqueradeFaction")]
        public string MasqueradeFaction { get; set; } = "";
    }
    
    public class ArmorMasqueradeBindings
    {
        [JsonProperty("armor_masquerade_binding")]
        public List<ArmorMasqueradeBinding> ArmorMasqueradeBinding { get; set; } = new();
    }
    
    public class Exclusion
    {
        [JsonProperty("text")] public string Text { get; set; } = "";
        [JsonProperty("target")] public string Target { get; set; } = "";
        [JsonProperty("type")] public string Type { get; set; } = "";
    }
    
    public class ReforgeExclusions
    {
        [JsonProperty("exclusion")]
        public List<Exclusion> Exclusion { get; set; } = new();
    }
    
    public class Armor
    {
        [JsonProperty("armor_settings")]
        public ArmorSettings ArmorSettings { get; set; } = new();

        [JsonProperty("armor_modifier_bindings")]
        public ArmorModifierBindings ArmorModifierBindings { get; set; } = new();

        [JsonProperty("armor_modifiers")]
        public ArmorModifiers ArmorModifiers { get; set; } = new();

        [JsonProperty("armor_material_bindings")]
        public ArmorMaterialBindings ArmorMaterialBindings { get; set; } = new();

        [JsonProperty("armor_materials")]
        public ArmorMaterials ArmorMaterials { get; set; } = new();

        [JsonProperty("armor_masquerade_bindings")]
        public ArmorMasqueradeBindings ArmorMasqueradeBindings { get; set; } = new();

        [JsonProperty("reforge_exclusions")]
        public ReforgeExclusions ReforgeExclusions { get; set; } = new();
    }
}