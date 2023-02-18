using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using RimWorld;
using Verse;

namespace Dangerchem.PawnReplace
{
    public class DefModExt_PawnReplace : DefModExtension
    {
        public PawnKindDef defaultPawnKind;
        public PawnKindDef animalPawnKind = null;
        /// <summary>
        /// Viable inputs for conversion. Uses the defName of the race itself. Should be able to accept any pawn race if left 'null'.
        /// </summary>
        public List<ThingDef> requiredinputDefs = null;

        public float requiredsize = 0.7f;

        public bool isfixedgender = false;
        public Gender fixedgender = Gender.None;
        public float StartingAge = 0.5f;
        public float AnimalStartingAge = 0.5f;
        public List<HediffDef> StartingHediffs = null;
        public List<BodyPartDef> HediffTargets = null;
        public string backstory = null;

        public bool forceDropEquipment;
        public bool killiflowmass = false;
        public bool animalifwrongrace = false;
        public bool killifwrongrace = false;
        public bool killPawn;
    }
}
