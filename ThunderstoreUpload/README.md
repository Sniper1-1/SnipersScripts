
# SnipersScripts
A collection of misc scripts for moon/interior/item makers. ([My thread in the LC Modding Discord](https://discord.com/channels/1168655651455639582/1245084720614604873))

## Docs For Devs

### Components: Can be found under "AddComponent>SnipersScripts"

- **AudioClipEvents**: Invokes events based on the progression of an audio clip.
  - `AudioSource`: The audio source the clip plays from.
  - `OnAudioClipStart`: Invokes when clip starts playing.
  - `OnAudioClipEnd`: Invokes when clip finishes.
  - `OnAudioClipStop`: Invokes when the clip is stopped early.
  -  `PlayAudioClip(AudioClip clip)`: Plays `clip` on `AudioSource`.
  - `StopAudioClip()`: Stops the currently playing clip.

- **CommentComponent**: Empty component with an editable textbox. Does nothing.

- **EnemyRequiresNestOverride**: Temporarily makes the specified `EnemyType` not require a nest. (Useful if used in sync with an enemy spawner script to force an Old Bird to spawn at a specific location)
  - `OnEnemyDoesNotRequireNest<EnemyType>`: Invokes in the window that the `EnemyType` does not require a nest, passing on the `EnemyType` as a parameter.
  - `ToggleEnemyNestRequirement(EnemyType enemyType)`: Briefly makes the specified `EnemyType` not need a nest to spawn (supports blank reference).
  - `ToggleEnemyNestRequirement(string enemyName)`: Briefly makes the specified enemy based on the `EnemyName` field within an `EnemyType` ScriptableObject not need a nest to spawn (supports blank reference).

- **EntranceTeleportLock**: Used to lock/unlock main and/or fire exits. When using a door pair locked on one side from the unlocked side, the locked side will unlock.
  - `LockedMessage`: The `HoverTip` on a locked door.
  - `LockAll()`: Locks all main/fire exits both inside and outside.
  - `UnlockAll()`: Unlocks all main/fire exits both inside and outside.
  - `LockInside()`: Locks all inside main/fire exits.
  - `UnlockInside()`: Unlocks all inside main/fire exits.
  - `LockMainInside()`: Locks main from the inside.
  - `UnlockMainInside()`: Unlocks main from the inside.
  - `LockFireInside()`: Locks fire exits inside.
  - `UnlockFireInside()`: Unlocks fire exits inside.
  - `LockIndividualInside(EntranceTeleport tp)`: Locks the inside of the specified `EntranceTeleport`.
  - `UnlockIndividualInside(EntranceTeleport tp)`: Unlocks the inside of the specified `EntranceTeleport`.
  - `LockOutside()`: Locks all outside main/fire exits.
  - `UnlockOutside()`: Unlocks all outside main/fire exits.
  - `LockMainOutside()`: Locks main from the outside.
  - `UnlockMainOutside()`: Unlocks main from the outside.
  - `LockFireOutside()`: Locks fire exits outside.
  - `UnlockFireOutside()`: Unlocks fire exits outside.
  - `LockIndividualOutside(EntranceTeleport tp)`: Locks the outside of the specified `EntranceTeleport`.
  - `UnlockIndividualOutside(EntranceTeleport tp)`: Unlocks the outside of the specified `EntranceTeleport`.
  - `SetLockedMessageRpc(string message)`: sets `LockedMessage`to `message`.

- **ItemDropDistanceOverride**: Used to increase/decrease how far items can fall.
  - `ItemDropDistance`: How far items can be dropped (default is vanilla distance).
  - `ApplyOverrideOnStart`: If the override should apply automatically on the GameObject's start using `ItemDropDistance`.
  - `ApplyDropDistanceOverrideRpc(float distance)`: Sets the override distance.

- **LoggerScript**: Used to print messages to log.
  - `Log`: A message to log.
  - `LogType`: The channel to log `Log` to. `{Debug, Info, Warning, Error, Fatal}`
  - `LogOnStart`: Determines if `Log` should be printed to the`LogType` channel on the GameObject's start.
  - `PrintDebug(string message)`: Prints `message` to the debug channel.
  - `PrintInfo(string message)`: Prints `message` to the info channel.
  - `PrintWarning(string message)`: Prints `message` to the warning channel.
  - `PrintError(string message)`: Prints `message` to the error channel.
  - `PrintFatal(string message)`: Prints `message` to the fatal channel.

- **PlayerInShipDetector**: Used to detect and invoke events depending on if the player is inside or outside the ship.
  - `CheckPassively`: Determines if the detector passively checks in the background, invoking the events when conditions are met, or only check on a manual check.
  - `CheckImmediately`: Players state is checked and events are invoked immediately if true. If false, events only invoke on the player's first state swtich.
  - `OnPlayerEnterShip<PlayerControllerB>`: Invokes an event on the player that entered the ship.
  - `OnPlayerExitShip<PlayerControllerB>`: Invokes an event on the player that exited the ship.
  - `OnPlayerSwitchLocation<PlayerControllerB>`: Invokes an event on the player that switched between being outside/inside the ship.
  - `EvaluateIsInShipManualInvoke(PlayerControllerB player)`: Checks if `player` is inside or outside the ship.

- **Raycast**: Fires raycasts from GameObject, invoking events at start and end position as well as if it fails.
  - `FireRaysOnStart`: if the raycasts should fire on the GameObject's start.
  - `Raycasts`: List of `RaycastOptions`.
    - `RaycastOptions`: Specific options for a particular raycast.
      - `Direction`: The `{X Y Z}` direction for the raycast to travel.
      - `Distance`: How far the raycast should go before failing.
      - `GlobalAxis`: If true, `Direction` is global orientation, if false, it is relative to the GameObject's rotation.
      - `Mask`: What layers the raycast can hit.
      - `OnRayStart<Vector3>`: Invokes where raycast starts.
      - `OnRayHit<Vector3>`: Invokes where raycast hits something in `Mask`.
      - `OnRayFail<Vector3>`: Invokes where raycast starts if it fails to hit something in `Mask`.
  - `FireRays()`: Fires all `RaycastOptions` in `Raycasts`.
  - `FireRay(int rayIndex)`: fires the `RaycastOptions` at index `rayIndex` in `Raycasts`.

- **RemoteScrapEvents**: Extends the remote scrap item's functionality to invoke an event in addition to toggling the ship lights.
  - `OnRemoteClick`: The event that invokes when the remote scrap item is clicked.

- **ShipController**: Controls and invokes various events related to the ship.
  - `OnMagnetEnable`: Invokes when the ship magnet turns on.
  - `OnMagnetDisable`: Invokes when the ship magnet turns off.
  - `OnMagnetToggle`: Invokes when the ship magnet is toggled.
  - `OnShipDescend`: Invokes when the ship starts landing.
  - `OnShipLand`: Invokes when the ship finishes landing.
  - `OnShipAscend`: Invokes when the ship starts takeoff.
  - `OnShipEnterOrbit`: Invokes when the ship returns to orbit.
  - `OnShipMessageStart`: Invokes when a ship message starts (like the midnight alert).
  - `OnShipMessageEnd`: Invokes when a ship message ends (like the midnight alert).
  - `OnSpeakerMute`: Invokes when the ship speaker is muted.
  - `OnSignalTransmitStart`: Invokes when a message sent over the signal transmitter starts.
  - `OnSignalTransmitEnd`: Invokes when a message sent over the signal transmitter ends.
  - `OnHornPull`: Invokes when the ship horn is pulled.
  - `WhileHornPulled`: Invokes continuously while the ship horn is pulled.
  - `OnHornRelease`: Invokes when the ship horn is released.
  - `OnDoorOpen`: Invokes when the ship door opens.
  - `OnDoorClose`: Invokes when the ship door closes.
  - `OnDoorToggle`: Invokes when the ship door is toggled.
  - `OnScreenTurnOn`: Invokes when the radar screen turns on.
  - `OnScreenTurnOff`: Invokes when the radar screen turns off.
  - `OnScreenPoweredToggle`: Invokes when the radar screen power is toggled.
  - `OnScreenSpectatorToggle`: Invokes when the target of the radar screen's spectate is toggled.
  - `OnTeleportStart`: Invokes when the teleporter sequence starts.
  - `OnTeleportEnd`: Invokes when the teleporter sequence ends.
  - `OnInversetStart`: Invokes when the inverse teleporter sequence starts.
  - `OnInverseEnd`: Invokes when the inverse teleporter sequence ends.
  - `OnShipLightsTurnOn`: Invokes when the ship lights turn on.
  - `OnShipLightsTurnOff`: Invokes when the ship lights turn off.
  - `OnShipLightsToggle`: Invokes when the ship lights toggle.
  - `OnClampLock`: Invokes when the electric chair clamps are closed.
  - `OnClampUnlock`: Invokes when the electric chair clamps are opened.
  - `OnClampToggle`: Invokes when the electric chair clamps are toggled.
  - `OnShockStart`: Invokes when the electric chair shock starts.
  - `OnShockEnd`: Invokes when the electric chair shock ends.
  - `OnTvTurnOn`: Invokes when the tv turns on.
  - `OnTvTurnOff`: Invokes when the tv turns off.
  - `OnTvToggle`: Invokes when the tv power toggles.
  - `OnTvStationChange`: Invokes when the tv switches to playing a new clip.
  - `PullHorn()`: Activates the ship horn if it exists.
  - `SetChairClamped(bool clamp)`: Closes electric chair clamps if true, or opens them if false. Only if electric chair exists.
  - `SetMagnetPowered(bool powered)`: Turns magnet of if true, off if false. Only if electric chair exists.
  - `SetShipDoorOpen(bool open)`: Opens ship door if true, closes it if false.
  - `SetShipLanded(bool land)`: Lands the ship if true. Takes off if false. Both can only occur if it is possible for the ship to perferm said action.
  - `SetShipLightsOn(bool on)`: Turns lights on if true, off if false.
  - `SetShipMessage(ShipMessageSO message)`: Plays `message` over the HUD. (Like the midnight warning. See `ShipMessageSO` under the "Scriptable Objects" section for more.)
  - `SetShipScreenOn(bool powered)`: Turns on the radar screen if true, off if false.
  - `SetShipSpeakerAudio(AudioClip audioClip)`: Plays `audioClip` over the ship speaker, or mutes it if `audioClip` is null.
  - `SetTransmitterMessage(string message)`: Broadcasts `message` over the signal transmitter if it exists.
  - `SetTvClip(TelevisionClipContainerSO clip)`: Plays `clip` over the tv if it exists. (See `TelevisionClipContainerSO` under the "Scriptable Objects" section for more.)
  - `SetTvOn(bool on)`: Turns tv on if true, off if false and tv exists.
  - `ShockElectricChairRpc()`: Shocks the electric chair if it exists.
  - `TeleportPlayer(bool inverse)`: Activates the regular teleporter if it exists and `inverse` is false. Activates the inverse teleporter if it exists and `inverse` is true.
  - `ToggleChairClamps()`: Inverts the electric chair clamps closed/open state if the electric chair exists.
  - `ToggleMagnetPower()`: Inverts the ship magnet power.
  - `ToggleShipDoor()`: Inverts the ship door open/closed state.
  - `ToggleShipFlight()`: Switches the ship between takeoff and landing, if it is possible to do so.
  - `ToggleShipLights()`: Inverts the ship light power.
  - `ToggleShipScreenPower()`: Inverts the radar screen power.
  - `ToggleShipScreenSpectator()`: Switches the specate target of the radar screen.
  - `ToggleTv()`: Switches the tv power if it exists.

- **WaitRandomTime**: Waits a random amount of time within a range.
  - `MinWaitTime`: The minimum time it will wait.
  - `MaxWaitTime`: The maximum time it will wait. 
  - `RunOnStart`: If the timer should start automatically on Game Object's Start.
  - `OnlyRandomizeOnce`: If the wait time should only be randomized once or on every invokation.
  - `OnWaitStart`: Invokes when the timer starts.
  - `OnWaitComplete`: Invokes when the timer completes.
  - `OnWaitStop`: Invokes if the timer is cancelled.
  - `StartWaitRpc()`: Starts the timer.
  - `StopWaitRpc()`: Stops the running timer.

- **WaterloggedSensor**: Invokes events depending on if the GameObject is in water. Must have an `IsTrigger` true collider on the same GameObject.
  - `CheckOnStart`: If the sensor should evaluate its state on the GameObject's start.
  - `OnSubmerge`: Invokes if sensor is under water.
  - `OnEmerge`: Invokes if sensor is above water.
  - `CheckSensorRpc()`: Manually call to check if the sensor is in or out of water, invoking the appropriate event.

### Scriptable Objects: Can be found under "Assets (or right click in project)>Create>SnipersScripts"

- **CommentSO**: Empty ScriptableObject with an editable textbox. Does nothing.

- **ShipMessageSO**: A container for a ship message to be played over the HUD (See `ShipController.SetShipMessage(ShipMessageSO message)` for more information).
  - `Ship Message`: An array of `DialogueSegment`s to form the message.
    - `DialogueSegment`: A container for one popup of the whole message. Having multiple segments will swap through them in order.
      - `BodyText`: The text to display in the main, bottom, bigger textbox.
      - `SpeakerText`: The text to display in the smaller, top textbox (Often "PILOT COMPUTER" in vanilla).
      - `WaitTime`: How long the segment should display before moving to the next segment or ending the message.

- **TelevisionClipContainerSO**: A container for a tv clip to be played over the tv (See `ShipController.SetTvClip(TelevisionClipContainerSO clip)` for more information).
  - `Clip`: The `VideoClip` to display visually on the tv.
  - `Audio`: The `AudioClip` to play over the tv speaker.
  - `ForceTvOn`: If true, the tv will turn on when this clip is played. If false, the tv will not turn on if it is currently off, but will play in the background meaning that if the tv is turned on, the clip may be partway through.
  - `ForceTvOff`: If true, the tv will turn off when this clip is finished. If false, the tv will resume playing vanilla videos.


**NOTE**
  ---
Not all functionality is synced. They can be synced by invoking them from synced events that run on all clients, for example, by running it through something like [JLL](https://thunderstore.io/c/lethal-company/p/JacobG5/JLL/)'s `JClientSync`'s `SyncedEvent` as an intermediate step. Known non-synced functionality:
  - `AudioClipEvents`
  - `EnemyRequiresNestOverride`
  - `RayCast`
  - `ShipController`: Partial
    - `WhileHornPulled`
    - `OnHornRelease`
    - `SetShipMessage(ShipMessageSO message)`
    - `SetShipSpeakerAudio(AudioClip audioClip)`
    - `SetTvClip(TelevisionClipContainerSO clip)`

## Credits

- The developers of this mod's dependencies as it literally could not exist without them.
- [Audio Knight](https://www.youtube.com/@knightofaudio) on YouTube for a handy starting tutorial.
- [Nomnomab's project patcher](https://github.com/nomnomab/lc-project-patcher) to access vanilla LC through Unity.
- Those that helped provide information on the modding Discord, mainly through the [Dev-general channel on Discord](https://discord.com/channels/1168655651455639582/1168656318345777313).
- Debugging tools like [Imperium](https://thunderstore.io/c/lethal-company/p/giosuel/Imperium/) and [UnityExplorer](https://thunderstore.io/c/lethal-company/p/LethalCompanyModding/Yukieji_UnityExplorer/).
- Xilophor's [modding template](https://github.com/Xilophor/Lethal-Company-Mod-Templates) for a handy way to start.
- [JLL](https://thunderstore.io/c/lethal-company/p/JacobG5/JLL/) and [itolib](https://thunderstore.io/c/lethal-company/p/pacoito/itolib/) for some inspiration.
- Thunderstore for hosting this mod as I wouldn't know how to distribute without it.