# webapi-exercise

This project has been created for a demonstration purposes. This project is very opinionated and utilizes a number of 3rd party open source libraries.

## Getting started

Solution consists of a number of ASP.NET MVC Core 2.1 web APIs:
 * Customers - service for retrieval of client personal informations
 * Transactions - service for managing account transactions
 * Accounts - service for managing customer's account operations

```
Please note, there is a number of simplifications in application functional scope (e.g. scope of information 
and capabilities served), and technical implementation (e.g. in-memory data persistence or inattentive error handling
or error response).

At the same time, with a dumbed-down scope, you might notice somewhat of an overkill with projects number and structure: 
the goal in here has been to demonstrate larger scale backend solution layering, supposedly driven by different dev teams 
in different pipelines.

Would the goal be to write example single API to show your bookshelf content, don't go where I have, 
you're most likely fine with much more cleaner-neater-smaller design.
```

### Development/Interactive testing urls

* Swagger UI for CustomersAPI: http://localhost:50081/docs/index.html?url=/swagger/v1/swagger.json
* Swagger UI for TransactionsAPI: http://localhost:5000/docs/index.html?url=/swagger/v1/swagger.json
* Swagger UI for AccountsAPI: http://localhost:50084/docs/index.html?url=/swagger/v1/swagger.json

*Swagger/Open API*
The sample project exposes a Swagger compliant UI (3.0) and thus supports any external Swagger compliant tool or viewer.  More information on the Swagger and Open API specifications and tool ecosystem can be found here: https://swagger.io/

*Postman, Fiddler*
Many developers find it useful to use the cross-platform Postman tool (https://www.getpostman.com/) to interact with their API during development. Alternatively you can try out beta's from Fiddler (https://www.telerik.com/fiddler)

### Build

You can take advantage of Gitlab pipeline using the simple .gitlab-ci.yml files, that covers: build, test, publish and deploy stages. Please note, deploy stage is arbitrary, as such pipelines here only mock it.

```
Please note, in the real world Customers, Transactions, Accounts and Shared would make it to different repositories. 
Would the APIs and shared packages be versioned, four independent pipelines were applicable.
```

### <del>Greatest regrets</del> Areas for improvements 

This project has been created within a very limited amount of time and resources, as such there is a vast field for further growth:
 * No Authentication/Authorization whatsoever is implemented at the moment, for an ease of use and demonstration. In real world, please DO NOT create non-weather dedicated APIs without authorization. Ever.
 * API Versioning libraries are not utilized yet (see: https://github.com/Microsoft/aspnet-api-versioning).
 * Beautiful touch of instant robustness in everyday applications, i.e. resiliency strategies with Polly has not made it to this version (see: https://github.com/App-vNext/Polly)
 * Unit and integration tests code coverage is on a shy side. Apologies for not making more effort into it.

The only explanation I have for cutting out those cool must-haves features deliberately, is that while this exercise has a full potential of demonstrating modern design of set of maintainable code stack with migration to microservices architecture in mind, this is only a short warm up. 
	





