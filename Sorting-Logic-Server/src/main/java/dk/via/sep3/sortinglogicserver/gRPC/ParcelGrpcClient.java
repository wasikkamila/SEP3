package dk.via.sep3.sortinglogicserver.gRPC;

import dk.via.sep3.grpc.GetParcelRequest;
import dk.via.sep3.grpc.ParcelResponse;
import dk.via.sep3.grpc.ParcelServiceGrpc;
import dk.via.sep3.grpc.SaveParcelRequest;
import org.springframework.stereotype.Component;

import java.util.concurrent.TimeUnit;

@Component
public class ParcelGrpcClient
{
  private final ParcelServiceGrpc.ParcelServiceBlockingStub stub;

  public ParcelGrpcClient(ParcelServiceGrpc.ParcelServiceBlockingStub stub){
    this.stub=stub;
  }

  public ParcelResponse saveParcel(
      String trackingId,
      double weight,
      String destination) {

    SaveParcelRequest request =
        SaveParcelRequest.newBuilder()
            .setTrackingId(trackingId)
            .setWeight(weight)
            .setDestination(destination)
            .build();

    return stub.withDeadlineAfter(5, TimeUnit.SECONDS)
        .saveParcel(request);
  }

  public ParcelResponse getParcel(int id) {

    GetParcelRequest request =
        GetParcelRequest.newBuilder()
            .setId(id)
            .build();

    return stub.withDeadlineAfter(5, TimeUnit.SECONDS)
        .getParcel(request);
  }
}
