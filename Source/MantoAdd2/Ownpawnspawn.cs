using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace AddSpawn
{
    public class OwnPawnSpawnComp : ThingComp
    {
        public bool didspawn = false;
        public override void CompTick()
        {
            if (parent.Map != null)
            {
                if (!didspawn)
                {
                    didspawn = true;
                    this.SpawnDude();
                }
                else
                    this.parent.Destroy();
            }
        }

        /// <summary>
        /// By default props returns the base CompProperties class.
        /// You can get props and cast it everywhere you use it, 
        /// or you create a Getter like this, which casts once and returns it.
        /// Careful of case sensitivity!
        /// </summary>
        public OwnPawnSpawnCompProperties Props => (OwnPawnSpawnCompProperties)this.props;

        public bool ExampleBool => Props.myExampleBool;

        public void SpawnDude()
        {
            Verse.PawnGenerationRequest request = new Verse.PawnGenerationRequest(this.Props.Pawnkind, Faction.OfPlayer, PawnGenerationContext.NonPlayer, fixedBiologicalAge : Props.StartingAge, fixedChronologicalAge : Props.StartingAge);
            if (Props.isfixedgender)
            {
                request.FixedGender = Props.fixedgender;
            }
            Pawn pawn = PawnGenerator.GeneratePawn(request);
            if (!pawn.RaceProps.Humanlike)
            {
                if (!pawn.training.HasLearned(DefDatabase<TrainableDef>.GetNamed("Obedience")) && pawn.training.CanBeTrained(DefDatabase<TrainableDef>.GetNamed("Obedience")))
                    pawn.training.Train(DefDatabase<TrainableDef>.GetNamed("Obedience"), null, true);
                if (!pawn.training.HasLearned(DefDatabase<TrainableDef>.GetNamed("Release")) && pawn.training.CanBeTrained(DefDatabase<TrainableDef>.GetNamed("Release")))
                    pawn.training.Train(DefDatabase<TrainableDef>.GetNamed("Release"), null, true);
//                pawn.training.pawn = null;
/*                if (!pawn.training.HasLearned(DefDatabase<TrainableDef>.GetNamed("KillingTraining")) && pawn.training.CanBeTrained(DefDatabase<TrainableDef>.GetNamed("KillingTraining")))
                    pawn.training.Train(DefDatabase<TrainableDef>.GetNamed("KillingTraining"), pawn, true);*/
            }
            else
            {
                if (Props.backstory != null)
                {
//                    BackstoryDef tstory;
//                    string tstoryname = BackstoryDatabase.GetIdentifierClosestMatch(Props.backstory);
//                    Log.Error("tstoryname  " + tstoryname.ToStringSafe());
//                    BackstoryDatabase.TryGetWithIdentifier(tstoryname, out tstory);
//                    Log.Error("tstory " + tstory.ToStringSafe());
//                    pawn.story.Adulthood = tstory;// BackstoryDatabase.RandomBackstory(BackstorySlot.Adulthood);
                }
            }

//            Log.Error("starting spawn");
            GenSpawn.Spawn(pawn, parent.Position, parent.Map);

//            Log.Error("starting hediffs");
            if (Props.StartingHediffs != null)
            {
                for (int i = 0; i < Props.StartingHediffs.Count; i++)
                {
//                    Log.Error("hediff" + Props.StartingHediffs[i]);
                    BodyPartRecord CurrentTarget = null;
                    if (Props.HediffTargets != null)
                        if (Props.HediffTargets.Count > i)
                        {
                            //                    CurrentTarget = Props.HediffTargets[i];
                            //                        Log.Error(pawn.RaceProps.body.GetPartAtIndex(0).ToStringSafe());
                            //                        Log.Error(pawn.RaceProps.body.GetPartAtIndex(1).ToStringSafe());
                            CurrentTarget = pawn.RaceProps.body.GetPartsWithDef(Props.HediffTargets[i]).First();
                            //                        Log.Error(CurrentTarget.ToStringSafe());
                        }
                    if (Props.StartingHediffs[i] != null)
                    {
                        Log.Error(Props.StartingHediffs[i].ToStringSafe());
                        if (CurrentTarget == null)
                        {
//                            Log.Error("general hediff" + Props.StartingHediffs[i]);
                            CurrentTarget = pawn.RaceProps.body.GetPartAtIndex(0);
                            pawn.health.AddHediff(Props.StartingHediffs[i], CurrentTarget);
                        }
                        else
                        {
//                            Log.Error("local hediff" + Props.StartingHediffs[i]);
                            pawn.health.AddHediff(Props.StartingHediffs[i], CurrentTarget);
                        }
                    }
                }
            }

        }
    }

    public class OwnPawnSpawnCompProperties : CompProperties
    {
        public bool myExampleBool;

        public bool isfixedgender = false;
        public Gender fixedgender = Gender.None;
        public float StartingAge = 0.5f;
        public PawnKindDef Pawnkind;
        public List<HediffDef> StartingHediffs = null;
        public List<BodyPartDef> HediffTargets = null;
        public string backstory = null;

        //        public HediffDef StartingHediff;
        public float ExFloat;

        //todo struct for starting hediffs

        /// <summary>
        /// These constructors aren't strictly required if the compClass is set in the XML.
        /// </summary>
        public OwnPawnSpawnCompProperties()
        {
            this.compClass = typeof(OwnPawnSpawnComp);
        }

        public OwnPawnSpawnCompProperties(Type compClass) : base(compClass)
        {
            this.compClass = compClass;
        }
    }
}
