
package dk.via.sep3.sortinglogicserver.service;

import dk.via.sep3.grpc.VanData;
import dk.via.sep3.grpc.VanParcelCount;
import dk.via.sep3.sortinglogicserver.gRPC.VanGrpcClient;

import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.web.server.ResponseStatusException;

import java.util.Comparator;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service
public class VanSuggestionService {

  private final VanGrpcClient vanGrpcClient;

  public VanSuggestionService(VanGrpcClient vanGrpcClient) {
    this.vanGrpcClient = vanGrpcClient;
  }

  // Automatically suggest an eligible van.
  public VanData suggest(String destination) {

    return eligibleVans(destination)
        .stream()
        .min(Comparator.comparingInt(VanData::getVanId))
        .orElseThrow(() ->
            new ResponseStatusException(
                HttpStatus.CONFLICT,
                "No suitable van available"
            )
        );
  }

  // Check whether a manually selected van is eligible.
  public boolean isEligible(String destination, int vanId) {

    return eligibleVans(destination)
        .stream()
        .anyMatch(van -> van.getVanId() == vanId);
  }

  // Shared eligibility calculation.
  private List<VanData> eligibleVans(String destination) {

    if (destination == null || destination.isBlank()) {
      throw new ResponseStatusException(
          HttpStatus.BAD_REQUEST,
          "Destination is required"
      );
    }

    Map<Integer, Integer> parcelCounts = new HashMap<>();

    for (VanParcelCount count :
        vanGrpcClient.getParcelCounts().getCountsList()) {

      parcelCounts.put(
          count.getVanId(),
          count.getParcelCount()
      );
    }

    var vans = vanGrpcClient.getAvailableVans(destination);

    return vans.getVansList()
        .stream()
        .filter(van -> van.getDestination()
            .equalsIgnoreCase(destination))
        .filter(van ->
            parcelCounts.getOrDefault(
                van.getVanId(), 0
            ) < van.getCapacity()
        )
        .toList();
  }
}
