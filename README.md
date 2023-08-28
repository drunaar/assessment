### Problem (Developer Assessment)
---

Using ASP.NET Core, implement a RESTful API to retrieve the details of the best ***n*** stories from the Hacker News API, as determined by their score, where ***n*** is specified by the caller to the API.

The Hacker News API is documented here: <https://github.com/HackerNews/API>.

The IDs for the stories can be retrieved from this URI: <https://hacker-news.firebaseio.com/v0/beststories.json>.

The details for an individual story ID can be retrieved from this URI: <https://hacker-news.firebaseio.com/v0/item/21233041.json> (in this case for the story with **ID 21233041**).

The API should return an array of the best
n
stories as returned by the Hacker News API in descending order of score, in the form:
```json
[
    {
        "title": "A uBlock Origin update was rejected from the Chrome Web Store",
        "uri": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
        "postedBy": "ismaildonmez",
        "time": "2019-10-12T13:43:01+00:00",
        "score": 1716,
        "commentCount": 572
    },
    { ... },
    { ... },
    { ... },
    ...
]
```
In addition to the above, your API should be able to efficiently service large numbers of requests without risking overloading of the Hacker News API.

A public repository with a solution should include a README.md file that describes how to run the application, any assumptions that have been made, and any enhancements or changes that would be made, given the time.

### Solution
---

> **DISCLAIMER:** This source code can't be considered as the production-ready grade. It's created just to demonstrate an approach to software development.

The service was developed with performance, reliability, and maintainability in mind. Although many thoughts have still not been implemented, right now there is a mechanism to regulate the parallelism of requests to an external source (HackerNews in our case) and enqueueing exceeded requests. The same mechanism can add a jitter between subsequent calls to make a load to a 3rd-party service smoother (not yet implemented). Based on the nature of the external source (HackerNews' list of best stories changes slowly in time), the solution caches HackerNews' stories allowing a drastic decrease a load on external service along with decreasing serving time for clients of local service. The current solution contains the memory-based cache that is optimal for demonstration but can be easily changed to the distributed cache (any cache provider that supports the IDistributedCache interface). Also, it offers the OpenAPI-based description of provided endpoints for ease of integration with other systems, as well as the Swagger dashboard for human interactions.

#### ToDo (in not determined order)

- Check corner-cases and imlement handling of possible exceptions.
- Adopt a [Hangfire](https://www.hangfire.io/) as the background task runner and scheduler. Implement a kind of CQRS pattern when background tasks by a given schedule update the state of the best stories list and request handlers return data to our clients in virtually no time.
- Observability. Add health check endpoints. Adopt structured logging with ability to use synks to on-site or external APM system ([Serilog](https://serilog.net/)).
- Readiness to multi-node deployments. Distributed cache. API Gateway ([Ocelot](https://github.com/ThreeMammals/Ocelot))
- MediatR sometimes considered as an anti-pattern. Explore alternatives with the "endpoint/handler" semantics, local bus-based communication, and interceptors/filters: Minimal API, [FastEndpoints](https://fast-endpoints.com/)
- make code "bullet-proof". more validations. OpenAPI definitions are as strict as possible.
- API versioning
- ...


#### Installation and usage

I suggest using Docker to install and launch the HackerNews retrieval API. However, it's possible to install and run it in several ways:

- standalone executable
- Windows or systemd service (but without dedicated support to it right now, e.g. start/pause/stop signal processing)
- IIS / IIS Express site or application
- Docker container or Kubernetes pod

Here I describe just two of them, the standalone executable and the Docker container. But before, you should clone the source code repository from GitHub

```shell
git clone https://github.com/drunaar/assessment.git
cd assessment
```

##### Standalone executable

In short, running the following command in the root of the directory where the repository was cloned, will get an executable built along with all its dependencies.

```shell
dotnet publish -c release .\src\Assessment.HackerNewsBestStories.API\Assessment.HackerNewsBestStories.API.csproj -o build
```

This will build the latest source code and place the result artifacts into **build** directory. You can start the service by running this command:

```shell
./build/Assessment.HackerNewsBestStories.API.exe
```

and then navigate to the Swagger dashboard located at **http://localhost:5000/swagger**.

##### Docker container

And again, running the folloing command in the root of the directory where the repository was cloned, will get a Docker image built.

```shell
docker build --force-rm -t assessment:rel --build-arg "BUILD_CONFIGURATION=Release" .
```

This will build the latest source code, pack them as a Docker image and place it into the local Docker repository marked as **assessment:rel**. You can start the container by running this command:


```shell
docker run -dt -e "ASPNETCORE_LOGGING__CONSOLE__DISABLECOLORS=true" -e "ASPNETCORE_ENVIRONMENT=Production" -p 5000:80 --name Assessment.HackerNewsBestStories.API assessment:rel
```

and then navigate to the Swagger dashboard located at **http://localhost:5000/swagger**.
