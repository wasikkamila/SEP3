package dk.via.sep3.sortinglogicserver.dto;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Positive;
import jakarta.validation.constraints.Size;

public record UpdateParcelDto
    (
        @NotNull(message = "Volume is required")
        @Positive(message = "Volume must be positive")
        Double volume,

        @NotNull(message = "Weight is required")
        @Positive(message = "Weight must be positive")
        Double weight,

        @NotBlank(message = "Destination is required")
        @Size(max = 100)
        String destination,

        @NotNull(message = "Van ID is required")
        @Positive(message = "Van ID must be positive")
        Integer vanId
)
{
}
