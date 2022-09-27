using System.Linq;
using Microsoft.Extensions.Logging;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;
using SynthusMaximus.Data;
using static Mutagen.Bethesda.FormKeys.SkyrimSE.PerkusMaximus_Master.Global;

namespace SynthusMaximus.Patchers
{
    public class GlobalVariablePatcher : APatcher<GlobalVariablePatcher>
    {
        public GlobalVariablePatcher(ILogger<GlobalVariablePatcher> logger, DataStorage storage, IPatcherState<ISkyrimMod, ISkyrimModGetter> state) : base(logger, storage, state)
        {
        }

        protected override void RunPatcherInner()
        {
            var isMage = (GlobalShort)Patch.Globals.GetOrAddAsOverride(xMAIsPerMaMageRunning.Resolve(State.LinkCache));
            isMage.Data = (short) (Storage.UseMage ? 1 : 0);
            
            var isWarrior = (GlobalShort)Patch.Globals.GetOrAddAsOverride(xMAIsPerMaWarriorRunning.Resolve(State.LinkCache));
            isWarrior.Data = (short) (Storage.UseWarrior ? 1 : 0);
            
            var isThief = (GlobalShort)Patch.Globals.GetOrAddAsOverride(xMAIsPerMaThiefRunning.Resolve(State.LinkCache));
            isThief.Data = (short) (Storage.UseThief ? 1 : 0);
        }
    }
}
