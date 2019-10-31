#  Wwise Tool in Unity
BlackBox Realities decided to use the Wwise audio engine inside of the Unity game engine. While the tool has been used in triple-A titles, the barebone Unity SDK makes it difficult to work with. To help our audio engineer Krystian and I developed a set of tools to improve workflow. 

The first of the asset I we created was to solve the issue of Unity Cloud Build not working with Wwise. Our projects were over 32 gigabytes and took hours to build, so it was crucial for us to offload building of the projects onto the cloud. 
If you’ve used Wwise and Unity Cloud Build, you’re probably all to familiar with this error
```
 - 2455: [Unity] Assets/Wwise/Deployment/API/Handwritten/Windows/AkWindowsSettings.cs(48,26): error CS0246: The type or namespace name `AkAudioAPI' could not be found. Are you missing an assembly reference?
 - 2456: [Unity] Assets/Wwise/Deployment/API/Handwritten/Windows/AkWindowsSettings.cs(48,13): error CS1061: Type `AkPlatformInitSettings' does not contain a definition for `eAudioAPI' and no extension method `eAudioAPI' of type `AkPlatformInitSettings' could be found. Are you missing an assembly reference?
 - 2457: [Unity] Assets/Wwise/Deployment/API/Generated/Mac/AkPlatformInitSettings_Mac.cs(13,14): (Location of the symbol related to previous error)
 - 2458: [Unity] Assets/Wwise/Deployment/API/Handwritten/Windows/AkWindowsSettings.cs(49,13): error CS1061: Type `AkPlatformInitSettings' does not contain a definition for `bGlobalFocus' and no extension method `bGlobalFocus' of type `AkPlatformInitSettings' could be found. Are you missing an assembly reference?
 - 2459: [Unity] Assets/Wwise/Deployment/API/Generated/Mac/AkPlatformInitSettings_Mac.cs(13,14): (Location of the symbol related to previous error)
```
Thanks to **cshankland_unity** from Unity Forums and there solution:
https://forum.unity.com/threads/wwise-integration-build-failed-at-unity-cloud-only.562462/

The solution requires to edit core sdk code which can be tasking. In order to aid with the multiple script changes, we created a tool that helps hunt down the `Define Symbol` by going through the sdk and make the following changes with a simple edior button:
 - `#if (UNITY_STANDALONE_OSX && !UNITY_EDITOR) || UNITY_EDITOR_OSX `
 - ` #if (UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN `
  to
 -  ` #if (UNITY_STANDALONE_OSX && !UNITY_EDITOR) || (UNITY_EDITOR_OSX && !UNITY_STANDALONE_WIN) ` 
 - ` #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_WSA`

