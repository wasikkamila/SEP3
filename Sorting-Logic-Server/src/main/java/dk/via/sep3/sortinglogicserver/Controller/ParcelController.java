package dk.via.sep3.sortinglogicserver.Controller;

import dk.via.sep3.sortinglogicserver.dto.ParcelRequest;
import dk.via.sep3.sortinglogicserver.dto.ParcelResponse;
import dk.via.sep3.sortinglogicserver.service.ParcelService;
import jakarta.validation.Valid;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
public class ParcelController
{
  private final ParcelService parcelService;

  public ParcelController(ParcelService parcelService) {
    this.parcelService = parcelService;
  }

  @GetMapping("/api/Hello")
  public String hello() {
    return "Hello from sep3 java sorting logic server";
  }

  @PostMapping("/parcels")
  public ResponseEntity<?> registerParcel(
      @Valid @RequestBody ParcelRequest request) {

    var grpcResponse = parcelService.registerParcel(request);

    if (!grpcResponse.getSuccess()) {
      return ResponseEntity.badRequest()
          .body(grpcResponse.getMessage());
    }

    if (!grpcResponse.hasParcel()) {
      return ResponseEntity.status(HttpStatus.BAD_GATEWAY)
          .body("Data service returned no parcel");
    }

    var parcel = grpcResponse.getParcel();

    ParcelResponse response = new ParcelResponse(
        parcel.getId(),
        parcel.getTrackingId(),
        parcel.getWeight(),
        parcel.getDestination(),
        parcel.getStatus()
    );

    return ResponseEntity.status(HttpStatus.CREATED)
        .body(response);
  }

  @GetMapping("/parcels/{id}")
  public ResponseEntity<?> getParcel(@PathVariable int id) {

    var grpcResponse = parcelService.getParcel(id);

    if (!grpcResponse.getSuccess()) {
      return ResponseEntity.status(HttpStatus.NOT_FOUND)
          .body(grpcResponse.getMessage());
    }

    if (!grpcResponse.hasParcel()) {
      return ResponseEntity.status(HttpStatus.BAD_GATEWAY)
          .body("Data service returned no parcel");
    }

    var parcel = grpcResponse.getParcel();

    ParcelResponse response = new ParcelResponse(
        parcel.getId(),
        parcel.getTrackingId(),
        parcel.getWeight(),
        parcel.getDestination(),
        parcel.getStatus()
    );

    return ResponseEntity.ok(response);
  }
}
