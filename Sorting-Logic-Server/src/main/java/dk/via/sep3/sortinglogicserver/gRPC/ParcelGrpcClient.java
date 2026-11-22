
package dk.via.sep3.sortinglogicserver.gRPC;

import dk.via.sep3.grpc.*;

import org.springframework.stereotype.Component;

import java.util.concurrent.TimeUnit;

@Component
public class ParcelGrpcClient {

  private final ParcelServiceGrpc.ParcelServiceBlockingStub stub;

  public ParcelGrpcClient(
      ParcelServiceGrpc.ParcelServiceBlockingStub stub) {
    this.stub = stub;
  }

  // Register a parcel in the C# Data Server
  public ParcelResponse createParcel(
      double volume,
      double weight,
      String destination,
      int vanId,
      int registeredByUserId) {

    SaveParcelRequest request =
        SaveParcelRequest.newBuilder()
            .setVolume(volume)
            .setWeight(weight)
            .setDestination(destination)
            .setVanId(vanId)
            .setRegisteredByUserId(registeredByUserId)
            .build();

    return stub.withDeadlineAfter(5, TimeUnit.SECONDS)
        .createParcel(request);
  }

  // Retrieve a parcel using its ID
  public ParcelResponse getParcel(int id) {

    GetParcelRequest request =
        GetParcelRequest.newBuilder()
            .setParcelId(id)
            .build();

    return stub.withDeadlineAfter(5, TimeUnit.SECONDS)
        .getParcelById(request);
  }


  public ParcelResponse getLatestParcel() {

    GetLatestParcelRequest request =
        GetLatestParcelRequest.newBuilder()
            .build();

    return stub.withDeadlineAfter(5, TimeUnit.SECONDS)
        .getLatestParcel(request);
  }
  public ParcelResponse updateParcel(int parcelId,
      double volume,double weight,
      String destination,int vanId)
  {
    UpdateParcelRequest request= UpdateParcelRequest.newBuilder().
    setParcelId(parcelId).
        setVolume(volume).
        setWeight(weight).
        setDestination(destination).
        setVanId(vanId)
        .build();

    return stub.withDeadlineAfter(5,TimeUnit.SECONDS).
        updateParcel(request);
  }
  public DeleteParcelResponse deleteParcel(int parcelId){
    DeleteParcelRequest request=DeleteParcelRequest.newBuilder().
        setParcelId(parcelId).build();

    return stub.withDeadlineAfter(5,TimeUnit.SECONDS)
        .deleteParcel(request);
  }
}
