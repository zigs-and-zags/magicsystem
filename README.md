# Loom
## Setup
```bash
git clone https://github.com/zigs-and-zags/magicsystem.git && cd magicsystem
./scripts/setup.sh
dotnet run --project ./src/MagicSystem.BFF --launch-profile "https"
echo "go to: https://localhost:7121/swagger/index.html"
```


## Context
A personal "command center" of sorts. Analogized as a magicsystem which can control some of my own infrastructure (in the future). The system will be event-based, where the events are called "spells". These spells (and access to the system) can be provisioned to other people by giving them a "tome" which basically acts as a sort of apikey, granting access to specific events for a given amount of time.

This is still very much a work in progress. A lot of work still has to be done on functionality and some technical wrinkles have to be ironed out, but the direction and basic working system is already provided as a starting point. Feedback and code review is welcome, I'm interested to know what other people think about the style of the technical implementation since it diverges somewhat from what dotnet devs are used to and "the microsoft way of doing things".


## Ideas
TODO: write out

- slices, refactoring projects, sharing "infrastructure"
- trying to avoid technical naming and unpleasant conventions -> opinionated
- static classes and functional influences > class based and OO -> explicit behavior / DI tradeoffs, ...
- configuration setup
- statically typing almost everything
- event based setup (event-stream not fully implemented yet)
- why not DI in event handlers -> deliberate choice, a big "infra" related implementation will be the next big addition to the backbone of the system
- ...