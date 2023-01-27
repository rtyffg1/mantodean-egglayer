using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace AddSpawn
{
    public class EggSpawnComp : ThingComp
    {
        public override void CompTick()
        {
            if (parent.Map != null)
            {
                //                Log.Error("hatcher get");
                //                CompHatcher ehatcher = this.parent.TryGetComp<CompHatcher>();
                //                Log.Error("Hatcher got");
                //                if (ehatcher == null)
                //                {
                //                    Log.Error("Not foung hatcher to give faction");
                //                    return;
                //                }
                //                Log.Error("Setting faction");
                //                ehatcher.hatcheeFaction = Faction.OfPlayer;

                //                Log.Error("Spawning");
                if (this.Props.Eggcount > 1)
                {
                    for (int i = 0; i < this.Props.Eggcount; i++)
                    {
                        this.SpawnEgg();
                    }
                }
                else
                {
                    this.SpawnEgg();
                }
                this.parent.Destroy();
//                Log.Error("Spawned");
            }
        }

        /// <summary>
        /// By default props returns the base CompProperties class.
        /// You can get props and cast it everywhere you use it, 
        /// or you create a Getter like this, which casts once and returns it.
        /// Careful of case sensitivity!
        /// </summary>
        public EggSpawnCompProperties Props => (EggSpawnCompProperties)this.props;

        public bool ExampleBool => Props.myExampleBool;

        public void SpawnEgg()
        {
            //            PawnGenerationRequest request = new PawnGenerationRequest(this.Props.Pawnkind, Faction.OfPlayer, PawnGenerationContext.NonPlayer);
            //            Pawn pawn = PawnGenerator.GeneratePawn(request);
            Thing ething = ThingMaker.MakeThing(this.Props.Eggdef);
            CompHatcher ehatcher = ething.TryGetComp<CompHatcher>();
            if (ehatcher == null)
            {
                Log.Error("Not foung hatcher to give faction");
                return;
            }
            ehatcher.hatcheeFaction = Faction.OfPlayer;

            GenSpawn.Spawn(ething, parent.Position, parent.Map);
        }
    }

    public class EggSpawnCompProperties : CompProperties
    {
        public bool myExampleBool;
        public float myExampleFloat;
        public ThingDef Eggdef;
        public int Eggcount = 1;

        /// <summary>
        /// These constructors aren't strictly required if the compClass is set in the XML.
        /// </summary>
        public EggSpawnCompProperties()
        {
            this.compClass = typeof(EggSpawnComp);
        }

        public EggSpawnCompProperties(Type compClass) : base(compClass)
        {
            this.compClass = compClass;
        }
    }
}
