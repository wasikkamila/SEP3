
package dk.via.sep3.sortinglogicserver.gRPC;

import dk.via.sep3.grpc.*;
import org.springframework.stereotype.Component;

import java.util.concurrent.TimeUnit;

@Component
public class VanGrpcClient {

  private final VanServiceGrpc.VanServiceBlockingStub stub;

  public VanGrpcClient(
      VanServiceGrpc.VanServiceBlockingStub stub) {
    this.stub = stub;
  }

  // Find available vans for a destination
  public GetAvailableVansResponse getAvailableVans(
      String destination) {

    GetAvailableVansRequest request =
        GetAvailableVansRequest.newBuilder()
            .setDestination(destination)
            .build();

    return stub.withDeadlineAfter(5, TimeUnit.SECONDS)
        .getAvailableVans(request);
  }

  // Get the number of parcels assigned to each van
  public GetParcelCountsResponse getParcelCounts() {

    GetParcelCountsRequest request =
        GetParcelCountsRequest.newBuilder()
            .build();

    return stub.withDeadlineAfter(5, TimeUnit.SECONDS)
        .getParcelCounts(request);
  }

  // Assign or reassign a parcel to a van
  public AssignParcelToVanResponse assignParcelToVan(
      int parcelId,
      int vanId) {

    AssignParcelToVanRequest request =
        AssignParcelToVanRequest.newBuilder()
            .setParcelId(parcelId)
            .setVanId(vanId)
            .build();

    return stub.withDeadlineAfter(5, TimeUnit.SECONDS)
        .assignParcelToVan(request);
  }
}
