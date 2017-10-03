module App {
    export class Application {
        private server: IPositionServer = null;
        private client: PositionClient = null;

        init = () => {

           
            this.server = $.connection.positionHub.server;
            this.client = new PositionClient(this.server.onUpdateBounds);

            $.connection.positionHub.client = this.client;
            $.connection.hub.start().done(_ => this.client.initMap());
        };
    }
}

var app = new App.Application();

$(app.init);