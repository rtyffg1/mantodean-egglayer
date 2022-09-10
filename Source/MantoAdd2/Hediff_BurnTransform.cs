using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace Dangerchem.PawnReplace
{
    public class Hediff_BurnTransform : HediffWithComps
    {
        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            if (base.Part != null && base.Part.coverageAbs <= 0f)
            {
                Log.Error(string.Concat(new object[]
                {
                    "Added injury to ",
                    base.Part.def,
                    " but it should be impossible to hit it. pawn=",
                    this.pawn.ToStringSafe<Pawn>(),
                    " dinfo=",
                    dinfo.ToStringSafe<DamageInfo?>()
                }), false);
            }
        }

        public override void Tick()
        {
            DefModExt_BurnTransform modExt = def.GetModExtension<DefModExt_BurnTransform>();

            //            EffectTick();
            base.Tick();

            //            if (Severity >= 0.1)
/*            Log.Error(string.Concat(new object[]
                {
                    "burn ",
                    Find.TickManager.TicksGame
                }), false);*/
            if ((Find.TickManager.TicksGame % modExt.TickTime) == 0)
            {
                //Log.Error("burn1");
                Thing aitem = ThingMaker.MakeThing(modExt.GenItem);
                pawn.inventory.innerContainer.TryAdd(aitem, modExt.Itemcount,true);
            }
            if ((Find.TickManager.TicksGame % modExt.TickTime2) == 0)
            {
                //Log.Error("burn2");
                //BodyPartRecord bPart;
                //bPart = null;// pawn.RaceProps.body.GetPartAtIndex(targetnum);
                //                BodyPartRecord tarpart = pawn.health.hediffSet.GetRandomNotMissingPart(DamageDefOf.Frostbite, BodyPartHeight.Bottom, BodyPartDepth.Outside);
                /*                BodyPartRecord tarpart = pawn.health.hediffSet.GetRandomNotMissingPart(DamageDefOf.Frostbite, BodyPartHeight.Bottom);
                                if (tarpart == null)
                                    tarpart = pawn.health.hediffSet.GetRandomNotMissingPart(DamageDefOf.Frostbite, BodyPartHeight.Middle);
                                if (tarpart == null)
                                    tarpart = pawn.health.hediffSet.GetRandomNotMissingPart(DamageDefOf.Frostbite, BodyPartHeight.Undefined, BodyPartDepth.Undefined);*/
                /*                BodyPartRecord tarpart = pawn.health.hediffSet.GetRandomNotMissingPart(DamageDefOf.Frostbite, BodyPartHeight.Undefined, BodyPartDepth.Outside);
                                if (tarpart == null)
                                    tarpart = pawn.health.hediffSet.GetRandomNotMissingPart(DamageDefOf.Frostbite);*/
                //                DamageInfo dinfo = new DamageInfo(DamageDefOf.Frostbite, modExt.DamageTick, 999, -1.0f, null, tarpart, null);
                DamageInfo dinfo = new DamageInfo(DamageDefOf.Frostbite, modExt.DamageTick, 999, -1.0f, null, null, null);
                dinfo.SetInstantPermanentInjury(true);
                DamageWorker.DamageResult dres = pawn.TakeDamage(dinfo);
            }
        }

    }
}
