#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using DesktopOjisan.Core;
using DesktopOjisan.Server;
using DesktopOjisan.UI;

namespace DesktopOjisan.Editor
{
    /// <summary>
    /// デスクトップおじさんのUI自動セットアップ
    /// </summary>
    public class UISetupEditor : EditorWindow
    {
        [MenuItem("DesktopOjisan/Setup Scene")]
        public static void SetupScene()
        {
            CreateGameManager();
            CreateCanvas();
            CreateOjisanUI();
            CreateCommandUI();

            Debug.Log("[UISetupEditor] シーンセットアップ完了！");
        }

        private static void CreateGameManager()
        {
            var existing = FindObjectOfType<GameManager>();
            if (existing != null)
            {
                Debug.Log("[UISetupEditor] GameManager は既に存在します");
                return;
            }

            var go = new GameObject("GameManager");
            go.AddComponent<ApiServer>();
            go.AddComponent<GameManager>();

            Undo.RegisterCreatedObjectUndo(go, "Create GameManager");
        }

        private static Canvas CreateCanvas()
        {
            var existingCanvas = FindObjectOfType<Canvas>();
            if (existingCanvas != null)
            {
                return existingCanvas;
            }

            var canvasGo = new GameObject("Canvas");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            canvasGo.AddComponent<CanvasScaler>();
            canvasGo.AddComponent<GraphicRaycaster>();

            Undo.RegisterCreatedObjectUndo(canvasGo, "Create Canvas");

            // EventSystem
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                var eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                Undo.RegisterCreatedObjectUndo(eventSystem, "Create EventSystem");
            }

            return canvas;
        }

        private static void CreateOjisanUI()
        {
            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                canvas = CreateCanvas();
            }

            // DialogUI
            var dialogGo = new GameObject("OjisanDialog");
            dialogGo.transform.SetParent(canvas.transform, false);

            var dialogUI = dialogGo.AddComponent<OjisanDialogUI>();

            // Dialog Panel
            var panel = CreatePanel(dialogGo.transform, "DialogPanel");
            var panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0, 0);
            panelRect.anchorMax = new Vector2(1, 0);
            panelRect.pivot = new Vector2(0.5f, 0);
            panelRect.anchoredPosition = new Vector2(0, 20);
            panelRect.sizeDelta = new Vector2(-40, 100);

            // Dialog Text
            var textGo = new GameObject("DialogText");
            textGo.transform.SetParent(panel.transform, false);
            var text = textGo.AddComponent<TextMeshProUGUI>();
            text.text = "おじさんのセリフがここに表示されます";
            text.fontSize = 24;
            text.alignment = TextAlignmentOptions.Center;
            text.color = Color.white;

            var textRect = text.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(20, 10);
            textRect.offsetMax = new Vector2(-20, -10);

            // SerializedObject で参照を設定
            var serializedDialog = new SerializedObject(dialogUI);
            serializedDialog.FindProperty("dialogPanel").objectReferenceValue = panel;
            serializedDialog.FindProperty("dialogText").objectReferenceValue = text;
            serializedDialog.ApplyModifiedProperties();

            Undo.RegisterCreatedObjectUndo(dialogGo, "Create OjisanDialog");
        }

        private static void CreateCommandUI()
        {
            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                canvas = CreateCanvas();
            }

            // Command Button
            var buttonGo = CreateButton(canvas.transform, "CommandButton", "コマンド");
            var buttonRect = buttonGo.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(1, 0);
            buttonRect.anchorMax = new Vector2(1, 0);
            buttonRect.pivot = new Vector2(1, 0);
            buttonRect.anchoredPosition = new Vector2(-20, 130);
            buttonRect.sizeDelta = new Vector2(120, 40);

            var commandButton = buttonGo.AddComponent<CommandButtonUI>();
            var serializedButton = new SerializedObject(commandButton);
            serializedButton.FindProperty("mainButton").objectReferenceValue = buttonGo.GetComponent<Button>();
            serializedButton.FindProperty("mainButtonText").objectReferenceValue = buttonGo.GetComponentInChildren<TextMeshProUGUI>();
            serializedButton.ApplyModifiedProperties();

            // Command Menu
            var menuGo = new GameObject("CommandMenu");
            menuGo.transform.SetParent(canvas.transform, false);
            var commandMenu = menuGo.AddComponent<CommandMenuUI>();

            // Menu Panel
            var menuPanel = CreatePanel(menuGo.transform, "MenuPanel");
            var menuRect = menuPanel.GetComponent<RectTransform>();
            menuRect.anchorMin = new Vector2(1, 0);
            menuRect.anchorMax = new Vector2(1, 0);
            menuRect.pivot = new Vector2(1, 0);
            menuRect.anchoredPosition = new Vector2(-20, 180);
            menuRect.sizeDelta = new Vector2(200, 300);

            // Scroll View for command list
            var scrollGo = new GameObject("CommandList");
            scrollGo.transform.SetParent(menuPanel.transform, false);
            var scrollRect = scrollGo.AddComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0, 0.1f);
            scrollRect.anchorMax = Vector2.one;
            scrollRect.offsetMin = new Vector2(10, 10);
            scrollRect.offsetMax = new Vector2(-10, -10);

            var vlg = scrollGo.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = 5;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;
            vlg.childControlWidth = true;
            vlg.childControlHeight = true;

            // Close Button
            var closeBtn = CreateButton(menuPanel.transform, "CloseButton", "×");
            var closeRect = closeBtn.GetComponent<RectTransform>();
            closeRect.anchorMin = new Vector2(1, 1);
            closeRect.anchorMax = new Vector2(1, 1);
            closeRect.pivot = new Vector2(1, 1);
            closeRect.anchoredPosition = new Vector2(-5, -5);
            closeRect.sizeDelta = new Vector2(30, 30);

            // Command Item Prefab
            var itemPrefab = CreateCommandItemPrefab();

            var serializedMenu = new SerializedObject(commandMenu);
            serializedMenu.FindProperty("menuPanel").objectReferenceValue = menuPanel;
            serializedMenu.FindProperty("commandListParent").objectReferenceValue = scrollGo.transform;
            serializedMenu.FindProperty("commandItemPrefab").objectReferenceValue = itemPrefab;
            serializedMenu.FindProperty("closeButton").objectReferenceValue = closeBtn.GetComponent<Button>();
            serializedMenu.ApplyModifiedProperties();

            Undo.RegisterCreatedObjectUndo(buttonGo, "Create CommandButton");
            Undo.RegisterCreatedObjectUndo(menuGo, "Create CommandMenu");

            // GameManager に参照を設定
            var gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                var serializedGM = new SerializedObject(gameManager);
                serializedGM.FindProperty("apiServer").objectReferenceValue = FindObjectOfType<ApiServer>();
                serializedGM.FindProperty("dialogUI").objectReferenceValue = FindObjectOfType<OjisanDialogUI>();
                serializedGM.FindProperty("commandButton").objectReferenceValue = commandButton;
                serializedGM.FindProperty("commandMenu").objectReferenceValue = commandMenu;
                serializedGM.ApplyModifiedProperties();
            }
        }

        private static GameObject CreatePanel(Transform parent, string name)
        {
            var panel = new GameObject(name);
            panel.transform.SetParent(parent, false);

            var image = panel.AddComponent<Image>();
            image.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

            return panel;
        }

        private static GameObject CreateButton(Transform parent, string name, string label)
        {
            var buttonGo = new GameObject(name);
            buttonGo.transform.SetParent(parent, false);

            var image = buttonGo.AddComponent<Image>();
            image.color = new Color(0.2f, 0.5f, 0.8f, 1f);

            var button = buttonGo.AddComponent<Button>();
            button.targetGraphic = image;

            var textGo = new GameObject("Text");
            textGo.transform.SetParent(buttonGo.transform, false);

            var text = textGo.AddComponent<TextMeshProUGUI>();
            text.text = label;
            text.fontSize = 18;
            text.alignment = TextAlignmentOptions.Center;
            text.color = Color.white;

            var textRect = text.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            return buttonGo;
        }

        private static GameObject CreateCommandItemPrefab()
        {
            var itemGo = new GameObject("CommandItem");

            var image = itemGo.AddComponent<Image>();
            image.color = new Color(0.3f, 0.3f, 0.3f, 1f);

            var button = itemGo.AddComponent<Button>();
            button.targetGraphic = image;

            var layout = itemGo.AddComponent<LayoutElement>();
            layout.minHeight = 40;
            layout.preferredHeight = 40;

            var textGo = new GameObject("Text");
            textGo.transform.SetParent(itemGo.transform, false);

            var text = textGo.AddComponent<TextMeshProUGUI>();
            text.text = "Command";
            text.fontSize = 16;
            text.alignment = TextAlignmentOptions.Center;
            text.color = Color.white;

            var textRect = text.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            // Prefabとして保存
            var prefabPath = "Assets/Prefabs";
            if (!AssetDatabase.IsValidFolder(prefabPath))
            {
                AssetDatabase.CreateFolder("Assets", "Prefabs");
            }

            var prefab = PrefabUtility.SaveAsPrefabAsset(itemGo, $"{prefabPath}/CommandItem.prefab");
            DestroyImmediate(itemGo);

            return prefab;
        }
    }
}
#endif
