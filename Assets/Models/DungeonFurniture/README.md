# Dungeon furniture

Six Blender-generated static low-poly props. Shared wood and iron palette textures use URP/Lit materials. No gameplay components or room placements are included.

Ready-to-place prefabs: `Assets/Prefabs/Tiles/Structures/Furniture/`.
Each prefab has a floor-level centre pivot, unit root scale and a solid bounding BoxCollider (including the open space under tables and inside shelves). These colliders are intended for static dungeon obstacles.

| Asset | Unity bounds, metres (X/Y/Z) | Imported triangles |
| --- | --- | --- |
| Shelf_Tall | 1.460 / 1.970 / 0.530 | 1884 |
| Shelf_Low | 1.660 / 1.070 / 0.530 | 1578 |
| Chair_Reinforced | 0.770 / 1.127 / 0.650 | 1236 |
| Chair_Simple | 0.650 / 1.028 / 0.649 | 752 |
| Table_Trestle | 2.000 / 0.870 / 0.990 | 1090 |
| Table_Round | 1.360 / 0.834 / 1.352 | 2464 |

Local editable source: `ArtSource/DungeonFurniture/DungeonFurniture.blend`.
The Blender file arranges the models for a gallery preview; individual FBXs are exported at the origin before that arrangement.
Generator: `Tools/Blender/generate_dungeon_furniture.py` (Blender 5.1).
Unity importer: `Tools/Unity/BuildDungeonFurniture.cs`, entry `BuildDungeonFurniture.Build` via Unity Pipeline `script run`.
`ArtSource/` and `Tools/` are excluded by the project's existing Git rules; preserve them separately if the editable source is needed on another machine.

Regeneration writes the six FBXs and two palettes. Run the Unity importer afterward to rebuild the material remaps and prefabs. No existing scenes or room prefabs are modified.
