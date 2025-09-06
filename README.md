
🎮 Third-Person Interaction System for Unity

A robust and extensible interaction system for third-person Unity games, designed to manage interactions with objects like chests, doors, NPCs, and in-world UI prompts.
This system focuses on modularity, clean architecture using ScriptableObjects for event-driven communication, and visual feedback for intuitive gameplay.

✅ Features

✅ Interaction system using IInteractable and IHighlightable interfaces

✅ Highlight interactable objects with layer-based outline visuals

✅ Supports multiple interaction types:

Talk

PickUp

OpenClose

Open

✅ Event-driven architecture using generic EventChannel<T>

✅ Handles interaction UI prompts (interaction type, anchored near interactable)

✅ Smooth third-person camera control

✅ Footstep and landing sounds triggered by animation events

✅ Customizable jump, sprint, and movement settings

✅ Chat bubble UI for displaying messages

✅ Well-structured code with clear separation of concerns

🎯 System Architecture
Core Concepts

Interactable Objects implement IInteractable and optionally IHighlightable.

Event Channels (EventChannel<T>) decouple event producers and consumers, making the system highly modular and extensible.

InteractionSensor continuously detects interactable objects in range.

InteractionController listens for user input and manages interaction logic.

UIInteraction handles showing/hiding interaction prompts.

OutlineHighlighter applies layer-based highlighting.

📦 Key Components
Interfaces

IInteractable: Defines Interact() method, InteractableType, and UIAnchor.

IHighlightable: Defines Highlight() and UnHighlight().

Event Channels

VoidEventChannelSO: For events with no parameters.

IInteractableEventChannelSO: For broadcasting interactable objects.

InteractableHoverUIEventChannelSO: Passes (Transform, InteractableType) on hover.

Interactable Implementations

Chest: Opens chest lid on interaction.

Door: Opens and closes two-part sliding door.

DoorComputer: Triggers door interaction.

Suitedman: Plays interaction animation + shows chat bubble.

Player Components

PlayerController: Manages movement, jumping, sprinting, and gravity.

CameraThirdPersonLook: Third-person camera controller.

MovementAnimationEventTrigger: Handles footstep and landing audio.

UI Components

ChatBubble: Displays messages from interactables.

UIInteraction: Displays interaction prompts above interactable objects.

⚙️ Usage
1. Setup Scene

Add InteractionSensor and InteractionController to your player.

Configure layer mask to specify which layers contain interactable objects.

Attach InputReader for reading player input.

2. Create Interactable Objects

Implement IInteractable interface.

Assign UIAnchor and set InteractableType.

Attach OutlineHighlighter with the appropriate visuals.

Create and link event channels via ScriptableObjects.

3. Input System Integration

Configure InputReader to trigger interaction, sprint, and jump actions.

4. UI Setup

Setup UIInteraction prefab with TextMeshPro for displaying prompts.

Setup ChatBubble prefab for NPC dialogues.

⚡ Example Interaction Flow

Player approaches a chest → InteractionSensor detects it → raises OnInteractableFound.

InteractionController highlights chest and shows interaction UI prompt.

Player presses interact button → chest opens via Interact() method → layer reset and highlight removed.

UIInteraction hides the prompt once interaction is complete.

🎯 Why This System?

✔️ Decoupled, reusable architecture

✔️ Easy to extend: Add new interactable objects by implementing the interfaces

✔️ Clear visual feedback

✔️ Fully configurable via Unity Inspector

✔️ No monolithic hardcoded logic

⚡ Future Improvements

Add localization support for interaction text.

Support multiplayer interaction sync.

Extend event system to support event chaining or priority handling.

Add visual indicators for cooldowns or active interactions.

📚 Dependencies

DOTween
 for smooth animations

TextMeshPro
 for UI text

✅ License

MIT License – Free to use and extend in your projects.

🚀 Contribution

Feel free to fork and submit PRs.
For bug reports and feature requests, please open an issue.