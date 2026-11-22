
package dk.via.sep3.sortinglogicserver.service;

import dk.via.sep3.grpc.VerifyLoginResponse;
import dk.via.sep3.sortinglogicserver.dto.LoginRequest;
import dk.via.sep3.sortinglogicserver.dto.UserDTO;
import dk.via.sep3.sortinglogicserver.gRPC.EmployeeGrpcClient;

import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.web.server.ResponseStatusException;

@Service
public class AuthService {

  private final EmployeeGrpcClient employeeGrpcClient;

  public AuthService(EmployeeGrpcClient employeeGrpcClient) {
    this.employeeGrpcClient = employeeGrpcClient;
  }

  public UserDTO login(LoginRequest request) {

    // 1. Ask C# to verify the employee's credentials.
    VerifyLoginResponse grpcResponse =
        employeeGrpcClient.verifyLogin(
            request.username(),
            request.password()
        );

    // 2. Reject invalid credentials.
    if (!grpcResponse.getAuthenticated()) {
      throw new ResponseStatusException(
          HttpStatus.UNAUTHORIZED,
          "Invalid username or password"
      );
    }

    // 3. Convert the gRPC response into our REST DTO.
    return new UserDTO(
        grpcResponse.getEmployeeId(),
        grpcResponse.getUsername(),
        grpcResponse.getRole().name()
    );
  }
}
