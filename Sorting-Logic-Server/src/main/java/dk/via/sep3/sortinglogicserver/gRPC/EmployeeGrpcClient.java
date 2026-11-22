
package dk.via.sep3.sortinglogicserver.gRPC;

import dk.via.sep3.grpc.*;

import org.springframework.stereotype.Component;

import java.util.concurrent.TimeUnit;

@Component
public class EmployeeGrpcClient {

  private final EmployeeServiceGrpc.EmployeeServiceBlockingStub stub;

  public EmployeeGrpcClient(
      EmployeeServiceGrpc.EmployeeServiceBlockingStub stub) {
    this.stub = stub;
  }

  public VerifyLoginResponse verifyLogin(
      String username,
      String password) {

    VerifyLoginRequest request =
        VerifyLoginRequest.newBuilder()
            .setUsername(username)
            .setPassword(password)
            .build();

    return stub.withDeadlineAfter(5, TimeUnit.SECONDS)
        .verifyLogin(request);
  }
}
