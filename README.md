#### Problem (Developer Assessment)

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
