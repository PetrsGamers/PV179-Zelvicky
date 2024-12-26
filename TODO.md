# PV179 Beer Cap Collector Project TODO

## David's Tasks

### Fixes from Last Milestone
- [x] Fix synchronous calls to the DB
- [x] change random UUIDs to static ones
- [x] throw conn string into .env (or at least just program.cs) and remove the second conn string in DBContext file
- [x] remove redundant `this` keywords (Rider says NOPE, but only somewhere :kekw: )

### Milestone 2 Tasks
- [x] Create admin and leaderboard service
- [x] Write tests to Ted's service using mocking. Ensure tests are meaningful.
- [x] Modify data seeding to use Bogus (or a similar NuGet package)
- [x] Develop Middleware to:
    - Transform API response to XML or JSON


## Petr's Tasks

- [x] Create user service
- [x] Write tests to David's service using mocking. Ensure tests are meaningful.
- [x] Update GitLab settings to:
    - Allow only successful merge requests to be merged
    - Require 1 approval for merging (effective from the start of working on the 2nd milestone)
- [x] Integrate an Identity Framework:
    - Develop a separate MVC application for this
    - For the current milestone, only implement authentication using the Identity Framework
- [x] Implement Audit Logging:
    - Track information about who edited the Product entity and how many times it was edited overall


## Ted's Tasks

### Fixes from Last Milestone
- [x] Split project into multiple projects

### Milestone 2 Tasks
- [x] Create Cap, Color, Country services
- [x] Write tests to Petr's service using mocking. Ensure tests are meaningful.
- [x] Set up a CI/CD pipeline for merge requests
- [x] Update documentation:
    - Update the readme to match the current codebase
- [x] Ensure log data from LogMiddleware is saved into a database (hint: consider using a separate database for storing logs)
- [x] Client requests the ability to use images on the website and prefers that they be saved directly to the hard drive for easy access and management


# Misc
- [x] optional: add .ini / config file
- [x] tests for services and API / controllers
- [x] remove overkill Actions in Loggers (It works, dont touch it)
