using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/************************************************************************************
* @说    明：添加自定义Button的指令
* @作    者：zhoumingfeng
* @版 本 号：V1.00
* @描    述：可右键添加Button组件, 也可以用"GameObject/UI/UEasyUI/XXButton"菜单来添加;
* @创建时间：2022.09.22
*************************************************************************************/

namespace UEasyUI.Tools
{
    public class AddCustomButtonEditor : Editor
    {
        [MenuItem("GameObject/UI/UEasyUI/CDButton", false, 2031)]
        private static void AddCDButton(MenuCommand menuCommand)
        {
            GameObject child = new GameObject("CDButton");
            RectTransform rectTransform = child.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100f, 100f);
            child.AddComponent<Image>();
            child.AddComponent<CDButton>();
            PlaceUIElementRoot(child, menuCommand);
        }

        [MenuItem("GameObject/UI/UEasyUI/SwitchButton", false, 2032)]
        private static void AddSwitchButton(MenuCommand menuCommand)
        {
            GameObject child = new GameObject("SwitchButton");
            RectTransform rectTransform = child.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100f, 100f);
            child.AddComponent<Image>();
            child.AddComponent<SwitchButton>();
            PlaceUIElementRoot(child, menuCommand);
        }

        [MenuItem("GameObject/UI/UEasyUI/GroupButton", false, 2033)]
        private static void AddGroupButton(MenuCommand menuCommand)
        {
            GameObject child = new GameObject("GroupButton");
            RectTransform rectTransform = child.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100f, 100f);
            child.AddComponent<Image>();
            child.AddComponent<GroupButton>();
            PlaceUIElementRoot(child, menuCommand);
        }

        [MenuItem("GameObject/UI/UEasyUI/CheckBoxButton", false, 2034)]
        private static void AddCheckBoxButton(MenuCommand menuCommand)
        {
            GameObject child = new GameObject("CheckBoxButton");
            RectTransform rectTransform = child.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100f, 100f);
            child.AddComponent<Image>();
            child.AddComponent<CheckBoxButton>();
            PlaceUIElementRoot(child, menuCommand);
        }

        private static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
        {
            GameObject parent = menuCommand.context as GameObject;
            if (parent == null || parent.GetComponentInParent<Canvas>() == null)
            {
                parent = GetOrCreateCanvasGameObject();
            }

            string uniqueName = GameObjectUtility.GetUniqueNameForSibling(parent.transform, element.name);
            element.name = uniqueName;
            Undo.RegisterCreatedObjectUndo(element, "Create " + element.name);
            Undo.SetTransformParent(element.transform, parent.transform, "Parent " + element.name);
            GameObjectUtility.SetParentAndAlign(element, parent);
            if (parent != menuCommand.context) // not a context click, so center in sceneview
                SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());

            CreateChildText(element);

            Selection.activeGameObject = element;
        }

        private static void CreateChildText(GameObject parent)
        {
            GameObject element = new GameObject("txt");
            RectTransform rectTransform = element.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100f, 100f);
            element.AddComponent<TextMeshProUEUI>();

            string uniqueName = GameObjectUtility.GetUniqueNameForSibling(parent.transform, element.name);
            element.name = uniqueName;
            Undo.RegisterCreatedObjectUndo(element, "Create " + element.name);
            Undo.SetTransformParent(element.transform, parent.transform, "Parent " + element.name);
            GameObjectUtility.SetParentAndAlign(element, parent);
            SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());
        }

        private static void CreateEventSystem(bool select, GameObject parent = null)
        {
            var esys = Object.FindObjectOfType<EventSystem>();
            if (esys == null)
            {
                var eventSystem = new GameObject("EventSystem");
                GameObjectUtility.SetParentAndAlign(eventSystem, parent);
                esys = eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();

                Undo.RegisterCreatedObjectUndo(eventSystem, "Create " + eventSystem.name);
            }

            if (select && esys != null)
            {
                Selection.activeGameObject = esys.gameObject;
            }
        }

        static public GameObject CreateNewUI()
        {
            // Root for the UI
            var root = new GameObject("Canvas");
            root.layer = LayerMask.NameToLayer("UI");
            Canvas canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            root.AddComponent<CanvasScaler>();
            root.AddComponent<GraphicRaycaster>();
            Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);

            // if there is no event system add one...
            CreateEventSystem(false);
            return root;
        }

        static public GameObject GetOrCreateCanvasGameObject()
        {
            GameObject selectedGo = Selection.activeGameObject;

            // Try to find a gameobject that is the selected GO or one if its parents.
            Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
            if (canvas != null && canvas.gameObject.activeInHierarchy)
                return canvas.gameObject;

            // No canvas in selection or its parents? Then use just any canvas..
            canvas = Object.FindObjectOfType(typeof(Canvas)) as Canvas;
            if (canvas != null && canvas.gameObject.activeInHierarchy)
                return canvas.gameObject;

            // No canvas in the scene at all? Then create a new one.
            return CreateNewUI();
        }

        private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
        {
            // Find the best scene view
            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null && SceneView.sceneViews.Count > 0)
                sceneView = SceneView.sceneViews[0] as SceneView;

            // Couldn't find a SceneView. Don't set position.
            if (sceneView == null || sceneView.camera == null)
                return;

            // Create world space Plane from canvas position.
            Vector2 localPlanePosition;
            Camera camera = sceneView.camera;
            Vector3 position = Vector3.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition))
            {
                // Adjust for canvas pivot
                localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
                localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

                localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
                localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

                // Adjust for anchoring
                position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
                position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

                Vector3 minLocalPosition;
                minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
                minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

                Vector3 maxLocalPosition;
                maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
                maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

                position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
                position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
            }

            itemTransform.anchoredPosition = position;
            itemTransform.localRotation = Quaternion.identity;
            itemTransform.localScale = Vector3.one;
        }
    }
}