
package dk.via.sep3.sortinglogicserver.Controller;

import dk.via.sep3.sortinglogicserver.dto.LoginRequest;
import dk.via.sep3.sortinglogicserver.dto.UserDTO;
import dk.via.sep3.sortinglogicserver.service.AuthService;

import jakarta.validation.Valid;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/auth")
public class AuthController {

  private final AuthService authService;

  public AuthController(AuthService authService) {
    this.authService = authService;
  }

  @PostMapping("/login")
  public ResponseEntity<UserDTO> login(
      @Valid @RequestBody LoginRequest request) {

    UserDTO user = authService.login(request);

    return ResponseEntity.ok(user);
  }
}
