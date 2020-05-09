# BoogiepopTCore
A class library containing general methods, structure and algorithms for recommendation.
Started as a generalization of the methods used in [ReBoogiepopT](https://github.com/Drens5/ReBoogiepopT).

## Documentation / Reference
[Mathematical Documentation of BoogiepopT Core](https://github.com/Drens5/BoogiepopTCore/blob/master/BoogiepopTCore.pdf).

## Short Synopsis of Current Methods
### Extract Metric
Abstract class that defines functionality to be implemented to provide a metric on a collection of objects.

### Metric Lift
An algorithm to compare ordered pairs of objects from which (a set of) associated objects can be defined.
The set of all such associated objects must be a metric space.

### More Coming Soon...

## Projects Using BoogiepopTCore
### KamikishiroN
A seasonal anime recommendation webapp, find it [here](http://drens5.ephialtes.feralhosting.com:15163/).
The backend to KamikishiroN provides a concrete implementation of BoogiepopTCore using anime genre and tag metadata.
