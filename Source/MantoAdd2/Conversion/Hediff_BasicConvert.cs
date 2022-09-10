using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using RimWorld;
using Verse;

namespace O21Toolbox_edit.PawnConversion
{
    public class Hediff_BasicConvert : HediffWithComps
    {
        public override void Tick()
        {
            base.Tick();

            if(Severity >= 1.0)
            {
                DoBasicConvert();
            }
        }

        public static bool IsViableRace(Pawn pawn, DefModExt_BasicConvert modExt)
        {
            if (modExt.requiredinputDefs != null)
            {
                if (!modExt.requiredinputDefs.Contains(pawn.def))
                {
                    return false;
                }
            }
            return true;
        }

        private void DoBasicConvert()
        {
            DefModExt_BasicConvert modExt = def.GetModExtension<DefModExt_BasicConvert>();
            bool killpawn = false;
            bool shouldconvert = true;
            PawnGenerationRequest request = new PawnGenerationRequest(
                modExt.defaultPawnKind,
                faction: Faction.OfPlayer,
                forceGenerateNewPawn: true,
                canGeneratePawnRelations: false,
                colonistRelationChanceFactor: 0f,
                fixedBiologicalAge: pawn.ageTracker.AgeBiologicalYearsFloat,
                fixedChronologicalAge: pawn.ageTracker.AgeChronologicalYearsFloat,
                allowFood: false,
                allowAddictions: false);


            if (!IsViableRace(pawn, modExt))
            {
                if (modExt.animalifwrongrace)
                {
                    request.KindDef = modExt.animalPawnKind;
                }
                if (modExt.killifwrongrace)
                {
                    killpawn = true;
                    shouldconvert = false;
                }
                if (modExt.requiredsize > 0)
                {
                    /*Log.Error(string.Concat(new object[]
                    {
                        pawn.BodySize,
                        " size against ",
                        modExt.requiredsize,
                    }), false);*/
                    if (pawn.BodySize < modExt.requiredsize)
                    {
                        killpawn = true;
                        shouldconvert = false;
                    }
                }
                //checking if pawn is animal and if there is an animal conversion
                if (!pawn.RaceProps.Humanlike && modExt.animalPawnKind != null)
                    request.KindDef = modExt.animalPawnKind;
            }

            if (shouldconvert)
            {
                Pawn convertedPawn = PawnGenerator.GeneratePawn(request);
                GenPlace.TryPlaceThing(convertedPawn, pawn.Position, pawn.Map, ThingPlaceMode.Direct);
                if (modExt.killPawn)
                    killpawn = true;
            }
            if (modExt.forceDropEquipment)
            {
                if(pawn.inventory != null)
                {
                    pawn.inventory.DropAllNearPawn(pawn.Position);
                }
                if(pawn.apparel != null)
                {
                    pawn.apparel.DropAll(pawn.Position);
                }
                if(pawn.equipment != null)
                {
                    pawn.equipment.DropAllEquipment(pawn.Position);
                }
            }
            if (killpawn)
            {
                pawn.Kill(null, this);
            }
            else
            {
                pawn.Destroy();
            }
        }
    }
}
