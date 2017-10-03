﻿module App {
    export interface IAggregatedData {
        Latitude: number;
        Longitude: number;
        VehicleCount: number;
    }

    export interface IPositionChanged {
        Id: string;
        Latitude: number;
        Longitude: number;
        Bearing: number;
        GpsStatus: GpsStatus;
        Source: string;
    }

    export interface IUpdatedBounds {
        LatitudeNorthEast: number;
        LongitudeNorthEast: number;
        LatitudeSouthWest: number;
        LongitudeSouthWest: number;
        ZoomLevel: number;
    }

    export interface IPositionServer {
        init: () => void;
        onUpdateBounds: IOnUpdateBounds;
        //joinSource: (source: string) => void;
        //leaveSource: (source: string) => void;
    }

    export interface IOnUpdateBounds {
        (bounds: IUpdatedBounds): void
    }

    export interface IPositionClient {
        positionChanged: (position: IPositionChanged) => void;
        //sourceAdded: (source: string) => void;
        //initialize: (sources: string[]) => void;
    }

    export interface ITaxiStatus {
        Id: string;
        GpsStatus: GpsStatus;
    }

    export enum GpsStatus {
        inactive = 0,
        active = 1,
        parked = 2,
    }

}



interface JQueryStatic {
    connection: {
        positionHub: {
            client: App.PositionClient;
            server: App.IPositionServer;
        };
        hub: { start: any; error: any};
    }
}