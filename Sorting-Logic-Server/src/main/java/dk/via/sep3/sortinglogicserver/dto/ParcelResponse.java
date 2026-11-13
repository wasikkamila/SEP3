
package dk.via.sep3.sortinglogicserver.dto;

public record ParcelResponse(
    int parcelId,
    double volume,
    double weight,
    String destination,
    String status,
    String timeRegistered,
    String vanId,
    int registeredByUserId
) {
}
