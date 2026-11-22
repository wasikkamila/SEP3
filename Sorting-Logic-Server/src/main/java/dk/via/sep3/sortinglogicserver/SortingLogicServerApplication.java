package dk.via.sep3.sortinglogicserver;
import dk.via.sep3.grpc.*;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.grpc.client.ImportGrpcClients;

@SpringBootApplication
@ImportGrpcClients(
    target = "parcel-data",
    types ={
        ParcelServiceGrpc.ParcelServiceBlockingStub.class,
        VanServiceGrpc.VanServiceBlockingStub.class,
        EmployeeServiceGrpc.EmployeeServiceBlockingStub.class
    }
)
public class SortingLogicServerApplication
{

  public static void main(String[] args)
  {
    SpringApplication.run(SortingLogicServerApplication.class, args);
  }

}
