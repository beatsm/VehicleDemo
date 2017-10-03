using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using NewFront2.Actors;
using TaxiFrontend.Actors;

namespace TaxiFrontend.Hubs
{
    [HubName("positionHub")]
    public class PositionHub : Hub
    {
        public PositionHub()
        {
            
        }
        public void OnUpdateBounds(PresentingActor.UpdatedBounds updatedBounds)
        {
            updatedBounds.UserId = Context.ConnectionId;
            ActorSystem.PresenterActor.Tell(updatedBounds);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
        //    ActorSystem.PresenterActor.Tell(new PresentingActor.Disconnected(Context.ConnectionId));
            return base.OnDisconnected(stopCalled);
        }
    }
}