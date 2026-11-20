# Dungeon torch

Low poly props authored in Blender 5.1, with flat normals and a shared palette.
Materials use URP/Lit. No gameplay components or scene changes are included.

| Model | Triangles | Size |
| --- | ---: | --- |
| TorchHolder.fbx | 544 | 0.176 m wide, 0.521 m high, 0.351 m deep |
| DungeonTorch.fbx | 946 | 0.719 m high, approximately 0.145 m across the head |
| TorchFlame.fbx | 162 | 0.320 m high; optional static geometry |

Ready-to-use visual prefabs: `Assets/Prefabs/Props/DungeonTorch/`.

- `TorchHolder`: empty wall holder.
- `DungeonTorch`: unlit torch with a FlameSocket at local Y = 0.70 m.
- `DungeonTorchLit`: torch, separate emissive flame and a warm point light.
- `WallTorchLit`: assembled holder and lit torch.

Unity coordinates: +Y up, +Z out of the wall. The holder pivot is at the centre
of the wall plate on the wall surface. The torch pivot is at the handle base.
For assembly, place the torch at local (0, -0.135, 0.29) relative to the holder.
The flame FBX shares the torch's origin, so its local position is (0, 0, 0)
when parented directly to the torch. Under FlameSocket use (0, -0.70, 0).

The flame is a static mesh; animation and flicker are not included. The point
light has shadows disabled and can be tuned per room. No colliders are included.

Local source: `ArtSource/DungeonTorch/DungeonTorch.blend`.
Rebuild: `Tools/Blender/generate_dungeon_torch.py`. These source directories are
excluded from Git by the project's existing .gitignore; exported Unity assets
and their metadata are stored in Assets.
