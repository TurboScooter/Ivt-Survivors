# Quantum Particles v1.0.1

This package can be downloaded for free from the Discord Server below. Please report any issues you encounter in the #bug-reports channel or ask any questions you may have in the #support-questions channel.
Please make sure to read the provided LICENSE, for more information on what you can and cannot do with this package check https://link250.github.io/QuantumDocs/legal/ToS

Video Tutorial: https://www.youtube.com/watch?v=rttDqk_KsjM
Documentation can be found at: https://link250.github.io/QuantumDocs/docs/particles/
Discord Server: https://discord.gg/Va5VPev

If you want to support me and get early access to the latest features, consider becoming a Patron: https://www.patreon.com/QuantumLoT

## VRCFury Setup Instructions

Simply drag the "QP VRCFury Setup" prefab (located in Quantum/Particles/Prefabs) into your scene and then onto your Avatar's root object, FRCFury will automatically handle the rest.

If you want to use the Presets, use the "QP Preset Base" prefab (located in Quantum/Particles/Prefabs/Presets (VRCFury)) instead.
After adding it to your Avatar's root object, open up the "QP Preset Base" prefab and drag the preset you want to use (located in the same folder as the base) onto the "Quantum Particles" GameObject.

A quick demonstration of these two setups can be seen in the video below:
https://youtu.be/qiwn_u5nTME

## Manual Setup Instructions

~Avatar Setup~
1. Drag "QP Avatar Setup" prefab into your Unity scene (for base scaling). This is located in Quantum/Particles/Prefabs
2. Right click "QP Avatar Setup" in your hierarchy and click "Unpack Prefab"
3. Drag "Quantum Particles" onto your avatar's root object.
4. Drag all the GameObjects inside ">>>MOVE TO ARMATURE BONES<<<" to their corresponding bones on your armature (Left Hand, Right Hand, Neck, Head).
 - You can also move the Left/Right Hand GameObjects to any other bone you prefer, like the Left/Right Index Finger.
5. Reset the Transform components of the GameObjects from the previous step by clicking the settings cog in the top right of the Transform component in your Inspector and clicking "Reset"
6. (Optional) Position the Left/Right Hand Anchors to your preference.
7. Select "Desktop Anchor" under "[Neck] Proxy" and position it to your preference (typically at eye/viewpoint level and in front of you at about a distance equal to your armspan)

~AV3 Setup ~
It is advised to use something like AV3Manager to merge the "Quantum Particles Parameters" and "Quantum Particles FX" Animator Controller found under "Quantum/Particles/Resources/Expressions" with your Avatars Animator Controller and Parameters.
Finally, add a new sub-menu control to your avatar's Menu file and drag in "Quantum Particles Menu" as the target sub-menu.
