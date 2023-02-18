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
    public class Hediff_PawnReplace : HediffWithComps
    {
        public override void Tick()
        {
            base.Tick();

            if(Severity >= 1.0)
            {
                if (pawn.Map != null)
                {
                    DoBasicConvert();
                }
            }
        }

        public static bool IsViableRace(Pawn pawn, DefModExt_PawnReplace modExt)
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
            DefModExt_PawnReplace modExt = def.GetModExtension<DefModExt_PawnReplace>();
            bool killpawn = false;
            bool shouldconvert = true;
            Pawn convertedPawn;
            Verse.PawnGenerationRequest request = new Verse.PawnGenerationRequest(modExt.defaultPawnKind,
                Faction.OfPlayer, PawnGenerationContext.NonPlayer,
                fixedBiologicalAge: modExt.StartingAge, fixedChronologicalAge: modExt.StartingAge);
//            PawnGenerationRequest request = new PawnGenerationRequest(modExt.defaultPawnKind, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, true, false, false, true, false, 1f, false, true, true, false, false, false, false, false, 0f, 0f, null, 0f, null, null, null, null, null, modExt.StartingAge, modExt.StartingAge, null, null, null, null, null, null);

            /*            Verse.PawnGenerationRequest request = new Verse.PawnGenerationRequest(
                            modExt.defaultPawnKind,
                            faction: Faction.OfPlayer,
                            context: PawnGenerationContext.NonPlayer,
                            forceGenerateNewPawn: true,
                            canGeneratePawnRelations: false,
                            colonistRelationChanceFactor: 0f,
                            fixedBiologicalAge: modExt.StartingAge,
                            fixedChronologicalAge: modExt.StartingAge,
            //                allowFood: false,
                            allowAddictions: false);*/
            request.CanGeneratePawnRelations = false;
            request.ColonistRelationChanceFactor = 0f;
            request.AllowAddictions = false;
            request.AllowDowned = true;
            if (modExt.isfixedgender)
            {
                request.FixedGender = modExt.fixedgender;
            }

//            Log.Error("Checking race");
            if (!IsViableRace(pawn, modExt))
            {
                if (modExt.animalifwrongrace)
                {
                    request.KindDef = modExt.animalPawnKind;
                    request.FixedBiologicalAge = modExt.AnimalStartingAge;
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
                //                if (pawn.needs.food.CurLevel < 0.5);

                //checking if pawn is animal and if there is an animal conversion
                if (!pawn.RaceProps.Humanlike && modExt.animalPawnKind != null)
                {
                    request.KindDef = modExt.animalPawnKind;
                    request.FixedBiologicalAge = pawn.ageTracker.AdultMinAge;
                    request.FixedChronologicalAge = pawn.ageTracker.AdultMinAge;

                }
            }

//            Log.Error("Checking convert");
            if (shouldconvert)
            {
//                Log.Error(request.ToString());
                convertedPawn = PawnGenerator.GeneratePawn(request);
//                Log.Error("Checking justgenerated");
                if (convertedPawn.RaceProps.Humanlike)
                {
                    if (modExt.backstory != null)
                    {
//                        Backstory tstory;
//                        string tstoryname = BackstoryDatabase.GetIdentifierClosestMatch(modExt.backstory);
//                        BackstoryDatabase.TryGetWithIdentifier(tstoryname, out tstory);
//                        convertedPawn.story.adulthood = tstory;// BackstoryDatabase.RandomBackstory(BackstorySlot.Adulthood);

                    }
                }
                else
                {
                    if (!convertedPawn.training.HasLearned(DefDatabase<TrainableDef>.GetNamed("Obedience")) && convertedPawn.training.CanBeTrained(DefDatabase<TrainableDef>.GetNamed("Obedience")))
                        convertedPawn.training.Train(DefDatabase<TrainableDef>.GetNamed("Obedience"), null, true);
                    if (!convertedPawn.training.HasLearned(DefDatabase<TrainableDef>.GetNamed("Release")) && convertedPawn.training.CanBeTrained(DefDatabase<TrainableDef>.GetNamed("Release")))
                        convertedPawn.training.Train(DefDatabase<TrainableDef>.GetNamed("Release"), null, true);
                    //                pawn.training.pawn = null;
                    /*                if (!pawn.training.HasLearned(DefDatabase<TrainableDef>.GetNamed("KillingTraining")) && pawn.training.CanBeTrained(DefDatabase<TrainableDef>.GetNamed("KillingTraining")))
                                        pawn.training.Train(DefDatabase<TrainableDef>.GetNamed("KillingTraining"), pawn, true);*/
                }


                //                GenPlace.TryPlaceThing(convertedPawn, pawn.Position, pawn.Map, ThingPlaceMode.Direct);
                /*if (!PawnUtility.TrySpawnHatchedOrBornPawn(convertedPawn, pawn))
                {
                    Find.WorldPawns.PassToWorld(convertedPawn, RimWorld.Planet.PawnDiscardDecideMode.Discard);
                }*/

//                Log.Error("Checking tospawn");
                GenSpawn.Spawn(convertedPawn, pawn.Position, pawn.Map);
//                Log.Error("Checking justspawned");

                if (modExt.killPawn)
                    killpawn = true;
//                Log.Error("starting hediffs");
                if (modExt.StartingHediffs != null)
                {
                    for (int i = 0; i < modExt.StartingHediffs.Count; i++)
                    {
//                        Log.Error("hediff" + modExt.StartingHediffs[i]);
                        BodyPartRecord CurrentTarget = null;
                        if (modExt.HediffTargets != null)
                            if (modExt.HediffTargets.Count > i)
                            {
                                CurrentTarget = convertedPawn.RaceProps.body.GetPartsWithDef(modExt.HediffTargets[i]).First();
                            }
                        if (modExt.StartingHediffs[i] != null)
                        {
                            if (CurrentTarget == null)
                            {
                                CurrentTarget = convertedPawn.RaceProps.body.GetPartAtIndex(0);
                                convertedPawn.health.AddHediff(modExt.StartingHediffs[i], CurrentTarget);
                            }
                            else
                            {
                                convertedPawn.health.AddHediff(modExt.StartingHediffs[i], CurrentTarget);
                            }
                        }
                    }
                }
            }
//            Log.Error("Checking drop");
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
                pawn.Destroy(DestroyMode.Vanish);
                //pawn.Destroy();
            }
        }
    }
}
