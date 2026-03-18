# FUTBOLERO Solution .NET 10.0 Upgrade Tasks

## Overview

This document tracks the execution of the FUTBOLERO Blazor WebAssembly solution upgrade from .NET Core 3.1 to .NET 10.0. All three projects (Shared, Client, Server) will be upgraded simultaneously in a single atomic operation, followed by comprehensive testing and validation.

**Progress**: 4/4 tasks complete (100%) ![0%](https://progress-bar.xyz/100)

---

## Tasks

### [✓] TASK-001: Verify prerequisites *(Completed: 2026-03-16 04:38)*
**References**: Plan §Phase 0

- [✓] (1) Verify .NET 10.0 SDK installed per Plan §Prerequisites
- [✓] (2) .NET 10.0 SDK installation confirmed (**Verify**)

---

### [✓] TASK-002: Atomic framework and package upgrade with compilation fixes *(Completed: 2026-03-16 04:43)*
**References**: Plan §Phase 1, Plan §Project-by-Project Migration Plans, Plan §Package Update Reference, Plan §Breaking Changes Catalog

- [✓] (1) Update TargetFramework to net10.0 in all 3 project files per Plan §Project-by-Project Migration Plans (FUTBOLERO.Shared.csproj, FUTBOLERO.Client.csproj, FUTBOLERO.Server.csproj)
- [✓] (2) All project files updated to net10.0 (**Verify**)
- [✓] (3) Update package references per Plan §Package Update Reference (update 8 packages: Microsoft.AspNetCore.Components.WebAssembly packages to 10.0.5, EntityFrameworkCore packages to 10.0.5, EPPlus to 7.5.3; remove System.Net.Http.Json and System.ComponentModel.Annotations)
- [✓] (4) All package references updated (**Verify**)
- [✓] (5) Restore all dependencies across solution
- [✓] (6) All dependencies restored successfully (**Verify**)
- [✓] (7) Replace SHA256Managed with SHA256.Create() in 8 controller files per Plan §BC-001 (AccesoController.cs, ArbitroController.cs, EquipoController.cs, EquipoInvitadoController.cs, JugadorController.cs, PartidoController.cs, TarjetaController.cs, TorneoController.cs)
- [✓] (8) All 8 SHA256Managed instances replaced (**Verify**)
- [✓] (9) Build solution and fix all remaining compilation errors per Plan §Breaking Changes Catalog
- [✓] (10) Solution builds with 0 errors (**Verify**)

---

### [✓] TASK-003: Run full test suite and validate upgrade *(Completed: 2026-03-15 21:43)*
**References**: Plan §Testing & Validation Strategy

- [✓] (1) Run all test projects in solution
- [✓] (2) Fix any test failures (reference Plan §Phase 2 Validation and §Breaking Changes Catalog for common issues)
- [✓] (3) Re-run tests after fixes
- [✓] (4) All tests pass with 0 failures (**Verify**)

---

### [✓] TASK-004: Final commit *(Completed: 2026-03-16 04:44)*
**References**: Plan §Source Control Strategy

- [✓] (1) Commit all changes with message: "feat: Upgrade FUTBOLERO solution to .NET 10.0"

---






