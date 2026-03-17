<div align="center">

# Less is More

Every death is a stepping stone — a 2D platformer where your corpses become platforms.

![Last Commit](https://img.shields.io/github/last-commit/emirbesir/less-is-more?style=flat&logo=git&logoColor=white&color=0080ff)
![Top Language](https://img.shields.io/github/languages/top/emirbesir/less-is-more?style=flat&color=0080ff)
![Unity](https://img.shields.io/badge/Unity-FFFFFF.svg?style=flat&logo=Unity&logoColor=black)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

_Result: **1st out of 7 teams** — BTK Academy Advanced Unity Bootcamp Jam_

_Made and tested with **Unity 6000.3.2f1**_

</div>

## Gameplay

When you die, your corpse stays in the scene as a solid, persistent platform. Each failure becomes a tool for reaching previously inaccessible areas on your next attempt. Each level has a limited number of deaths — exceed it and the level restarts entirely.

## Screenshots

<!-- Add screenshots to docs/img/ and update the paths here -->
_Coming soon_

## Technical Highlights

- **Corpse Stacking System:** Deaths convert into kinematic Rigidbodies, managed via circular buffer (max 50 corpses)
- **Responsive Controls:** Coyote time and jump buffering for fluid 2D movement (Tarodev controller)
- **Event-Driven Architecture:** GameManager communicates state via C# events (`OnDeathCountChanged`, `OnLevelCompleted`)
- **Per-Level Difficulty:** ScriptableObject-based difficulty configuration (death limits, speed, gravity)

## Links

- [Play (itch.io)](https://calippooo.itch.io/less-is-more)
- [Video (YouTube)](https://www.youtube.com/watch?v=4ptHAemZCW4)
