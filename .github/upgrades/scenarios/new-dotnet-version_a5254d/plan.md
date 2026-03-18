# .NET 10.0 Upgrade Plan - FUTBOLERO Solution

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Migration Plans](#project-by-project-migration-plans)
  - [FUTBOLERO.Shared.csproj](#futbolerosharedcsproj)
  - [FUTBOLERO.Client.csproj](#futboleroclientcsproj)
  - [FUTBOLERO.Server.csproj](#futboleroservercsproj)
- [Package Update Reference](#package-update-reference)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Risk Management](#risk-management)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Description

This plan details the upgrade of the FUTBOLERO Blazor WebAssembly solution from **.NET Core 3.1** to **.NET 10.0 (LTS)**. The solution consists of a three-tier Blazor WebAssembly architecture with a shared class library, client-side WebAssembly components, and an ASP.NET Core server backend.

### Scope

**Projects Affected:** 3 projects
- **FUTBOLERO.Server.csproj** - ASP.NET Core 3.1 backend → .NET 10.0
- **FUTBOLERO.Client.csproj** - Blazor WebAssembly (netstandard2.1) → .NET 10.0
- **FUTBOLERO.Shared.csproj** - Shared class library (netstandard2.1) → .NET 10.0

**Current State:**
- Total LOC: 12,587
- Total Files: 118
- NuGet Packages: 12 (8 require updates)
- Issues Identified: 24

**Target State:**
- All projects targeting .NET 10.0
- All Microsoft packages updated to 10.0.5
- Legacy cryptography (SHA256Managed) replaced with modern alternatives
- Deprecated packages removed

### Selected Strategy

**All-At-Once Strategy** - All projects upgraded simultaneously in a single coordinated operation.

**Rationale:**
- Small solution (3 projects)
- Clear, simple dependency structure (depth: 2)
- All projects currently on modern .NET (Core 3.1 / netstandard2.1)
- Low complexity across all projects (🟢 Low difficulty rating)
- All package updates have clear target versions available
- No security vulnerabilities or critical blocking issues
- Clean migration path from .NET Core 3.1 to .NET 10.0

### Complexity Assessment

**Discovered Metrics:**
- **Project Count:** 3 (small solution)
- **Dependency Depth:** 2 levels (Server → Client/Shared, Client → Shared)
- **Circular Dependencies:** None
- **High-Risk Projects:** 0
- **API Compatibility Issues:** 12 total
  - 8 Source Incompatible (SHA256Managed usage)
  - 4 Behavioral Changes (Uri, ExceptionHandler)
- **Security Vulnerabilities:** 0 critical issues
- **Package Updates Required:** 8 packages

**Classification:** Simple Solution

**Critical Issues:**
- ⚠️ **Legacy Cryptography:** 8 instances of `SHA256Managed` (obsolete) must be replaced with `SHA256.Create()`
- ⚠️ **Deprecated Package:** EPPlus 5.7.4 is marked deprecated - requires evaluation
- ℹ️ **Redundant Packages:** 2 packages now included in framework

### Recommended Approach

**All-At-Once Atomic Upgrade:** Update all project files, packages, and code simultaneously in a single operation. This approach is optimal for this solution's small size and clear structure.

### Iteration Strategy

This plan will be completed in **6-7 iterations**:
1. ✅ Phase 1: Discovery & Classification (complete)
2. ⏳ Phase 2: Foundation (Dependency Analysis, Migration Strategy, Project Stubs)
3. ⏳ Phase 3: Detailed Project Plans (all projects batched together)
4. ⏳ Final: Testing Strategy, Success Criteria, Source Control

---

## Migration Strategy

### Approach Selection: All-At-Once Strategy

**Selected Approach:** Atomic simultaneous upgrade of all projects

**Justification:**
1. **Solution Size:** 3 projects (well below 30-project threshold)
2. **Current Framework Compatibility:** All projects on .NET Core 3.1 or netstandard2.1 (no .NET Framework legacy)
3. **Dependency Simplicity:** Linear dependency chain, no circular references
4. **Codebase Size:** 12.6K LOC total - manageable for atomic upgrade
5. **Package Compatibility:** All required packages have confirmed .NET 10.0 versions
6. **Test Coverage:** Low estimated impact (12+ LOC modifications) reduces risk
7. **Team Efficiency:** Single coordinated operation minimizes context switching

### All-At-Once Strategy Rationale

**Advantages for this Solution:**
- **Speed:** Fastest path to .NET 10.0 (single upgrade cycle vs. multiple phases)
- **Simplicity:** No multi-targeting complexity, no intermediate states
- **Consistency:** All projects benefit from .NET 10.0 features simultaneously
- **Clean Dependencies:** Package resolution straightforward with aligned versions
- **Lower Coordination Overhead:** Single PR, single review, single deployment

**Risk Mitigation:**
- Small scope limits blast radius
- Low complexity rating (🟢) across all projects
- Clear breaking changes catalog (8 SHA256Managed replacements)
- Comprehensive validation before commit

### Dependency-Based Ordering

**Validation Order (for testing):**
Although all files are updated simultaneously, validation follows dependency order:

1. **FUTBOLERO.Shared.csproj** - Validate first (no dependencies)
2. **FUTBOLERO.Client.csproj** - Validate second (depends on Shared)
3. **FUTBOLERO.Server.csproj** - Validate last (depends on Client and Shared)

**Rationale:**
- Errors in leaf projects (Shared) detected first
- Cascading errors avoided by validating dependencies before dependents
- Build failures isolated to specific layers

### Execution Approach

**Single Atomic Operation:**

**Phase 0: Prerequisites**
- Verify .NET 10.0 SDK installed
- No global.json detected - no conflicts

**Phase 1: Atomic Upgrade** (all operations performed as single coordinated batch)
1. Update all project TargetFramework properties (3 projects)
2. Update all package references (8 packages across projects)
3. Restore dependencies (`dotnet restore`)
4. Replace legacy cryptography code (8 instances of SHA256Managed)
5. Address behavioral changes (Uri, ExceptionHandler)
6. Build solution and fix compilation errors
7. Verify: Solution builds with 0 errors

**Phase 2: Validation**
1. Build verification (solution-wide)
2. Smoke testing (if applicable)
3. Runtime validation

**Deliverables:**
- All projects targeting net10.0
- All packages updated to 10.0.5 (or latest compatible)
- Zero build errors/warnings
- Legacy cryptography eliminated
- Single commit with complete upgrade

---

## Detailed Dependency Analysis

### Dependency Graph Summary

The FUTBOLERO solution follows a classic three-tier Blazor WebAssembly architecture with clear separation of concerns:

```
FUTBOLERO.Server.csproj (netcoreapp3.1)
├── FUTBOLERO.Client.csproj (netstandard2.1)
│   └── FUTBOLERO.Shared.csproj (netstandard2.1)
└── FUTBOLERO.Shared.csproj (netstandard2.1)
```

**Dependency Characteristics:**
- **Depth:** 2 levels
- **Leaf Projects:** 1 (FUTBOLERO.Shared)
- **Root Projects:** 1 (FUTBOLERO.Server - the main application)
- **Circular Dependencies:** None
- **Multi-Targeted Projects:** None

### Project Groupings by Migration Phase

**All-At-Once Strategy:** All projects will be upgraded simultaneously in a single atomic operation.

**Single Migration Group (All Projects):**
1. **FUTBOLERO.Shared.csproj** - Shared class library (leaf node)
2. **FUTBOLERO.Client.csproj** - Blazor WebAssembly client (depends on Shared)
3. **FUTBOLERO.Server.csproj** - ASP.NET Core server (depends on Client and Shared)

**Rationale for Simultaneous Upgrade:**
- Projects are tightly coupled through direct references
- Client and Server both reference Shared - must maintain consistency
- Server references Client for static asset hosting
- Small codebase enables atomic changes
- All projects move from compatible baselines (.NET Core 3.1 / netstandard2.1)

### Critical Path Identification

**Primary Upgrade Path:**
Shared → Client → Server (dependency order for validation)

**Key Constraints:**
- Shared must compile successfully before Client/Server can build
- Client package versions must align with Server versions (Blazor components)
- All Microsoft.AspNetCore.Components.* packages must use same version (10.0.5)

**Risk Factors:**
- Blazor WebAssembly projects require careful package alignment
- Server hosts Client as static assets - both must target compatible frameworks
- Entity Framework packages must match target framework version

### Circular Dependencies

**Status:** None detected ✅

All dependencies flow in a single direction: Server → Client → Shared

## Project-by-Project Migration Plans

All projects will be upgraded simultaneously as part of the All-At-Once strategy. Details below provide specific guidance for each project's unique characteristics.

---

### FUTBOLERO.Shared.csproj

**Current State:**
- Target Framework: netstandard2.1
- Project Type: Class Library (SDK-style)
- LOC: 1,085
- Files: 42
- Dependencies: 0 project dependencies
- Dependants: 2 (Client, Server)
- NuGet Packages: 1
- Issues: 1 (redundant package)

**Target State:**
- Target Framework: net10.0
- Package: System.ComponentModel.Annotations removed (included in framework)
- Zero compilation errors

**Migration Difficulty:** 🟢 Low  
**Risk Level:** Low  
**Estimated Effort:** 15 minutes

---

#### Changes Required

**1. Update Target Framework**

**File:** `Shared\FUTBOLERO.Shared.csproj`

**Change:**
```xml
<!-- Before -->
<TargetFramework>netstandard2.1</TargetFramework>

<!-- After -->
<TargetFramework>net10.0</TargetFramework>
```

**2. Remove Redundant Package**

**File:** `Shared\FUTBOLERO.Shared.csproj`

**Change:**
```xml
<!-- Remove this line -->
<PackageReference Include="System.ComponentModel.Annotations" Version="5.0.0" />
```

**Reason:** System.ComponentModel.Annotations is included in .NET 10.0 base libraries. All annotation attributes (Required, StringLength, Display, etc.) remain available without the package.

**Validation:**
- No namespace changes required
- All `using System.ComponentModel.DataAnnotations;` statements work identically
- Validation attributes function without changes

---

#### Testing Strategy

**Build Validation:**
```bash
dotnet build Shared\FUTBOLERO.Shared.csproj
```

**Expected Result:** 0 errors, 0 warnings

**Code Review:**
- Verify all model classes compile successfully
- Check validation attributes still recognized
- Ensure no missing namespace errors

---

#### Dependencies Impact

**Upstream (Projects that depend on Shared):**
- FUTBOLERO.Client.csproj - Must upgrade simultaneously
- FUTBOLERO.Server.csproj - Must upgrade simultaneously

**Rationale:** Changing from netstandard2.1 to net10.0 requires dependent projects to target compatible frameworks (.NET 10.0).

---

#### Success Criteria

- ✅ Project builds with 0 errors
- ✅ Target framework updated to net10.0
- ✅ System.ComponentModel.Annotations package removed
- ✅ All 42 files compile without namespace errors
- ✅ Client and Server projects reference updated successfully

---

### FUTBOLERO.Client.csproj

**Current State:**
- Target Framework: netstandard2.1
- Project Type: Blazor WebAssembly (SDK-style)
- LOC: 116
- Files: 156
- Dependencies: 1 (Shared)
- Dependants: 1 (Server)
- NuGet Packages: 7
- Issues: 7 (package updates, deprecated package, behavioral changes)

**Target State:**
- Target Framework: net10.0
- All Microsoft.AspNetCore.Components.* packages updated to 10.0.5
- EPPlus deprecation addressed
- Behavioral changes validated

**Migration Difficulty:** 🟡 Medium (EPPlus decision required)  
**Risk Level:** Medium  
**Estimated Effort:** 2-4 hours (depends on EPPlus strategy)

---

#### Changes Required

**1. Update Target Framework**

**File:** `Client\FUTBOLERO.Client.csproj`

**Change:**
```xml
<!-- Before -->
<TargetFramework>netstandard2.1</TargetFramework>

<!-- After -->
<TargetFramework>net10.0</TargetFramework>
```

**2. Update Blazor WebAssembly Packages**

**File:** `Client\FUTBOLERO.Client.csproj`

**Change:**
```xml
<!-- Before -->
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly" Version="3.2.1" />
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly.Build" Version="3.2.1" />
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly.DevServer" Version="3.2.1" />

<!-- After -->
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly" Version="10.0.5" />
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly.Build" Version="10.0.5" />
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly.DevServer" Version="10.0.5" />
```

**3. Remove Redundant Package**

**File:** `Client\FUTBOLERO.Client.csproj`

**Change:**
```xml
<!-- Remove this line -->
<PackageReference Include="System.Net.Http.Json" Version="3.2.1" />
```

**Reason:** System.Net.Http.Json is included in .NET 10.0. All HttpClient extension methods (GetFromJsonAsync, PostAsJsonAsync, etc.) remain available.

**4. Handle EPPlus Deprecation (Decision Required)**

**File:** `Client\FUTBOLERO.Client.csproj`

**Current:**
```xml
<PackageReference Include="EPPlus" Version="5.7.4" />
```

**Option A: Upgrade to EPPlus 7.5.3** (Requires commercial license)
```xml
<PackageReference Include="EPPlus" Version="7.5.3" />
```

**Action Required:**
- Review license requirements (Polyform Noncommercial)
- Acquire commercial license if needed
- Update EPPlus API calls (minor namespace changes)

**Option B: Migrate to ClosedXML** (Open source, LGPL)
```xml
<PackageReference Include="ClosedXML" Version="0.104.1" />
```

**Action Required:**
- Refactor Excel export/import code
- Replace EPPlus API calls with ClosedXML equivalents
- Test Excel file generation/parsing

**Recommendation:** Decision checkpoint - evaluate license budget before proceeding. If budget unavailable, migrate to ClosedXML.

---

#### Code Changes Required

**No code changes required** unless EPPlus API migration needed (Option B above).

**Behavioral Changes to Validate:**

**BC-002: Uri.TryCreate** - Validate usage:
```bash
# Search for Uri.TryCreate usage
grep -r "Uri.TryCreate" Client/
```

Expected: Minimal usage in Blazor client (NavigationManager handles routing)

---

#### Testing Strategy

**Build Validation:**
```bash
dotnet build Client\FUTBOLERO.Client.csproj
```

**Runtime Validation:**
```bash
dotnet run --project Server\FUTBOLERO.Server.csproj
# Navigate to application in browser
# Test Excel export/import functionality (if EPPlus used)
```

**EPPlus-Specific Testing (if applicable):**
- Export data to Excel
- Import Excel file
- Verify formatting preserved
- Check file compatibility with Excel/LibreOffice

---

#### Success Criteria

- ✅ Project builds with 0 errors
- ✅ Target framework updated to net10.0
- ✅ All Blazor packages updated to 10.0.5
- ✅ System.Net.Http.Json package removed
- ✅ EPPlus deprecation resolved (upgrade or migration)
- ✅ Application loads in browser
- ✅ Navigation functions correctly
- ✅ Excel functionality works (if applicable)

---

### FUTBOLERO.Server.csproj

**Current State:**
- Target Framework: netcoreapp3.1
- Project Type: ASP.NET Core Web API (SDK-style)
- LOC: 11,386
- Files: 75
- Dependencies: 2 (Client, Shared)
- Dependants: 0 (root application)
- NuGet Packages: 3
- Issues: 9 (8 cryptography + 1 behavioral change)

**Target State:**
- Target Framework: net10.0
- All Entity Framework packages updated to 10.0.5
- SHA256Managed replaced with SHA256.Create() (8 instances)
- ExceptionHandler behavioral change validated

**Migration Difficulty:** 🟡 Medium (8 controller updates)  
**Risk Level:** Medium  
**Estimated Effort:** 3-5 hours

---

#### Changes Required

**1. Update Target Framework**

**File:** `Server\FUTBOLERO.Server.csproj`

**Change:**
```xml
<!-- Before -->
<TargetFramework>netcoreapp3.1</TargetFramework>

<!-- After -->
<TargetFramework>net10.0</TargetFramework>
```

**2. Update Entity Framework Packages**

**File:** `Server\FUTBOLERO.Server.csproj`

**Change:**
```xml
<!-- Before -->
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="3.1.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="3.1.10" />

<!-- After -->
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.5" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.5" />
```

**3. Replace SHA256Managed (8 Controllers)**

**Critical:** This change is **source incompatible** - compilation will fail without it.

**Files to Update:**
1. `Server\Controllers\AccesoController.cs`
2. `Server\Controllers\ArbitroController.cs`
3. `Server\Controllers\EquipoController.cs`
4. `Server\Controllers\EquipoInvitadoController.cs`
5. `Server\Controllers\JugadorController.cs`
6. `Server\Controllers\PartidoController.cs`
7. `Server\Controllers\TarjetaController.cs`
8. `Server\Controllers\TorneoController.cs`

**Pattern to Replace:**

**Before:**
```csharp
using System.Security.Cryptography;

SHA256Managed sha256 = new SHA256Managed();
byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
string hashString = Convert.ToBase64String(hash);
// ... use hashString for authentication
```

**After:**
```csharp
using System.Security.Cryptography;

byte[] hash;
using (SHA256 sha256 = SHA256.Create())
{
    hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
}
string hashString = Convert.ToBase64String(hash);
// ... use hashString for authentication
```

**Key Points:**
- Replace `new SHA256Managed()` with `SHA256.Create()`
- Wrap in `using` statement for proper disposal
- Hash output is **identical** - no data migration needed
- Existing passwords continue to work

**Validation:**
- Test login functionality after migration
- Verify hashes match expected values
- No database changes required

---

#### Code Changes Summary

**Total Files Modified:** 8 controllers

**Per-Controller Changes:**
1. Locate SHA256Managed instantiation
2. Replace with SHA256.Create() wrapped in using statement
3. Verify variable scoping (hash variable may need hoisting)
4. Ensure proper disposal

**Example (EquipoInvitadoController.cs):**

The file header shows `using System.Security.Cryptography;` is already imported. Search for SHA256Managed usage and replace:

```csharp
// Before
SHA256Managed sha = new SHA256Managed();
var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));

// After
byte[] hashBytes;
using (SHA256 sha = SHA256.Create())
{
    hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
}
```

---

#### Testing Strategy

**Build Validation:**
```bash
dotnet build Server\FUTBOLERO.Server.csproj
```

**Expected Result:** 0 errors (SHA256Managed errors resolved)

**Runtime Validation:**
```bash
dotnet run --project Server\FUTBOLERO.Server.csproj
```

**Functional Testing:**
1. **Authentication Tests:**
   - Login with existing user credentials
   - Create new user and verify login
   - Test arbitrator authentication
   - Validate team representative access

2. **Data Integrity:**
   - Create team and verify hash generation
   - Create player and test authentication
   - Schedule match and verify hash validation
   - Issue card and check hash integrity

3. **Tournament Management:**
   - Create tournament
   - Invite teams
   - Verify authentication at each step

**Entity Framework Testing:**
1. CRUD operations on all entities
2. Complex queries (LINQ to SQL)
3. Migration application (if migrations exist)
4. Connection pooling behavior

---

#### Success Criteria

- ✅ Project builds with 0 errors
- ✅ Target framework updated to net10.0
- ✅ Entity Framework packages updated to 10.0.5
- ✅ All 8 controllers updated (SHA256Managed → SHA256.Create)
- ✅ Application starts successfully
- ✅ All authentication flows work with existing data
- ✅ Database queries execute correctly
- ✅ No hash validation errors
- ✅ API endpoints respond correctly

---

#### Risk Mitigation

**Database Compatibility:**
- EF Core 10.0 queries may translate differently than 3.1
- Review SQL Server query logs for performance changes
- Test complex LINQ queries thoroughly

**Cryptography Validation:**
- Hash outputs must match exactly (critical for authentication)
- Validate with unit tests comparing hash results
- Test with known input/output pairs

**Breaking Changes:**
- BC-003 (ExceptionHandler) - Review exception handling middleware
- Ensure custom exception handlers don't throw exceptions

---

## Package Update Reference

This section catalogs all NuGet package updates required across the solution.

### Update Summary Table

| Package Name | Current Version | Target Version | Project(s) | Action | Priority |
|--------------|----------------|----------------|------------|---------|----------|
| Microsoft.AspNetCore.Components.WebAssembly | 3.2.1 | 10.0.5 | Client | Update | High |
| Microsoft.AspNetCore.Components.WebAssembly.Build | 3.2.1 | 10.0.5 | Client | Update | High |
| Microsoft.AspNetCore.Components.WebAssembly.DevServer | 3.2.1 | 10.0.5 | Client | Update | High |
| Microsoft.EntityFrameworkCore.SqlServer | 3.1.10 | 10.0.5 | Server | Update | High |
| Microsoft.EntityFrameworkCore.Tools | 3.1.10 | 10.0.5 | Server | Update | High |
| EPPlus | 5.7.4 | 7.5.3 | Client | Update | High ⚠️ Deprecated |
| System.Net.Http.Json | 3.2.1 | Framework | Client | Remove | Medium |
| System.ComponentModel.Annotations | 5.0.0 | Framework | Shared | Remove | Medium |

### Package Update Details

#### Microsoft.AspNetCore.Components.WebAssembly (Client)

**Change:** 3.2.1 → 10.0.5  
**Type:** Major version upgrade (7 versions)  
**Reason:** Core Blazor WebAssembly runtime  
**Breaking Changes:** None identified  
**Notes:** Must align with all other Microsoft.AspNetCore.Components.* packages

#### Microsoft.AspNetCore.Components.WebAssembly.Build (Client)

**Change:** 3.2.1 → 10.0.5  
**Type:** Major version upgrade (7 versions)  
**Reason:** Blazor build tooling  
**Breaking Changes:** None identified  
**Notes:** Critical for IL linking and AOT compilation

#### Microsoft.AspNetCore.Components.WebAssembly.DevServer (Client)

**Change:** 3.2.1 → 10.0.5  
**Type:** Major version upgrade (7 versions)  
**Reason:** Development server for Blazor WASM  
**Breaking Changes:** None identified  
**Notes:** Development-time dependency only

#### Microsoft.EntityFrameworkCore.SqlServer (Server)

**Change:** 3.1.10 → 10.0.5  
**Type:** Major version upgrade (7 versions)  
**Reason:** SQL Server database provider  
**Breaking Changes:** None identified for basic CRUD operations  
**Notes:** Review query translation changes in EF Core 6-10

#### Microsoft.EntityFrameworkCore.Tools (Server)

**Change:** 3.1.10 → 10.0.5  
**Type:** Major version upgrade (7 versions)  
**Reason:** Migration and scaffolding tools  
**Breaking Changes:** None identified  
**Notes:** Development-time dependency

#### EPPlus (Client) ⚠️ DEPRECATED

**Change:** 5.7.4 → 7.5.3  
**Type:** Major version upgrade (2 versions)  
**Reason:** Package marked deprecated; license change to Polyform Noncommercial  
**Breaking Changes:**  
- **License:** Version 5+ uses Polyform Noncommercial (not LGPL)
- May require commercial license for production use
- API changes between v5 and v7 (namespace consolidation)

**Action Items:**
1. Review license requirements for your use case
2. Consider alternatives if commercial license not viable:
   - ClosedXML (LGPL, active development)
   - NPOI (Apache 2.0)
   - Open XML SDK (MIT)
3. If proceeding with EPPlus 7.5.3, audit API usage for breaking changes

**Assessment Note:** ruleId NuGet.0005 indicates deprecation but doesn't block upgrade

#### System.Net.Http.Json (Client) - REMOVE

**Change:** 3.2.1 → Remove (included in framework)  
**Type:** Package removal  
**Reason:** Functionality included in .NET 10.0 base class libraries  
**Breaking Changes:** None - APIs remain identical  
**Action:** Remove PackageReference from Client.csproj

#### System.ComponentModel.Annotations (Shared) - REMOVE

**Change:** 5.0.0 → Remove (included in framework)  
**Type:** Package removal  
**Reason:** Functionality included in .NET 10.0 base class libraries  
**Breaking Changes:** None - APIs remain identical  
**Action:** Remove PackageReference from Shared.csproj

### Package Installation Commands

After updating .csproj files, restore packages:

```bash
dotnet restore C:\NUEVO FUTBOLEANDO WEB\FUTBOLERO.sln
```

### Version Alignment Notes

**Critical:** All Microsoft.AspNetCore.* packages must use the same version (10.0.5) to avoid runtime conflicts. The Blazor WebAssembly hosting model requires exact version alignment between Client and Server components.

---

## Breaking Changes Catalog

This section details all breaking changes identified during assessment, organized by severity and project impact.

### Summary Statistics

- **Total Issues:** 24 across 3 projects
- **Source Incompatible (Api.0002):** 8 instances (SHA256Managed)
- **Behavioral Changes (Api.0003):** 4 instances
- **Package Issues (NuGet.*):** 12 instances

### Critical Breaking Changes (Source Incompatible)

#### BC-001: SHA256Managed Class Obsoleted (Api.0002)

**Severity:** 🔴 Critical - Source Incompatible  
**RuleId:** Api.0002  
**Affects:** FUTBOLERO.Server.csproj (8 instances)  
**Target Framework:** .NET 5.0+ (obsolete), .NET 7.0+ (compile error)

**Description:**  
The `SHA256Managed` class is marked obsolete in .NET 5.0 and produces compilation errors in .NET 7.0+. This cryptographic class was replaced by `SHA256.Create()` which automatically selects the best available implementation.

**Affected Files & Locations:**

1. **Server\Controllers\AccesoController.cs** (1 instance)
   - Line: SHA256Managed usage
   - Context: Password hashing/validation

2. **Server\Controllers\ArbitroController.cs** (1 instance)
   - Line: SHA256Managed usage
   - Context: Arbitrator authentication

3. **Server\Controllers\EquipoController.cs** (1 instance)
   - Line: SHA256Managed usage
   - Context: Team data hashing

4. **Server\Controllers\EquipoInvitadoController.cs** (1 instance)
   - Line: SHA256Managed usage (visible in file context header)
   - Context: Invited team authentication

5. **Server\Controllers\JugadorController.cs** (1 instance)
   - Line: SHA256Managed usage
   - Context: Player authentication

6. **Server\Controllers\PartidoController.cs** (1 instance)
   - Line: SHA256Managed usage
   - Context: Match data integrity

7. **Server\Controllers\TarjetaController.cs** (1 instance)
   - Line: SHA256Managed usage
   - Context: Card validation

8. **Server\Controllers\TorneoController.cs** (1 instance)
   - Line: SHA256Managed usage
   - Context: Tournament authentication

**Migration Strategy:**

**Before (.NET 3.1):**
```csharp
using System.Security.Cryptography;

SHA256Managed sha256 = new SHA256Managed();
byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
```

**After (.NET 10.0):**
```csharp
using System.Security.Cryptography;

using (SHA256 sha256 = SHA256.Create())
{
    byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
}
```

**Key Changes:**
- Replace `new SHA256Managed()` with `SHA256.Create()`
- Add `using` statement for proper disposal
- Result hashes are **identical** - no data migration required
- Algorithm behavior unchanged

**Validation:**
- Unit test hash outputs match existing values
- Authentication still works with existing password hashes
- No database migrations needed

---

### Behavioral Changes (Non-Breaking)

#### BC-002: Uri.TryCreate Relative Uri Behavior (Api.0003)

**Severity:** 🟡 Medium - Behavioral Change  
**RuleId:** Api.0003  
**Affects:** FUTBOLERO.Client.csproj  
**Target Framework:** .NET 7.0+

**Description:**  
Starting in .NET 7.0, `Uri.TryCreate` with `UriKind.Relative` has stricter validation for relative URIs. Previously accepted malformed relative URIs may now return `false`.

**Impact Assessment:**  
- Low impact - most Blazor navigation uses NavigationManager
- Review if using Uri.TryCreate for route parsing
- Primarily affects custom routing logic

**Migration Strategy:**
- Audit Client code for Uri.TryCreate usage
- Validate relative URIs follow proper format
- Consider using NavigationManager.ToAbsoluteUri() for Blazor routes

**No immediate action required** - likely no usage in Blazor client code.

---

#### BC-003: ExceptionHandler Behavioral Change (Api.0003)

**Severity:** 🟡 Medium - Behavioral Change  
**RuleId:** Api.0003  
**Affects:** FUTBOLERO.Server.csproj  
**Target Framework:** .NET 8.0+

**Description:**  
In .NET 8.0+, the exception handler middleware behavior changed for handling exceptions during the exception handling pipeline itself. If an exception occurs while processing an exception, it's now logged but not re-thrown.

**Impact Assessment:**  
- Low impact - standard exception handling continues to work
- Only affects exception-within-exception scenarios
- Improves reliability (prevents infinite loops)

**Migration Strategy:**
- Review Startup.cs / Program.cs exception handling configuration
- Ensure exception handlers don't throw exceptions themselves
- Add try-catch within custom exception handlers if needed

**No immediate action required** - behavioral improvement.

---

#### BC-004: Uri Property Access Changes (Api.0003)

**Severity:** 🟡 Medium - Behavioral Change  
**RuleId:** Api.0003  
**Affects:** FUTBOLERO.Client.csproj  
**Target Framework:** .NET 5.0+

**Description:**  
Minor changes to Uri property behaviors for edge cases (IPv6, international characters, etc.).

**Impact Assessment:**  
- Very low impact for typical web applications
- Affects advanced Uri manipulation scenarios

**No action required** for standard Blazor navigation.

---

### Package-Related Issues

#### BC-005: EPPlus Deprecation (NuGet.0005)

**Severity:** 🟠 High - Deprecated Package  
**RuleId:** NuGet.0005  
**Affects:** FUTBOLERO.Client.csproj  
**Current Version:** 5.7.4  
**Target Version:** 7.5.3 (or alternative)

**Description:**  
EPPlus 5.7.4 is marked deprecated. Version 5.0+ changed from LGPL to Polyform Noncommercial license, requiring commercial license for production use.

**Impact Assessment:**  
- License compliance issue
- API breaking changes between v5 and v7
- Functionality unchanged if license acquired

**Migration Options:**

**Option 1: Upgrade to EPPlus 7.5.3**
- Requires commercial license for production
- API changes minimal (namespace consolidation)
- Active development, best Excel feature support

**Option 2: Migrate to ClosedXML**
- LGPL license (open source friendly)
- Active community
- Good Excel feature parity

**Option 3: Migrate to NPOI**
- Apache 2.0 license
- Java port (mature codebase)
- Broader file format support

**Recommended Action:**  
Evaluate license requirements before deciding. If commercial license viable, upgrade to EPPlus 7.5.3. Otherwise, migrate to ClosedXML.

---

#### BC-006: Redundant Packages (NuGet.0003)

**Severity:** 🟢 Low - Optimization  
**RuleId:** NuGet.0003  
**Affects:** Client (System.Net.Http.Json), Shared (System.ComponentModel.Annotations)

**Description:**  
Two packages now included in .NET 10.0 base class libraries can be safely removed.

**Action:**  
- Remove `System.Net.Http.Json` from Client.csproj
- Remove `System.ComponentModel.Annotations` from Shared.csproj
- No code changes required - APIs remain identical

---

### Breaking Changes Summary Matrix

| Change ID | Severity | Project | Files Affected | Action Required | Effort |
|-----------|----------|---------|----------------|-----------------|--------|
| BC-001 | 🔴 Critical | Server | 8 controllers | Replace SHA256Managed | Medium |
| BC-002 | 🟡 Medium | Client | 0-1 files | Validate Uri usage | Low |
| BC-003 | 🟡 Medium | Server | 0-1 files | Validate exception handling | Low |
| BC-004 | 🟡 Medium | Client | 0 files | No action | None |
| BC-005 | 🟠 High | Client | Unknown | Evaluate license/migrate | High |
| BC-006 | 🟢 Low | Client, Shared | 0 files (csproj only) | Remove packages | Low |

**Total Estimated Effort:** 8-12 hours (dependent on EPPlus decision)

---

## Risk Management

### Identified Risks & Mitigation Strategies

#### Risk Matrix

| Risk ID | Risk Description | Probability | Impact | Severity | Mitigation Strategy |
|---------|-----------------|-------------|---------|----------|---------------------|
| R-001 | SHA256Managed replacement breaks authentication | Low | High | Medium | Unit test hash outputs; test with existing passwords |
| R-002 | EPPlus license compliance violation | Medium | High | High | Evaluate license before upgrade; have alternative ready |
| R-003 | Entity Framework query behavior changes | Low | Medium | Low | Review query logs; performance test |
| R-004 | Blazor client runtime errors (package misalignment) | Low | High | Medium | Ensure exact version alignment (10.0.5) |
| R-005 | Build failures during upgrade | Low | Medium | Low | All-at-once strategy; restore dependencies carefully |
| R-006 | Data loss or corruption | Very Low | Critical | Low | No schema changes; backup database before testing |

---

### Risk Details & Mitigation

#### R-001: SHA256Managed Replacement Breaks Authentication

**Risk:** Replacing SHA256Managed with SHA256.Create() produces different hash outputs, breaking authentication.

**Probability:** Low (hash algorithm unchanged, only implementation)

**Impact:** High (users cannot log in)

**Mitigation:**
1. **Pre-Validation:**
   - Unit test comparing SHA256Managed vs SHA256.Create() outputs
   - Verify identical results for known inputs

2. **Testing:**
   - Test login with existing user accounts
   - Create new account and verify authentication
   - Test all 8 affected controllers individually

3. **Rollback:**
   - Keep .NET 3.1 branch available
   - Document rollback procedure

**Validation Test:**
```csharp
// Unit test to verify hash equivalence
[Fact]
public void SHA256_OutputEquivalence()
{
    string input = "test_password_123";
    byte[] inputBytes = Encoding.UTF8.GetBytes(input);

    // Legacy implementation (for reference)
    byte[] hash1;
    using (var legacy = new SHA256Managed())
    {
        hash1 = legacy.ComputeHash(inputBytes);
    }

    // New implementation
    byte[] hash2;
    using (var modern = SHA256.Create())
    {
        hash2 = modern.ComputeHash(inputBytes);
    }

    Assert.Equal(hash1, hash2);
}
```

---

#### R-002: EPPlus License Compliance Violation

**Risk:** Upgrading to EPPlus 7.5.3 without commercial license violates Polyform Noncommercial terms.

**Probability:** Medium (depends on application usage)

**Impact:** High (legal/compliance issue)

**Mitigation:**
1. **Decision Checkpoint (REQUIRED BEFORE UPGRADE):**
   - Review EPPlus usage in codebase
   - Determine if application use is commercial
   - Evaluate budget for commercial license (~$799/year)

2. **Alternative Path:**
   - If license not viable, migrate to ClosedXML
   - Budget 4-8 hours for migration
   - Test Excel functionality thoroughly

3. **Implementation Order:**
   - Make EPPlus decision BEFORE starting upgrade
   - Update plan with chosen approach
   - Allocate time accordingly

**Decision Tree:**
```
Is FUTBOLERO used commercially?
├─ No → Upgrade to EPPlus 7.5.3 (Polyform allows non-commercial)
└─ Yes → Can budget commercial license?
    ├─ Yes → Upgrade to EPPlus 7.5.3 + Purchase license
    └─ No → Migrate to ClosedXML (LGPL, free)
```

---

#### R-003: Entity Framework Query Behavior Changes

**Risk:** EF Core 10.0 translates LINQ queries differently than 3.1, causing performance degradation or incorrect results.

**Probability:** Low (mostly improvements in EF 6-10)

**Impact:** Medium (slow queries, potential timeouts)

**Mitigation:**
1. **Pre-Upgrade Analysis:**
   - Review complex LINQ queries in controllers
   - Identify queries with joins, grouping, subqueries

2. **Testing:**
   - Enable SQL query logging
   - Compare SQL output before/after upgrade
   - Performance test slow queries

3. **Monitoring:**
   - Monitor database performance after deployment
   - Have indexes ready if queries slow down

**Query Hotspots to Review:**
- `EquipoInvitadoController.ListarEquipoInvitado` (complex ordering, filtering)
- `EquipoInvitadoController.RecuperarInformacionEquipoInvitado` (joins, aggregations)
- Any queries with `.Contains()`, `.Any()`, or `.Count()`

---

#### R-004: Blazor Client Runtime Errors

**Risk:** Package version misalignment between Client and Server causes runtime errors in browser.

**Probability:** Low (upgrade script ensures alignment)

**Impact:** High (application unusable)

**Mitigation:**
1. **Version Alignment:**
   - All Microsoft.AspNetCore.Components.* packages: 10.0.5
   - Validate in .csproj files before building

2. **Testing:**
   - Test in multiple browsers (Chrome, Firefox, Edge)
   - Check browser console for WebAssembly errors
   - Verify network requests succeed (F12 DevTools)

3. **Validation:**
   ```bash
   dotnet list Client/FUTBOLERO.Client.csproj package
   # Verify all Microsoft.AspNetCore.Components.* show version 10.0.5
   ```

---

#### R-005: Build Failures During Upgrade

**Risk:** Project builds fail after upgrade due to missing dependencies or configuration errors.

**Probability:** Low (clear migration path)

**Impact:** Medium (delays upgrade)

**Mitigation:**
1. **Dependency Restore:**
   ```bash
   dotnet clean
   dotnet restore
   dotnet build
   ```

2. **Incremental Validation:**
   - Build Shared first
   - Then Client
   - Finally Server

3. **Fallback:**
   - Keep .NET 3.1 branch intact
   - Use git to revert if needed

---

#### R-006: Data Loss or Corruption

**Risk:** Database changes cause data loss or corruption.

**Probability:** Very Low (no schema changes planned)

**Impact:** Critical (data loss)

**Mitigation:**
1. **Pre-Upgrade:**
   - **Backup database** before testing
   - Verify backup restoration procedure

2. **No Schema Changes:**
   - No migrations planned
   - EF Core 10.0 compatible with existing schema

3. **Testing Environment:**
   - Test against copy of production database
   - Validate data integrity after upgrade

**Backup Command (SQL Server):**
```sql
BACKUP DATABASE [FUTBOLEANDO] 
TO DISK = 'C:\Backups\FUTBOLEANDO_PreUpgrade.bak'
WITH FORMAT, INIT, NAME = 'Pre-.NET 10 Upgrade Backup';
```

---

### Risk Response Plan

**If Build Fails:**
1. Review error messages for specific issues
2. Check package restore completed successfully
3. Verify .NET 10.0 SDK installed
4. Consult breaking changes catalog
5. Rollback if blockers identified

**If Authentication Fails:**
1. Verify SHA256 hash outputs match (unit test)
2. Check database connection strings
3. Review Entity Framework query logs
4. Test with newly created account
5. Rollback if existing users cannot authenticate

**If Runtime Errors Occur:**
1. Check browser console for JavaScript/WASM errors
2. Verify package versions aligned (10.0.5)
3. Test with fresh browser cache
4. Review server logs for exceptions
5. Rollback if critical functionality broken

**Rollback Criteria:**
- Authentication broken for existing users
- Critical functionality unavailable
- Database corruption detected
- Legal/compliance blocker (EPPlus license)
- Build failures unresolved after 2 hours

---

## Testing & Validation Strategy

### Testing Approach Overview

**Strategy:** Layered validation following dependency order (Shared → Client → Server)

**Test Phases:**
1. **Build Validation** - Compilation success
2. **Unit Testing** - Isolated component verification
3. **Integration Testing** - Cross-layer functionality
4. **Functional Testing** - End-to-end user scenarios
5. **Performance Testing** - Query performance comparison

---

### Phase 1: Build Validation

**Objective:** Ensure all projects compile without errors

**Test Commands:**
```bash
# Clean solution
dotnet clean "C:\NUEVO FUTBOLEANDO WEB\FUTBOLERO.sln"

# Restore dependencies
dotnet restore "C:\NUEVO FUTBOLEANDO WEB\FUTBOLERO.sln"

# Build in dependency order
dotnet build "Shared\FUTBOLERO.Shared.csproj"
dotnet build "Client\FUTBOLERO.Client.csproj"
dotnet build "Server\FUTBOLERO.Server.csproj"

# Build solution
dotnet build "C:\NUEVO FUTBOLEANDO WEB\FUTBOLERO.sln"
```

**Success Criteria:**
- ✅ 0 compilation errors
- ✅ 0 warnings (or only suppressible warnings)
- ✅ All packages restored successfully
- ✅ Dependency resolution clean

**Expected Duration:** 5 minutes

---

### Phase 2: Unit Testing - SHA256 Hash Equivalence

**Objective:** Verify SHA256.Create() produces identical hashes to SHA256Managed

**Test Implementation:**
Create temporary test file: `Server\Tests\CryptographyUpgradeTests.cs`

```csharp
using System;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace FUTBOLERO.Server.Tests
{
    public class CryptographyUpgradeTests
    {
        [Theory]
        [InlineData("password123")]
        [InlineData("árbitro_2024")]
        [InlineData("Team@Name!2024")]
        [InlineData("")]
        [InlineData("verylongpasswordwithspecialcharacters!@#$%^&*()_+{}|:<>?")]
        public void SHA256Create_ProducesIdenticalHash_ToLegacyImplementation(string input)
        {
            // Arrange
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // Act - New implementation
            byte[] modernHash;
            using (SHA256 sha = SHA256.Create())
            {
                modernHash = sha.ComputeHash(inputBytes);
            }

            // Assert - Verify expected hash structure
            Assert.Equal(32, modernHash.Length); // SHA256 always produces 32 bytes
            Assert.NotNull(modernHash);

            // Convert to base64 (standard format in controllers)
            string hashString = Convert.ToBase64String(modernHash);
            Assert.NotEmpty(hashString);
        }

        [Fact]
        public void SHA256Create_IsDisposable()
        {
            // Verify using statement works correctly
            byte[] hash = null;
            using (SHA256 sha = SHA256.Create())
            {
                hash = sha.ComputeHash(Encoding.UTF8.GetBytes("test"));
            }

            Assert.NotNull(hash);
            Assert.Equal(32, hash.Length);
        }
    }
}
```

**Success Criteria:**
- ✅ All tests pass
- ✅ Hash length correct (32 bytes)
- ✅ No disposal errors
- ✅ Special characters handled correctly

**Expected Duration:** 10 minutes

---

### Phase 3: Integration Testing - Authentication Flows

**Objective:** Validate authentication works with existing data

**Test Scenarios:**

#### 3.1: User Login (AccesoController)
```http
POST /api/Acceso/Login
Content-Type: application/json

{
  "usuario": "existing_user",
  "contraseña": "existing_password"
}
```
**Expected:** 200 OK, authentication token returned

#### 3.2: Team Registration (EquipoController)
```http
POST /api/Equipo/Registrar
Content-Type: application/json

{
  "nombre": "Test Team",
  "representante": "John Doe",
  "password": "test123"
}
```
**Expected:** 200 OK, team created with hashed password

#### 3.3: Player Authentication (JugadorController)
```http
POST /api/Jugador/Autenticar
Content-Type: application/json

{
  "idjugador": 123,
  "codigo": "player_code"
}
```
**Expected:** 200 OK, player authenticated

#### 3.4: Arbitrator Access (ArbitroController)
```http
POST /api/Arbitro/Login
Content-Type: application/json

{
  "usuario": "arbitro1",
  "contraseña": "arbitro_pass"
}
```
**Expected:** 200 OK, arbitrator session created

**Success Criteria:**
- ✅ Existing users can log in
- ✅ New users can register and log in immediately
- ✅ Password hashes stored correctly in database
- ✅ All 8 affected controllers authenticate successfully

**Expected Duration:** 30 minutes

---

### Phase 4: Functional Testing - Core Workflows

**Objective:** Validate end-to-end business functionality

#### 4.1: Tournament Management Workflow
1. Create tournament (TorneoController)
2. Register teams (EquipoController)
3. Invite teams (EquipoInvitadoController)
4. Schedule matches (PartidoController)
5. Record results and issue cards (TarjetaController)
6. View standings (EquipoInvitadoController.ListarEquipoInvitado)

**Expected:** Complete workflow succeeds without errors

#### 4.2: Player Management Workflow
1. Create player account (JugadorController)
2. Assign to team (EquipoController)
3. View player statistics
4. Update player information

**Expected:** All CRUD operations succeed

#### 4.3: Blazor Client Functionality
1. Navigate to application in browser
2. Load standings page
3. Filter teams (EquipoInvitadoController.FiltrarEquipoInvitado)
4. View team details (EquipoInvitadoController.RecuperarInformacionEquipoInvitado)
5. Calculate player ages correctly

**Expected:** UI renders correctly, API calls succeed

#### 4.4: Excel Export/Import (if EPPlus used)
1. Export data to Excel
2. Verify file downloads
3. Open file in Excel/LibreOffice
4. Import Excel data
5. Verify data integrity

**Expected:** Excel functionality works (or migrated to alternative)

**Success Criteria:**
- ✅ All workflows complete without errors
- ✅ UI loads and renders correctly
- ✅ Data persists correctly to database
- ✅ Excel functionality works (if applicable)

**Expected Duration:** 1 hour

---

### Phase 5: Performance Testing

**Objective:** Ensure EF Core 10.0 queries perform as well or better than 3.1

**Test Scenarios:**

#### 5.1: Query Performance Comparison

**Setup:** Enable SQL logging
```csharp
// In Startup.cs or Program.cs
optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
```

**Test Queries:**
1. **ListarEquipoInvitado** - Complex ordering with multiple criteria
   ```bash
   GET /api/EquipoInvitado/ListarEquipoInvitado/1
   ```
   - Measure: Response time, SQL query complexity

2. **RecuperarInformacionEquipoInvitado** - Joins and aggregations
   ```bash
   GET /api/EquipoInvitado/RecuperarInformacionEquipoInvitado/123
   ```
   - Measure: Response time, number of queries (N+1 check)

3. **FiltrarEquipoInvitado** - Dynamic filtering
   ```bash
   GET /api/EquipoInvitado/FiltrarEquipoInvitado/TestTeam/1
   ```
   - Measure: Query execution time

**Baseline Comparison:**
- Record query times in .NET 3.1
- Compare with .NET 10.0 results
- Investigate any regressions > 20%

**Success Criteria:**
- ✅ No query performance regressions > 20%
- ✅ SQL queries still use indexes appropriately
- ✅ No N+1 query problems introduced

**Expected Duration:** 30 minutes

---

### Phase 6: Browser Compatibility Testing

**Objective:** Ensure Blazor WebAssembly works across browsers

**Test Browsers:**
- Google Chrome (latest)
- Microsoft Edge (latest)
- Mozilla Firefox (latest)

**Test Actions:**
1. Load application
2. Check browser console for errors (F12)
3. Navigate between pages
4. Submit forms
5. Test Excel export/import

**Success Criteria:**
- ✅ Application loads in all browsers
- ✅ No JavaScript/WebAssembly errors
- ✅ UI renders consistently
- ✅ All functionality works

**Expected Duration:** 20 minutes

---

### Phase 7: Database Integrity Validation

**Objective:** Ensure no data corruption occurred

**Test Queries:**
```sql
-- Verify record counts unchanged
SELECT 'Equipos' AS Tabla, COUNT(*) AS Total FROM Equipo
UNION ALL
SELECT 'Jugadores', COUNT(*) FROM Jugador
UNION ALL
SELECT 'Torneos', COUNT(*) FROM Torneo
UNION ALL
SELECT 'Partidos', COUNT(*) FROM Partido;

-- Verify hash formats unchanged (should be base64 strings)
SELECT TOP 10 Idequipo, Nombre, LEN(PasswordHash) AS HashLength
FROM Equipo
WHERE PasswordHash IS NOT NULL;

-- Verify no NULL values introduced
SELECT 'Equipos' AS Tabla, COUNT(*) AS NullCount
FROM Equipo
WHERE Puntos IS NULL OR Puntosextras IS NULL
UNION ALL
SELECT 'Jugadores', COUNT(*)
FROM Jugador
WHERE Goles IS NULL;
```

**Success Criteria:**
- ✅ Record counts match pre-upgrade baseline
- ✅ Hash lengths consistent (base64 encoded SHA256)
- ✅ No unexpected NULL values
- ✅ Data types unchanged

**Expected Duration:** 10 minutes

---

### Test Execution Summary

| Phase | Duration | Prerequisites | Blocking? |
|-------|----------|---------------|-----------|
| 1. Build Validation | 5 min | None | Yes |
| 2. Unit Testing | 10 min | Phase 1 complete | No |
| 3. Integration Testing | 30 min | Phase 1 complete | Yes |
| 4. Functional Testing | 60 min | Phase 3 complete | Yes |
| 5. Performance Testing | 30 min | Phase 4 complete | No |
| 6. Browser Testing | 20 min | Phase 4 complete | No |
| 7. Database Validation | 10 min | Phase 4 complete | No |

**Total Estimated Testing Time:** 2 hours 45 minutes

---

### Test Environment Requirements

**Required:**
- .NET 10.0 SDK installed
- SQL Server accessible
- Database backup available
- Modern web browser(s)

**Recommended:**
- Separate test database (copy of production)
- SQL Server Management Studio or Azure Data Studio
- Postman or similar API testing tool
- Git for version control (rollback capability)

---

### Acceptance Criteria (Gate for Production)

**Must Pass:**
- ✅ Build Validation (Phase 1)
- ✅ Integration Testing (Phase 3)
- ✅ Functional Testing (Phase 4)

**Should Pass (Investigate Failures):**
- ⚠️ Unit Testing (Phase 2)
- ⚠️ Performance Testing (Phase 5)
- ⚠️ Browser Testing (Phase 6)
- ⚠️ Database Validation (Phase 7)

**Production Deployment Criteria:**
- All "Must Pass" phases: 100% success
- All "Should Pass" phases: 90%+ success with documented exceptions
- No critical bugs identified
- Rollback plan tested and ready

---

## Complexity & Effort Assessment

### Complexity Analysis

**Overall Complexity Rating:** 🟡 Medium

#### Complexity Factors

| Factor | Rating | Justification |
|--------|--------|---------------|
| Solution Size | 🟢 Low | 3 projects, 12.6K LOC |
| Dependency Structure | 🟢 Low | Linear, depth 2, no circular dependencies |
| Framework Gap | 🟠 Medium | 7 major versions (.NET 3.1 → 10.0) |
| API Breaking Changes | 🟡 Medium | 8 source-incompatible changes (SHA256Managed) |
| Package Updates | 🟡 Medium | 8 packages, 1 deprecated (EPPlus) |
| Code Modifications | 🟢 Low | 8 files, focused changes (cryptography) |
| Testing Requirements | 🟡 Medium | Authentication critical, requires thorough testing |
| External Dependencies | 🟢 Low | All packages have clear .NET 10.0 versions |

**Complexity Score:** 5.5/10 (Medium)

---

### Effort Estimation

#### Effort Breakdown by Activity

| Activity | Estimated Hours | Confidence | Notes |
|----------|----------------|------------|-------|
| **Planning & Preparation** | 1.0 | High | Review plan, make EPPlus decision |
| **Project File Updates** | 0.5 | High | Update 3 .csproj files |
| **Package Updates** | 0.5 | High | Update 8 package references |
| **SHA256Managed Migration** | 2.0 | Medium | Update 8 controllers, unit test |
| **EPPlus Decision & Action** | 0-4.0 | Low | 0 hrs if upgrade, 4 hrs if migrate |
| **Build & Fix Compilation** | 1.0 | Medium | Resolve any unexpected errors |
| **Unit Testing** | 1.0 | High | Hash equivalence tests |
| **Integration Testing** | 1.5 | Medium | Authentication workflows |
| **Functional Testing** | 2.0 | Medium | End-to-end scenarios |
| **Performance Testing** | 0.5 | High | Query comparison |
| **Documentation** | 0.5 | High | Update README, deployment notes |
| **Contingency** | 2.0 | - | 20% buffer for unknowns |

**Total Effort Estimate (Base):** 12.5 hours  
**Total Effort Estimate (with EPPlus migration):** 16.5 hours

#### Effort by Role

**Developer (Primary):** 10-14 hours
- Code changes
- Build fixes
- Unit testing

**QA/Tester:** 3-4 hours
- Integration testing
- Functional testing
- Performance testing

**DevOps (if applicable):** 1-2 hours
- .NET 10.0 SDK installation
- Environment validation
- Deployment preparation

---

### Timeline Estimation

**Fast Track (Dedicated Focus):**
- **Day 1 (4 hours):** Planning, project updates, package updates, SHA256 migration
- **Day 2 (4 hours):** Build fixes, unit testing, integration testing
- **Day 3 (4 hours):** Functional testing, performance testing, documentation
- **Total:** 3 business days (12 hours)

**Standard Track (Part-Time):**
- **Week 1:** Planning, updates, SHA256 migration (2 hrs/day × 3 days = 6 hrs)
- **Week 2:** Testing, fixes, validation (2 hrs/day × 3 days = 6 hrs)
- **Total:** 2 weeks (12 hours)

**Conservative (With EPPlus Migration):**
- **Week 1:** Planning, EPPlus decision, project updates (8 hrs)
- **Week 2:** EPPlus migration OR upgrade (4-8 hrs)
- **Week 3:** Testing and validation (6 hrs)
- **Total:** 3 weeks (18 hours)

---

### Resource Requirements

**Required Skills:**
- ✅ C# / .NET development experience
- ✅ ASP.NET Core Web API knowledge
- ✅ Entity Framework Core experience
- ✅ Blazor WebAssembly familiarity
- ✅ SQL Server database knowledge
- ⚠️ Cryptography basics (SHA256 understanding)
- ⚠️ Excel library experience (if EPPlus migration needed)

**Team Size:**
- **Minimum:** 1 full-stack developer (can handle entire upgrade)
- **Recommended:** 1 developer + 1 QA tester (faster, better quality)

**Tools Required:**
- .NET 10.0 SDK
- Visual Studio 2022 (17.12+) or VS Code with C# extension
- SQL Server Management Studio or Azure Data Studio
- Git (version control)
- Postman or similar API testing tool (optional)

---

### Risk-Adjusted Effort

**Best Case Scenario** (No issues, EPPlus upgrade):
- **Effort:** 10 hours
- **Timeline:** 2 business days
- **Conditions:**
  - All packages update cleanly
  - No unexpected compilation errors
  - Tests pass on first attempt
  - EPPlus license acquired, simple upgrade

**Expected Case Scenario** (Minor issues, EPPlus upgrade):
- **Effort:** 12.5 hours
- **Timeline:** 3 business days
- **Conditions:**
  - Few minor compilation warnings
  - Some test adjustments needed
  - Expected issues from breaking changes catalog

**Worst Case Scenario** (Major issues, EPPlus migration):
- **Effort:** 20 hours
- **Timeline:** 1 week (full-time)
- **Conditions:**
  - EPPlus requires full migration to ClosedXML
  - Unexpected EF Core query issues
  - Performance regressions require optimization
  - Multiple test iterations needed

---

### Complexity Comparison

**Compared to Typical .NET Upgrades:**

| Characteristic | FUTBOLERO | Typical Simple | Typical Complex |
|----------------|-----------|----------------|-----------------|
| Project Count | 3 | 1-5 | 10-50+ |
| LOC | 12.6K | < 10K | 50K-500K+ |
| Framework Gap | 7 versions | 1-2 versions | 5-10 versions |
| Breaking Changes | 12 | 0-5 | 20-100+ |
| External Dependencies | 12 packages | 5-10 | 50-200+ |
| Code Changes | 8 files | 0-5 | 50-500+ |
| **Effort** | **12.5 hrs** | **2-8 hrs** | **100-1000+ hrs** |

**Conclusion:** FUTBOLERO falls solidly in the "Simple to Medium" category. Straightforward upgrade with well-defined scope.

---

### Effort Optimization Opportunities

**To Reduce Effort:**
1. **EPPlus Decision Early:** Make license/migration decision BEFORE starting (saves 4 hours of rework)
2. **Automated Testing:** Implement hash equivalence tests before migration (early validation)
3. **Parallel Work:** Developer handles code changes while QA prepares test cases
4. **Use Plan:** Follow this document step-by-step (reduces trial-and-error)

**To Improve Quality:**
1. **Code Review:** Have second developer review SHA256 changes
2. **Extended Testing:** Add 2-4 hours for comprehensive testing if critical application
3. **Performance Baseline:** Record .NET 3.1 performance metrics before upgrade
4. **Documentation:** Document all changes for future reference (included in estimate)

---

## Source Control Strategy

### Git Strategy (If Repository Available)

**Note:** No Git repository detected during initial analysis. If using version control, follow this strategy:

---

### Branching Strategy

**Recommended Approach:** Feature branch workflow

**Branch Structure:**
```
main (or master)
└── feature/upgrade-dotnet-10
    ├── [All upgrade changes committed here]
    └── [Merge to main after validation]
```

**Branch Naming:**
- **Branch Name:** `feature/upgrade-dotnet-10`
- **Alternative:** `upgrade/net10-migration`

---

### Commit Strategy

**Approach:** Logical atomic commits for easy review and rollback

**Recommended Commit Sequence:**

#### Commit 1: Project File Updates
```bash
git checkout -b feature/upgrade-dotnet-10

# Stage all .csproj changes
git add **/*.csproj

git commit -m "chore: Update target frameworks to net10.0

- Update FUTBOLERO.Shared.csproj: netstandard2.1 → net10.0
- Update FUTBOLERO.Client.csproj: netstandard2.1 → net10.0
- Update FUTBOLERO.Server.csproj: netcoreapp3.1 → net10.0

Part of .NET 10.0 upgrade initiative."
```

#### Commit 2: Package Updates
```bash
# Update package references
git add **/*.csproj

git commit -m "chore: Update NuGet packages to .NET 10.0 compatible versions

Package Updates:
- Microsoft.AspNetCore.Components.WebAssembly: 3.2.1 → 10.0.5
- Microsoft.AspNetCore.Components.WebAssembly.Build: 3.2.1 → 10.0.5
- Microsoft.AspNetCore.Components.WebAssembly.DevServer: 3.2.1 → 10.0.5
- Microsoft.EntityFrameworkCore.SqlServer: 3.1.10 → 10.0.5
- Microsoft.EntityFrameworkCore.Tools: 3.1.10 → 10.0.5
- EPPlus: 5.7.4 → 7.5.3 [or migration to ClosedXML]

Package Removals (now in framework):
- System.Net.Http.Json (Client)
- System.ComponentModel.Annotations (Shared)

Part of .NET 10.0 upgrade initiative."
```

#### Commit 3: SHA256Managed Migration
```bash
# Update controller files
git add Server/Controllers/*Controller.cs

git commit -m "fix: Replace obsolete SHA256Managed with SHA256.Create()

Replaced SHA256Managed (obsolete in .NET 5+) with SHA256.Create() 
in 8 controllers to ensure .NET 10.0 compatibility.

Updated Files:
- Server/Controllers/AccesoController.cs
- Server/Controllers/ArbitroController.cs
- Server/Controllers/EquipoController.cs
- Server/Controllers/EquipoInvitadoController.cs
- Server/Controllers/JugadorController.cs
- Server/Controllers/PartidoController.cs
- Server/Controllers/TarjetaController.cs
- Server/Controllers/TorneoController.cs

Hash outputs remain identical - no data migration required.
Resolves Api.0002 breaking change.

Part of .NET 10.0 upgrade initiative."
```

#### Commit 4: EPPlus Changes (If Applicable)
```bash
# Option A: EPPlus upgrade
git add Client/**/*.cs Client/FUTBOLERO.Client.csproj

git commit -m "chore: Upgrade EPPlus to 7.5.3

Updated EPPlus from deprecated 5.7.4 to 7.5.3.
Commercial license acquired for production use.
Minor API adjustments for namespace changes.

Part of .NET 10.0 upgrade initiative."

# Option B: EPPlus migration
git add Client/**/*.cs Client/FUTBOLERO.Client.csproj

git commit -m "refactor: Migrate from EPPlus to ClosedXML

Replaced EPPlus (Polyform Noncommercial license) with ClosedXML 
(LGPL license) to address licensing requirements.

Changes:
- Removed EPPlus 5.7.4 package
- Added ClosedXML 0.104.1 package
- Refactored Excel export/import logic
- Updated unit tests for Excel functionality

Part of .NET 10.0 upgrade initiative."
```

#### Commit 5: Build Fixes (If Any)
```bash
git add .

git commit -m "fix: Resolve compilation errors after .NET 10.0 upgrade

[Describe specific errors resolved]

Part of .NET 10.0 upgrade initiative."
```

#### Commit 6: Test Updates
```bash
git add **/*Tests.cs

git commit -m "test: Add SHA256 hash equivalence unit tests

Added unit tests to verify SHA256.Create() produces identical 
outputs to legacy SHA256Managed implementation.

Ensures authentication continues to work with existing data.

Part of .NET 10.0 upgrade initiative."
```

#### Commit 7: Documentation
```bash
git add README.md CHANGELOG.md docs/

git commit -m "docs: Update documentation for .NET 10.0 upgrade

Updated:
- README.md: .NET 10.0 SDK requirement
- CHANGELOG.md: Version upgrade entry
- Deployment docs: Updated prerequisites

Part of .NET 10.0 upgrade initiative."
```

---

### Alternative: Single Commit Strategy

**For small teams or rapid deployment:**

```bash
git checkout -b feature/upgrade-dotnet-10

# Make all changes
git add .

git commit -m "feat: Upgrade solution to .NET 10.0

Major Changes:
- Target frameworks: netcoreapp3.1/netstandard2.1 → net10.0
- Package updates: All Microsoft packages → 10.0.5
- EPPlus: 5.7.4 → 7.5.3 [or ClosedXML migration]
- Cryptography: SHA256Managed → SHA256.Create() (8 controllers)
- Removed redundant packages (System.Net.Http.Json, System.ComponentModel.Annotations)

Breaking Changes Addressed:
- Api.0002: SHA256Managed obsolescence (8 instances)
- NuGet.0005: EPPlus deprecation

Testing:
- All unit tests passing
- Integration tests validated
- Authentication workflows verified

Migration Time: [X] hours
Issues Encountered: [None | Describe]

Resolves: #[issue-number] (if applicable)"
```

---

### Pull Request Strategy

**PR Title:**
```
feat: Upgrade FUTBOLERO solution to .NET 10.0 LTS
```

**PR Description Template:**
```markdown
## Summary
Upgrades the FUTBOLERO Blazor WebAssembly solution from .NET Core 3.1 to .NET 10.0 (LTS).

## Changes
### Target Frameworks
- FUTBOLERO.Shared: netstandard2.1 → net10.0
- FUTBOLERO.Client: netstandard2.1 → net10.0
- FUTBOLERO.Server: netcoreapp3.1 → net10.0

### Package Updates
- All Microsoft.AspNetCore.Components.* packages: 3.2.1 → 10.0.5
- Entity Framework Core: 3.1.10 → 10.0.5
- EPPlus: 5.7.4 → 7.5.3 [or ClosedXML migration]

### Code Changes
- **SHA256 Migration (8 files):** Replaced obsolete `SHA256Managed` with `SHA256.Create()`
  - Server/Controllers: Acceso, Arbitro, Equipo, EquipoInvitado, Jugador, Partido, Tarjeta, Torneo
- **Package Removals:** Removed System.Net.Http.Json and System.ComponentModel.Annotations (now in framework)

## Testing Performed
- [x] Build validation (all projects compile)
- [x] Unit tests (SHA256 hash equivalence)
- [x] Integration tests (authentication workflows)
- [x] Functional tests (end-to-end scenarios)
- [x] Performance tests (query comparison)
- [x] Browser compatibility (Chrome, Edge, Firefox)

## Breaking Changes
- **For Developers:** Requires .NET 10.0 SDK installation
- **For Users:** None - application behavior unchanged
- **For Database:** None - schema unchanged

## Rollback Plan
If issues arise post-deployment:
1. Revert merge commit
2. Redeploy from previous release branch
3. Estimated rollback time: 15 minutes

## Deployment Notes
- Requires .NET 10.0 Runtime on production servers
- Database backup recommended before deployment (standard practice)
- No database migrations required
- Estimated deployment downtime: 5 minutes

## Checklist
- [x] All tests passing
- [x] No compilation warnings
- [x] Documentation updated (README, CHANGELOG)
- [x] Code reviewed by [reviewer name]
- [x] Performance validated (no regressions)
- [x] EPPlus license compliance verified

## Related Issues
Closes #[issue-number] (if applicable)
```

---

### Code Review Checklist

**For Reviewers:**

**Project Files:**
- [ ] All .csproj files target net10.0
- [ ] Package versions aligned (all Microsoft.AspNetCore.* = 10.0.5)
- [ ] Redundant packages removed (System.Net.Http.Json, System.ComponentModel.Annotations)

**Code Changes:**
- [ ] All 8 SHA256Managed instances replaced with SHA256.Create()
- [ ] Using statements added for proper disposal
- [ ] No hardcoded hash values or temporary debugging code

**Testing:**
- [ ] Unit tests verify hash equivalence
- [ ] Integration tests cover authentication scenarios
- [ ] Performance tests show no regressions

**Documentation:**
- [ ] README updated with .NET 10.0 SDK requirement
- [ ] CHANGELOG entry added
- [ ] Comments clear and concise

---

### Merge Strategy

**Recommended:** Squash and merge (if many small commits)

**Alternative:** Merge commit (if commit history valuable)

**Post-Merge:**
```bash
# After PR approved and merged
git checkout main
git pull origin main

# Tag the release
git tag -a v2.0.0-net10 -m "Release: .NET 10.0 upgrade"
git push origin v2.0.0-net10

# Delete feature branch
git branch -d feature/upgrade-dotnet-10
git push origin --delete feature/upgrade-dotnet-10
```

---

### Rollback Procedure

**If upgrade causes production issues:**

**Option 1: Revert Merge Commit**
```bash
# Identify merge commit
git log --oneline --graph

# Revert the merge
git revert -m 1 <merge-commit-hash>
git push origin main

# Deploy previous version
```

**Option 2: Deploy Previous Tag**
```bash
# Checkout previous stable version
git checkout tags/v1.9.0-net31

# Deploy from this tag
```

**Option 3: Branch Rollback**
```bash
# Reset to commit before merge (DESTRUCTIVE - use with caution)
git reset --hard <commit-before-merge>
git push origin main --force
```

---

### No Git Repository

**If not using version control:**

1. **Create Full Backup:**
   - Copy entire solution folder
   - Name: `FUTBOLERO_Backup_Net31_[Date]`

2. **Make Changes In-Place:**
   - Follow plan step-by-step
   - Test thoroughly before proceeding

3. **Document Changes:**
   - Keep notes of all modifications
   - Save list of package versions

4. **Recommendation:**
   - Consider initializing Git repository before upgrade
   - Provides safety net for rollback

---

## Success Criteria

### Definition of Success

The .NET 10.0 upgrade is considered **successful** when all criteria below are met.

---

### Critical Success Criteria (Must Pass)

These criteria are **mandatory** for production deployment:

#### ✅ SC-001: Build Success
**Criterion:** Solution builds without errors  
**Validation:**
```bash
dotnet build "C:\NUEVO FUTBOLEANDO WEB\FUTBOLERO.sln"
```
**Expected:** 0 errors, 0 critical warnings  
**Status:** [ ] Pass / [ ] Fail

---

#### ✅ SC-002: Target Framework Updated
**Criterion:** All projects target net10.0  
**Validation:** Review .csproj files
- [ ] FUTBOLERO.Shared.csproj: `<TargetFramework>net10.0</TargetFramework>`
- [ ] FUTBOLERO.Client.csproj: `<TargetFramework>net10.0</TargetFramework>`
- [ ] FUTBOLERO.Server.csproj: `<TargetFramework>net10.0</TargetFramework>`

**Status:** [ ] Pass / [ ] Fail

---

#### ✅ SC-003: Package Updates Complete
**Criterion:** All packages updated to compatible versions  
**Validation:**
```bash
dotnet list package --outdated
```
**Expected:** No outdated Microsoft.* packages (or only non-critical)  
**Checklist:**
- [ ] Microsoft.AspNetCore.Components.WebAssembly: 10.0.5
- [ ] Microsoft.AspNetCore.Components.WebAssembly.Build: 10.0.5
- [ ] Microsoft.AspNetCore.Components.WebAssembly.DevServer: 10.0.5
- [ ] Microsoft.EntityFrameworkCore.SqlServer: 10.0.5
- [ ] Microsoft.EntityFrameworkCore.Tools: 10.0.5
- [ ] EPPlus: 7.5.3 [or ClosedXML equivalent]
- [ ] System.Net.Http.Json: REMOVED
- [ ] System.ComponentModel.Annotations: REMOVED

**Status:** [ ] Pass / [ ] Fail

---

#### ✅ SC-004: SHA256 Migration Complete
**Criterion:** All SHA256Managed instances replaced  
**Validation:**
```bash
# Search for legacy implementation
grep -r "SHA256Managed" Server/Controllers/
```
**Expected:** 0 results (all instances replaced)  
**Checklist:**
- [ ] AccesoController.cs
- [ ] ArbitroController.cs
- [ ] EquipoController.cs
- [ ] EquipoInvitadoController.cs
- [ ] JugadorController.cs
- [ ] PartidoController.cs
- [ ] TarjetaController.cs
- [ ] TorneoController.cs

**Status:** [ ] Pass / [ ] Fail

---

#### ✅ SC-005: Authentication Works
**Criterion:** Existing users can authenticate  
**Test:**
1. Start application
2. Login with existing user credentials
3. Verify successful authentication

**Expected:** Login succeeds, session created  
**Status:** [ ] Pass / [ ] Fail

---

#### ✅ SC-006: Application Starts
**Criterion:** Application runs without runtime errors  
**Validation:**
```bash
dotnet run --project Server/FUTBOLERO.Server.csproj
```
**Expected:** Application starts, listens on configured port, no exceptions  
**Status:** [ ] Pass / [ ] Fail

---

#### ✅ SC-007: Core Functionality Works
**Criterion:** Key user workflows complete successfully  
**Test Scenarios:**
1. [ ] View team standings (ListarEquipoInvitado)
2. [ ] Filter teams (FiltrarEquipoInvitado)
3. [ ] View team details (RecuperarInformacionEquipoInvitado)
4. [ ] Register new team
5. [ ] Create player account
6. [ ] Schedule match

**Expected:** All workflows complete without errors  
**Status:** [ ] Pass / [ ] Fail

---

#### ✅ SC-008: No Data Corruption
**Criterion:** Database integrity preserved  
**Validation:**
```sql
-- Verify record counts unchanged
SELECT 'Equipos' AS Tabla, COUNT(*) AS Total FROM Equipo
UNION ALL
SELECT 'Jugadores', COUNT(*) FROM Jugador
UNION ALL
SELECT 'Torneos', COUNT(*) FROM Torneo;
```
**Expected:** Record counts match pre-upgrade baseline  
**Status:** [ ] Pass / [ ] Fail

---

### Important Success Criteria (Should Pass)

These criteria are **important** but not strictly blocking:

#### ⚠️ SC-009: Performance Maintained
**Criterion:** No query performance regressions > 20%  
**Validation:** Compare response times before/after upgrade  
**Test Queries:**
- [ ] ListarEquipoInvitado (complex ordering)
- [ ] RecuperarInformacionEquipoInvitado (joins/aggregations)
- [ ] FiltrarEquipoInvitado (dynamic filtering)

**Status:** [ ] Pass / [ ] Acceptable / [ ] Fail

---

#### ⚠️ SC-010: Browser Compatibility
**Criterion:** Application works in all major browsers  
**Test:**
- [ ] Google Chrome (latest)
- [ ] Microsoft Edge (latest)
- [ ] Mozilla Firefox (latest)

**Expected:** UI renders correctly, no console errors  
**Status:** [ ] Pass / [ ] Acceptable / [ ] Fail

---

#### ⚠️ SC-011: Excel Functionality
**Criterion:** Excel export/import works (if applicable)  
**Test:**
1. [ ] Export data to Excel file
2. [ ] Download succeeds
3. [ ] File opens in Excel/LibreOffice
4. [ ] Import Excel file
5. [ ] Data persists correctly

**Status:** [ ] Pass / [ ] Not Applicable / [ ] Fail

---

#### ⚠️ SC-012: Unit Tests Pass
**Criterion:** All unit tests execute successfully  
**Validation:**
```bash
dotnet test "C:\NUEVO FUTBOLEANDO WEB\FUTBOLERO.sln"
```
**Expected:** 100% pass rate (or known failures documented)  
**Status:** [ ] Pass / [ ] Acceptable / [ ] Fail

---

### Operational Success Criteria

#### ✅ SC-013: Documentation Updated
**Criterion:** All documentation reflects .NET 10.0 requirements  
**Checklist:**
- [ ] README.md: .NET 10.0 SDK requirement documented
- [ ] CHANGELOG.md: Upgrade entry added
- [ ] Deployment docs: Updated prerequisites
- [ ] Developer setup guide: .NET 10.0 installation steps

**Status:** [ ] Pass / [ ] Fail

---

#### ✅ SC-014: EPPlus Compliance
**Criterion:** EPPlus licensing resolved  
**Options:**
- [ ] Option A: Commercial license acquired (if EPPlus 7.5.3)
- [ ] Option B: Migrated to open-source alternative (ClosedXML/NPOI)
- [ ] Option C: Excel functionality removed/deprecated

**Status:** [ ] Pass / [ ] Fail

---

#### ⚠️ SC-015: Code Quality Maintained
**Criterion:** No new code quality issues introduced  
**Validation:**
- [ ] No compiler warnings added
- [ ] Code follows existing patterns
- [ ] No hardcoded values or debug code
- [ ] Proper error handling maintained

**Status:** [ ] Pass / [ ] Acceptable / [ ] Fail

---

### Deployment Readiness Criteria

#### ✅ SC-016: Rollback Plan Tested
**Criterion:** Rollback procedure validated  
**Test:**
1. [ ] Backup created and restorable
2. [ ] Git revert tested (if using version control)
3. [ ] Rollback duration estimated: ___ minutes
4. [ ] Team knows rollback procedure

**Status:** [ ] Pass / [ ] Fail

---

#### ✅ SC-017: Production Environment Ready
**Criterion:** Production infrastructure prepared  
**Checklist:**
- [ ] .NET 10.0 Runtime installed on production servers
- [ ] Database backup completed
- [ ] Deployment window scheduled
- [ ] Stakeholders notified
- [ ] Monitoring configured

**Status:** [ ] Pass / [ ] Fail

---

### Success Criteria Summary Matrix

| Criterion | Type | Status | Notes |
|-----------|------|--------|-------|
| SC-001: Build Success | Critical | ☐ | |
| SC-002: Target Framework | Critical | ☐ | |
| SC-003: Package Updates | Critical | ☐ | |
| SC-004: SHA256 Migration | Critical | ☐ | |
| SC-005: Authentication | Critical | ☐ | |
| SC-006: Application Starts | Critical | ☐ | |
| SC-007: Core Functionality | Critical | ☐ | |
| SC-008: No Data Corruption | Critical | ☐ | |
| SC-009: Performance | Important | ☐ | |
| SC-010: Browser Compatibility | Important | ☐ | |
| SC-011: Excel Functionality | Important | ☐ | |
| SC-012: Unit Tests | Important | ☐ | |
| SC-013: Documentation | Operational | ☐ | |
| SC-014: EPPlus Compliance | Operational | ☐ | |
| SC-015: Code Quality | Operational | ☐ | |
| SC-016: Rollback Plan | Deployment | ☐ | |
| SC-017: Production Ready | Deployment | ☐ | |

---

### Go/No-Go Decision

**Production Deployment is APPROVED when:**

✅ **All 8 Critical Criteria (SC-001 through SC-008): PASS**  
✅ **All 4 Operational Criteria (SC-013 through SC-016): PASS**  
⚠️ **At least 3 of 4 Important Criteria (SC-009 through SC-012): PASS or ACCEPTABLE**

**Production Deployment is ON HOLD when:**
- Any Critical criterion fails
- More than 1 Important criterion fails
- SC-016 (Rollback Plan) not validated

**Production Deployment is REJECTED when:**
- SC-005 (Authentication) fails
- SC-008 (Data Corruption) fails
- Critical bugs discovered in testing

---

### Post-Deployment Success Metrics

**Monitor for 7 days after deployment:**

1. **Application Availability:** ≥99.9% uptime
2. **Error Rate:** <0.1% of requests
3. **Response Time:** p95 latency within 10% of baseline
4. **Authentication Success Rate:** ≥99.5%
5. **Database Query Performance:** No queries >2x slower
6. **User Complaints:** <5 support tickets related to upgrade

**If any metric fails:** Initiate incident response and evaluate rollback.

---

### Final Sign-Off

**Before Production Deployment:**

- [ ] **Technical Lead:** All technical criteria met
  - Signature: _________________ Date: _______

- [ ] **QA Lead:** All testing completed successfully
  - Signature: _________________ Date: _______

- [ ] **Product Owner:** Acceptance criteria met, deployment approved
  - Signature: _________________ Date: _______

- [ ] **DevOps/Ops:** Infrastructure ready, rollback plan validated
  - Signature: _________________ Date: _______

**Deployment Authorization:**
- [ ] **Authorized by:** _________________ 
- [ ] **Date:** _______
- [ ] **Deployment Window:** _______ to _______

---

**Congratulations!** When all success criteria are met, your FUTBOLERO solution will be successfully upgraded to .NET 10.0 LTS, ready for years of continued development and support. 🎉
