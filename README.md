# xo-tasktree

[This README is very much a work in progress]

## Examples

### if-else
```c#
var mn = this._stateManager
    .RootIf<IY_OutConstBool_SyncService>()
    .Then<IY_InStr_OutConstInt_AsyncService>(
        configure => configure.MatchArg("<<arg-1>>"),
        then => then.Then<IY_InInt_OutBool_SyncService>(configure: c => c.RequireResult())
    )
    .Else<IY_InStr_AsyncService>(c => c.MatchArg<IY_InStr_OutConstStr_AsyncService>(c => c.MatchArg("<<arg-2>>")));

var n = mn.Build();

var msgs = await n.Resolve(cancellationToken);
```

### is-not-null
```c#
var mn = this._stateManager
    .IsNotNull<IY_OutObj_SyncService>()
    .Then<IY_InObj_OutConstInt_AsyncService>(c => c.RequireResult())
    .Else<IY_InStr_AsyncService>(c => c.AddArg("<<args>>", "args3"));

var n = mn.Build();

var msgs = await n.Resolve(cancellationToken);
```

### match-args
```c#
var mn = this._stateManager
    .Root<IY_InBoolStr_OutConstInt_AsyncService>(c => 
        c
            .MatchArg<IY_OutConstBool_SyncService>()
            .MatchArg<IY_InBool_OutConstStrIfFalseElseDynamicStr_AsyncService>(c => c.MatchArg(true))
    );

var n = mn.Build();

var msgs = await n.Resolve(cancellationToken);
```

### key-hash
```c#
var mn = this._stateManager
    .Root<IY_OutConstBool_SyncService>()
    .Key<IY_InBool_OutConstStr_AsyncService>(c => c.RequireResult())
    .Hash<IY_InBoolStr_OutConstInt_AsyncService, IY_AsyncService>(
        c => c.MatchArg(true).MatchArg("<<arg>>").Key("<<str>>"),
        c => c.Key("key-a"),
        then => then.Then<IY_InStr_OutConstInt_AsyncService>(c => c.MatchArg("<<arg>>"))
    );

var n = mn.Build();

var msgs = await n.Resolve(cancellationToken);
```

### path
```c#
var mn = this._stateManager
    .Root<IY_OutConstBool_SyncService>()
    .Path<IY_InBool_OutConstStr_AsyncService, IY_InStr_OutConstInt_AsyncService, IY_InInt_OutConstInt_AsyncService>(
        c => c.RequireResult(),
        c => c.RequireResult(),
        c => c.RequireResult()
    );

var n = mn.Build();

var msgs = await n.Resolve(cancellationToken);
```

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

- IFn
- INodeEdge
- INode
- IController
- INodeBuilder
- INodeResolver
- IArgs
