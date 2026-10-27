# Parcel gRPC Contract

The parcel.proto file defines communication between the Java LogicServer and the C# DataServer.

## Service
ParcelService

## RPC Methods
- SaveParcel: Saves a parcel in the DataServer.
- GetParcel: Retrieves a parcel using its database ID.

## Shared Messages
- SaveParcelRequest
- GetParcelRequest
- ParcelResponse
- ParcelData

## Integration
Both Java and C# must use the same parcel.proto contract.

The C# DataServer is configured to generate gRPC code from parcel.proto.

The Java LogicServer will import this contract when its project is created.

## Status
Contract defined. Java integration and end-to-end testing pending.
