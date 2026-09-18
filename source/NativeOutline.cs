using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Timberborn.BlockSystem;
using Timberborn.BlueprintSystem;
using Timberborn.BuildingsNavigation;
using Timberborn.LevelVisibilitySystem;
using Timberborn.MapStateSystem;
using UnityEngine;

namespace PersistentWorkAreas
{
    // Timberborn's bounds renderer is internal. Keep all reflection in this adapter.
    // This owns an independent renderer; it never alters the vanilla renderer or materials.
    internal sealed class NativeOutline : IDisposable
    {
        private const BindingFlags Fields = BindingFlags.Instance | BindingFlags.NonPublic;
        private readonly object _drawer;
        private readonly Action<IReadOnlyCollection<Vector3Int>> _update;
        private readonly Action _draw;
        private bool _disposed;

        public NativeOutline(IBlockService blocks, PreviewBlockService previews,
            ILevelVisibilityService visibility, MapSize mapSize, ISpecService specs)
        {
            var assembly = typeof(BuildingTerrainRange).Assembly;
            var calculatorType = assembly.GetType("Timberborn.BuildingsNavigation.BoundsNavRangeCalculator", true);
            var drawerType = assembly.GetType("Timberborn.BuildingsNavigation.BoundsNavRangeDrawer", true);
            var calculator = Activator.CreateInstance(calculatorType, blocks, previews, visibility);
            _drawer = Activator.CreateInstance(drawerType, calculator, mapSize, specs);
            _update = (Action<IReadOnlyCollection<Vector3Int>>)drawerType.GetMethod("UpdateArea")
                .CreateDelegate(typeof(Action<IReadOnlyCollection<Vector3Int>>), _drawer);
            _draw = (Action)drawerType.GetMethod("Draw").CreateDelegate(typeof(Action), _drawer);
            try { drawerType.GetMethod("Load").Invoke(_drawer, null); }
            catch { Dispose(); throw; }
        }

        public void Update(IReadOnlyCollection<Vector3Int> cells) => _update(cells);
        public void Draw() { if (!_disposed) _draw(); }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            // The native renderer creates a mesh and cloned material for each height layer.
            // Its shared spec assets must never be destroyed.
            var bounds = _drawer.GetType().GetField("_boundsMesh", Fields).GetValue(_drawer);
            var layers = (IDictionary)bounds.GetType().GetField("_layers", Fields).GetValue(bounds);
            foreach (var layer in layers.Values)
            {
                var type = layer.GetType();
                UnityEngine.Object.Destroy((Mesh)type.GetField("_mesh", Fields).GetValue(layer));
                UnityEngine.Object.Destroy((Material)type.GetField("_material", Fields).GetValue(layer));
            }
            layers.Clear();
        }
    }
}
