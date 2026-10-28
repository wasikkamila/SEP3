package dk.via.sep3.sortinglogicserver.dto;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Positive;

public record ParcelRequest(
    @NotBlank(message = "Tracking ID is required")
    String trackingId,

    @Positive(message = "Weight must be positive")
    double weight,

    @NotBlank(message = "Destination is required")
    String destination
)
{

}
