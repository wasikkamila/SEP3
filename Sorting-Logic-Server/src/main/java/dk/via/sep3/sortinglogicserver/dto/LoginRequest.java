package dk.via.sep3.sortinglogicserver.dto;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Size;

public record LoginRequest(
    @NotBlank(message = "Username is required")
    @Size(max=100)
    String username,
    @NotBlank(message = "Password is require")
    String password
)
{
  @Override
  public String toString(){
    return"Login Request : Username: "+username+
        ", Password: [------]";


  }

}
