package dk.via.sep3.sortinglogicserver.dto;

public record ParcelResponse(
      int id,
      String trackingId,
      double weight,
      String destination,
      String status
  ) {}


