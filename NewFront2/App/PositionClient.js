var App;
(function (App) {
    var PositionClient = (function () {
        function PositionClient(onUpdatedBounds) {
            var _this = this;
            this.onUpdatedBounds = onUpdatedBounds;
            this.map = null;
            this.vehicles = [];
            this.searchText = "";
            this.includedStateValues = [App.GpsStatus.active];
            this.isIncluded = function (status) {
                return _this.includedStateValues.indexOf(status) !== -1;
            };
            this.filter = function (vehicle) {
                if (_this.isIncluded(vehicle.status) === false)
                    return false;
                if (_this.searchText.length > 0) {
                    return vehicle.id.indexOf(_this.searchText) !== -1;
                }
                return vehicle.inBounds;
            };
            this.initMap = function () {
                var mapOptions = {
                    zoom: 13,
                    center: new google.maps.LatLng(57.70887000, 11.97456000)
                };
                _this.map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);
                google.maps.event.addListener(_this.map, "idle", function () {
                    _this.vehicles.forEach(function (v) { return v.viewPortChanged(); });
                    _this.updateBounds();
                });
            };
            this.updateBounds = function () {
                var northEast = _this.map.getBounds().getNorthEast();
                var southWest = _this.map.getBounds().getSouthWest();
                var zoom = _this.map.getZoom();
                _this.onUpdatedBounds({
                    LatitudeNorthEast: northEast.lat(),
                    LongitudeNorthEast: northEast.lng(),
                    LatitudeSouthWest: southWest.lat(),
                    LongitudeSouthWest: southWest.lng(),
                    ZoomLevel: zoom,
                });
            };
            this.positionChanged = function (position) {
                var latlng = new google.maps.LatLng(position.Latitude, position.Longitude);
                var vehicle = _this.getVehicle(position.Id);
                vehicle.setPosition(position.Bearing, latlng, position.GpsStatus);
            };
            //TODO: use dictionary lookup
            this.getVehicle = function (id) {
                var vehicles = _this.vehicles.filter(function (d) { return d.id === id; });
                if (vehicles.length === 0) {
                    var item = new App.MovingVehicle(id, _this.map);
                    _this.vehicles.push(item);
                    return item;
                }
                var positionPair = vehicles[0];
                return positionPair;
            };
            //  track(this);
        }
        Object.defineProperty(PositionClient.prototype, "orderedVehicles", {
            get: function () {
                return this.vehicles.filter(this.filter)
                    .sort(function (a, b) { return a.id > b.id ? 1 : -1; });
            },
            enumerable: true,
            configurable: true
        });
        return PositionClient;
    }());
    App.PositionClient = PositionClient;
})(App || (App = {}));
//# sourceMappingURL=PositionClient.js.map