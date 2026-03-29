# Contributing to SharpVG

Thanks for your interest in contributing.

## Prerequisites

- .NET 8 or 9 SDK
- Git

## Local Development

Clone and enter the repository:

```bash
git clone https://github.com/ChrisNikkel/SharpVG.git
cd SharpVG
```

Build:

```bash
dotnet build
```

Run tests:

```bash
dotnet test Tests
```

Run an example:

```bash
dotnet run -p Examples/Triangle/Triangle.fsproj
```

## Pull Requests

- Keep changes focused and small when possible.
- Add or update tests for behavior changes.
- Ensure `dotnet build` and `dotnet test Tests` pass locally before opening a PR.
- Update docs when public behavior changes.

## Coding Style

- Follow repository conventions in `CLAUDE.md`.
- Keep APIs pipeline-friendly (`create`, `with*`, `add*`, `to*`).
- Prefer full words over abbreviations (`duration` not `dur`, `position` not `pos`).
- Use `Point.ofInts (x, y)` and `Area.ofInts (w, h)` tuple form.

## Reporting Issues

When filing an issue, include:

- Expected behavior
- Actual behavior
- Minimal reproduction
- Environment details (OS, .NET SDK version)
