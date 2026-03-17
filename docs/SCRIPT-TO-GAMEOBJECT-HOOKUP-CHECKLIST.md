# Script-to-GameObject Hookup Checklist

**Project:** Zombie Rescue Game (Phase 1 MVP)
**Engine:** Unity | **Networking:** Mirror

---

## 1. Purpose of This Document

This file maps every Phase 1 script to the Unity scene objects and prefabs it belongs on. Use it as a step-by-step editor setup checklist after the scripts have been created. It is not a code guide — it is a wiring guide for manually assembling the scene hierarchy, assigning components, and filling in inspector references.

Work through each section in order. By the end you should have a minimal but fully playable build.

---

## 2. Assumptions

- All Phase 1 scripts already exist in the project.
- Mirror is installed and available in the Package Manager.
- You will create scenes, prefabs, and UI Canvas objects manually in the Unity Editor.
- Placeholder capsule/cube models are acceptable for first-pass testing. Art polish comes later.
- Unity's built-in Input System or a thin input adapter wrapper is in place.
- NavMesh is baked on the gameplay scene for zombie movement if NavMeshAgent is used.

---

## 3. Recommended Scene List

| Scene Name | Purpose |
|---|---|
| **Boot** | First scene loaded; initializes persistent managers and routes to MainMenu |
| **MainMenu** | Title screen with Single Player, Host Co-op, Join Co-op, and Quit buttons |
| **Lobby** | Mirror lobby/ready-up screen for co-op; transitions to gameplay on host launch |
| **CityMap_01** | Primary Phase 1 gameplay scene; contains map geometry, spawn points, HUD, and results canvas |

---

## 4. Build Settings Checklist

Open **File → Build Settings** and add scenes in this order:

- [ ] Index 0 — `Boot`
- [ ] Index 1 — `MainMenu`
- [ ] Index 2 — `Lobby`
- [ ] Index 3 — `CityMap_01`

> Mirror's `NetworkManager` references gameplay and lobby scenes by name; make sure scene names match exactly.

---

## 5. Global / Persistent Managers

These objects live in the **Boot** scene (and optionally persist across scenes via `DontDestroyOnLoad`). Some may also be re-created at the root of the gameplay scene if you prefer scene-local managers.

### 5.1 `GameSessionManager`

| Field | Value |
|---|---|
| **Recommended GameObject name** | `GameSessionManager` |
| **Scripts to attach** | `GameSessionManager` |
| **Inspector — default scene names** | Fill in `mainMenuScene`, `lobbyScene`, `gameplayScene` string fields |
| **Inspector — mode flag** | `isSinglePlayer` bool (set at runtime by menu) |

### 5.2 `NetworkRoot` *(co-op only, optional in Boot)*

| Field | Value |
|---|---|
| **Recommended GameObject name** | `NetworkRoot` |
| **Scripts to attach** | `CoopZombieRoomManager` (extends `NetworkRoomManager`), `KcpTransport` (or your chosen Mirror transport) |
| **Inspector — Room Scene** | `Lobby` |
| **Inspector — Gameplay Scene** | `CityMap_01` |
| **Inspector — Max Players** | `4` |
| **Inspector — Player Prefab** | drag `Player_NetworkRoot` prefab here (see §9) |

If you keep `NetworkRoot` only in the Lobby scene, leave this object out of Boot.

### 5.3 `ScoreManager`

| Field | Value |
|---|---|
| **Recommended GameObject name** | `ScoreManager` |
| **Scripts to attach** | `ScoreManager` |
| **Inspector — Score Profile** | drag a `ScoreProfile` ScriptableObject asset here (see §16) |

### 5.4 `MapSpawnController`

| Field | Value |
|---|---|
| **Recommended GameObject name** | `MapSpawnController` |
| **Scripts to attach** | `MapSpawnController` |
| **Inspector — Map Config** | drag a `MapLevelConfig` ScriptableObject asset here (see §15) |
| **Inspector — Zombie Prefab** | drag `Zombie_Basic` prefab |
| **Inspector — Survivor Prefab** | drag `Survivor_Basic` prefab |
| **Inspector — Player Prefab** *(SP only)* | drag `Player_NetworkRoot` prefab |

---

## 6. Main Menu Scene Hookup

### 6.1 Scene hierarchy

```
MainMenu (scene root)
  └── MainMenuCanvas          [Canvas, CanvasScaler, GraphicRaycaster]
        ├── MainMenuController [MainMenuController script]
        ├── SinglePlayerButton [Button]
        ├── HostCoopButton     [Button]
        ├── JoinCoopButton     [Button]
        └── QuitButton         [Button]
```

### 6.2 `MainMenuController`

- Attach script `MainMenuController` to the **MainMenuController** child GameObject (or directly to the Canvas root — either works).
- Inspector fields to assign:

| Inspector Field | Assign |
|---|---|
| `gameSessionManager` | drag `GameSessionManager` object (from Boot, if persistent) |
| `joinIPField` | drag the InputField used to type a host IP address (optional for Phase 1) |

### 6.3 Button `OnClick` wiring

Open each Button's **OnClick** list in the inspector and wire as follows:

| Button | Target Object | Method |
|---|---|---|
| `SinglePlayerButton` | `MainMenuController` | `OnSinglePlayerClicked()` |
| `HostCoopButton` | `MainMenuController` | `OnHostCoopClicked()` |
| `JoinCoopButton` | `MainMenuController` | `OnJoinCoopClicked()` |
| `QuitButton` | `MainMenuController` | `OnQuitClicked()` |

---

## 7. Lobby Scene Hookup

### 7.1 Scene hierarchy

```
Lobby (scene root)
  ├── NetworkRoot              [CoopZombieRoomManager, KcpTransport]
  └── LobbyCanvas             [Canvas]
        ├── LobbyUIController  [LobbyUIController script]
        ├── PlayerSlot_01 … 04 [Text or panel, updated by LobbyUIController]
        ├── ReadyButton        [Button]
        └── StartButton        [Button]  (host only — disable for clients)
```

### 7.2 `NetworkRoot`

- **Scripts to attach:** `CoopZombieRoomManager`, your Mirror transport (e.g., `KcpTransport`).
- **Inspector fields:**

| Inspector Field | Value |
|---|---|
| `roomScene` | `"Lobby"` |
| `gameplayScene` | `"CityMap_01"` |
| `roomPlayerPrefab` | drag `LobbyPlayer` prefab (a simple Mirror `NetworkRoomPlayer` wrapper) |
| `playerPrefab` | drag `Player_NetworkRoot` gameplay prefab |
| `maxConnections` | `4` |
| Transport component | `KcpTransport` — default port `7777` for LAN/direct IP |

### 7.3 `LobbyUIController`

- Attach `LobbyUIController` to the **LobbyUIController** GameObject.
- Inspector fields:

| Inspector Field | Assign |
|---|---|
| `readyButton` | drag `ReadyButton` |
| `startButton` | drag `StartButton` |
| `playerSlots[]` | drag each `PlayerSlot_0x` text/panel in order |
| `roomManager` | drag `NetworkRoot` |

### 7.4 Button `OnClick` wiring

| Button | Method |
|---|---|
| `ReadyButton` | `LobbyUIController.OnReadyClicked()` |
| `StartButton` | `LobbyUIController.OnStartClicked()` *(visible on host only)* |

---

## 8. Gameplay Scene Root Setup (`CityMap_01`)

### 8.1 Recommended root objects

```
CityMap_01 (scene root)
  ├── GameSession              [GameSessionManager or local session script]
  ├── MapSpawnController       [MapSpawnController]
  ├── ScoreManager             [ScoreManager]
  ├── SpawnPoints              [empty — parent for all spawn markers]
  │     ├── Spawn_Player_01 … 02
  │     ├── Spawn_Zombie_01 … 05
  │     └── Spawn_Survivor_01 … 05
  ├── HUDCanvas                [Canvas, CanvasScaler — Screen Space Overlay]
  │     └── HUDController      [HUDController script + UI children]
  ├── ResultsCanvas            [Canvas — disabled by default]
  │     └── ResultsController  [ResultsController script + UI children]
  └── CameraRig                [ThirdPersonCameraFollow script]
```

### 8.2 Inspector assignments per object

**`GameSession`**

| Inspector Field | Value |
|---|---|
| `scoreManager` | drag `ScoreManager` |
| `spawnController` | drag `MapSpawnController` |
| `hudController` | drag `HUDController` |
| `resultsController` | drag `ResultsController` |
| `mapConfig` | drag `MapLevelConfig` asset (see §15) |

**`MapSpawnController`**

| Inspector Field | Value |
|---|---|
| `mapConfig` | drag `MapLevelConfig` asset |
| `zombiePrefab` | drag `Zombie_Basic` prefab |
| `survivorPrefab` | drag `Survivor_Basic` prefab |
| `playerPrefab` *(SP)* | drag `Player_NetworkRoot` prefab |

**`ScoreManager`**

| Inspector Field | Value |
|---|---|
| `scoreProfile` | drag `ScoreProfile` asset |

**`CameraRig`**

| Inspector Field | Value |
|---|---|
| Script | `ThirdPersonCameraFollow` |
| `target` | leave blank at design time; assigned at runtime to local player's `CameraTarget` transform |
| `followOffset` | e.g., `(0, 5, -8)` — adjust to taste |
| `smoothSpeed` | `0.12` |

---

## 9. Player Prefab Hookup

This is the most detailed section. The player prefab is used by Mirror in co-op and instantiated directly in single-player.

### 9.1 Recommended prefab hierarchy

```
Player_NetworkRoot               [root]
  ├── Visual                     [MeshRenderer / SkinnedMeshRenderer placeholder]
  ├── FirePoint                  [empty Transform — muzzle position]
  ├── SurvivorCollectTrigger     [Trigger Collider only]
  └── CameraTarget               [empty Transform — camera looks at this]
```

### 9.2 Components on root `Player_NetworkRoot`

| Component | Notes |
|---|---|
| `CharacterController` | Required if movement script uses `CharacterController.Move()` |
| `NetworkIdentity` | Required by Mirror; check **Server Only** if authoritative movement |
| `NetworkTransform` | Syncs position/rotation to clients |
| `PlayerController` | Core movement script |
| `PlayerHealth` | Health value, damage handling, death event |
| `WeaponInventory` | Holds references to both weapon definitions; handles switching |
| `CombatController` | Reads fire input, instantiates projectiles from FirePoint |
| `PlayerStats` | Tracks kills, rescues, damage taken for scoring |
| `PlayerInputAdapter` | Bridges Unity Input System (or legacy input) to the above scripts |
| `NetworkRoomPlayer` *(co-op)* | If using a separate lobby player, this may not be on the gameplay prefab |

### 9.3 Inspector references on `Player_NetworkRoot`

| Field | Assign |
|---|---|
| `PlayerController.inputAdapter` | drag `PlayerInputAdapter` component |
| `PlayerController.characterController` | drag `CharacterController` component |
| `CombatController.firePoint` | drag child `FirePoint` transform |
| `CombatController.weaponInventory` | drag `WeaponInventory` component |
| `WeaponInventory.pistolDefinition` | drag `WD_Pistol` ScriptableObject asset |
| `WeaponInventory.rifleDefinition` | drag `WD_Rifle` ScriptableObject asset |
| `PlayerHealth.maxHealth` | e.g., `100` |
| `PlayerStats.scoreManager` | leave blank; assigned at runtime by `GameSessionManager` |
| `CameraTarget` | the child transform used by `ThirdPersonCameraFollow` |

### 9.4 `SurvivorCollectTrigger` child

| Component | Value |
|---|---|
| Collider (Sphere or Capsule) | **Is Trigger** = ✓ |
| Radius | e.g., `2.0` — tune in play-testing |
| Script | `SurvivorCollector` (if a separate component) or handled by `PlayerController` via `OnTriggerEnter` |

### 9.5 Single-player vs. co-op hookup differences

| Concern | Single-Player | Co-op |
|---|---|---|
| `NetworkIdentity` | Not strictly needed (Mirror not active) but keep it on the prefab for shared code paths | Required — Mirror spawns the prefab |
| Input | Always active for local player | Enable input only on `isLocalPlayer == true` |
| HUD references | Assign at spawn time by `GameSessionManager` | Assign only for local player; clients ignore remote player HUD |
| Camera | `ThirdPersonCameraFollow.target` set to this player's `CameraTarget` | Same, but only for the local player's instance |

> **Rule:** Guard any local-only setup (camera, HUD, input enable) behind `if (isLocalPlayer)` inside the player's `OnStartLocalPlayer()` override.

---

## 10. Weapon Definition Asset Setup

Weapon definitions are ScriptableObjects. Create one asset per weapon.

### 10.1 Creating assets

1. Right-click in the **Project** window → **Create → Game → WeaponDefinition** (or whichever menu path your `[CreateAssetMenu]` attribute defines).
2. Name the assets `WD_Pistol` and `WD_Rifle`.

### 10.2 Fields to fill in

| Field | WD_Pistol (example) | WD_Rifle (example) |
|---|---|---|
| `weaponName` | `"Pistol"` | `"Rifle"` |
| `damage` | `15` | `30` |
| `fireRate` | `0.4` (seconds between shots) | `0.15` |
| `magazineSize` | `12` | `30` |
| `reloadDuration` | `1.2` | `2.0` |
| `range` | `30` | `60` |
| `isAutomatic` | `false` | `true` |
| `projectilePrefab` | drag bullet prefab | drag bullet prefab |
| `muzzleFlashPrefab` | optional | optional |

### 10.3 Assigning to the player

Drag each asset into the corresponding fields on `WeaponInventory` in the Player prefab inspector (see §9.3).

---

## 11. Zombie Prefab Hookup

### 11.1 Recommended prefab hierarchy

```
Zombie_Basic                    [root]
  ├── Visual                    [MeshRenderer placeholder]
  └── AttackOrigin              [empty Transform — optional, for melee hit detection]
```

### 11.2 Components on root `Zombie_Basic`

| Component | Notes |
|---|---|
| `CapsuleCollider` | Physical body collider; **Is Trigger** = ✗ |
| `NavMeshAgent` | Required for pathfinding; bake NavMesh in scene before testing |
| `NetworkIdentity` | Required for Mirror sync; **Server Authority** recommended |
| `NetworkTransform` | Syncs zombie position to clients |
| `ZombieAIController` | Main state machine; drives movement and state transitions |
| `ZombieHealth` | Hit points, damage reception, death |
| `ZombieAttack` | Melee attack logic; deals damage to target on contact or with a short reach |
| `ZombieTargeting` | Scans for nearest player or unrescued survivor within detection radius |

### 11.3 Inspector fields

| Field | Assign |
|---|---|
| `ZombieAIController.zombieDefinition` | drag `ZD_Basic` ScriptableObject asset |
| `ZombieAIController.targeting` | drag `ZombieTargeting` component |
| `ZombieAIController.attack` | drag `ZombieAttack` component |
| `ZombieAIController.agent` | drag `NavMeshAgent` component |
| `ZombieTargeting.detectionRadius` | e.g., `12.0` |
| `ZombieTargeting.chaseRadius` | e.g., `20.0` |
| `ZombieAttack.attackOrigin` | drag child `AttackOrigin` transform |
| `ZombieAttack.attackRadius` | e.g., `1.5` |
| `ZombieAttack.damage` | override or read from `zombieDefinition` |
| `ZombieHealth.scoreManager` | assigned at runtime by spawn controller |

### 11.4 Creating stronger variants

To make a tougher zombie for later:

1. Duplicate the `Zombie_Basic` prefab and rename it `Zombie_Brute`.
2. Drag a `ZD_Brute` ScriptableObject into `ZombieAIController.zombieDefinition`.
3. Adjust stats in the definition asset — no script changes required.

---

## 12. Zombie Definition Asset Setup

Create one `ZombieDefinition` ScriptableObject asset per zombie type.

1. Right-click in Project → **Create → Game → ZombieDefinition**.
2. Name it `ZD_Basic`.

### Fields to configure

| Field | ZD_Basic (example) |
|---|---|
| `zombieName` | `"Basic Zombie"` |
| `maxHealth` | `50` |
| `moveSpeed` | `3.5` |
| `attackDamage` | `10` |
| `attackCooldown` | `1.5` |
| `detectionRadius` | `12.0` |
| `scoreValue` | `10` |
| `roamSpeed` | `1.5` |

Assign this asset to `ZombieAIController.zombieDefinition` on the prefab.

---

## 13. Survivor Prefab Hookup

### 13.1 Recommended prefab hierarchy

```
Survivor_Basic                  [root]
  ├── Visual                    [MeshRenderer placeholder — distinct colour from zombies]
  ├── RescueTrigger             [Trigger Collider — detects player proximity]
  └── FollowAnchor              [optional empty Transform for follow offset calculation]
```

### 13.2 Components on root `Survivor_Basic`

| Component | Notes |
|---|---|
| `CapsuleCollider` | Physical body (non-trigger) so zombies can detect/hit them |
| `NetworkIdentity` | Mirror sync; **Server Authority** |
| `SurvivorController` | Manages rescued/alive/dead states |
| `SurvivorFollowTarget` | Simple follow logic toward rescuing player's position |

### 13.3 `RescueTrigger` child

| Component | Value |
|---|---|
| Collider (Sphere) | **Is Trigger** = ✓ |
| Radius | e.g., `2.0` |

No script on this child is required if `SurvivorController` implements `OnTriggerEnter`. If you use a separate script, attach `SurvivorRescueTrigger` and give it a reference back to the parent `SurvivorController`.

### 13.4 Tags / Layers

- Assign the **`Survivor`** tag to `Survivor_Basic` so `ZombieTargeting` can find unrescued survivors via `GameObject.FindGameObjectsWithTag`.
- Optionally use a **`Survivor`** physics layer to control what can collide with the rescue trigger.

### 13.5 Inspector fields

| Field | Assign |
|---|---|
| `SurvivorController.followTarget` | drag `SurvivorFollowTarget` component |
| `SurvivorController.scoreManager` | assigned at runtime |
| `SurvivorFollowTarget.followOffset` | e.g., `(0, 0, -1.5)` — trailing offset behind rescuing player |
| `SurvivorFollowTarget.moveSpeed` | e.g., `4.0` |
| `SurvivorFollowTarget.followAnchor` | drag child `FollowAnchor` transform (optional) |

---

## 14. Spawn Point Setup

Spawn markers are lightweight GameObjects that tell the spawn controller where to place entities.

### 14.1 Creating a spawn marker

1. Create an empty GameObject in the scene.
2. Attach the `SpawnPointMarker` script.
3. Set `SpawnMarkerType` in the inspector to `Player`, `Zombie`, or `Survivor`.
4. Optionally add a Gizmo-drawing component for editor visibility.

### 14.2 Naming convention

Use consistent names for easy scene navigation:

```
Spawn_Player_01
Spawn_Player_02
Spawn_Zombie_01 … Spawn_Zombie_05
Spawn_Survivor_01 … Spawn_Survivor_05
```

### 14.3 Placement checklist

- [ ] Place all spawn markers under the `SpawnPoints` parent object in the scene hierarchy.
- [ ] Set correct `SpawnMarkerType` on every marker — wrong types cause silent spawn failures.
- [ ] Place at least **2** player spawn markers (one per expected test player).
- [ ] Place at least **5** zombie spawn markers spread around the map edges.
- [ ] Place at least **5** survivor spawn markers distributed across the map interior.
- [ ] Make sure no spawn marker is inside geometry or below the NavMesh surface.

### 14.4 Assigning to `MapSpawnController`

`MapSpawnController` finds markers automatically using `FindObjectsOfType<SpawnPointMarker>()` at runtime, or you can drag them into serialised arrays in the inspector for explicit control.

---

## 15. Map Config Asset Setup

Create one `MapLevelConfig` ScriptableObject per playable map.

1. Right-click in Project → **Create → Game → MapLevelConfig**.
2. Name it `MC_CityMap_01`.

### 15.1 Fields to fill in

| Field | Example Value |
|---|---|
| `mapName` | `"City Map 01"` |
| `zombieCount` | `15` |
| `survivorCount` | `8` |
| `allowedZombieDefinitions[]` | drag `ZD_Basic` asset |
| `targetSurvivorsToRescue` | `6` (75% of 8) |
| `maximumSurvivorsLost` | `3` |
| `timeLimitSeconds` | `0` (0 = no limit) |
| `scoreProfile` | drag `SP_Default` ScriptableObject asset (see §16) |

### 15.2 Assigning in the scene

Drag `MC_CityMap_01` into:

- `MapSpawnController.mapConfig`
- `GameSession.mapConfig`

---

## 16. Score Profile Asset Setup

Create one `ScoreProfile` ScriptableObject for Phase 1.

1. Right-click in Project → **Create → Game → ScoreProfile**.
2. Name it `SP_Default`.

### 16.1 Fields to fill in

| Field | Example Value | Notes |
|---|---|---|
| `pointsPerZombieKill` | `10` | |
| `pointsPerSurvivorRescued` | `50` | |
| `penaltyPerSurvivorLost` | `-30` | negative value |
| `penaltyPerDamageTaken` | `-1` | per HP lost |
| `timeBonusPerSecondRemaining` | `2` | `0` if no time limit |
| `accuracyBonusMultiplier` | `0` | `0` to disable in Phase 1 |

### 16.2 Where it is referenced

- Drag into `ScoreManager.scoreProfile`.
- Also referenced by `MapLevelConfig.scoreProfile` so the map can override scoring per level if needed.

---

## 17. HUD Setup

### 17.1 HUD Canvas hierarchy

```
HUDCanvas                       [Canvas — Screen Space Overlay]
  └── HUDController             [HUDController script]
        ├── HealthBar           [Slider or Image (fill)]
        ├── HealthText          [TextMeshProUGUI — e.g., "75 / 100"]
        ├── WeaponNameText      [TextMeshProUGUI — e.g., "Rifle"]
        ├── AmmoText            [TextMeshProUGUI — e.g., "∞"]
        ├── SurvivorsRescuedText[TextMeshProUGUI — e.g., "3 / 8"]
        └── ObjectiveText       [TextMeshProUGUI — e.g., "Rescue 6 survivors"]
```

### 17.2 `HUDController` inspector fields

| Field | Assign |
|---|---|
| `healthBar` | drag `HealthBar` Slider |
| `healthText` | drag `HealthText` TMP component |
| `weaponNameText` | drag `WeaponNameText` TMP component |
| `ammoText` | drag `AmmoText` TMP component |
| `survivorsRescuedText` | drag `SurvivorsRescuedText` TMP component |
| `objectiveText` | drag `ObjectiveText` TMP component |

### 17.3 How the HUD finds the local player

`HUDController` should not be linked to the player prefab in the scene. Instead, `GameSessionManager` (or the player's `OnStartLocalPlayer()`) calls `HUDController.BindPlayer(PlayerHealth health, WeaponInventory weapons, PlayerStats stats)` at runtime. This keeps the HUD decoupled from the prefab.

---

## 18. Results Screen Setup

### 18.1 Results Canvas hierarchy

```
ResultsCanvas                   [Canvas — disabled by default]
  └── ResultsController         [ResultsController script]
        ├── ResultTitleText     [TextMeshProUGUI — "VICTORY" or "DEFEAT"]
        ├── ScoreSummaryPanel   [Panel with child TextMeshProUGUI rows]
        │     ├── ZombiesKilledText
        │     ├── SurvivorsRescuedText
        │     ├── SurvivorsLostText
        │     ├── DamageTakenText
        │     └── TotalScoreText
        ├── PlayerRankingList   [ScrollRect + VerticalLayoutGroup — co-op only]
        ├── RetryButton         [Button]
        └── BackToMenuButton    [Button]
```

### 18.2 `ResultsController` inspector fields

| Field | Assign |
|---|---|
| `resultTitleText` | drag `ResultTitleText` |
| `zombiesKilledText` | drag `ZombiesKilledText` |
| `survivorsRescuedText` | drag `SurvivorsRescuedText` |
| `survivorsLostText` | drag `SurvivorsLostText` |
| `damageTakenText` | drag `DamageTakenText` |
| `totalScoreText` | drag `TotalScoreText` |
| `playerRankingList` | drag `PlayerRankingList` ScrollRect (co-op) |
| `retryButton` | drag `RetryButton` |
| `backToMenuButton` | drag `BackToMenuButton` |

### 18.3 Button `OnClick` wiring

| Button | Method |
|---|---|
| `RetryButton` | `ResultsController.OnRetryClicked()` |
| `BackToMenuButton` | `ResultsController.OnBackToMenuClicked()` |

`ResultsController` should be activated by `GameSessionManager.ShowResults(RoundResult result)` — not enabled in the scene by default.

---

## 19. Camera Setup

### 19.1 Camera object

```
CameraRig                       [empty GameObject at scene root]
  └── Main Camera               [Camera component + AudioListener]
```

Attach `ThirdPersonCameraFollow` to `CameraRig` (or directly to `Main Camera` — either works).

### 19.2 Inspector fields

| Field | Value |
|---|---|
| `target` | **leave blank** at design time; assigned at runtime to local player's `CameraTarget` child |
| `followOffset` | `(0, 5, -8)` — starting suggestion; tune in play-testing |
| `lookOffset` | `(0, 1.5, 0)` — look slightly above the character root |
| `smoothSpeed` | `0.12` — lerp factor per frame |
| `rotateWithPlayer` | `false` or `true` — depends on your desired camera behaviour |

### 19.3 Local player only

The camera follows only the local player. In co-op, `OnStartLocalPlayer()` on the player script calls:

```csharp
FindObjectOfType<ThirdPersonCameraFollow>().SetTarget(cameraTargetTransform);
```

Remote player instances must **not** reassign the camera target.

---

## 20. Single-Player Test Checklist

Run through these steps after completing the hookup above:

- [ ] Open the **Boot** scene and press Play.
- [ ] Click **Single Player** in the main menu — gameplay scene loads.
- [ ] Player spawns at a `Spawn_Player` marker.
- [ ] Player moves with WASD / gamepad / touch input.
- [ ] Third-person camera follows the player.
- [ ] Zombies spawn and begin chasing the player.
- [ ] Shooting fires a projectile from `FirePoint`.
- [ ] Projectile damages and kills zombies; kill is registered by `ScoreManager`.
- [ ] Player can reach a survivor's trigger zone — survivor is rescued and follows.
- [ ] Rescued survivor moves behind the player.
- [ ] Zombie reaches an unrescued survivor — survivor dies and is removed.
- [ ] Player health decreases when a zombie attacks.
- [ ] Player death triggers lose condition.
- [ ] Rescuing the required number of survivors triggers the win condition.
- [ ] `ResultsCanvas` appears with correct stats.
- [ ] **Retry** returns to gameplay; **Back to Menu** returns to `MainMenu`.

---

## 21. Co-op Test Checklist

Run these steps with two instances of the build (or Editor + standalone):

- [ ] **Instance A:** Click **Host Co-op** → Lobby loads.
- [ ] **Instance B:** Click **Join Co-op**, enter Host IP → joins Lobby.
- [ ] Both instances show each other in the player slot list.
- [ ] Both click **Ready**; Host clicks **Start** → both load `CityMap_01`.
- [ ] Both players spawn at different `Spawn_Player` markers.
- [ ] Zombies are visible and synchronized on both instances.
- [ ] Zombies target the nearest player correctly.
- [ ] A survivor rescue by Player A is reflected on Player B's HUD.
- [ ] Zombie kill by Player A increments shared score.
- [ ] Player damage is synchronized — health drops on both views.
- [ ] Win/lose condition fires on host and is communicated to all clients.
- [ ] `ResultsCanvas` appears on all instances with consistent stats.
- [ ] No obvious authority conflicts (duplicate zombies, ghost survivors, etc.).

---

## 22. Common Inspector Mistakes

| Symptom | Likely Cause |
|---|---|
| Weapons don't fire | `WeaponInventory.pistolDefinition` or `rifleDefinition` not assigned |
| No projectiles spawn | `CombatController.firePoint` not assigned |
| Zombies don't spawn | No `Spawn_Zombie` markers in scene, or `SpawnMarkerType` set incorrectly |
| Survivors don't spawn | No `Spawn_Survivor` markers, or `MapLevelConfig.survivorCount` is 0 |
| Player doesn't spawn (SP) | `MapSpawnController.playerPrefab` not assigned |
| Zombie ignores player | `ZombieAIController.zombieDefinition` not assigned; detection radius is 0 |
| HUD not updating | `HUDController` references are unassigned; `BindPlayer()` not called at runtime |
| Camera not following | `ThirdPersonCameraFollow.target` is null; `SetTarget()` not called in `OnStartLocalPlayer()` |
| Co-op player not spawning | Player prefab not assigned in `NetworkRoomManager.playerPrefab` |
| Lobby not transitioning | `roomScene` or `gameplayScene` string in `NetworkRoomManager` doesn't match actual scene name |
| Rescue trigger not firing | `RescueTrigger` collider has **Is Trigger** unchecked, or physics layer matrix blocks interaction |
| Survivor not following | `SurvivorFollowTarget.moveSpeed` is 0, or `SurvivorController` state not set to Rescued |
| Win condition never fires | `MapLevelConfig.targetSurvivorsToRescue` is higher than `survivorCount` |
| Results screen stays hidden | `GameSessionManager` reference to `ResultsController` unassigned |

---

## 23. Minimal First-Test Setup Recommendation

For the fastest path to a playable slice, assemble only the following and nothing else:

- [ ] **1 gameplay scene** — `CityMap_01` with flat terrain and NavMesh baked
- [ ] **1 player prefab** — `Player_NetworkRoot` (capsule, no art)
- [ ] **1 zombie prefab** — `Zombie_Basic` (capsule, NavMeshAgent)
- [ ] **1 survivor prefab** — `Survivor_Basic` (capsule, rescue trigger)
- [ ] **1 weapon definition** — `WD_Pistol` with basic stats
- [ ] **1 score profile** — `SP_Default`
- [ ] **1 map config** — `MC_CityMap_01` (5 zombies, 3 survivors, rescue 2)
- [ ] **2 player spawn markers** — `Spawn_Player_01`, `Spawn_Player_02`
- [ ] **5 zombie spawn markers** — `Spawn_Zombie_01` through `Spawn_Zombie_05`
- [ ] **5 survivor spawn markers** — `Spawn_Survivor_01` through `Spawn_Survivor_05`
- [ ] **HUD Canvas** — health, ammo, survivors rescued (text only is fine)
- [ ] **Results Canvas** — title text + back to menu button

Once player movement, zombie chase, survivor rescue, and the win condition all work on this minimal setup, expand to the full scene and assets.

---

## Summary Table

| Object / Asset | Scripts to Attach | Key Assignments |
|---|---|---|
| `GameSessionManager` | `GameSessionManager` | scene name strings, `scoreManager`, `hudController`, `resultsController` |
| `NetworkRoot` (Lobby & optional Boot) | `CoopZombieRoomManager`, `KcpTransport` | `roomScene`, `gameplayScene`, `playerPrefab`, `maxConnections` |
| `ScoreManager` | `ScoreManager` | `scoreProfile` → `SP_Default` |
| `MapSpawnController` | `MapSpawnController` | `mapConfig`, `zombiePrefab`, `survivorPrefab`, `playerPrefab` |
| `MainMenuController` GO | `MainMenuController` | Button `OnClick` events wired to public methods |
| `LobbyUIController` GO | `LobbyUIController` | `roomManager`, `readyButton`, `startButton`, `playerSlots[]` |
| `CameraRig` | `ThirdPersonCameraFollow` | `target` set at runtime via `SetTarget()` |
| `HUDController` GO | `HUDController` | all TextMeshPro/Slider references; bound to local player at runtime |
| `ResultsController` GO | `ResultsController` | all text references, retry/back buttons; activated by `GameSessionManager` |
| `Player_NetworkRoot` (prefab root) | `PlayerController`, `PlayerHealth`, `WeaponInventory`, `CombatController`, `PlayerStats`, `PlayerInputAdapter`, `NetworkIdentity`, `NetworkTransform`, `CharacterController` | `firePoint`, `weaponInventory`, `pistolDefinition`, `rifleDefinition`, `cameraTarget` |
| `SurvivorCollectTrigger` (player child) | *(trigger collider only, or `SurvivorCollector`)* | Is Trigger ✓, radius tuned |
| `Zombie_Basic` (prefab root) | `ZombieAIController`, `ZombieHealth`, `ZombieAttack`, `ZombieTargeting`, `NavMeshAgent`, `NetworkIdentity`, `NetworkTransform`, `CapsuleCollider` | `zombieDefinition` → `ZD_Basic`, `attackOrigin`, detection radii |
| `Survivor_Basic` (prefab root) | `SurvivorController`, `SurvivorFollowTarget`, `NetworkIdentity`, `CapsuleCollider` | `followOffset`, `moveSpeed`, tag = `"Survivor"` |
| `RescueTrigger` (survivor child) | *(trigger collider only)* | Is Trigger ✓, radius tuned |
| `Spawn_Player_XX` | `SpawnPointMarker` | `SpawnMarkerType` = Player |
| `Spawn_Zombie_XX` | `SpawnPointMarker` | `SpawnMarkerType` = Zombie |
| `Spawn_Survivor_XX` | `SpawnPointMarker` | `SpawnMarkerType` = Survivor |
| `WD_Pistol` (ScriptableObject) | — | `damage`, `fireRate`, `magazineSize`, `reloadDuration`, `range`, `isAutomatic` |
| `WD_Rifle` (ScriptableObject) | — | `damage`, `fireRate`, `magazineSize`, `reloadDuration`, `range`, `isAutomatic` |
| `ZD_Basic` (ScriptableObject) | — | `maxHealth`, `moveSpeed`, `attackDamage`, `attackCooldown`, `detectionRadius`, `scoreValue` |
| `MC_CityMap_01` (ScriptableObject) | — | `zombieCount`, `survivorCount`, `targetSurvivorsToRescue`, `maximumSurvivorsLost`, `allowedZombieDefinitions[]`, `scoreProfile` |
| `SP_Default` (ScriptableObject) | — | `pointsPerZombieKill`, `pointsPerSurvivorRescued`, `penaltyPerSurvivorLost`, `penaltyPerDamageTaken` |
