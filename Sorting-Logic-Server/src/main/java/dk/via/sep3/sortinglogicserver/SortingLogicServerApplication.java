package dk.via.sep3.sortinglogicserver;
import dk.via.sep3.grpc.ParcelServiceGrpc;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.grpc.client.ImportGrpcClients;

@SpringBootApplication
@ImportGrpcClients(
    target = "parcel-data",
    types = ParcelServiceGrpc.ParcelServiceBlockingStub.class
)
public class SortingLogicServerApplication
{

  public static void main(String[] args)
  {
    SpringApplication.run(SortingLogicServerApplication.class, args);
  }

}
