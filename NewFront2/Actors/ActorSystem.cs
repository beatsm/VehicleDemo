using NewFront2.Providers;
using Proto;
using TaxiShared;

namespace NewFront2.Actors
{
    public static class ActorSystem
    {
        public static PID PresenterActor;

        public static void Start()
        {
            PresenterActor = Actor.Spawn(Actor.FromProducer(() => new PresentingActor()));


            var vehicleManagerActor = Actor.SpawnNamed(Actor.FromProducer(() => new VehicleManagerActor(PresenterActor)), "publisher");

            VästtrafikProvider.Run(vehicleManagerActor);

            LadotProvider.Run(vehicleManagerActor);

        }
    }
}