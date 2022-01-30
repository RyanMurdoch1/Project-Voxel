using Environment_Builder;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class EnvironmentBuilder : OdinMenuEditorWindow
    {
        private const string EnvironmentObjectsMenuPath = "Environment Objects";
        private const string EnvironmentObjectsFolderPath = "Assets/Data/Environment Objects";
        private static bool _placementActive;
        private static OdinMenuTreeSelection _menuSelection;
        private static EnvironmentBuilder _environmentBuilderWindow;
        private string _buildButtonText = "Start Building";
        private Color _buildButtonColour = Color.red;
  
        [MenuItem("Window/Environment Builder")]
        private static void Init()
        {
            _environmentBuilderWindow = (EnvironmentBuilder)GetWindow(typeof(EnvironmentBuilder));
            _environmentBuilderWindow.Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.AddAllAssetsAtPath(EnvironmentObjectsMenuPath, EnvironmentObjectsFolderPath, typeof(EnvironmentObject));
            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            if (MenuTree is {Selection: { }})
            {
                _menuSelection = MenuTree.Selection;
            }
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            
            GUI.backgroundColor = _buildButtonColour;
            if (GUILayout.Button(_buildButtonText))
            {
                ToggleBuilderPlacement();
            }

            if (SceneViewObjectPositioner.CanUndo())
            {
                GUI.backgroundColor = Color.gray;
                if (GUILayout.Button("Undo Last (Z)"))
                {
                    SceneViewObjectPositioner.Undo();
                }
            }
        }

        private void ToggleBuilderPlacement()
        {
            if (_placementActive)
            {
                _buildButtonText = "Start Building";
                _buildButtonColour = Color.green;
                _placementActive = false;
                SceneViewObjectPositioner.IsPlacingObject = false;
            }
            else
            {
                _buildButtonText = "Stop Building";
                _buildButtonColour = Color.red;
                _placementActive = true;
            }
        }

        private void ManageSceneView(SceneView view)
        {
            if (!_placementActive) return;
            
            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && !SceneViewObjectPositioner.IsPlacingObject && _menuSelection.SelectedValue != null)
                {
                    SceneViewObjectPositioner.BeginObjectPlacement(hit, _menuSelection.SelectedValue as EnvironmentObject);
                }

                if (SceneViewObjectPositioner.IsPlacingObject)
                {
                    SceneViewObjectPositioner.RenderObjectPlacement(hit);
                }
            }

            if (Event.current.type == EventType.MouseUp && Event.current.button == 1 && SceneViewObjectPositioner.IsPlacingObject && SceneViewObjectPositioner.IsPlaceable())
            {
                SceneViewObjectPositioner.PlaceObject();
            }

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Q)
            {
                SceneViewObjectPositioner.RotateObjectLeft();
            }
            else if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.E)
            {
                SceneViewObjectPositioner.RotateObjectRight();
            }
            else if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Z && SceneViewObjectPositioner.CanUndo())
            {
                SceneViewObjectPositioner.Undo();
            }

            Event.current.Use();
        }
        
        protected override void OnEnable() => SceneView.duringSceneGui += ManageSceneView;
        private void OnDisable() => SceneView.duringSceneGui -= ManageSceneView;
    }
}
