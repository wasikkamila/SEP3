
package dk.via.sep3.sortinglogicserver.dto;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Positive;

public record ParcelRequest(

    @Positive(message = "Volume must be positive")
    double volume,

    @Positive(message = "Weight must be positive")
    double weight,

    @NotBlank(message = "Destination is required")
    String destination,

    @Positive(message = "User ID must be positive")
    int registeredByUserId,

    String preferredVanId

) {
}
