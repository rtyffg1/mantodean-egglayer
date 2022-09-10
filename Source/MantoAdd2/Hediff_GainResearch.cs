using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace Dangerchem.GainResearch
{
    public class Hediff_GainResearch : HediffWithComps
    {

        public override void Tick()
        {
            base.Tick();

            if ((Find.TickManager.TicksGame % 300) == 0)
            {
                if (pawn.RaceProps.Humanlike && (pawn.IsColonist || pawn.IsPrisoner))
                    Find.ResearchManager.ResearchPerformed(1000, null);
            }
        }

    }
}
