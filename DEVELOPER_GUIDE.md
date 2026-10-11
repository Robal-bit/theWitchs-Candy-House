# The Witch's Candy House

## Open and play

1. Open Unity Hub and select **WitchCandyHouse_Unity**. Use **Unity 6.3 LTS, 6000.3.25f1**. This is the working copy beside this guide. The original course project remains in your CSUF folder.
2. In Unity's **Project** panel, expand **Assets > WitchGame_P1 > Scenes** and double-click **MainMenu**.
3. Press the triangle **Play** button at the top of the Editor. Click **START GAME**.
4. Click inside the **Game** view before using the keyboard. Move with **WASD** or the **arrow keys**. Your last movement determines the direction of a throw. The small candy next to the witch marks that direction.
5. Choose a candy with **1**, **2**, **3**, or its bottom button. Press **Space** to throw it. Each throw replaces that card with a random candy.
6. Feed both children to **10 fullness** within **100 seconds**. **Gummies** add 1 fullness and no suspicion. **Lollipops** add 2 and no suspicion. **Chocolate** adds 3 fullness and 1 suspicion. Reaching **6 suspicion** loses the round.
7. Press **Escape** or click **PAUSE**. Resume with Escape or **RESUME**. **MAIN MENU** returns to the menu. **QUIT GAME** closes the standalone game and stops Play mode when testing in the Editor.

The children stay in fixed positions, so you can learn to aim. Stand beneath Hansel or Gretel, tap upward, release the movement key, and throw. Candy that touches a wall or lasts longer than three seconds returns to the pool.

## Project layout

Your earlier assignment folders and scenes remain intact. The game uses these files:

| Folder | Purpose |
| --- | --- |
| `Assets/WitchGame_P1/Scenes` | `MainMenu.unity` and `GingerbreadHouse.unity` |
| `Assets/WitchGame_P1/Scripts` | Small gameplay MonoBehaviours and candy data class |
| `Assets/WitchGame_P1/Art` | The nine supplied pixel-art sprites |
| `Assets/WitchGame_P1/Input` | `WitchControls.inputactions` |
| `Assets/WitchGame_P1/Prefabs` | Reusable `CandyProjectile.prefab` |
| `Assets/WitchGame_P1/Data` | Gummies, Lollipop and Chocolate ScriptableObject assets |
| `Assets/WitchGame_P1/Tiles` | Floor and wall Tile assets |
| `Assets/WitchGame_P1/Materials` | Sprite-Lit material for the room and characters |

Unity-generated `Library`, `Temp` and `Logs` folders can be rebuilt. Preserve `.meta` files alongside assets: their GUIDs connect scenes, scripts, sprites and prefabs.

## Scene objects and how they work

### MainMenu

- **Main Camera** and **Global Light 2D** supply the scene camera and rendering.
- **MenuManager** has `WitchSceneNavigation` and `WitchMenuStats`. Navigation loads the gameplay scene, shows instructions, or quits. Stats reads saved wins and best time.
- **MenuCanvas** uses Screen Space Overlay and a Canvas Scaler at 1280 × 800. It contains the title, art, Start/How to Play/Quit buttons, saved stats, and the initially inactive **InstructionsPanel**.
- **EventSystem** has **Input System UI Input Module**, so Canvas buttons work with the new Input System.

### GingerbreadHouse

- **HouseGrid > Ground** holds the gingerbread floor Tilemap.
- **HouseGrid > Walls** holds the frosting perimeter and central counter tiles. Its **Tilemap Collider 2D** blocks the witch. It has tag `Wall` and layer `HouseWalls`.
- **WitchPlayer** combines a Sprite Renderer, Dynamic Rigidbody2D, Box Collider 2D, Player Input, and `WitchPlayerController`. Movement goes through `Rigidbody2D.MovePosition` in `FixedUpdate`, preserving wall collision.
- **WitchPlayer > AimMarker** shows the last movement direction.
- **Hansel** and **Gretel** each have a Sprite Renderer, trigger Box Collider 2D, and `WitchChildTarget`. `OnTriggerEnter2D` checks the incoming `CandyProjectile` tag, consumes that instance once, and updates the appropriate fullness score.
- **CandyPool** creates 12 instances in `Awake`. A throw finds an inactive one, resets its position, direction, values and timer, and activates it. Hits, walls and expiry deactivate it. If all instances are busy, the pool grows by one, matching Assignment 3's growing-pool exercise.
- **GameManager** owns the round's fullness, suspicion, countdown, random card hand and result. It updates the HUD and saves wins/best time after a victory.
- **MagicCauldron > Cauldron Point Light 2D** casts a mint glow on Sprite-Lit renderers. `WitchCauldronGlow` gently changes its intensity using `Mathf.PingPong` and `Time.time`.
- **GameCanvas** contains the fullness/suspicion bars, timer, hint, candy cards, **PausePanel**, and **ResultPanel**. The two overlay panels start inactive.
- **PauseManager** has `WitchPauseMenu`. It sets `Time.timeScale` to 0 or 1 and shows/hides the pause panel. The timer uses `Time.deltaTime` and motion uses physics updates, so they stop while paused.
- **Navigation** handles retry, return to Main Menu and Quit. Scene changes restore timeScale to 1.

## Script reference

| Script | Responsibility and course connection |
| --- | --- |
| `WitchPlayerController` | `OnMove`/`OnFire` and card-selection InputValue callbacks; Rigidbody2D movement in FixedUpdate. Weeks 3–4. A short cooldown prevents duplicate rapid throws. |
| `WitchCandyPool` | List, startup Instantiate, inactive search, SetActive reuse and optional growth. Week 3 / Assignment 3. |
| `WitchCandyProjectile` | MovePosition, lifetime countdown, reset on reactivation, one-time Consume, wall trigger. Week 3. |
| `WitchChildTarget` | OnTriggerEnter2D and CompareTag; calls the round manager; briefly tints the sprite on a hit. Weeks 3 and 6. |
| `WitchGameManager` | Arrays, random card draws, conditionals, score bounds, UI labels/bars, win/loss and PlayerPrefs. Weeks 1–6. |
| `WitchCandyCard` | ScriptableObject data with CreateAssetMenu and readable properties. Week 7's shared-data pattern. Round scores stay in the manager, never in the asset. |
| `WitchPauseMenu` | Escape polling, panel toggling and timeScale. Matches Week 6 / PauseManager.cs. |
| `WitchSceneNavigation` | SceneManager.LoadScene, instructions visibility and Application.Quit. Weeks 5–6. |
| `WitchMenuStats` | Reads `WitchWins` and `WitchBestTime` when MainMenu starts. Week 4. |
| `WitchCauldronGlow` | Changes a real Point Light 2D's intensity over time. Week 6 lighting with basic math. |

Gameplay uses serialized fields, MonoBehaviour lifecycle methods, ordinary arrays/lists, components and straightforward conditionals. Temporary authoring and automated-test tools are separate from the delivered gameplay source.

## Exact Inspector settings

Stop Play mode before changing scene objects. Play-mode edits to scene objects revert when you stop. ScriptableObject asset edits persist, as explained in Week 7.

### Witch and input

1. Open **GingerbreadHouse**. In the **Hierarchy**, select **WitchPlayer**.
2. In **Rigidbody 2D**, confirm **Body Type = Dynamic**, **Gravity Scale = 0**, **Collision Detection = Continuous**, **Interpolate = Interpolate**, and **Constraints > Freeze Rotation Z** checked. Expected result: top-down motion with no falling or spinning.
3. In **Box Collider 2D**, confirm **Is Trigger** unchecked, **Size X = 0.38, Y = 0.46**, **Offset X = 0, Y = -0.06**. The object's scale is 1.6. Expected result: the body collides with walls without the hat catching on them.
4. In **Witch Player Controller**, set **Game Manager** by dragging **GameManager** from the Hierarchy into that exact field. **Speed = 4**, **Throw Cooldown = 0.18**. Expand WitchPlayer and drag **AimMarker** into **Aim Marker**.
5. In **Player Input**, **Actions** must reference `Assets/WitchGame_P1/Input/WitchControls.inputactions`. **Default Map = Player**, **Behavior = Send Messages**. Expected result: the Move action invokes `OnMove(InputValue)` and Fire invokes `OnFire(InputValue)` on this same GameObject.
6. To inspect bindings, double-click **WitchControls** in the Project panel. Select **Player > Move**. Its action type is **Value**, control type **Vector2**, with separate **2D Vector** composites for WASD and arrows. **Fire** is a **Button** bound to `<Keyboard>/space`. **SelectOne**, **SelectTwo**, **SelectThree** are Buttons bound to keyboard 1, 2, 3. Save the asset after edits.

### Tiles and physics

1. Select **HouseGrid > Ground**. Its **Tilemap Renderer > Material** references **CandyHouseLit**. It has no blocking collider.
2. Select **HouseGrid > Walls**. Confirm **Tag = Wall**, **Layer = HouseWalls**, and **Tilemap Collider 2D > Is Trigger** unchecked.
3. To add a wall, open **Window > 2D > Tile Palette**. Create/select a palette, drag **Assets/WitchGame_P1/Tiles/FrostingWallTile.asset** into it, choose **Walls** as the active Tilemap, select the brush and paint in Scene view. Save the scene. Expected result: the painted cells also become solid.
4. In **Edit > Project Settings > Physics 2D**, inspect the **Layer Collision Matrix**. Candy interacts with Children and HouseWalls. WitchPlayer interacts with HouseWalls. Candy ignores WitchPlayer and Candy; WitchPlayer ignores Children.

### Candy prefab and pool

1. In the Project panel, double-click **Assets > WitchGame_P1 > Prefabs > CandyProjectile** to open Prefab Mode.
2. Confirm **Tag = CandyProjectile**, **Layer = Candy**. Its **Rigidbody 2D** uses **Kinematic**, **Gravity Scale = 0**, **Collision Detection = Continuous**, and **Full Kinematic Contacts** enabled.
3. In **Circle Collider 2D**, **Is Trigger** is checked and **Radius = 0.31**. In **Witch Candy Projectile**, **Movement Speed = 7**, **Lifetime = 3**. Save and leave Prefab Mode using the back arrow above the Hierarchy.
4. Select the scene's **CandyPool**. In **Witch Candy Pool > Candy Prefab**, drag the prefab from the Project panel into that field. **Initial Pool Size = 12**. Expected result: entering Play mode immediately creates 12 inactive child instances under CandyPool.

### Children and round rules

1. Select **Hansel**. Confirm **Box Collider 2D > Is Trigger** checked and **Size X = 0.45, Y = 0.60**. **Tag = Child**, **Layer = Children**.
2. In **Witch Child Target**, drag **GameManager** into **Game Manager**. **Is Hansel** is checked. Select **Gretel** and use the same reference with **Is Hansel** unchecked.
3. Select **GameManager > Witch Game Manager**. **Target Fullness = 10**, **Maximum Suspicion = 6**, **Game Seconds = 100**. **Candy Pool** references CandyPool.
4. Expand **Available Cards**. Size is 3. Element 0 = **GummiesCard**, 1 = **LollipopCard**, 2 = **ChocolateCard**, from **Assets/WitchGame_P1/Data**. To rebalance candy, click one of these assets and edit **Fullness Points**, **Suspicion Points**, or **Projectile Color**.
5. The manager's UI references point to the correspondingly named text objects under **GameCanvas**. **Hansel Fullness Fill**, **Gretel Fullness Fill** and **Suspicion Fill** reference the Image children named `...Fill`. **Card Backgrounds** has the Image components on CandyCard1–3. **Result Panel** references ResultPanel, and **Result Text** references ResultPanel/ResultText.

### Button wiring, pause and lights

1. To inspect any button, select it in the Hierarchy, scroll to its **Button** component, and expand **On Click ()**. The target object appears in the left object field; the function dropdown selects its method. To repair a missing event, click **+**, drag the named target into the new object field, then choose the method from the dropdown.

| Button | Target GameObject | On Click method |
| --- | --- | --- |
| MainMenu: StartButton | MenuManager | WitchSceneNavigation.StartGame |
| InstructionsButton / InstructionsBackButton | MenuManager | ShowInstructions / HideInstructions |
| QuitButton | MenuManager | QuitGame |
| Gameplay: CandyCard1 / 2 / 3 | GameManager | SelectFirstCard / SelectSecondCard / SelectThirdCard |
| PauseButton / ResumeButton | PauseManager | WitchPauseMenu.Pause / Resume |
| PauseMainMenuButton / ResultMainMenuButton | Navigation | WitchSceneNavigation.GoToMainMenu |
| PauseQuitButton | Navigation | WitchSceneNavigation.QuitGame |
| RetryButton | Navigation | WitchSceneNavigation.RestartGame |

2. Select **PauseManager > Witch Pause Menu**. **Pause Panel** references **GameCanvas/PausePanel**; **Game Manager** references GameManager. Select PausePanel and confirm its active checkbox at the top of the Inspector is unchecked before playing.
3. Select **MagicCauldron > Cauldron Point Light 2D**. In **Light 2D**, **Light Type = Point**, **Color = mint**, **Inner Radius = 0.6**, **Outer Radius = 4.2**. The cauldron's `WitchCauldronGlow` uses **Minimum Intensity = 0.9**, **Pulse Amount = 0.3** and a reference to this light. Expected result: a green glow spreads across the floor and pulses gently during gameplay.
4. All world Sprite Renderers and Tilemap Renderers use **CandyHouseLit**, whose shader is **Universal Render Pipeline/2D/Sprite-Lit-Default**. Select **Main Camera > Universal Additional Camera Data** and use the project's 2D renderer. A lit material and 2D renderer are necessary for Light 2D to affect the sprites.

## Eight rubric requirements

| Requirement | Implementation | How to demonstrate |
| --- | --- | --- |
| 1. Two scenes | MainMenu and GingerbreadHouse linked with SceneManager | Start, then pause and return to Main Menu |
| 2. New Input System + 2D physics | Input Actions asset, PlayerInput, Rigidbody2D and BoxCollider2D | Move with both bindings; walk into a frosting wall |
| 3. Tilemap layout/collision | Ground and Walls Tilemaps; Walls Tilemap Collider 2D | Inspect tiles, then show the wall stops movement |
| 4. Prefab pooling | CandyProjectile prefab and List-based growing pool | Expand CandyPool in Play mode; fire repeatedly and watch instances reactivate |
| 5. Tags/layers/triggers | Tagged candy, child triggers, wall tag and custom layers | Candy hit increases the correct child's fullness once |
| 6. Canvas pause/quit | PausePanel, Resume/Main Menu/Quit; timeScale 0/1 | Pause during movement, confirm timer stops, then resume |
| 7. PlayerPrefs | WitchWins and WitchBestTime saved on victory | Win, quit the standalone game, relaunch and read saved stats |
| 8. Lighting/material touch | Sprite-Lit material and pulsing cauldron Point Light 2D | Show the mint lighting in GingerbreadHouse |

## Export and verify a build

1. Stop Play mode. Open **File > Build Profiles**. Under **Scene List**, confirm **MainMenu** is enabled at index **0** and **GingerbreadHouse** is enabled at index **1**. If a scene is missing, open it, return here and click **Add Open Scenes**. Drag MainMenu to the first position.
2. Select **macOS**, click **Switch Platform** if needed, and confirm the installed 6000.3.25f1 Editor has Mac Build Support. Click **Build**.
3. Choose a new empty **Build** folder outside Assets. Use **The Witch's Candy House.app** as the build name. Expected result: an application bundle that starts at MainMenu.
4. Launch that application from Finder. Play a complete round, test pause/resume/menu/quit, and win once. Quit and launch again. Expected result: WINS and BEST TIME retain their saved values.
5. Zip the complete Build folder for portability. Extract that ZIP into a different folder and launch the extracted app once before submission.

The game uses its own company/product names, so its PlayerPrefs are separate from your earlier coursework. Automated tests restore both the Editor and standalone app's pre-test score values.

## Editing and debugging

- If the player does not respond, click the Game view, check Player Input's Actions/Default Map/Behavior, then inspect the Console for the first error.
- If candy passes through children without scoring, inspect the prefab tag and target Is Trigger checkbox, and verify Candy–Children is enabled in Physics 2D.
- If the pool grows continually, confirm projectile Lifetime is positive and no old Destroy call has been added. Normal throws should reuse inactive objects.
- If menus stop after a scene change, verify an EventSystem with Input System UI Input Module exists in that scene.
- If a new round is frozen, inspect navigation's timeScale reset and avoid leaving a result/pause panel active in the saved scene.
- A yellow warning deserves review, but fix red errors first. Double-click the first Console error to open the relevant script line in VS Code.

## Assets and licenses

The nine sprite PNGs under Assets/WitchGame_P1/Art provide the witch, Hansel, Gretel, candy projectile, cauldron, gingerbread floor, frosting wall, gumdrop decoration and night backdrop. The WitchGame_P1 sprite kit supplies the game's visual assets.

Unity's Input System, URP 2D, Tilemap, Canvas UI/TextMeshPro, Burst, Collections and Mathematics packages provide engine functionality. Original package copyright and license notices are included unchanged under THIRD_PARTY_NOTICES.

The Liberation Sans font includes Red Hat/Google copyright and SIL Open Font License 1.1 notices. Unity's included EmojiOne sample and its attribution notice are retained. That sample is not intentionally used in the game's interface. Unity/Mono runtime notices remain intact. No new artwork license is asserted by this document.

Read PROJECT_1_DOCUMENTATION.md for the project description and verification. Preserve every .meta file alongside its asset.
