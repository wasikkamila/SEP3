
package dk.via.sep3.sortinglogicserver.Controller;

import dk.via.sep3.grpc.VanData;
import dk.via.sep3.sortinglogicserver.dto.VanSuggestionDto;
import dk.via.sep3.sortinglogicserver.dto.VanParcelCountDto;
import dk.via.sep3.sortinglogicserver.service.VanSuggestionService;
import dk.via.sep3.sortinglogicserver.gRPC.VanGrpcClient;

import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/vans")
public class VanController {

  private final VanSuggestionService vanSuggestionService;
  private final VanGrpcClient vanGrpcClient;

  public VanController(
      VanSuggestionService vanSuggestionService,
      VanGrpcClient vanGrpcClient) {

    this.vanSuggestionService = vanSuggestionService;
    this.vanGrpcClient = vanGrpcClient;
  }

  // Task 6: Suggest a suitable van
  @GetMapping("/suggestion")
  public VanSuggestionDto suggest(
      @RequestParam String destination) {

    VanData van = vanSuggestionService.suggest(destination);

    return new VanSuggestionDto(
        van.getVanId(),
        van.getDestination()
    );
  }

  // Task 8: Get parcel counts per van
  @GetMapping("/counts")
  public List<VanParcelCountDto> getParcelCounts() {

    var grpcResponse = vanGrpcClient.getParcelCounts();

    return grpcResponse.getCountsList()
        .stream()
        .map(count -> new VanParcelCountDto(
            count.getVanId(),
            count.getParcelCount()
        ))
        .toList();
  }
}
