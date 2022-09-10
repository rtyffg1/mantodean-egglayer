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
        public override void CompTick()
        {
            if (parent.Map != null)
            {
                this.SpawnDude();
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
            Verse.PawnGenerationRequest request = new Verse.PawnGenerationRequest(this.Props.Pawnkind, Faction.OfPlayer, PawnGenerationContext.NonPlayer);
            Pawn pawn = PawnGenerator.GeneratePawn(request);
            if (!pawn.RaceProps.Humanlike)
            {
                if (!pawn.training.HasLearned(DefDatabase<TrainableDef>.GetNamed("Obedience")) && pawn.training.CanBeTrained(DefDatabase<TrainableDef>.GetNamed("Obedience")))
                    pawn.training.Train(DefDatabase<TrainableDef>.GetNamed("Obedience"), pawn, true);
                if (!pawn.training.HasLearned(DefDatabase<TrainableDef>.GetNamed("Release")) && pawn.training.CanBeTrained(DefDatabase<TrainableDef>.GetNamed("Release")))
                    pawn.training.Train(DefDatabase<TrainableDef>.GetNamed("Release"), pawn, true);
/*                if (!pawn.training.HasLearned(DefDatabase<TrainableDef>.GetNamed("KillingTraining")) && pawn.training.CanBeTrained(DefDatabase<TrainableDef>.GetNamed("KillingTraining")))
                    pawn.training.Train(DefDatabase<TrainableDef>.GetNamed("KillingTraining"), pawn, true);*/
            }

            if (this.Props.HediffDef != null)
                pawn.health.AddHediff(this.Props.HediffDef);

            GenSpawn.Spawn(pawn, parent.Position, parent.Map);
        }
    }

    public class OwnPawnSpawnCompProperties : CompProperties
    {
        public bool myExampleBool;
        public float myExampleFloat;
        public PawnKindDef Pawnkind;
        public HediffDef HediffDef;
        public float ExFloat;

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
