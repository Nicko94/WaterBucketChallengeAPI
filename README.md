# Water Bucket Challenge RESTful API
This API provides a solution to the classic water bucket challenge in which the task is to measure a target amount of water by filling, transferring and emptying two water buckets of a given capacity, ideally with the least amount of steps possible.

The project consists of three primary layers:

1. **API Layer:** Handles incoming requests (WaterBucketController).

2. **Application Layer:** Contains Services and DTOs for Request/Response handling.

3. **Domain layer:** Includes the Node entity to represent the state in each step.


## Features
- Solve the water bucket challenge with two buckets and a target amount.
- Returns detailed steps in the solution path with the flag **"Solved"**.
- Error handling for edge cases like when there's no solution possible or the input values are invalid.
- Swagger is integrated for seamless testing.

### Table of contents
1. Setup
2. How to Run
3. API Endpoints
4. Algorithm explanation

## Setup

Prerequisites:
- .NET Core SDK installed
- A code editor like Visual Studio (2022 version used)

Installation:
1. Clone the repository

```bash
$ git clone https://github.com/Nicko94/WaterBucketChallenge.git
$ cd WaterBucketChallenge
```
2. Restore the dependencies:

```bash
$ dotnet restore
```
3. Build the project

```bash
$ dotnet build
```
   
## How to Run
1. Navigate to the project's directory:

```bash
cd WaterBucketChallenge.API
```
2. Start the API:

```bash
$ dotnet run
```
3. The API will run on http://localhost:5000. You can now send request using Swagger or similar tools like Postman

## API Endpoints
### URI: /solve
### Method: POST

**Request Parameters:**

The request body should be in JSON format with the following structure:

**Request Body:**

```JSON
{
  "xCapacity": 3,
  "yCapacity": 5,
  "zTarget": 4
}

```

- **xCapacity:** (int) The capacity of bucket X.
- **yCapacity:** (int) The capacity of bucket Y.
- **zTarget:** (int) The target amount of water to measure. 

**Response Format:**
- **200 OK** (Success Response)

```JSON
{
  "solution": [
    {"stepNumber": 1, "bucketX": 0, "bucketY": 5, "action": "Fill bucket Y"},
    {"stepNumber": 2, "bucketX": 3, "bucketY": 2, "action": "Transfer from bucket Y to X"},
    {"stepNumber": 3, "bucketX": 0, "bucketY": 2, "action": "Empty bucket X"},
    {"stepNumber": 4, "bucketX": 2, "bucketY": 0, "action": "Transfer from bucket Y to X"},
    {"stepNumber": 5, "bucketX": 2, "bucketY": 5, "action": "Fill bucket Y"},
    {"stepNumber": 6, "bucketX": 3, "bucketY": 4, "action": "Transfer from bucket Y to X", "status":"Solved"}
  ]
}
```
### Error Responses:
- **400 Bad request** (Error Response)

```JSON
{ 
  "message": "X, Y, and Z must be positive integers." 
}
```
- **400 Bad request** (Error Response)

```JSON
{ 
  "message": "Target volume cannot be larger than the largest bucket." 
}
```

 ## Algorithm Explanation

The algorithm uses a **Breadth-First Search (BFS)** approach to explore all possible states of the two buckets. Each state is represented by a Node object containing:
- **X:** Amount of water in the bucket X
- **Y:** Amount of water in the bucket Y
- **Operation:** Specific action taken to reach the current state ("Fill bucket X")

# Steps of the algorithm
1. **Initialize:** Start with **both** buckets empty.
2. **Explore States:** In each step, apply all possible actions:
   - Fill bucket X or Y.
   - Empty bucket X or Y
   - Transfer water between buckets
3. **Track visited steps:** Use a HashSet to avoid revisiting past steps.
4. **Terminate:**
   - If the target amount is found in either bucket at any point of execution, return the solution path.
   - If no solution is found within the specified limit, return an error response.

## Conclusion

This API offers a simple but effective solution to the water bucket challenge using BFS. The code is well structured with distinct layers for clarithy and separation of tasks.
