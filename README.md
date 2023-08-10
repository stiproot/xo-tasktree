# xo-task-workflow-builder

## VERY MUCH A WORK IN PROGRESS

## Laws
- A Node contains a single fn.
- Nodes do not directly reference other nodes, nodes reference edges. Edges reference nodes.
- There should be a single core node type. ie. no different type for a decision making node.

## Branching
### Branch Types

``` mermaid
graph LR
    N((node)) --> M{edge} --> O((node))

    X((node)) --> 
    Y{edge} --> Z((node))
    Y{edge} --> P((node))

    A((node)) --> E((node))
    B((node)) --> E((node))
    E((node)) --> F((node))
```

## Logic

### Decisions

$$
S : S(x) \rightarrow s
\\
f : f(S) \rightarrow f(S(x)) \rightarrow s
\\
$$

## Syntax

### Abstract

