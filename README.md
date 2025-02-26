# Surf fares

## Tools

- install vs code mermaid extension to view the diagrams

## Archtitecture Air.Domain.Fares

Inspired from @matlus Shiv Kumar
https://www.youtube.com/watch?v=t6i0XJQoKnY


```mermaid
graph TD
    Manager1 --> ManagerUsages
    Manager1 --> DataLayerFacade
    Manager2 --> DataLayerFacade
    Manager2 --> ManagerUsages
    DataManager1 --> DataManagerUsages
    DataManager2 --> DataManagerUsages

    subgraph Application/DomainEntry
    DomainFacade --> Manager1
    DomainFacade --> Manager2
    end

    subgraph ManagerUsages
    Services
    Validators
    Models
    Exceptions
    Extensions
    end

    subgraph Persistance
    DataLayerFacade --> DataManager1
    DataLayerFacade --> DataManager2
    end

    subgraph DataManagerUsages
    DbContext
    DataServices
    DataExceptions
    end
```
