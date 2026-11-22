
package dk.via.sep3.sortinglogicserver.Service;

import dk.via.sep3.grpc.*;
import dk.via.sep3.sortinglogicserver.gRPC.VanGrpcClient;

import dk.via.sep3.sortinglogicserver.service.VanSuggestionService;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class VanSuggestionServiceTest {

  @Mock
  private VanGrpcClient vanGrpcClient;

  @InjectMocks
  private VanSuggestionService vanSuggestionService;

  @Test
  void suggestReturnsVanWithAvailableCapacity() {

    // Van 1 is full.
    VanData van1 = VanData.newBuilder()
        .setVanId(1)
        .setDestination("Aarhus")
        .setCapacity(50)
        .build();

    // Van 2 has available capacity.
    VanData van2 = VanData.newBuilder()
        .setVanId(2)
        .setDestination("Aarhus")
        .setCapacity(40)
        .build();

    // Simulate C# returning vans.
    when(vanGrpcClient.getAvailableVans("Aarhus"))
        .thenReturn(
            GetAvailableVansResponse.newBuilder()
                .addVans(van1)
                .addVans(van2)
                .build()
        );

    // Simulate C# returning parcel counts.
    when(vanGrpcClient.getParcelCounts())
        .thenReturn(
            GetParcelCountsResponse.newBuilder()
                .addCounts(
                    VanParcelCount.newBuilder()
                        .setVanId(1)
                        .setParcelCount(50)
                        .build()
                )
                .addCounts(
                    VanParcelCount.newBuilder()
                        .setVanId(2)
                        .setParcelCount(25)
                        .build()
                )
                .build()
        );

    // Execute the actual Java sorting logic.
    VanData result = vanSuggestionService.suggest("Aarhus");

    // Van 2 should be selected.
    assertEquals(2, result.getVanId());
  }

  @Test
  void suggestThrowsExceptionWhenNoVanFits() {

    // The only van is full.
    VanData fullVan = VanData.newBuilder()
        .setVanId(1)
        .setDestination("Aarhus")
        .setCapacity(50)
        .build();

    when(vanGrpcClient.getAvailableVans("Aarhus"))
        .thenReturn(
            GetAvailableVansResponse.newBuilder()
                .addVans(fullVan)
                .build()
        );

    when(vanGrpcClient.getParcelCounts())
        .thenReturn(
            GetParcelCountsResponse.newBuilder()
                .addCounts(
                    VanParcelCount.newBuilder()
                        .setVanId(1)
                        .setParcelCount(50)
                        .build()
                )
                .build()
        );

    // No suitable van should be available.
    ResponseStatusException exception = assertThrows(
        ResponseStatusException.class,
        () -> vanSuggestionService.suggest("Aarhus")
    );

    assertEquals(
        HttpStatus.CONFLICT,
        exception.getStatusCode()
    );
  }
}
