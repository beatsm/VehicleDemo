var App;
(function (App) {
    var Application = (function () {
        function Application() {
            var _this = this;
            this.server = null;
            this.client = null;
            this.init = function () {
                _this.server = $.connection.positionHub.server;
                _this.client = new App.PositionClient(_this.server.onUpdateBounds);
                $.connection.positionHub.client = _this.client;
                $.connection.hub.start().done(function (_) { return _this.client.initMap(); });
            };
        }
        return Application;
    }());
    App.Application = Application;
})(App || (App = {}));
var app = new App.Application();
$(app.init);
//# sourceMappingURL=App.js.map