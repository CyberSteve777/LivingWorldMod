﻿using System;
using LivingWorldMod.Custom.Classes;
using LivingWorldMod.Custom.Utilities;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LivingWorldMod.Core.Patches {
    /// <summary>
    /// Loadable class that applies IL/On patches related to NPC AI.
    /// </summary>
    //TODO: Finish NPC umbrella stuff
    [Autoload(false)]
    public class NPCAIPatches : LoadablePatch {
        public override void LoadPatches() {
            IL_NPC.AI_007_TownEntities += TownNPCAI;
        }

        private void TownNPCAI(ILContext il) {
            currentContext = il;
            //REALLY simply patch. Remove the check for rain that prevents Town NPCs from walking around.

            ILCursor c = new(il);

            //Navigate to RIGHT after rain check
            c.ErrorOnFailedGotoNext(MoveType.After, i => i.MatchLdsfld<Main>(nameof(Main.raining)));
            //Pop its normal value
            c.Emit(OpCodes.Pop);
            //Emit our own check (Only vanilla NPCs can walk around during rain)
            c.Emit(OpCodes.Ldarg_0);
            c.EmitDelegate<Func<NPC, bool>>(npc => npc.type >= NPCID.Count && Main.raining);
        }
    }
}