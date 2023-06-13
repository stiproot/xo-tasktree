# xo-task-workflow-builder

## Laws
- A Node contains a single functory.
- Nodes do not directly reference other nodes, nodes reference edges. Edges reference nodes. 

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