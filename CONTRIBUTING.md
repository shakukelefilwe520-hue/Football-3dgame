# Contributing to Football 3D Game

## Getting Started

1. Fork the repository
2. Clone your fork: `git clone https://github.com/YOUR-USERNAME/Football-3dgame.git`
3. Add upstream: `git remote add upstream https://github.com/shakukelefilwe520-hue/Football-3dgame.git`
4. Create a feature branch: `git checkout -b feature/your-feature-name`

## Development Workflow

### 1. Before Starting
- Check existing issues and PRs to avoid duplicates
- Create an issue for your feature or bug fix
- Wait for approval from maintainers

### 2. Code Style Guidelines

**C# Naming Conventions**
```csharp
// Classes and Methods: PascalCase
public class PlayerController { }
public void UpdatePlayerPosition() { }

// Variables and Parameters: camelCase
private Vector3 playerPosition;
public void Move(Vector3 direction) { }

// Constants: UPPER_SNAKE_CASE
private const float MAX_SPEED = 10f;
private const int PLAYER_COUNT = 11;

// Properties: PascalCase
public float Health { get; set; }
public Team CurrentTeam { get; private set; }

// Private fields: _camelCase (optional)
private int _playerScore;
```

**Code Organization**
```csharp
public class PlayerController : MonoBehaviour
{
    // Constants at top
    private const float SPEED = 5f;
    
    // Public properties
    public float CurrentSpeed { get; set; }
    
    // Private fields
    private Player currentPlayer;
    private InputSystem inputSystem;
    
    // Lifecycle methods
    private void Awake() { }
    private void Start() { }
    private void Update() { }
    
    // Public methods
    public void Move(Vector3 direction) { }
    public void Jump() { }
    
    // Private methods
    private void ValidateInput() { }
    private void ApplyMovement() { }
}
```

### 3. Documentation

All public methods and classes must have XML documentation:

```csharp
/// <summary>
/// Moves the player in the specified direction with the given speed.
/// </summary>
/// <param name="direction">The direction to move (normalized).</param>
/// <param name="speed">The movement speed multiplier.</param>
/// <returns>True if movement was successful, false otherwise.</returns>
public bool MovePlayer(Vector3 direction, float speed)
{
    // Implementation
}

/// <summary>
/// Represents a football player with attributes and statistics.
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>The unique player identifier.</summary>
    public string PlayerId { get; set; }
    
    /// <summary>The player's current health/stamina percentage.</summary>
    public float Stamina { get; set; }
}
```

### 4. File Organization

Each class in its own file:
```
Assets/Scripts/Player/
├── PlayerController.cs
├── Player.cs
├── PlayerStats.cs
├── PlayerAttributes.cs
└── Equipment.cs
```

Namespaces match folder structure:
```csharp
// File: Assets/Scripts/Player/PlayerController.cs
namespace Football3D.Player
{
    public class PlayerController : MonoBehaviour
    {
        // Implementation
    }
}
```

### 5. Commit Messages

Format: `[Category] Brief description`

**Categories**:
- `[Feature]` - New functionality
- `[Fix]` - Bug fixes
- `[Refactor]` - Code restructuring
- `[Docs]` - Documentation updates
- `[Test]` - Test additions
- `[Perf]` - Performance improvements
- `[Chore]` - Maintenance tasks

**Examples**:
```bash
git commit -m "[Feature] Add player shooting mechanics"
git commit -m "[Fix] Correct ball physics collision detection"
git commit -m "[Refactor] Optimize AI pathfinding algorithm"
git commit -m "[Docs] Update README with setup instructions"
git commit -m "[Test] Add unit tests for player movement"
git commit -m "[Perf] Reduce memory allocation in game loop"
```

### 6. Pull Request Process

1. **Before Submitting**
   - Update your branch with latest changes: `git pull upstream main`
   - Test your changes thoroughly in editor and on device
   - Run all tests: `Assets/Tests/RunAllTests.cs`
   - Check for compiler warnings and errors

2. **Create Pull Request**
   - Push to your fork: `git push origin feature/your-feature-name`
   - Create PR on GitHub with clear title and description
   - Link related issue: "Fixes #123"
   - Add screenshots/videos if applicable

3. **PR Template**
   ```markdown
   ## Description
   Brief description of changes
   
   ## Type of Change
   - [ ] New feature
   - [ ] Bug fix
   - [ ] Breaking change
   - [ ] Documentation update
   
   ## Related Issue
   Fixes #(issue number)
   
   ## Testing
   How was this tested?
   - [ ] Tested in editor
   - [ ] Tested on Android device
   - [ ] Added/updated tests
   
   ## Screenshots
   Add screenshots if UI changes
   
   ## Checklist
   - [ ] Code follows style guidelines
   - [ ] Added documentation
   - [ ] No compiler warnings
   - [ ] Tests pass
   - [ ] No performance regression
   ```

4. **Code Review**
   - Maintainers will review your code
   - Address feedback and push updates
   - No force pushes after PR creation
   - Respond to comments within 48 hours

5. **Merge**
   - PR must have at least 2 approvals
   - All checks must pass
   - Squash commits before merging
   - Delete branch after merge

## Testing Guidelines

### Unit Tests
```csharp
[TestFixture]
public class PlayerControllerTests
{
    private PlayerController playerController;
    
    [SetUp]
    public void Setup()
    {
        playerController = new GameObject("Player").AddComponent<PlayerController>();
    }
    
    [Test]
    public void MovePlayer_WithValidDirection_UpdatesPosition()
    {
        // Arrange
        Vector3 direction = Vector3.forward;
        
        // Act
        playerController.Move(direction);
        
        // Assert
        Assert.IsTrue(playerController.transform.position.z > 0);
    }
}
```

### Integration Tests
- Test game flow from menu to match
- Test multiplayer synchronization
- Test career progression

## Performance Guidelines

- Target 60 FPS on mid-range Android devices
- Memory usage < 500MB
- Network latency < 100ms
- Load times < 10 seconds

## Security

- Never commit sensitive data (API keys, passwords)
- Validate all user input
- Implement server-side validation
- Use HTTPS for network communication
- Encrypt sensitive save data

## Reporting Issues

**Bug Report Template**
```markdown
## Description
Clear description of the bug

## Reproduction Steps
1. Step 1
2. Step 2
3. Expected result
4. Actual result

## Environment
- Unity Version: 2022.3.0f1
- Android API Level: 34
- Device: Samsung Galaxy S20

## Logs
Paste relevant console errors

## Screenshots
Attach screenshots if helpful
```

## Feature Requests

**Feature Request Template**
```markdown
## Description
Clear description of desired feature

## Use Case
Why is this feature needed?

## Implementation Idea
Suggested implementation (optional)

## Priority
- [ ] Low
- [ ] Medium
- [ ] High
```

## Community Guidelines

- Be respectful and inclusive
- Provide constructive feedback
- Help others in discussions
- Follow GitHub Community Guidelines
- Report inappropriate behavior to maintainers

## Resources

- [Unity Documentation](https://docs.unity3d.com)
- [C# Documentation](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [Git Guide](https://git-scm.com/doc)
- [GitHub Flow](https://guides.github.com/introduction/flow/)

## Questions?

Create an issue or start a discussion on GitHub.

---
**Last Updated**: 2026-04-25