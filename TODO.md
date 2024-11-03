# PV179 Beer cap collector project TODO
+ [ ] change random UUIDs to static ones
+ [ ] throw conn string into .env (or at least just program.cs) and remove the second conn string in DBContext file
+ [ ] optional: add .ini / config file
+ [ ] write some meaningful tests for DAL 
+ [ ] tests for services and API / controllers


## items based on Milestone1 review
+ [ ] remove overkill Actions in Loggers
+ [ ] remove redundant `this` keywords
+ [ ] fix synchronous calls to the DB


## M2 stuff (copied from our Discord server)

Milestone 2 Requirements:

*Introduce a business layer to the application.*


```
Ensure every team member:
a) Creates at least 1 meaningful service.
b) Tests at least 1 service from another member using mocking. Ensure tests are meaningful.
```

**@bivɒᗡ:** admin service, leaderboard service

**@PetrsGamer:** user service

**@Ted:**
```
Set up a CI/CD pipeline for Merge requests.
```

**@PetrsGamer:**
```
Update GitLab settings to:
Allow only successful merge requests to be merged.
Require 1 approval for merging (effective from the start of working on 2nd milestone).
```

**@PetrsGamer:**
```
Integrate an Identity Framework:
a) Develop a separate MVC application for this.
b) For the current milestone, only implement authentication using the Identity Framework.
```


**@bivɒᗡ:**
```
Develop Middleware to:
Transform API response to XML or JSON.
Default format should be JSON unless specified in the query parameter.
Ensure that MVC and WebAPI are set up as separate projects, allowing them to:
Operate under different configurations.
Use different database setups.
Maintain clear boundaries, ensuring API endpoints are not accessible from MVC, and vice versa.
```

**@Ted:**
```
Update Documentation:
Update the readme to match the current codebase.
```


**Klidně @bivɒᗡ:**
```
Implement Audit Logging:
Track information about who edited the Product entity and how many times it was edited overall.
```


==========================================
```
Change Request:
Client requests that data seeding be modified to use Bogus (or a similar NuGet package).
```

**@bivɒᗡ:**

```
Client requests the ability to use images on the website and prefers that they be saved directly to the hard drive for easy access and management.
```

**@Ted:**
```
Client requests that log data from LogMiddleware be saved into a database.
Hint: A different database more suited for storing logs can be used.
```
