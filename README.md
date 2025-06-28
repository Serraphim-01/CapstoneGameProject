## 🧠 Game Concept  
*Kill* is a **cinematic, level-based 3D action game** where players must navigate through increasingly hostile environments, clear waves of enemies, and survive tactical combat using intelligent movement, precise aiming, and situational awareness.  

The game combines **stealth, strategy, and shooter mechanics**, with both mobile and PC-friendly controls and dynamic transitions between lobby, shop, and levels.  

You don't just *fight*—you **adapt**. You don't just *play*—you **survive**.

---

## 🔥 Core Gameplay Loop

1. **Start in the Player Lobby**  
   - Play through challenges (Level 1–3)
   - Access the **Shop** (Coming Soon)
   - View **Instructions** before combat

2. **Enter a Level (1–3)**  
   - An immersive 2-second cinematic intro locks controls
   - Defeat all enemies (Common, Elite, Boss)
   - Use crouching, aiming, and camera-relative movement
   - Progress to the next level or return to the lobby

3. **Victory / Defeat UI**
   - Displays outcome with context-sensitive buttons:
     - Restart Level
     - Return to Lobby
     - Advance to Next Level (if not on final stage)

---

## 🕹️ Controls & Input

| Action          | PC / Web                          | Mobile                               |
|-----------------|-----------------------------------|---------------------------------------|
| Move            | `WASD`                            | Floating Joystick                     |
| Aim             | Hold Right Mouse Button           | Auto Aim (Camera-relative facing)     |
| Shoot           | Left Mouse Button (while aiming)  | On-screen fire button (Not yet implemented)      |
| Crouch          | `C`                               | Crouch Button                         |
| Toggle Cursor   | `Ctrl`                            | (Auto-handled)                        |

---

## 📱 Mobile Support

- Full floating **joystick-based movement**
- Screen input for **camera lookaround**
- Platform detection auto-switches to **landscape mode** on mobile
- Touch UI fully responsive (USS powered)

---

## 🎧 Audio Design

- **Button Sound FX**: Every UI click plays a sharp, responsive sound  
- **Lobby Music**: Calm and inviting theme for menu areas  
- **Gameplay Music**: Intense and immersive track for all levels  
- **Gunfire FX**: Fired with each successful player shot  
- **Victory/Defeat cues** enhance game atmosphere  

---

## ⚔️ Enemy System

Enemies are categorized by **difficulty and behavior**:

- **Common Enemies**: Basic attackers, low health  
- **Elite Enemies**: Tougher with smarter AI  
- **Boss Enemy**: A single powerful final opponent per level  

The `EnemyManager` tracks all active enemies. Victory is triggered only when all are eliminated.

---

## 🛠️ UI System (Unity UI Toolkit)

Built with modern **Unity UI Toolkit (UITK)**:
- UXML + USS used for modular, scalable layouts
- All UI panels (Main Menu, Shop, Instructions, GameOver) are responsive
- Buttons are styled with retro-futuristic pixelated panels
- Layouts scale based on device resolution (mobile/desktop)

---

## 🏆 Why This Game Wins

- ✨ **Polished UI and Audio Feedback** that immerses players  
- 🎮 **Cross-platform gameplay** from the ground up  
- 📦 **Expandable System** (more levels, shop items, player upgrades)  
- 🧠 **Player Feedback Loop** with victory tracking and level progression  
- 🔊 **Integrated UX Design**: From button sounds to gunshots  

---

## 💡 Future Features

- 🛍️ **Interactive Shop** with power-ups and weapons  
- 🎭 **Character customization**  
- 🧠 **Smart enemy AI** with vision cones and patrol logic  
- 🌐 **Multiplayer arena mode**  

---

## 📦 Project Structure Overview

```bash
Assets/
├── Audio/               # Gunshots, music, UI click sounds
├── Scripts/
│   ├── Player/          # Movement, Aim, Shooting
│   ├── UI/              # UI Toolkit managers
│   └── Managers/        # EnemyManager, UIAudioManager, etc
├── UI/
│   ├── UXML/            # Layouts
│   ├── USS/             # Styles
│   └── Sprites/         # Button backgrounds, panels
└── Scenes/
    ├── PlayerLobby
    ├── Level1
    ├── Level2
    └── Level3
