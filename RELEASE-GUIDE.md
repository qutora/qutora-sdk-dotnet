# Qutora SDK Release Guide

## Overview
This document describes the release process for the Qutora .NET SDK, including versioning strategy and automated deployment.

## Release Strategy

### Commit Message Triggers
- Commits with `[release]` tag will trigger automatic NuGet publishing
- Regular commits will only build and test, without publishing

### Examples
```bash
# ✅ Will publish to NuGet
git commit -m "Fix document upload authentication issue [release]"

# ✅ Will publish to NuGet  
git commit -m "Add metadata validation improvements [release]"

# ❌ Build only, no publish
git commit -m "Update documentation"

# ❌ Build only, no publish
git commit -m "Refactor service layer"
```

## Versioning Strategy

### Current Pattern
- Sequential versioning: `1.0.1` → `1.0.2` → `1.0.3`
- Preview versions: `1.0.1-preview` → `1.0.2-preview`
- Pipeline automatically increments patch version
- Fetches latest version from NuGet API to prevent conflicts

### Version Increments
- **Patch**: Bug fixes, small improvements
- **Minor**: New features, API additions  
- **Major**: Breaking changes, API redesign

## Automated Pipeline

### Requirements
- GitHub Secret: `NUGET_API_KEY` must be configured
- Valid NuGet.org API key with push permissions
- Main branch protection (optional but recommended)

### Workflow Location
- File: `.github/workflows/nuget-publish.yml`
- Triggers: Push to main branch with `[release]` in commit message
- Actions: Build → Test → Pack → Publish → Create GitHub Release

### Process
1. Detects `[release]` tag in commit message
2. Queries NuGet API for latest published version
3. Increments version number automatically
4. Updates `.csproj` file with new version
5. Builds and tests the project
6. Creates NuGet package
7. Publishes to NuGet.org
8. Creates GitHub release with tag

## Manual Override
If needed, version can be manually set in `Qutora.SDK.csproj`:
```xml
<Version>1.2.3</Version>
<AssemblyVersion>1.2.3.0</AssemblyVersion>
<FileVersion>1.2.3.0</FileVersion>
```

## Troubleshooting

### Common Issues
- **API Key Missing**: Configure `NUGET_API_KEY` in repository secrets
- **Version Conflict**: Check NuGet.org for existing versions
- **Build Failures**: Review workflow logs in Actions tab

### Emergency Release
For urgent fixes, use manual process:
```bash
# Update version in .csproj manually
# Then commit with release tag
git commit -m "Emergency fix for critical security issue [release]"
```

## Contact
For questions about the release process, contact the Qutora development team.

## Release History
- v1.0.1-preview: Initial release with comprehensive API support
- v1.0.2: Automated release pipeline implementation 