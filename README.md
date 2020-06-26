![Do icon](do.png)

DotNet Do (dotnet-do) is a simple task runner for the .NET ecosystem. Do enables developers to simplify repetitive tasks for all users of the repo without the use of restrictive scripts and with the flexibility and ease of a simple command from anywhere in the folder hierarchy.

# Goal
Development tooling is there to help us be as efficient and fluid as possible while building apps, but too often they are complex, inconsistent and/or feel like a barrier to results. The goal of this simple tool is to truly simplify automated tasks, making repetitive tasks an unpremeditated part of getting results.

# Getting started

If you don't have one already, add a tool-manifest (usually at the root of your repo):
```
dotnet new tool-manifest
```

Install Dotnet Do:
```
dotnet tool install dotnet-do
```

Create a sample tasks file (dotnet-tasks.yml):
```
dotnet do --create
```

Execute the sample task:
```
dotnet do echo
```

# Further guidance

More information about DotNet Do can be found via the [Wiki](https://github.com/LinqEm/DotNetDo/wiki). 