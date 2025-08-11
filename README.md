# xo-tasktree

![Build](https://github.com/stiproot/xo-tasktree/actions/workflows/dotnet-pipeline.yml/badge.svg)
![License: GPLv3](https://img.shields.io/badge/License-GPLv3-blue.svg)

**xo-tasktree** is a .NET 8 library for building composable, type-safe, and testable task workflows using a fluent, functional-style API. It enables advanced branching, argument matching, and workflow orchestration for complex business logic.

---

## Features
- Fluent API for workflow and branching logic
- Type-safe node and edge composition
- Supports conditional, hash, path, and parallel branching
- Extensible with custom functions and argument resolvers
- Integrates with Microsoft.Extensions.DependencyInjection & Logging

---

## Installation

Add the package to your project (when available on NuGet):

```sh
dotnet add package Xo.TaskTree
```

---

## Getting Started

1. **Register services** in your DI container:
    ```csharp
    services.AddTaskTree();
    ```

2. **Inject** `IStateManager` to build workflows using the TaskTree's fluent API.

3. **Resolve** your workflow asynchronously:
    ```csharp
    var result = await workflow.Resolve(cancellationToken);
    ```

---

## Usage Examples

### If-Else Branching
```csharp
var mn = _stateManager
    .RootIf<IY_OutConstBool_SyncService>()
    .Then<IY_InStr_OutConstInt_AsyncService>(
        configure => configure.MatchArg("<<arg-1>>"),
        then => then.Then<IY_InInt_OutBool_SyncService>(c => c.RequireResult())
    )
    .Else<IY_InStr_AsyncService>(c => c.MatchArg<IY_InStr_OutConstStr_AsyncService>(c => c.MatchArg("<<arg-2>>")));
var n = mn.Build();
var msgs = await n.Resolve(cancellationToken);
```

### Null Check Branch
```csharp
var mn = _stateManager
    .IsNotNull<IY_OutObj_SyncService>()
    .Then<IY_InObj_OutConstInt_AsyncService>(c => c.RequireResult())
    .Else<IY_InStr_AsyncService>(c => c.AddArg("<<args>>", "args3"));
var n = mn.Build();
var msgs = await n.Resolve(cancellationToken);
```

### Argument Matching
```csharp
var mn = _stateManager
    .Root<IY_InBoolStr_OutConstInt_AsyncService>(c =>
        c.MatchArg<IY_OutConstBool_SyncService>()
         .MatchArg<IY_InBool_OutConstStrIfFalseElseDynamicStr_AsyncService>(c => c.MatchArg(true))
    );
var n = mn.Build();
var msgs = await n.Resolve(cancellationToken);
```

### Key/Hash Branching
```csharp
var mn = _stateManager
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

### Path Branching
```csharp
var mn = _stateManager
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
Branching in xo-tasktree is modeled using three core edge types, each representing a different branching structure in your workflow graph:

### Monarius (Single Edge)
Represents a single outgoing edge from a node (linear or simple flow).

**Interface:**
```csharp
public interface IMonariusNodeEdge : INodeEdge {
    INode Edge { get; }
}
```

**Diagram:**
```mermaid
graph LR
    A((Node)) -- Monarius --> B((Node))
```

---

### Binarius (Dual Edge)
Represents a binary (two-way) branch, such as if/else or true/false logic.

**Interface:**
```csharp
public interface IBinariusNodeEdge : INodeEdge {
    INode? Edge1 { get; }
    INode? Edge2 { get; }
}
```

**Diagram:**
```mermaid
graph LR
    A((Node)) -- Edge1 --> B((Node))
    A((Node)) -- Edge2 --> C((Node))
```

---

### Multus (Multi Edge)
Represents a node with multiple outgoing edges (e.g., switch/case, hash, or parallel branches).

**Interface:**
```csharp
public interface IMultusNodeEdge : INodeEdge {
    IList<INode> Edges { get; }
}
```

**Diagram:**
```mermaid
graph LR
    A((Node)) -- Edge1 --> B((Node))
    A((Node)) -- Edge2 --> C((Node))
    A((Node)) -- Edge3 --> D((Node))
    %% ...and so on
```

---

These edge types allow you to model any workflow branching scenario, from simple linear flows to complex decision trees and parallel execution paths, all with type safety and composability.

## Logic

### Decisions

$$
S : S(x) \rightarrow s
\\
f : f(S) \rightarrow f(S(x)) \rightarrow s
\\
$$