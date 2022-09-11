Short introduction:
This Client-Server application converts entered number into word representation with currency name.
e.g. If a user enters 12,34, a program will convert it into: twelve dollars and thirty-four cents


Common folder:
Contains definition of contracts for communication between a service and clients. 
Since in our case both parts are implemented in .NET, I believe this approach is better than usage of service metadata

Server part:
1. You can launch the server using RunServerWithAdminRights.bat (usually requires a few seconds to start)
This bat file contains some logic that may ask you to confirm admin credentials 
2. WCF service contract is defined in Common, IConverterService interface
3. WCF service is implemented in ConverterServiceLib
4. It contains implementation of synchronous and asynchronous operations, since we don't have any time consuming operations, 
just to demonstrate how client handles asynchronous operations I've added Thread.Sleep(2000) operation to increase amount required to finish Async call
5. WCF service is hosted inside a simple console application that uses log4net to log different information
6. http://localhost:8080/Converter endpoint is used among with "basicHttpBinding", if needed more advanced binding could be added

Client part:
1. You can launch a client application using RunClient.bat
2. MVVM Light is used for implementation of MVVM pattern
3. To convert a number into a string a user should press Convert button or press Enter
4. To clear Output from a result of the previous operation, just start typing something again
5. WCF client is implemented in ConverterServiceClient.cs, channels created manually. 
This approach has some advantages over automatically generated ServiceReference.
6. Application uses log4net for logging different information and exceptions
7. Application doesn't hide technical details of possible exceptions from the user. 
