
package dk.via.sep3.sortinglogicserver.service;

import dk.via.sep3.sortinglogicserver.dto.ParcelRequest;
import dk.via.sep3.sortinglogicserver.gRPC.ParcelGrpcClient;
import org.springframework.stereotype.Service;

@Service
public class ParcelService {

  private final ParcelGrpcClient grpcClient;

  public ParcelService(ParcelGrpcClient grpcClient) {
    this.grpcClient = grpcClient;
  }

  public dk.via.sep3.grpc.ParcelResponse registerParcel(
      ParcelRequest request) {

    return grpcClient.saveParcel(
        request.trackingId(),
        request.weight(),
        request.destination()
    );
  }

  public dk.via.sep3.grpc.ParcelResponse getParcel(int id) {
    return grpcClient.getParcel(id);
  }
}
