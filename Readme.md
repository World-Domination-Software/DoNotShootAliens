# 🚀 Don't Shoot Aliens
## Game Design & Development Document

![Space Banner](https://images.unsplash.com/photo-1462331940025-496dfbfc7564)

👨‍💻 Studio: World Domination Software LLC  
🎮 Genre: Arcade Survival / Roguelite Exploration  
📱 Platform: Mobile (Primary)  
🕹 Engine: Unity  
⏱ Target Session Length: 2–5 minutes  

---

# 🌌 Game Overview

**Don't Shoot Aliens** is a fast-paced survival exploration game where players land on alien planets to collect fuel needed to travel deeper into space.

The game combines:

- 🛬 Lunar Lander style ship landing  
- 👾 Arcade survival gameplay  
- 💎 Resource collection mechanics  
- ⚖ Risk vs reward exploration  

Players must gather fuel crystals while avoiding alien swarms and safely returning to their ship.

The longer players remain on the planet, the more dangerous it becomes.

---

# 🔄 Core Gameplay Loop

![Gameplay Loop](https://upload.wikimedia.org/wikipedia/commons/thumb/3/3a/Game_design_flow_chart.svg/1024px-Game_design_flow_chart.svg.png)

Select Planet  
↓  
Land Ship  
↓  
Explore Planet  
↓  
Collect Fuel Crystals  
↓  
Avoid / Shoot Aliens  
↓  
Return Fuel to Ship  
↓  
Launch to Next Planet  
↓  
Unlock New Planets  
↓  
Repeat  

Sessions are intentionally short to encourage the feeling of:

**“Just one more run.”**

---

# 🪐 Planet Selection System

![Planet Map](https://images.unsplash.com/photo-1614728894747-a83421b8e8c6)

Players begin with **one available planet**.

As they collect fuel and store it in their ship, additional planets unlock.

Example progression:

| Planet | Theme | Difficulty |
|------|------|------|
| 🌿 Green Planet | Beginner | Easy |
| 🏜 Desert Planet | Midgame | Medium |
| ❄ Ice Planet | Dangerous | Hard |
| 🐜 Hive Planet | Alien Core | Extreme |

Each planet introduces:

- new alien types  
- higher spawn rates  
- environmental hazards  

---

# 🚀 Ship Mechanics

The spaceship functions as:

🏠 Safe zone  
⛽ Fuel storage  
👨‍🚀 Respawn location  
🛰 Travel vehicle  

Fuel crystals must be **returned to the ship** to count.

If the player dies before depositing fuel:

**fuel carried by the player is lost**

---

# 🛬 Landing Phase

![Lunar Lander](https://upload.wikimedia.org/wikipedia/commons/thumb/6/6c/Lunar_Lander_arcade_gameplay.png/800px-Lunar_Lander_arcade_gameplay.png)

Landing gameplay is inspired by the classic **Lunar Lander** arcade game.

### Player Controls

Vertical thrust  
Left / right drift  

Landing quality may influence starting conditions.

Hard landings may:

- ⚠ Damage ship  
- ⚠ Reduce starting fuel  
- ⚠ Place player further from resources  

---

# 🌍 Planet Exploration

![Alien Planet](https://images.unsplash.com/photo-1580428180121-3e7b5b2c1e5a)

After landing, the astronaut exits the ship and explores the planet.

Primary objectives:

- 💎 Collect fuel crystals  
- 👾 Avoid alien enemies  
- 🚀 Return to ship safely  

Maps are **small exploration arenas**.

---

# 💎 Fuel / Gem Collection

![Crystals](https://images.unsplash.com/photo-1602526432604-029a709e1312)

Fuel crystals represent energy needed for space travel.

Fuel is required to:

- unlock new planets  
- travel between planets  
- progress in the game  

Important rule:

Fuel only counts once **deposited at the ship**.

If the player dies before returning:

**un-deposited fuel is lost**

This creates strong **risk vs reward gameplay**.

---

# 👾 Alien Enemies

![Alien Creature](https://images.unsplash.com/photo-1611605698335-8b1569810432)

Aliens spawn dynamically while the player explores.

Spawn triggers include:

- player movement  
- exploration time  
- planet difficulty  

Aliens detect nearby players and begin chasing.

---

# 🧟 Alien Types

| Alien Type | Behavior |
|------|------|
| 🐛 Basic Alien | slow but numerous |
| ⚡ Fast Alien | faster than player |
| 🛡 Tank Alien | slow but high health |
| 🐜 Swarm Alien | appear in groups |

Alien type selection depends on planet difficulty.

---

# 🔫 Combat

Players can shoot aliens.

Combat mainly serves to:

- clear escape routes  
- survive alien swarms  
- increase score  

Aliens normally **do not drop fuel**.

---

# 🏛 Terrain Mechanics

![Alien Ruins](https://images.unsplash.com/photo-1542273917363-3b1817f69a2d)

Terrain objects affect alien movement.

Examples include:

- buildings  
- rock formations  
- craters  
- alien ruins  

These obstacles slow enemies and allow tactical escape.

---

# ⚖ Risk vs Reward System

Players must constantly decide:

Return to ship safely  
OR  
Stay longer to collect more fuel  

Remaining longer increases:

- alien spawn rate  
- enemy speed  
- difficulty level  

---

# 💀 Death System

If the astronaut dies:

**that astronaut is lost**

Default lives:

| Player Type | Astronaut Count |
|------|------|
| Free Player | 1 astronaut |
| Premium Player | 3 astronauts |

Respawns occur at the ship.

---

# 💰 Monetization

Ads are **optional and player initiated**.

### Continue After Death
Watch ad → deploy another astronaut

### Double Fuel Reward
Watch ad → double the fuel deposited

### Premium Upgrade
Removes ads and grants **3 astronauts per mission**

There are **no forced ads during gameplay**.

---

# 🏆 Score System

Score tracks:

- 👾 aliens killed  
- 💎 fuel collected  
- 🪐 planets reached  
- ⏱ survival time  

Leaderboards may include:

- global rankings  
- weekly challenges  
- personal best scores  

---

# 🗺 Map Design

Planet maps are small arenas designed for **short gameplay sessions**.

Maps include:

- fuel spawn locations  
- alien spawn zones  
- terrain obstacles  
- landing zone  
- ship location  

Future versions may include **procedural generation**.

---

# 📈 Difficulty Scaling

Difficulty increases based on:

- time spent on planet  
- planet difficulty level  
- enemy population  

Scaling mechanics include:

- faster aliens  
- larger waves  
- increased spawn rates  

---

# 🎯 Design Philosophy

The game should feel:

- simple  
- fast paced  
- highly replayable  

Players should always feel:

**“Just one more run.”**

---

# 👨‍💻 Developer Task Breakdown

## Current Prototype Status

The prototype currently includes:

✔ Player movement  
✔ Shooting mechanics  
✔ Alien spawning  
✔ Survival gameplay  

Missing systems:

❌ Ship system  
❌ Planet selection  
❌ Landing mechanics  
❌ Fuel progression  
❌ Planet unlocking  

---

# 🧩 Phase 1 – Core Systems

## Ship System
Tasks:

- create ship prefab  
- landing pad system  
- fuel storage system  
- astronaut spawning  
- launch mechanics  

---

## Landing System
Tasks:

- gravity simulation  
- thrust controls  
- drift physics  
- landing detection  
- crash detection  

---

## Planet Selection System
Tasks:

- planet map UI  
- planet unlock system  
- fuel requirement system  
- scene loading  

---

## Resource System
Tasks:

- fuel crystal spawning  
- pickup detection  
- player inventory  
- ship deposit mechanic  
- HUD display  

---

## Alien AI
Tasks:

- spawn manager  
- detection radius  
- chase behavior  
- attack behavior  
- difficulty scaling  

Alien types:

- basic alien  
- fast alien  
- tank alien  
- swarm alien  

---

## Combat System
Tasks:

- projectile system  
- enemy health system  
- damage handling  
- kill tracking  

---

## Map System
Tasks:

- terrain placement  
- spawn zones  
- landing zones  
- obstacle generation  

Optional later:

procedural generation

---

## Difficulty Scaling
Tasks:

- spawn rate scaling  
- alien speed scaling  
- planet difficulty modifiers  

---

## UI Systems
Required screens:

- main menu  
- planet selection  
- gameplay HUD  
- score screen  

HUD displays:

- fuel collected  
- health  
- astronaut count  
- score  

---

## Monetization System
Implement:

Continue after death → watch ad for new astronaut  

Fuel multiplier → watch ad to double fuel  

Premium upgrade → removes ads and grants 3 astronauts  

---

## Audio
Tasks:

- ambient planet sounds  
- alien effects  
- weapon sounds  
- UI sounds  

---

## Visual Effects
Tasks:

- alien explosion effects  
- fuel pickup animation  
- weapon effects  
- ship launch animation  

---

# 🧪 Phase 2 – Polish

Add:

- additional alien types  
- more planets  
- cosmetic unlocks  
- UI improvements  
- performance optimization  

---

# 🚀 Phase 3 – Release Preparation

Tasks:

- mobile optimization  
- ad network integration  
- analytics  
- crash reporting  
- store assets  

---

# 🎮 Initial Playable Target

First playable build should include:

- one planet  
- basic alien enemies  
- fuel collection  
- ship launch mechanic  
- score tracking  

Once stable, additional planets and gameplay systems can be added.