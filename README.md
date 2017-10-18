# MyCalculatorApplication
Baseline

Azure service fabric application POC with 2 Statefull services and one Stateless service, developed on .NET core with serilog  logging to azure table storage.

Notes for enabling the application to work in Azure
1) Open the port for stateless service on azure load balancer, httpRule with the give port and backend same port
2) Open the port of 19081, as this is the port for Reverse Proxy
