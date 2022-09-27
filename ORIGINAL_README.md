## Synthus Maximus - Perkus Maximus Patcher

### Introduction
This is a complete rewrite of the Perkus Maximus Patcher (Patchus Maximus) for SE using the Synthesis framework. Synthesis uses the Mutagen library 
allow programmers to build lightning fast patchers for Skyrim SE. In sort, this code replaces the old SkyProc patcher for Perkus Maximus

### Current State
The project is "code complete", meaning I've rewriten 100% of the code, but the resulting output has yet to be fully playtested and we need to write
config files for a lot of modern mods, see the Overlay section for more info on how to write these files.

### Overlay Files
This patcher loads a lot of configuration data, but does so in an extensible way. It first reads all the mods currently enabled, then loads config
files in the same order as those mods. So if your modlist is: 

* Skyrim.esm
* PerkusMaximus_Master.esp
* ModA.esp
* ModB.esp

If the loader is looking for `exclusions\npcs.json` it would look for files in the following order:

1. `Data\config\Skyrim.esm\exclusions\npcs.json`
1. `<Patcher Folder>\config\Skyrim.esm\exclusions\npcs.json`
1. `Data\config\PerkusMaximus_Master.esp\exclusions\npcs.json`
1. `<Patcher Folder>\config\PerkusMaximus_Master.esp\exclusions\npcs.json`
1. `Data\config\ModA.esp\exclusions\npcs.json`
1. `<Patcher Folder>\config\ModA.esp\exclusions\npcs.json`
1. `Data\config\ModB.esp\exclusions\npcs.json`
1. `<Patcher Folder>\config\ModB.esp\exclusions\npcs.json`

Each file is configured via a merge mode in the patcher code, for example, exclusion lists use a list concat merge mode, the contents of `npcs.json`
may look like this:

```json
{
  "EDID": [
    "Mannequin",
    "^dunSnaplegSpriggan$",
    "^EncSprigganSwarm$",
    "ARTHLALShip",
    "3DNPC",
    "Chicken"
  ],
  "NAME": [
    "NO\\ TEXT"
  ],
  "FORMID": []
}
```

In this case, each new file loaded, concats new values to the end of each sub-list in the above json example.

So to add support for your mod, just add a file in the `Data\config\<my_mod>\` folder that fits for the data you wish to add, and when it's enabled in 
MO2 the pather will pick the data up when it runs. Be sure to check the logger output of the patcher to confirm that it's working. 

### Data formats
#### Exclusions Lists 
Most exclusions lists follow the same a key, that represents the field to match against, and a regex that will exclude an item that matches on the given 
field.

Currently supported matcher fields:

* EDID - Match on the record's EntityID
* NAME - Match on the item's English Name
* MODNAME - Match on the item's mod Name (Skyrim.esm, myMod.esp, etc.)
* FORMID - Match on a FormID (in hex format)

Example Data:
```
{
  "EDID": [
    "Mannequin",
    "^dunSnaplegSpriggan$",
    "^EncSprigganSwarm$",
    "ARTHLALShip",
    "3DNPC",
    "Chicken"
  ],
  "NAME": [
    "NO\\ TEXT"
  ],
  "MODNAME": [
    "^Mymod\\.esm$"
  ],
  "FORMID": []
}
```


## Reworking Item Distribution
### Introduction
Item distribution in the old PerMa patcher is a bit wack. It works by requiring mod authors
to link new enchantments to vanilla enchantments. The patcher then finds all instances of the
base enchantment on leveled items, then duplicates those items with the new enchantment and adds
them to the leveled list. This requires authors to link completely unrelated enchantments. 

There's also a way to tell the patcher to "add similar" armors into the leveled list, so then this causes a
combinatorial explosion of armors and enchantments. There has to be a better way. 

### Standard approaches to adding Armors / Weapons
What's strange is that mod authors already have a system for adding enchantments and items to the game:
armor makers make armors and add them to vanilla leveled lists. Enchantment makers enchant vanilla items
and add them to vanilla leveled lists.

### New approach taken by this patcher (in the 0.3 release)

1. Find all armors, classify them by slot (feet, hands, head, etc.), and material type, and item level
2. Find all weapons, classify them by type, and material, and item level
3. Find all enchantments on these items, classify them by enchantment school, and item level
4. Create variants of vanilla items with new combinations of item and enchantments, grouped by level 
   and enchantment school/level. Don't duplicate existing item/enchantment combinations
5. Distribute these items into leveled lists, perhaps with a cap so we don't explode into thousands of items?   

Note: enchantment levels can't be based on the min level of the magic effect. New mods like Odin use scaling
enchantment magnitude to scale a item. So we should instead use the level list level. 