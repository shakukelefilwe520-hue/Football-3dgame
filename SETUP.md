# Project Setup Guide

## Prerequisites

1. **Unity Installation**
   - Download Unity Hub: https://unity.com/download
   - Install Unity 2022 LTS or newer
   - Install Android Build Support module

2. **Android Development**
   - Install Android SDK
   - Set up Android NDK
   - Configure Java Development Kit (JDK)

3. **Git**
   - Install Git from https://git-scm.com

## Initial Setup

### 1. Clone Repository
```bash
git clone https://github.com/shakukelefilwe520-hue/Football-3dgame.git
cd Football-3dgame
```

### 2. Open in Unity
```bash
# Navigate to project directory
cd Football-3dgame

# Open with Unity (adjust path to your Unity installation)
/Applications/Unity/Hub/Editor/2022.3.0f1/Unity.app/Contents/MacOS/Unity -projectPath .
```

### 3. Configure Project Settings

**Player Settings**
- Edit → Project Settings → Player
- Set Company Name: "Your Company"
- Set Product Name: "Football 3D Game"
- Set Default Icon
- Set Splash Screen

**Android Settings**
- Edit → Project Settings → Player → Android
- Minimum API Level: 24 (Android 7.0)
- Target API Level: 34 (Latest)
- Architecture: ARM64
- Graphics API: OpenGL ES 3.0

**Graphics Settings**
- Edit → Project Settings → Graphics
- Quality: High (Highest)
- Shadows: On
- Particle System: Medium

### 4. Import Essential Packages

1. **TextMesh Pro** (for UI text)
   - Window → TextMesh Pro → Import TMP Essential Resources

2. **Unity Netcode for GameObjects** (for multiplayer)
   - Window → Asset Store
   - Search "Netcode for GameObjects"
   - Import

3. **Mirror** (alternative multiplayer)
   - Or use Mirror from GitHub

## Folder Structure Setup

Create the following folders in `Assets/`:

```
Assets/
├── Scripts/
│   ├── Core/
│   ├── Player/
│   ├── AI/
│   ├── Physics/
│   ├── Network/
│   ├── UI/
│   ├── Audio/
│   ├── Data/
│   └── Utils/
├── Scenes/
│   ├── MainMenu.unity
│   ├── Stadium.unity
│   ├── CareerMode.unity
│   └── Multiplayer.unity
├── Prefabs/
│   ├── Player/
│   ├── Ball/
│   ├── UI/
│   └── Effects/
├── Models/
│   ├── Characters/
│   ├── Stadium/
│   ├── Ball/
│   └── Equipment/
├── Materials/
├── Textures/
├── Animations/
├── Audio/
│   ├── Music/
│   ├── SFX/
│   └── Ambience/
├── UI/
│   ├── Fonts/
│   └── Sprites/
└── Resources/
    ├── Data/
    ├── Configuration/
    └── Localization/
```

## Create Initial Scenes

### MainMenu Scene
1. Create new scene: File → New Scene
2. Save as: `Assets/Scenes/MainMenu.unity`
3. Add Canvas for UI
4. Create buttons: Start Game, Career Mode, Multiplayer, Settings, Exit

### Stadium Scene
1. Create new scene: File → New Scene
2. Save as: `Assets/Scenes/Stadium.unity`
3. Add plane for field (scale 100x60)
4. Add lighting
5. Add camera
6. Add network manager (for multiplayer)

### CareerMode Scene
1. Create new scene: File → New Scene
2. Save as: `Assets/Scenes/CareerMode.unity`
3. Add UI for team management
4. Add player roster display
5. Add statistics panel

## Build Configuration

### For Android

1. **Configure Build Settings**
   - File → Build Settings
   - Add scenes (drag .unity files)
   - Select "Android" platform
   - Click "Switch Platform"

2. **Player Settings**
   - Product Name: Football 3D Game
   - Package Name: com.yourcompany.football3dgame
   - API Level: 24 (min) to 34 (target)
   - Orientation: Portrait or Landscape (your choice)

3. **Build APK**
   - File → Build Settings → Build
   - Choose location to save APK
   - Wait for build completion

4. **Install on Device**
   ```bash
   adb install -r path/to/Football3DGame.apk
   ```

## Development Workflow

### Code Organization

1. **Namespaces**
   ```csharp
   namespace Football3D.Core { }
   namespace Football3D.Player { }
   namespace Football3D.AI { }
   namespace Football3D.UI { }
   namespace Football3D.Network { }
   ```

2. **Coding Standards**
   - Use PascalCase for classes and methods
   - Use camelCase for variables
   - Use UPPER_SNAKE_CASE for constants
   - Always use access modifiers (public, private, protected)
   - Add XML documentation comments

3. **Commit Messages**
   ```bash
   git commit -m "[Category] Brief description"
   # Examples:
   git commit -m "[Feature] Add player shooting mechanics"
   git commit -m "[Fix] Correct ball physics collision"
   git commit -m "[Refactor] Optimize AI pathfinding"
   ```

## Testing

### In Editor
1. Click "Play" button
2. Test game mechanics
3. Check console for errors (Window → General → Console)

### On Device
1. Connect Android device
2. Enable USB Debugging on device
3. File → Build Settings → Build and Run
4. APK installs and launches automatically

## Troubleshooting

### Issue: "Android SDK not found"
- Solution: Edit → Preferences → External Tools → Android SDK Path
- Set correct path to Android SDK

### Issue: "Build fails on symbol not found"
- Solution: Clean build cache
  - File → Build Settings → Clean Build
  - Delete Library folder
  - Reimport project

### Issue: "Game crashes on startup"
- Solution: Check Android Manifest
- Verify API Level compatibility
- Check logcat for errors: `adb logcat`

## Next Steps

1. ✅ Import 3D models and assets
2. ✅ Implement player characters
3. ✅ Create stadium environments
4. ✅ Add animations and effects
5. ✅ Implement AI behavior
6. ✅ Set up multiplayer network
7. ✅ Create UI screens
8. ✅ Add sound and music
9. ✅ Optimize for mobile
10. ✅ Beta testing and deployment

## Resources

- Unity Documentation: https://docs.unity3d.com
- Android Development: https://developer.android.com
- C# Documentation: https://docs.microsoft.com/en-us/dotnet/csharp
- Game Development Tutorials: https://learn.unity.com

---
**Last Updated**: 2026-04-25