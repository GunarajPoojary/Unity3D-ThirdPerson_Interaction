# Third-Person Interaction System for Unity

A robust, modular interaction system for third-person Unity games. This README has been updated to match the refactored scripts in the repository (no mentions of removed interfaces/components).

---

## Gameplay
<img src="ThirdPerson_Interaction_System/Media/InteractionSystem.gif" alt="Demo" loading="eager" />  

---

## Overview
This system is centered on a small set of focused scripts:

- A **generic, ScriptableObject-based event channel** (`EventChannel<T>`) used across the project.
- A single **`IInteractable`** interface and a concrete **`Interactable`** MonoBehaviour that implements it.
- An **`Interactor`** component that continuously detects nearby `IInteractable`s and raises hover/lost events.
- `UIInteraction` + `ChatBubble` for in-world interaction prompts and NPC messages.
- A set of **interactable implementations** (Chest, Door, DoorComputer, Suitedman) that subscribe to `Interactable.OnInteract`.
- An **`InputReader`** ScriptableObject (wraps the auto-generated `GameInput`) used by player and systems to receive input.

This README describes the behaviour and how to wire up the system in-editor.

---

## Features

- Lightweight, generic event channels (`EventChannel<T>`) and an `Empty` placeholder for void-style events.
- Central `IInteractable` interface for all interactable objects.
- `Interactable` adds and manages an `Outline` component at runtime to provide visual focus feedback.
- Fast overlap-sphere detection using `Physics.OverlapSphereNonAlloc` via `Interactor`.
- UI prompts anchored to interactables using `InteractableHoverUIEventChannelSO` which passes `(Transform, string)`.
- Interaction flow implemented by `Interactor` invoking `IInteractable.Interact()` when the interact button is triggered.
- Sample interactables: `Chest`, `Door`, `DoorComputer`, `Suitedman` (with animation event integration).
- Player-side utilities: `PlayerController`, `CameraThirdPersonLook`, `MovementAnimationEventTrigger`.

---

## System architecture (short)

- `IInteractable` — minimal contract for in-world interactables.
- `Interactable` — MonoBehaviour implementing `IInteractable`. It constructs an `Outline` component in `Awake`, and exposes `UIAnchor`, `InteractionTypeName`, and `CanInteract`.
- `Interactor` — attached to the player. It scans for interactables, manages focus transitions (calls `OnFocusGained` / `OnFocusLost`), and raises event channel messages for UI.
- `UIInteraction` — listens to the hover and lost event channels and positions/displays the interaction prompt.
- `InputReader` — ScriptableObject wrapper around the `GameInput` input actions. `Interactor` hooks into `InputReader.InteractStartedAction` to perform interactions.
- Interactable implementations handle the gameplay side of interactions (open chest, toggle door, play NPC talk animation, etc.).

---

## Key components (what exists in the scripts)

### Interfaces & Events
- `IInteractable` — members: `Transform UIAnchor { get; }`, `bool CanInteract { get; }`, `string InteractionTypeName { get; }`, `void Interact()`, `void OnFocusGained()`, `void OnFocusLost()`.
- `EventChannel<T>` — generic ScriptableObject event channel with `RaiseEvent(T value)`.
- `Empty` — placeholder class used for void-style events.
- `IInteractableEventChannelSO : EventChannel<IInteractable>`
- `InteractableHoverUIEventChannelSO : EventChannel<(Transform, string)>` — raised when an interactable is focused; payload is `(UIAnchor, InteractionTypeName)`.
- `VoidEventChannelSO : EventChannel<Empty>` — used for "lost" / generic no-parameter events.

### Core MonoBehaviours
- `Interactable` — serializable fields for outline width/color, exposes `UIAnchor` and `InteractionTypeName`, raises `OnInteract` event, toggles `Outline.enabled` on focus gained/lost.
- `Interactor` — detects nearby `IInteractable`s with an adjustable radius/offset, raises hover/lost events via the event channels, calls `Interact()` when input is received.

### Interactable implementations
- `Chest` — rotates/open chest lid when its `Interactable.OnInteract` is invoked.
- `Door` — sliding two-part door with `OpenOrClose()` and `OnDoorToggled` event.
- `DoorComputer` — triggers `Door.OpenOrClose()` on interaction and temporarily disables re-interaction via layer changes.
- `Suitedman` — triggers an animator `Interact` trigger and shows a `ChatBubble`; uses `SuitedmanAnimationEventTrigger` to re-enable interaction after animation.

### Player & UI
- `PlayerController` — simple walking controller driven by `InputReader.MoveInput`.
- `CameraThirdPersonLook` — rotates camera target from `InputReader.LookInput`.
- `MovementAnimationEventTrigger` — plays footstep/landing sounds from animation events.
- `InputReader` — ScriptableObject that initializes and wraps `GameInput` (auto-generated input class) and exposes UnityActions such as `InteractStartedAction`.
- `UIInteraction` — listens to `InteractableHoverUIEventChannelSO` and `VoidEventChannelSO` to show/hide a Canvas anchored to `IInteractable.UIAnchor`. It writes `InteractionTypeName` into a `TextMeshProUGUI`.
- `ChatBubble` & `Billboard` — chat bubble display and camera-facing behaviour for world UI elements.

---

## Quick setup (editor)

1. **Create Event Channel assets**
   - `IInteractableEventChannelSO`, `InteractableHoverUIEventChannelSO`, and `VoidEventChannelSO` can be created via the ScriptableObject `CreateAssetMenu` entries in the project. These are referenced by `Interactor` and `UIInteraction`.

2. **Create an `InputReader` ScriptableObject**
   - Create the `InputReader` asset (from the CreateAssetMenu entry). It lazily creates a `GameInput` instance at runtime and exposes `InteractStartedAction` which `Interactor` listens to.

3. **Player GameObject**
   - Add `Interactor` to the player and assign:
     - `InputReader` asset.
     - Interactable layer mask and detection radius/offset.
     - Assign the `InteractableHoverUIEventChannelSO` and `VoidEventChannelSO` assets.

4. **UI**
   - Add a `UIInteraction` GameObject (eg. a prefab) containing a Canvas and a `TextMeshProUGUI`.
   - Assign the same `InteractableHoverUIEventChannelSO`, `VoidEventChannelSO`, and, if used, the `VoidEventChannelSO` that represents the interact action.
   - The `UIInteraction` will automatically position the canvas under the `UIAnchor` transform when a hover event is raised.

5. **Make interactable objects**
   - Add the `Interactable` component to any GameObject you want to interact with. Set `UIAnchor` (a child transform where the prompt should appear) and `InteractionTypeName` (string shown in the prompt).
   - Add concrete behaviour components: `Chest`, `DoorComputer`, `Suitedman`, etc., and configure their serialized fields.

6. **Ensure `Outline` exists**
   - The `Interactable` script creates an `Outline` component at runtime (`gameObject.AddComponent<Outline>()`) and toggles its `enabled` state. Make sure an `Outline` MonoBehaviour is present in the project (for example the common QuickOutline / outline shader implementation) or provide your own `Outline` type matching the API used in `Interactable`.

---

## Example interaction flow (code-level)

1. `Interactor.Detect()` uses `Physics.OverlapSphereNonAlloc(...)` and looks for a component implementing `IInteractable`.
2. If found and `CanInteract == true`, `Interactor` sets it as focused and calls `OnFocusGained()` on the interactable.
3. `Interactor` raises `InteractableHoverUIEventChannelSO` with `(_focused.UIAnchor, _focused.InteractionTypeName)` so `UIInteraction` can show the prompt.
4. Player presses the interact button (wired through `InputReader` / `GameInput`) and `Interactor` calls `_focused.Interact()`.
5. The concrete interactable (Chest, DoorComputer, Suitedman) runs game logic, e.g. toggles door, opens chest, plays animation and shows `ChatBubble`.
6. When focus is lost, `Interactor` calls `OnFocusLost()` and raises `VoidEventChannelSO` to hide the UI.

---

## Notes & gotchas
- The `Interactable` script depends on an `Outline` type at runtime. If you remove/replace the outline implementation, adjust `Interactable` accordingly.
- `InteractableHoverUIEventChannelSO` carries a `(Transform, string)` tuple payload — update any existing UI listeners to expect that shape.
- `InputReader` lazily initializes `GameInput` at runtime — you only need to create an `InputReader` ScriptableObject and reference it from components.

---

## Why this structure
- Keeps gameplay logic decoupled (ScriptableObject event channels instead of direct references).
- `IInteractable` keeps the contract small and implementable by many object types.
- `Interactor` centralizes detection and input -> interaction flow so individual interactable scripts only contain their own behavior.
