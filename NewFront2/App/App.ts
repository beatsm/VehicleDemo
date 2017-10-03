module App {
    export class Application {
        private server: IPositionServer = null;
        private client: PositionClient = null;

        constructor() {
        //    track(this);
        }

        init = () => {

           
            this.server = $.connection.positionHub.server;
            this.client = new PositionClient(this.server.onUpdateBounds);

            $.connection.positionHub.client = this.client;
            $.connection.hub.error(err => {
                console.log("HUB ERROR : " + err);
            });
            $.connection.hub.start({ transport: ['webSockets', 'longPolling'] }).done(_ => this.client.initMap());

         //   ko.applyBindings(this, document.body);
        };
    }
}

var app = new App.Application();

$(app.init);