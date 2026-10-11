# The Witch's Candy House

CPSC 386 - Project 1 documentation

Unity 6000.3.25f1 (6.3 LTS). macOS Universal.

## Game description and controls

A small 2D top-down Hansel and Gretel-inspired game. You control the witch, move around a gingerbread room and throw candy at two stationary children. Fill both children to 10 before the 100-second timer expires. At 6 suspicion, the children escape. Each used candy card is replaced with a random card.

| Control | Action |
| --- | --- |
| WASD or arrow keys | Move and set the throw direction. The small candy by the witch shows aim. |
| Space | Throw the selected candy. |
| 1 / 2 / 3 or card button | Select one of the three candy cards. |
| Escape / PAUSE | Pause or resume; pause menu also provides Main Menu and Quit. |

| Candy | Effect |
| --- | --- |
| Gummies | +1 fullness; no suspicion |
| Lollipop | +2 fullness; no suspicion |
| Chocolate | +3 fullness; +1 suspicion |

## Scenes and GameObjects

MainMenu starts first. MenuManager provides Start, How to Play, Quit and saved statistics. MenuCanvas uses an EventSystem with Input System UI Input Module.

GingerbreadHouse is the single gameplay scene: tile room, witch, two targets, pooled candy, HUD, pause/results and cauldron lighting.

| GameObject | Components and interaction |
| --- | --- |
| HouseGrid / Ground / Walls | Grid, Tilemap, TilemapRenderer; Walls also has TilemapCollider2D. Ground is decorative; walls block movement. |
| WitchPlayer / AimMarker | SpriteRenderer, Dynamic Rigidbody2D (gravity 0), BoxCollider2D, PlayerInput and controller. AimMarker shows the last facing direction. |
| Hansel / Gretel | SpriteRenderer and trigger BoxCollider2D with WitchChildTarget. They report hits to GameManager and stay in fixed positions. |
| CandyPool / CandyProjectile | Pool component plus 12 reusable prefab children. Candy has Kinematic Rigidbody2D and trigger CircleCollider2D. |
| GameManager / GameCanvas | Round rules and Canvas labels, progress bars and candy buttons. PausePanel and ResultPanel are inactive until needed. |
| PauseManager / Navigation | Pause script and scene-navigation callbacks receive the Canvas button events. |
| MagicCauldron / Main Camera | Pulsing Point Light 2D, global lighting, lit material and an orthographic camera using the URP 2D renderer. |

## Scripts

| Script | Responsibility |
| --- | --- |
| WitchPlayerController.cs | InputValue callbacks from WitchControls select cards, remember aim and throw. FixedUpdate moves the Rigidbody2D with MovePosition. |
| WitchCandyPool.cs | Creates 12 inactive CandyProjectile prefab instances in Awake. A List searches for an inactive instance and grows only when all are in use. |
| WitchCandyProjectile.cs | Resets position, points and lifetime on launch, moves with Rigidbody2D, and deactivates on a hit, wall contact or three-second expiry. |
| WitchChildTarget.cs | OnTriggerEnter2D and CompareTag identify candy. Consume prevents duplicate scores; the correct child gains fullness and flashes briefly. |
| WitchGameManager.cs | Owns fullness, suspicion, countdown, random three-card hand, HUD and win/loss conditions. Victory saves wins and fastest time with PlayerPrefs. |
| WitchCandyCard.cs | ScriptableObject assets store candy names, fullness/suspicion points and colors. This uses the supplied Week 7 shared-data pattern. |
| WitchPauseMenu.cs | Escape and Canvas buttons toggle PausePanel and Time.timeScale. Physics and the timer stop while paused. |
| WitchSceneNavigation.cs | SceneManager loads Start, retry and Main Menu destinations; public callbacks show instructions or call Application.Quit. |
| WitchMenuStats.cs | Reads WitchWins and WitchBestTime from PlayerPrefs when MainMenu starts and displays the records. |
| WitchCauldronGlow.cs | Uses Time.time and Mathf.PingPong to gently pulse a mint Point Light 2D around MagicCauldron. |

Gameplay uses MonoBehaviour lifecycle methods, serialized component fields, ordinary arrays/lists and straightforward conditionals. Assignment 3 supplies the growing-pool pattern. Candy data uses Week 7 ScriptableObjects.

## Eight rubric requirements

| Requirement | Implementation and evidence |
| --- | --- |
| 1. Connected scenes | MainMenu starts at build index 0. START GAME loads GingerbreadHouse; pause and result buttons return to MainMenu through SceneManager. |
| 2. Input System + physics | WitchControls Input Actions and PlayerInput Send Messages receive WASD/arrows/Space. A Dynamic Rigidbody2D and BoxCollider2D move and collide. |
| 3. Tilemap level | HouseGrid contains painted Ground and Walls Tilemaps. The Walls TilemapCollider2D defines solid boundaries and the central counter. |
| 4. Prefab object pooling | CandyProjectile prefab instances are precreated and recycled with SetActive. The original 12 instances served ten scoring hits in the fresh cleaned-game test. |
| 5. Tags, layers, triggers | CandyProjectile, Child and Wall tags plus custom physics layers support child trigger scoring and wall returns. A candy is consumed only once. |
| 6. Canvas pause + Quit | PausePanel offers Resume, Main Menu and Quit. timeScale 0 freezes the timer and physics; Resume restores 1 and Quit closes the standalone app. |
| 7. PlayerPrefs persistence | A victory saves WitchWins and WitchBestTime. MainMenu loads them. Both values survived quitting and restarting the cleaned-game verification app. |
| 8. Lighting/material effect | CandyHouseLit uses Sprite-Lit-Default. A mint Point Light 2D visibly lights the floor by the cauldron and pulses during play. |

## Assets and licenses

The nine sprite PNGs under Assets/WitchGame_P1/Art provide the witch, Hansel, Gretel, candy projectile, cauldron, gingerbread floor, frosting wall, gumdrop decoration and night backdrop. The WitchGame_P1 sprite kit supplies the game's visual assets.

Unity's Input System, URP 2D, Tilemap, Canvas UI/TextMeshPro, Burst, Collections and Mathematics packages provide engine functionality. Original package copyright and license notices are included unchanged under THIRD_PARTY_NOTICES.

The Liberation Sans font includes Red Hat/Google copyright and SIL Open Font License 1.1 notices. Unity's included EmojiOne sample and its attribution notice are retained. That sample is not intentionally used in the game's interface. Unity/Mono runtime notices remain intact. No new artwork license is asserted by this document.

## Submission statement

This game is submitted as the student's work. ChatGPT assisted with organizing the submission materials and the written description. This document was prepared with AI assistance and is subject to the student's review.

## Verification

Standalone testing recorded 28 automated passes and zero failures across two process launches. The checks cover Main Menu, instructions, Start, WASD/arrows, solid Tilemap wall blocking, throwing, ten child-trigger scoring hits, a real victory, reuse of the original 12 candy instances, pause/resume, frozen timer/physics, menu navigation, Quit and saved wins/best time after relaunch. Button checks invoke the configured events, and keyboard checks use Input System state.

A separate manual check confirmed real pointer/keyboard controls, card selection, instructions/Start, pause/resume, a complete winning round, Quit/reopen and saved statistics. The submitted application matches the tested build. Its original source scripts, scenes, artwork and asset references remain intact.

Earlier Editor testing recorded 35 passes, including retry and both suspicion/time losses. That is historical evidence rather than a new Editor run. No new engine rebuild is claimed. The Universal x86_64/arm64 application was run on Apple Silicon; Intel execution was not tested. Its local ad-hoc signature validates. No Developer ID signature or notarization is claimed.

## Submission review

Extract the whole playable ZIP and keep the application bundle intact. Open Build and launch The Witch's Candy House.app. The application records a minimum macOS version of 12.0. The source project uses Unity 6000.3.25f1 (6.3 LTS).

Review the game on the intended playback computer. Check the Main Menu, a full round, pause/resume and saved statistics after quitting and restarting. Keep the original copyright and license notices with the application.

The separate source-code ZIP contains all ten gameplay scripts and their original .meta files. A complete editable Unity project is supplied alongside FINAL_SUBMISSION_READY. No public repository URL is claimed. The game has one room, two stationary targets, three candy types and no audio or character animation.
