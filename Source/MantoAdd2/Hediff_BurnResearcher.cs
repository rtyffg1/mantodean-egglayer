using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace Dangerchem.BurnResearcher
{
    public class Hediff_BurnResearcher : HediffWithComps
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
//            EffectTick();
            base.Tick();

//            if (Severity >= 0.1)
            if ((Find.TickManager.TicksGame % 300) == 0)
            {
                //Thing aitem = ThingMaker.MakeThing(ThingDef.Named("WoodLog"));
                //pawn.inventory.innerContainer.TryAdd(aitem, 10);
//                pawn.inventory.TryAddItemNotForSale();
                if (pawn.RaceProps.Humanlike && (pawn.IsColonist || pawn.IsPrisoner))
                    Find.ResearchManager.ResearchPerformed(1000, null);
                if ((Find.TickManager.TicksGame % 1500) == 0)
                {
                    //BodyPartRecord bPart;
                    //bPart = null;// pawn.RaceProps.body.GetPartAtIndex(targetnum);
                    DamageInfo dinfo = new DamageInfo(DamageDefOf.Frostbite, 5, 999, -1.0f, null, null, null);
                    dinfo.SetInstantPermanentInjury(true);
                    DamageWorker.DamageResult dres = pawn.TakeDamage(dinfo);
                }
            }
        }

        private void EffectTick()
        {
            float dam = 10;
            DamageWorker.DamageResult dres = pawn.TakeDamage(new DamageInfo(DamageDefOf.Rotting, dam, 999, -1.0f, null, null, null));
        }
    }
}
