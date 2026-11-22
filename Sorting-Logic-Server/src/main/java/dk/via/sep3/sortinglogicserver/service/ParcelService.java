
package dk.via.sep3.sortinglogicserver.service;

import dk.via.sep3.grpc.DeleteParcelResponse;
import dk.via.sep3.grpc.GetAvailableVansResponse;
import dk.via.sep3.grpc.ParcelResponse;
import dk.via.sep3.grpc.VanData;

import dk.via.sep3.sortinglogicserver.dto.ParcelRequest;
import dk.via.sep3.sortinglogicserver.dto.UpdateParcelDto;
import dk.via.sep3.sortinglogicserver.gRPC.ParcelGrpcClient;
import dk.via.sep3.sortinglogicserver.gRPC.VanGrpcClient;

import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.web.server.ResponseStatusException;

import java.util.List;

@Service
public class ParcelService {

  private final ParcelGrpcClient parcelGrpcClient;
  private final VanSuggestionService vanSuggestionService;

  public ParcelService(
      ParcelGrpcClient parcelGrpcClient,
      VanGrpcClient vanGrpcClient,
      VanSuggestionService vanSuggestionService) {

    this.parcelGrpcClient = parcelGrpcClient;
    this.vanSuggestionService = vanSuggestionService;
  }


  public ParcelResponse registerParcel(ParcelRequest request) {

    int selectedVanId;

    // 1. Check whether the employee selected a van.
    Integer preferredVanId = request.preferredVanId();

    if (preferredVanId != null) {

      // 2A. Manual selection: validate the selected van.
      boolean eligible = vanSuggestionService.isEligible(
          request.destination(),
          preferredVanId
      );

      if (!eligible) {
        throw new ResponseStatusException(
            HttpStatus.BAD_REQUEST,
            "Selected van is not suitable or is full"
        );
      }

      selectedVanId = preferredVanId;

    } else {

      // 2B. Automatic selection: ask the suggestion service.
      selectedVanId = vanSuggestionService
          .suggest(request.destination())
          .getVanId();
    }

    // 3. Registration must always have a valid Van ID.
    if (selectedVanId <= 0) {
      throw new ResponseStatusException(
          HttpStatus.CONFLICT,
          "A valid Van ID is required"
      );
    }

    // 4. Save the parcel and selected Van ID together.
    return parcelGrpcClient.createParcel(
        request.volume(),
        request.weight(),
        request.destination(),
        selectedVanId,
        request.registeredByUserId()
    );
  }

  public ParcelResponse getParcel(int id) {
    return parcelGrpcClient.getParcel(id);
  }
  public ParcelResponse getLastestParcel(){
    return parcelGrpcClient.getLatestParcel();
  }

  public ParcelResponse updateParcel(
      int parcelId,
      UpdateParcelDto request) {

    // 1. Validate IDs.
    if (parcelId <= 0 || request.vanId() == null
        || request.vanId() <= 0) {

      throw new ResponseStatusException(
          HttpStatus.BAD_REQUEST,
          "Invalid parcel or van ID"
      );
    }

    // 2. Retrieve the existing parcel.
    ParcelResponse existingResponse =
        parcelGrpcClient.getParcel(parcelId);

    if (!existingResponse.getSuccess()) {
      throw new ResponseStatusException(
          HttpStatus.NOT_FOUND,
          "Parcel not found"
      );
    }

    if (!existingResponse.hasParcel()) {
      throw new ResponseStatusException(
          HttpStatus.BAD_GATEWAY,
          "Data service returned no parcel"
      );
    }

    var existingParcel = existingResponse.getParcel();

    // 3. Check whether the assignment is unchanged.
    boolean sameAssignment =
        existingParcel.hasVanId()
            && existingParcel.getVanId() == request.vanId()
            && existingParcel.getDestination()
            .equalsIgnoreCase(request.destination());


    if (!sameAssignment) {

      boolean eligible = vanSuggestionService.isEligible(
          request.destination(),
          request.vanId()
      );

      if (!eligible) {
        throw new ResponseStatusException(
            HttpStatus.CONFLICT,
            "Selected van is not suitable or is full"
        );
      }
    }



    // 5. Send the complete update to C#.
    return parcelGrpcClient.updateParcel(
        parcelId,
        request.volume(),
        request.weight(),
        request.destination(),
        request.vanId()
    );
  }

  public DeleteParcelResponse deleteParcel(int parcelId) {

    if (parcelId <= 0) {
      throw new ResponseStatusException(
          HttpStatus.BAD_REQUEST,
          "Invalid parcel ID"
      );
    }

    return parcelGrpcClient.deleteParcel(parcelId);
  }


}
