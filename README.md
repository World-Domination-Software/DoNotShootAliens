# Working Title: [Zombie Rescue Game]

**Studio:** World Domination Software LLC
**Engine:** Unity
**Networking:** Mirror
**Platforms (target):** PC, Mobile
**Document Status:** Living Design Document — Phase 1 MVP Focus

---

## Elevator Pitch

A third-person arcade survival shooter where players run through a dangerous small city map, fight off waves of pursuing zombies, and rescue trapped survivors before it's too late. Play alone or team up with up to three others in local co-op hosted through Mirror. Shoot your way through the streets, keep your survivors alive, and get everyone out in one piece.

Fast. Readable. Designed for replayability.

---

## Phase-Based Development Strategy

This game is built in deliberate phases. The goal of phased development is to ship a clean, playable core loop first, then layer in additional content and systems only after the foundation is stable and fun.

**This document is focused on Phase 1 MVP.** Phase 2 and Phase 3 are defined at a high level only, to give the team directional awareness without distracting from the immediate build.

### Phase 1 — Core Playable Loop (Current Focus)

Build a fully playable single-player and co-op round on one small city map. All core systems — movement, shooting, zombie AI, survivor rescue, and win/lose logic — must be functional, readable, and stable before moving forward.

### Phase 2 — Expanded Content

Add additional maps, additional zombie variants, and quality-of-life improvements based on playtest feedback. Begin exploring limited ammo mechanics or optional pickups. Improve visual and audio polish.

### Phase 3 — Progression and Features

Introduce a progression system, player profiles, online leaderboards, weapon unlocks, upgrade paths, and more advanced multiplayer features. Target platform polish for mobile and PC storefronts.

---

## Phase 1 MVP Scope

### In Scope

- Third-person player movement and camera
- Aiming and shooting
- Two weapons: **pistol** and **rifle**
- Unlimited ammo for both weapons
- Weapon switching (pistol / rifle)
- Player health and damage from zombies
- Player death and failure conditions
- Zombie enemies that spawn, chase, and attack
- Survivors placed around the map — stationary until rescued
- Survivor rescue by touching a trigger area
- Rescued survivors follow the player
- Zombies can kill unrescued survivors
- Small city-style map with defined spawn and placement points
- Single-player offline play
- Co-op multiplayer for up to 4 players using Mirror (host + join)
- Simple lobby / ready-up flow for co-op
- Win and lose conditions
- End-of-round results screen (score summary)

### Out of Scope for Phase 1

- Power-ups of any kind
- Ammo pickups or ammo scarcity
- Weapon pickups
- Weapon unlocks
- Upgrade systems
- Inventory systems beyond pistol/rifle switching
- Player classes or perks
- Crafting
- Loot drops
- Extraction zones
- Multiple map objectives
- Boss zombies
- Special zombie variants beyond one optional stronger type
- Procedural map generation
- Online leaderboards or cloud services
- Player profiles or progression
- Monetization systems

---

## Genre, Camera, and Platform Targets

**Genre:** Third-person arcade survival shooter / rescue action game

**Inspiration:** Robotron 2084 — realized as a 3D third-person Unity game. The goal is fast, readable, arcade-like pressure: run, shoot, rescue, survive.

**Camera:** Third-person follow camera. The player character is always visible. The camera should provide clear sightlines for navigating streets and spotting both zombies and survivors.

**Platforms:**

- **PC** — primary development target
- **Mobile** — keep architecture and controls mobile-aware from the start
- **Cross-platform multiplayer** — Mirror-based networking architecture must not block future mobile/PC cross-play

Phase 1 development focuses on playable architecture and game loop correctness. Final platform polish (touch controls, mobile performance tuning, store integration) is deferred to a later phase.

---

## Core Gameplay Loop

```
Spawn into map
     ↓
Move through city streets
     ↓
Locate survivors (static on map)
     ↓
Avoid or engage pursuing zombies
     ↓
Reach survivor trigger → survivor rescued → follows player
     ↓
Zombies threaten both players and unrescued survivors
     ↓
Continue rescuing until objective met OR failure condition triggered
     ↓
Round ends → results screen shown
```

Players must constantly balance fighting zombies (to clear paths and protect survivors) against moving efficiently through the map to rescue survivors before zombies reach them. Tension escalates as rescued survivors following the player create a larger, harder-to-protect group.

---

## Game Modes

### Single Player

The player loads directly into the gameplay scene with no lobby required. All gameplay rules are identical to co-op — same map, same objectives, same win/lose conditions — so that single-player serves as a valid test environment for all core systems.

- Offline / local play only in Phase 1
- No network connection required
- Direct start from main menu into gameplay scene
- One player, full map, full zombie and survivor population

### Co-op Multiplayer

Up to 4 players can play together. One player acts as host; others join via Mirror.

- Host starts a lobby from the main menu
- Other players join using a connection method appropriate for Phase 1 (LAN or direct IP)
- Players see a lobby/ready-up screen before the round starts
- When all players are ready, the host launches the game
- All players spawn into the same gameplay scene
- Objectives are shared — survivors rescued by any player count toward the team total
- Combat is shared — any player can shoot zombies
- If a player dies, they are out for the round (no mid-round respawn in Phase 1) or the team continues until win/lose is triggered
- Results screen shown to all players at round end

---

## Player Design

### Movement

Third-person movement relative to camera direction. The player can run in any direction across the city map. A modest run speed is appropriate — fast enough to evade zombies, slow enough that kiting a crowd is a risk rather than a guarantee.

### Combat

- **Aim and fire** toward a target direction or crosshair
- Projectile-based shooting (not hitscan, but can be fast-moving projectiles for arcade feel)
- **Weapon switching** toggles between pistol and rifle

### Weapons

- **Pistol** — always available, no switching penalty
- **Rifle** — always available, no switching penalty
- Switching is instantaneous or near-instantaneous in Phase 1

### Ammo

- Unlimited ammo for both weapons in Phase 1
- Reloading may be included for game feel (e.g., a short reload animation after a clip empties), but reserve ammo is effectively infinite — there is no ammo management
- **There are no ammo pickups in Phase 1**

### Health

- Players have a health value
- Zombies deal damage on contact or via attack
- Player health is visible in the HUD
- When health reaches zero, the player dies

### Death

- A dead player is out of the round
- In single-player, player death triggers a loss condition
- In co-op, the round continues if other players are alive; if all players are dead, the round ends in failure

### Survivor Collection

- Survivors are collected by entering a trigger zone near them
- No button press required — proximity triggers rescue
- Rescued survivors attach to the player as a following group

---

## Weapons

### Pistol

- Steady, reliable sidearm
- Moderate fire rate
- Lower damage per shot
- Good for picking off isolated zombies and maintaining controlled fire
- Default weapon

### Rifle

- Higher damage or faster fire rate than pistol
- Better suited to crowd pressure — holding back groups, clearing corridors
- May have a slightly longer reload time for balance feel

Both weapons are immediately available to the player from the start of every round. There is no unlocking, no purchasing, and no running out of ammo. Switching is available at all times.

> **Future phases may include:** additional weapons, a limited ammo mode, ammo pickups, weapon upgrades, and per-weapon progression. None of this is part of Phase 1.

---

## Zombie Design

### Core Behavior

Zombies are the primary threat and pressure mechanic of the game. Their job is to force the player to keep moving, make decisions, and prioritize rescue over loitering.

**Basic Zombie Behavior (Phase 1):**

- Spawns at defined spawn points on the map
- Roams slowly when no player is nearby
- Detects players within a detection radius
- Chases the nearest detected target (player or unrescued survivor)
- Attacks at close range — deals damage on contact or via a short-range attack
- Can be killed by player projectiles
- Creates crowd danger when in groups — individual zombies are manageable; packs are threatening

### Threat to Survivors

Zombies do not only chase players. If a zombie reaches an **unrescued survivor** before the player does, the survivor dies and is lost for the round. This is the core pressure mechanic: move fast, plan your route, keep zombies away from survivor locations.

Rescued survivors following the player are also vulnerable — a zombie that catches the group can kill survivors before the player can react.

### Zombie Variants

Phase 1 can ship with a single basic zombie type. Optionally, one stronger variant (e.g., a slower but higher-health brute) can be included if it does not add complexity to Phase 1 implementation.

> **Future phases may include:** fast zombies, ranged attackers, boss zombies, and special variants with unique behaviors.

---

## Survivor Design

Survivors are the objective. Rescuing them is how the player wins.

### Phase 1 Behavior

- Survivors are placed at fixed points on the map at round start
- Survivors are stationary and passive — they do not move, fight, or flee on their own
- When a player enters the survivor's trigger zone, the survivor is instantly rescued
- Rescued survivors follow the rescuing player in a simple chain/follow pattern
- Survivors do not require complex AI in Phase 1 — a simple follow target behavior is sufficient
- Survivors have no weapons and cannot defend themselves

### Survivor Death

- If a zombie reaches an unrescued survivor (not yet inside trigger zone), the survivor dies
- Dead survivors are removed from the map and cannot be recovered
- Survivor deaths count against the player's score and may trigger a failure condition if too many are lost

### Survivor Contribution

- Each rescued survivor contributes to the round score
- The number of survivors rescued is tracked and displayed on the results screen
- Win conditions may be tied to rescuing a minimum number of survivors

---

## Map / Level Design

### Phase 1 Map Overview

One small city-style map. The map should be tight enough to feel dangerous and readable, but large enough that players must navigate and make routing decisions. It is not an open world, and it is not procedurally generated in Phase 1.

**Design goals:**

- Arcade-friendly layout — streets, intersections, and simple buildings
- Clear sightlines down key corridors for shooting and zombie visibility
- Enough structures to break line of sight and allow kiting
- Readable at a glance — players should be able to identify threats and objectives quickly

### Key Map Zones

| Zone | Purpose |
|---|---|
| Player Spawn Area | Safe start zone; players begin here |
| Zombie Spawn Points | Defined edge/alley points; zombies enter the map here |
| Survivor Placement Points | Fixed locations around the map; survivors wait here |
| Open Streets | Movement corridors; allow kiting and shooting lanes |
| Tight Alleys / Cover Zones | Create pressure; limit escape routes |

### Design Notes

- Player spawn should be relatively open to allow orientation
- Zombie spawns should be spread around the map edges to create pressure from multiple directions
- Survivor placements should be distributed — some close, some deep in danger zones — to create routing decisions
- The map should reward players who plan routes rather than running randomly

---

## Win / Lose Conditions

### Recommended Phase 1 Win Condition

**Rescue a required number of survivors** (e.g., 80% of total survivors on the map) before the round timer ends or before too many survivors are lost.

- Simple to implement
- Directly tied to the core objective
- Creates urgency without requiring complex multi-objective logic

### Failure Conditions

- **All players dead** — round ends in failure immediately
- **Too many survivors lost** — if zombie kills reduce the surviving pool below the minimum required, the round cannot be completed and ends in failure
- *(Optional)* A round timer that triggers failure if the objective isn't met in time

### Outcome States

| Result | Condition |
|---|---|
| **Victory** | Required survivors rescued, at least one player alive |
| **Defeat** | All players dead, or minimum survivors lost, or timer expired |

---

## Scoring / Results

At the end of each round, a results screen displays key stats. Phase 1 scoring should be local only — no cloud saves, no leaderboard services.

### Phase 1 Results Metrics

| Metric | Notes |
|---|---|
| Survivors rescued | Primary metric |
| Survivors lost | Negative outcome indicator |
| Zombies killed | Combat score |
| Damage taken | Player efficiency metric |
| Time to complete | Speed bonus indicator |
| Player accuracy | Optional, if shot tracking is implemented |
| Co-op ranking | In co-op, per-player rescue/kill count if tracked |

### Storage

Local high scores or session results saved to device storage are sufficient for Phase 1. A simple JSON or PlayerPrefs-based save is acceptable.

---

## Networking Approach

Co-op multiplayer uses **Mirror** in a host/client model.

### Architecture Summary

- One player acts as host — the host machine runs the authoritative game logic
- Up to 3 other players connect as clients
- The lobby scene handles connection, player readiness, and match launch
- On match start, all players load into the shared gameplay scene
- Authoritative game logic (zombie AI, survivor states, win/lose evaluation) runs on the host
- Client inputs are sent to host; host updates game state and syncs to clients
- Single-player and co-op share the same core gameplay systems — single-player is effectively a hosted session with no remote players

### Phase 1 Networking Scope

- LAN or direct IP connection is sufficient for Phase 1
- Steam, relay servers, or matchmaking are deferred
- Player count: 1–4
- Mirror NetworkManager handles scene transitions (lobby → gameplay → results)

---

## Scene Flow

```
Boot Scene
     ↓
Main Menu
  ├──→ Single Player → [load Gameplay Scene directly]
  └──→ Co-op → Lobby Scene
                   ↓
              [Players join, ready up]
                   ↓
              [Host launches]
                   ↓
              Gameplay Scene
                   ↓
              [Round ends — victory or defeat]
                   ↓
              Results Screen / Results UI
                   ↓
              [Return to Main Menu or Lobby]
```

The results screen can be a UI overlay within the gameplay scene or a separate results scene — either approach is acceptable for Phase 1.

---

## Art / Audio Direction

### Visual Style

- **Readability first.** Zombie silhouettes must be distinct from survivors and players — clear shape language, strong contrast
- **Survivors** should look clearly human and passive — not threatening
- **City map** should be readable and functional — simple geometry, clear lanes, no visual clutter that obscures gameplay
- **Arcade clarity over realism.** Assets do not need to be photorealistic; they need to communicate what they are instantly
- Placeholder geometry and prototype assets are fine for Phase 1 — do not block gameplay progress waiting on art

### Audio

- **Zombie audio:** growls, movement, attack sounds — audible zombie presence helps players track threats off-screen
- **Weapon audio:** distinct pistol and rifle sounds — give weapons a satisfying feel
- **Survivor rescue:** a clear audio cue when a survivor is collected
- **Zombie kill:** impact feedback sound
- **Music:** tension-building ambient track that escalates as danger increases; keep it loopable and simple
- **UI sounds:** minimal, functional

---

## Technical Design Notes

- **Engine:** Unity (latest stable LTS recommended)
- **Networking:** Mirror for all multiplayer features
- **Shared systems:** Single-player and co-op must use the same gameplay systems — no separate codepaths for core logic
- **Prefab-driven design:** Enemies, survivors, weapons, and spawn points should be prefab-based for easy map editing and configuration
- **Scriptable Objects:** Use ScriptableObjects for weapon stats, zombie configs, and survivor parameters — avoids hardcoding and simplifies tuning
- **Mobile awareness:** Avoid input patterns that cannot be mapped to touch controls; keep performance targets mobile-realistic from the start
- **Keep MVP lean:** Do not add systems that are not in Phase 1 scope. Resist scope creep. Build clean, test, then expand.

---

## Future Phases / Deferred Features

The following features are **intentionally deferred** and should not be designed or implemented until Phase 1 is stable:

- Power-ups of any kind
- Ammo pickups or a limited ammo mode
- Weapon pickups
- Weapon unlocks or weapon progression
- Upgrade systems
- Special zombie variants (fast, ranged, boss)
- Player classes or perk systems
- Extraction zones
- Multiple simultaneous map objectives
- Boss zombies
- Crafting or loot drops
- Online matchmaking or relay networking
- Cloud leaderboards or online services
- Player progression profiles
- Cosmetic systems
- Procedural map generation
- Additional maps (deferred to Phase 2)

---

## Development Priorities

Implement in this order. Do not move to the next item until the current item is stable and testable.

1. **Player movement and third-person camera** — everything depends on this
2. **Shooting** — projectiles, hit detection, zombie damage
3. **Zombie chase and attack** — spawn, detect, chase, attack, die
4. **Survivor rescue and follow** — placement, trigger, follow behavior, zombie kill logic
5. **Win and lose rules** — score threshold, failure conditions, round end trigger
6. **Single-player flow** — main menu → gameplay → results, fully playable offline
7. **Co-op lobby and networking** — Mirror integration, lobby scene, shared gameplay
8. **Results and scoring** — end-of-round display, local score storage
9. **Polish** — audio, visual feedback, map layout iteration, HUD refinement

---

## Current MVP Summary

A single small city map. Up to 4 players (1 host + 3 clients via Mirror, or 1 player offline). Players move in third person, shoot zombies with a pistol or rifle (unlimited ammo), and rescue survivors by touching them. Rescued survivors follow the player. Zombies chase and kill players and survivors. The round ends when enough survivors are rescued (victory) or when failure conditions are met. Results are shown at round end.

That is the entire game for Phase 1. Nothing else.

---

## Out of Scope for Phase 1

No power-ups. No pickups. No ammo limits. No weapon unlocks. No upgrades. No classes. No extraction zones. No boss zombies. No loot. No crafting. No online services. No leaderboards. No progression systems. No additional maps.

---

## Recommended Next Step After This Document

With this GDD agreed upon by the team, the recommended next step is to create the Unity project, set up the GitHub repository structure, and begin implementing **item 1 from the development priorities list**: player movement and the third-person camera. Do not begin any other system until the player can move correctly in the scene.
