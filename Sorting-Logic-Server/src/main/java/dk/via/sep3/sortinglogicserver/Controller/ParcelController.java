
package dk.via.sep3.sortinglogicserver.Controller;

import dk.via.sep3.grpc.*;
import dk.via.sep3.sortinglogicserver.dto.ParcelRequest;
import dk.via.sep3.sortinglogicserver.dto.ParcelResponse;
import dk.via.sep3.sortinglogicserver.dto.UpdateParcelDto;
import dk.via.sep3.sortinglogicserver.service.ParcelService;

import jakarta.validation.Valid;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.time.Instant;

@RestController
public class ParcelController {

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

    return ResponseEntity.status(HttpStatus.CREATED)
        .body(toDto(grpcResponse.getParcel()));
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

    return ResponseEntity.ok(toDto(grpcResponse.getParcel()));
  }

  private ParcelResponse toDto(ParcelData parcel) {

    // Convert Protobuf Timestamp into ISO-8601 String.
    String timeRegistered = null;

    if (parcel.hasRegisteredAt()) {
      timeRegistered = Instant.ofEpochSecond(
          parcel.getRegisteredAt().getSeconds(),
          parcel.getRegisteredAt().getNanos()
      ).toString();
    }

    // Convert optional Van ID into nullable Integer.
    Integer vanId = parcel.hasVanId()
        ? parcel.getVanId()
        : null;

    return new ParcelResponse(
        parcel.getParcelId(),
        parcel.getVolume(),
        parcel.getWeight(),
        parcel.getDestination(),
        parcel.getStatus().name(),
        timeRegistered,
        vanId,
        parcel.getRegisteredByUserId()
    );
  }

  @GetMapping("/parcels/latest")
  public ResponseEntity<?> getLatestParcel() {

    var grpcResponse =parcelService.getLastestParcel();

    // No parcel found.
    if (!grpcResponse.getSuccess()) {
      return ResponseEntity.status(HttpStatus.NOT_FOUND)
          .body(grpcResponse.getMessage());
    }

    // Unexpected response from the Data Server.
    if (!grpcResponse.hasParcel()) {
      return ResponseEntity.status(HttpStatus.BAD_GATEWAY)
          .body("Data service returned no parcel");
    }

    // Convert Protobuf data into our REST DTO.
    return ResponseEntity.ok(
        toDto(grpcResponse.getParcel())
    );
  }


  @PutMapping("/parcels/{id}")
  public ResponseEntity<?> updateParcel(
      @PathVariable int id,
      @Valid @RequestBody UpdateParcelDto request) {

    var grpcResponse = parcelService.updateParcel(id, request);

    if (!grpcResponse.getSuccess()) {
      return ResponseEntity.status(HttpStatus.CONFLICT)
          .body(grpcResponse.getMessage());
    }

    if (!grpcResponse.hasParcel()) {
      return ResponseEntity.status(HttpStatus.BAD_GATEWAY)
          .body("Data service returned no parcel");
    }

    return ResponseEntity.ok(
        toDto(grpcResponse.getParcel())
    );
  }

  @DeleteMapping("/parcels/{id}")
  public ResponseEntity<?> deleteParcel(@PathVariable int id) {

    var grpcResponse = parcelService.deleteParcel(id);

    if (!grpcResponse.getSuccess()) {
      return ResponseEntity.status(HttpStatus.NOT_FOUND)
          .body(grpcResponse.getMessage());
    }

    return ResponseEntity.noContent().build();
  }



}
