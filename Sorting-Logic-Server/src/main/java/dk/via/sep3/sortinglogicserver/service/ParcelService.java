
package dk.via.sep3.sortinglogicserver.service;

import dk.via.sep3.grpc.GetAvailableVansResponse;
import dk.via.sep3.grpc.VanData;
import dk.via.sep3.sortinglogicserver.dto.ParcelRequest;
import dk.via.sep3.sortinglogicserver.gRPC.ParcelGrpcClient;

import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.web.server.ResponseStatusException;

import java.time.LocalDateTime;
import java.util.List;

@Service
public class ParcelService {

  private final ParcelGrpcClient grpcClient;

  public ParcelService(ParcelGrpcClient grpcClient) {
    this.grpcClient = grpcClient;
  }

  public dk.via.sep3.grpc.ParcelResponse registerParcel(
      ParcelRequest request) {

    // 1. Retrieve available vans from the C# server.
    GetAvailableVansResponse vanResponse =
        grpcClient.getAvailableVans(request.destination());

    // 2. Filter the vans using our provisional rules.
    List<VanData> eligibleVans = vanResponse.getVansList()
        .stream()
        .filter(van -> van.getDestination()
            .equalsIgnoreCase(request.destination()))
        .filter(van -> van.getCapacity() > 0)
        .toList();

    if (eligibleVans.isEmpty()) {
      throw new ResponseStatusException(
          HttpStatus.CONFLICT,
          "No suitable van available"
      );
    }

    String selectedVanId;

    // 3. Manual override or automatic selection.
    if (request.preferredVanId() != null
        && !request.preferredVanId().isBlank()) {

      VanData selectedVan = eligibleVans.stream()
          .filter(van -> van.getVanId()
              .equals(request.preferredVanId()))
          .findFirst()
          .orElseThrow(() ->
              new ResponseStatusException(
                  HttpStatus.BAD_REQUEST,
                  "Selected van is not available"
              )
          );

      selectedVanId = selectedVan.getVanId();

    } else {
      // Provisional automatic selection:
      // choose the first eligible van.
      selectedVanId = eligibleVans.get(0).getVanId();
    }

    // 4. Reject missing or empty Van IDs.
    if (selectedVanId.isBlank()) {
      throw new ResponseStatusException(
          HttpStatus.CONFLICT,
          "A valid Van ID is required"
      );
    }

    // 5. Save parcel and selected Van ID together.
    return grpcClient.saveParcel(
        request.volume(),
        request.weight(),
        request.destination(),
        "Registered",
        LocalDateTime.now().toString(),
        selectedVanId,
        request.registeredByUserId()
    );
  }

  public dk.via.sep3.grpc.ParcelResponse getParcel(int id) {
    return grpcClient.getParcel(id);
  }
}
