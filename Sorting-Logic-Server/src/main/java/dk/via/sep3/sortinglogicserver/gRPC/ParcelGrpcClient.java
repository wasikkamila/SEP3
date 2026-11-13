
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

  public GetAvailableVansResponse getAvailableVans(
      String destination) {

    GetAvailableVansRequest request =
        GetAvailableVansRequest.newBuilder()
            .setDestination(destination)
            .build();

    return stub.withDeadlineAfter(5, TimeUnit.SECONDS)
        .getAvailableVans(request);
  }

  public ParcelResponse saveParcel(
      double volume,
      double weight,
      String destination,
      String status,
      String timeRegistered,
      String vanId,
      int registeredByUserId) {

    SaveParcelRequest request =
        SaveParcelRequest.newBuilder()
            .setVolume(volume)
            .setWeight(weight)
            .setDestination(destination)
            .setStatus(status)
            .setTimeRegistered(timeRegistered)
            .setVanId(vanId)
            .setRegisteredByUserId(registeredByUserId)
            .build();

    return stub.withDeadlineAfter(5, TimeUnit.SECONDS)
        .saveParcel(request);
  }

  public ParcelResponse getParcel(int id) {

    GetParcelRequest request =
        GetParcelRequest.newBuilder()
            .setParcelId(id)
            .build();

    return stub.withDeadlineAfter(5, TimeUnit.SECONDS)
        .getParcel(request);
  }
}
