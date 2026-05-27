# Unity Steamworks Wrapper

A modular and easy-to-use Steamworks wrapper for Unity with automatic initialization and service-based architecture.

---

## ✨ Features

- Stats system
- Achievements system
- Avatar loading (Texture2D / Sprite)
- Rich Presence (status)
- Cloud Save (Coming Soon)
- Modular services (plug-and-play)
- Custom Bootstrap system

---

## 🧰 Setup

### 1. Install Steamworks.NET
Import Steamworks.NET into your Unity project.

---

### 2. Add Manager to Scene

Add `SteamWrapperManager` to a GameObject.

- Set your **App ID**
- Assign `SteamBoostrap`

---

### 3. Configure Bootstrap

In `SteamBoostrap`:

- Enable **Should Start API**
- Add required services to **Component Prefabs**

Example:
- SteamStatsService  
- SteamOverlayService  
- SteamAvatarService  
- SteamCloudSaveService  

---

## 🚀 Usage

### Get Service

```csharp
var statsService = bootstrap.GetSteamComponent<SteamStatsService>();
