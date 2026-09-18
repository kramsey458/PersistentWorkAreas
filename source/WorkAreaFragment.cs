using System;
using Timberborn.BaseComponentSystem;
using Timberborn.EntityPanelSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace PersistentWorkAreas
{
    public sealed class WorkAreaFragment : IEntityPanelFragment
    {
        private readonly WorkAreaService _service;
        private VisualElement _root;
        private Toggle _toggle;
        private Label _hint;
        private BaseComponent _entity;
        public WorkAreaFragment(WorkAreaService service) { _service = service; }

        public VisualElement InitializeFragment()
        {
            _root = new VisualElement { name = "PersistentWorkAreasFragment" };
            _root.style.paddingLeft = _root.style.paddingRight = 10;
            _root.style.paddingTop = _root.style.paddingBottom = 8;
            _root.style.backgroundColor = new Color(.13f, .23f, .22f, .96f);
            _root.style.color = new Color(1f, .94f, .75f);
            _toggle = new Toggle("Keep working area visible");
            _toggle.name = "PersistentWorkAreasPin";
            _toggle.style.fontSize = 14;
            _toggle.style.whiteSpace = WhiteSpace.Normal;
            _toggle.tooltip = "Keep this building's working-area outline visible while planting or using other tools.";
            _toggle.RegisterValueChangedCallback(e => _service.SetPinned(_entity, e.newValue));
            _root.Add(_toggle);
            _hint = new Label("Use Clear pinned areas at the top right to clear all outlines.");
            _hint.style.fontSize = 12;
            _hint.style.whiteSpace = WhiteSpace.Normal;
            _hint.style.marginTop = 4;
            _root.Add(_hint);
            _service.Changed += Refresh;
            ClearFragment();
            return _root;
        }

        public void ShowFragment(BaseComponent entity) { _entity = entity; Refresh(); }
        public void ClearFragment() { _entity = null; if (_root != null) _root.style.display = DisplayStyle.None; }
        public void UpdateFragment() { }
        private void Refresh()
        {
            if (_root == null) return;
            bool supports = WorkAreaService.Supports(_entity);
            _root.style.display = supports ? DisplayStyle.Flex : DisplayStyle.None;
            _toggle.SetValueWithoutNotify(supports && _service.IsPinned(_entity));
            _toggle.SetEnabled(_service.Available);
            _hint.text = _service.Available
                ? "Use Clear pinned areas at the top right to clear all outlines."
                : "Working-area pinning is unavailable. Check the game log.";
        }

        internal static Button MakeButton(string text, Action clicked)
        {
            var button = new Button(clicked) { text = text };
            button.style.backgroundColor = new Color(.16f, .31f, .27f, .98f);
            button.style.color = new Color(1f, .94f, .75f);
            button.style.fontSize = 14;
            button.style.unityFontStyleAndWeight = FontStyle.Bold;
            button.style.paddingLeft = button.style.paddingRight = 12;
            button.style.paddingTop = button.style.paddingBottom = 7;
            button.style.borderTopWidth = button.style.borderBottomWidth = 1;
            button.style.borderLeftWidth = button.style.borderRightWidth = 1;
            var border = new Color(.52f, .67f, .47f);
            button.style.borderTopColor = button.style.borderBottomColor = border;
            button.style.borderLeftColor = button.style.borderRightColor = border;
            return button;
        }
    }
}
