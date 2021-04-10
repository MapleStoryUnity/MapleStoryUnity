#if (UNITY_EDITOR)
/**
 * $File: JCSUnity_EditorWindow.cs $
 * $Date: 2017-01-22 04:42:16 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace JCSUnity
{
    /// <summary>
    /// Main Unity Engine's Editor Window for JCSUnity.
    /// </summary>
    public class JCSUnity_EditorWindow
        : EditorWindow
    {
        /* Variables*/

        public static JCSUnity_EditorWindow instance = null;

        public int GAME_PAD_COUNT = 0;  // How many gampad in this game?

        public string PROJECT_NAME = "";

        public const string PROJECT_NAME_LASTING = "_Assets";

        public string[] ProjectSubFolders = {
            "Animations",
            "Editors",
            "Materials",
            "Models",
            "Movies",
            "Scenes",
            "Scripts",
            "Shaders",
            "Sounds",
            "Sprites",
        };

        public int SelectGamepadType = 0;
        public string[] GamepadPlatform = {
            "Select Platform",

            /* Sony Play Station */
            "PS",
            "PS2",
            "PS3",
            "PS4",

            /* Microsoft XBox */
            "XBox",
            "XBox 360",
            "XBox One",
        };

        private bool mOCSFoldeout = false;      // OCS = One click serialize
        private bool mBOFoldout = false;        // BO = Bases Object
        private bool mGUIFoldout = false;
        private bool m2DFoldout = false;
        private bool m3DFoldout = false;
        private bool mARVRFoldout = false;      // ARVR = AR / VR

        private bool mInputFoldout = false;      // Input

        private bool mToolFoldout = false;  // Utitlies

        /* Setter & Getter */

        /* Functions */

        private void OnEnable()
        {
            instance = this;
        }

        private void OnGUI()
        {
            JCSUnity_About.ReadINIFile();

            GUILayout.Label("** Editor Settings **", EditorStyles.boldLabel);

            mOCSFoldeout = EditorGUILayout.Foldout(mOCSFoldeout, "One click serialize");
            if (mOCSFoldeout)
                PartOneClickSerialize();

            mBOFoldout = EditorGUILayout.Foldout(mBOFoldout, "Bases Object");
            if (mBOFoldout)
                PartBaseObject();

            mGUIFoldout = EditorGUILayout.Foldout(mGUIFoldout, "GUI");
            if (mGUIFoldout)
                PartGUI();

            m2DFoldout = EditorGUILayout.Foldout(m2DFoldout, "2D");
            if (m2DFoldout)
                Part2D();

            m3DFoldout = EditorGUILayout.Foldout(m3DFoldout, "3D");
            if (m3DFoldout)
                Part3D();

            mARVRFoldout = EditorGUILayout.Foldout(mARVRFoldout, "AR / VR");
            if (mARVRFoldout)
                PartARVR();

            mInputFoldout = EditorGUILayout.Foldout(mInputFoldout, "Input");
            if (mInputFoldout)
                PartInput();

            mToolFoldout = EditorGUILayout.Foldout(mToolFoldout, "Tool");
            if (mToolFoldout)
                PartTool();
        }

        /// <summary>
        /// Initialize the one click serialize part buttons.
        /// </summary>
        private void PartOneClickSerialize()
        {
            if (GUILayout.Button("Serialize scene to JCSUnity 2D interface"))
                SerializeToJCSUnity2D();

            if (GUILayout.Button("Serialize scene to JCSUnity 3D interface"))
                SerializeToJCSUnity3D();
        }

        /// <summary>
        /// Initialize the  base object part buttons.
        /// </summary>
        private void PartBaseObject()
        {
            if (GUILayout.Button("Create JCSUnity setting"))
                CreateJCSSettings();

            if (GUILayout.Button("Create JCSUnity manager"))
                CreateJCSManagers();

            if (GUILayout.Button("Create JCSUnity canvas"))
                CreateJCSCanvas();

            if (GUILayout.Button("Create BGM Player"))
                CreateJCSBGMPlayer();

            if (GUILayout.Button("Create Debug Tools"))
                CreateDebugTools();
        }

        /// <summary>
        /// Compile the GUI part to Unity's GUI inspector.
        /// </summary>
        private void PartGUI()
        {
            GUILayout.Label("** Cursor **");

            if (GUILayout.Button("Create 2D Cursor"))
                Create2DCurosr();

            if (GUILayout.Button("Create 3D Cursor"))
                Create3DCurosr();


            GUILayout.Label("** Panel **");

            if (GUILayout.Button("Create Base Panel"))
                CreateBasePanel();

            if (GUILayout.Button("Create Dialogue Panel"))
                CreateDialoguePanel();

            if (GUILayout.Button("Create Tween Panel"))
                CreateTweenPanel();

            if (GUILayout.Button("Create Slide Panel 9x9 - 16:9"))
                CreateSlidePanel();


            GUILayout.Label("** Undo/Redo **");

            if (GUILayout.Button("Create Undo Redo System"))
                CreateUndoRedoSystem();
        }

        /// <summary>
        /// Compile the 2D part to GUI inspector.
        /// </summary>
        private void Part2D()
        {
            GUILayout.Label("** Camera **");

            if (GUILayout.Button("Create 2D camera"))
                Create2DCamera();

            GUILayout.Label("** Effects **");

            if (GUILayout.Button("Create mix damage pool"))
                CreateMixDamageTextPool();
        }

        /// <summary>
        /// Compile the 3D part to GUI inspector.
        /// </summary>
        private void Part3D()
        {
            GUILayout.Label("** Camera **");

            if (GUILayout.Button("Create 3D camera"))
                Create3DCamera();
        }

        /// <summary>
        /// Compile the AR/VR part to GUI inspector.
        /// </summary>
        private void PartARVR()
        {
            GUILayout.Label("N/A...");
        }

        /// <summary>
        /// Compile the Input part to Input inspector.
        /// </summary>
        private void PartInput()
        {
            GUILayout.Label("** Game Pad count **");

            instance.SelectGamepadType = EditorGUILayout.Popup("Gamepad Type", instance.SelectGamepadType, GamepadPlatform);

            instance.GAME_PAD_COUNT = (int)EditorGUILayout.Slider(instance.GAME_PAD_COUNT, 0, JCS_InputSettings.MAX_JOYSTICK_COUNT);

            if (GUILayout.Button("Add Input Manager depends on target gamepad type"))
                RefreshInputManager();

            if (GUILayout.Button("Clear Input Manager Settings"))
                ClearInputManager();

            if (GUILayout.Button("Add Default Input Manager Settings"))
                AddDefaultInputManager();
        }

        /// <summary>
        /// Compile the Tool part to Tool inspector.
        /// </summary>
        private void PartTool()
        {
            GUILayout.Label("** Framework /  Project **");

            /* Project Name */
            {
                // Provide default project name.
                if (instance.PROJECT_NAME == "")
                    instance.PROJECT_NAME = GetProjectName();

                instance.PROJECT_NAME = EditorGUILayout.TextField("Project Name: ", instance.PROJECT_NAME);
            }

            /* List of project sub folders. */
            {
                ScriptableObject target = this;
                SerializedObject so = new SerializedObject(target);
                SerializedProperty stringsProperty = so.FindProperty("ProjectSubFolders");

                EditorGUILayout.PropertyField(stringsProperty, true);
                so.ApplyModifiedProperties();
            }


            if (GUILayout.Button("Create project assets folder"))
                CreateProjectAssetsFolder();
        }


        /// <summary>
        /// Main JCSUnity Editor initialize function.
        /// </summary>
        [MenuItem("JCSUnity/JCSUnity Editor", false, 1)]
        private static void JCSUnityEditor()
        {
            JCSUnity_EditorWindow window = (JCSUnity_EditorWindow)GetWindow(typeof(JCSUnity_EditorWindow));
            window.titleContent = new GUIContent(JCSUnity_About.EDITOR_INI["editor_title"]);
            window.Show();
        }

        /// <summary>
        /// Serialize the current scene into JCSUnity 2d style.
        /// </summary>
        [MenuItem("JCSUnity/One Click Serialize/Serialize scene to JCSUnity 2D", false, 2)]
        private static void SerializeToJCSUnity2D()
        {
            // create settings
            CreateJCSSettings();

            // create managers
            CreateJCSManagers();

            Create2DCamera();

            // BGM player
            CreateJCSBGMPlayer();

            // create canvas
            GameObject canvasObj = CreateJCSCanvas();

            const string desc_path = "JCSUnity_Resources/GUI/Describe Panel";
            GameObject desc_obj = JCS_Utility.SpawnGameObject(desc_path);
            desc_obj.name = desc_obj.name.Replace("(Clone)", "");
            desc_obj.transform.SetParent(canvasObj.transform);
            desc_obj.transform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// Serialize the current scene into JCSUnity 3d style.
        /// </summary>
        [MenuItem("JCSUnity/One Click Serialize/Serialize scene to JCSUnity 3D", false, 2)]
        private static void SerializeToJCSUnity3D()
        {
            // create settings
            CreateJCSSettings();

            // create managers
            CreateJCSManagers();

            // create 3d camera
            Create3DCamera();

            // BGM player
            CreateJCSBGMPlayer();

            // create canvas
            CreateJCSCanvas();
        }

        /// <summary>
        /// Create managers for 3d game combine 
        /// with JCSUnity.
        /// </summary>
        [MenuItem("JCSUnity/Bases Object/JCSUnity Manager", false, 10)]
        private static GameObject CreateJCSManagers()
        {
            const string manager_path = "JCSUnity_Resources/JCS_Managers";
            GameObject gameObj = CreateHierarchyObject(manager_path);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create JCS Managers");

            return gameObj;
        }

        /// <summary>
        /// Create settings for 3d game combine 
        /// with JCSUnity.
        /// </summary>
        [MenuItem("JCSUnity/Bases Object/JCSUnity Setting", false, 10)]
        private static GameObject CreateJCSSettings()
        {
            const string setting_path = "JCSUnity_Resources/JCS_Settings";
            GameObject gameObj = CreateHierarchyObject(setting_path);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create JCS Settings");

            return gameObj;
        }

        /// <summary>
        /// Create settings for 3d game combine 
        /// with JCSUnity.
        /// </summary>
        [MenuItem("JCSUnity/Input/Add Input Manager depends on target gamepad type", false, 10)]
        private static void RefreshInputManager()
        {
            JCS_InputController.SetupInputManager();
        }

        [MenuItem("JCSUnity/Input/Clear Input Manager Settings", false, 10)]
        private static void ClearInputManager()
        {
            JCS_InputController.ClearInputManagerSettings();
        }

        [MenuItem("JCSUnity/Input/Add Default Input Manager Settings", false, 10)]
        private static void AddDefaultInputManager()
        {
            JCS_InputController.DefaultInputManagerSettings();
        }

        /// <summary>
        /// Create canvas for JCSUnity.
        /// </summary>
        private static GameObject CreateJCSCanvas()
        {
            const string canvas_path = "JCSUnity_Resources/LevelDesignUI/JCS_Canvas";
            GameObject canvasObj = CreateHierarchyObject(canvas_path);

            Undo.RegisterCreatedObjectUndo(canvasObj, "Create JCS Canvas");


            const string eventSystem_path = "JCSUnity_Resources/LevelDesignUI/EventSystem";
            GameObject evtSystemObj = CreateHierarchyObject(eventSystem_path);

            Undo.RegisterCreatedObjectUndo(evtSystemObj, "Create Event System");

            return canvasObj;
        }

        /// <summary>
        /// BGM player for game.
        /// </summary>
        [MenuItem("JCSUnity/Bases Object/JCS_BGMPlayer", false, 11)]
        private static void CreateJCSBGMPlayer()
        {
            const string player_path = "JCSUnity_Resources/Sound/JCS_BGMPlayer";
            GameObject gameObj = CreateHierarchyObject(player_path);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create BGM Player");
        }

        /// <summary>
        /// Debug tool using in JCSUnity.
        /// </summary>
        [MenuItem("JCSUnity/Bases Object/Debug Tools", false, 12)]
        private static void CreateDebugTools()
        {
            const string tools_path = "JCSUnity_Resources/Tools/JCS_Tools";
            GameObject gameObj = CreateHierarchyObject(tools_path);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create Debug Tools");
        }

        /// <summary>
        /// Create a new project.
        /// </summary>
        [MenuItem("JCSUnity/Tool/Create project assets folder", false, 13)]
        private static void CreateProjectAssetsFolder()
        {
            string parentFolder = "Assets";
            string newFolderName = instance.PROJECT_NAME + PROJECT_NAME_LASTING;

            string assetsPath = Application.dataPath + "/";
            string newProjectPath = assetsPath + newFolderName + "/";

            if (!Directory.Exists(newProjectPath))
                AssetDatabase.CreateFolder(parentFolder, newFolderName);

            foreach (string subFolderName in instance.ProjectSubFolders)
            {
                string newProjectName = parentFolder + "/" + newFolderName;
                string newSubFolderPath = newProjectPath + subFolderName + "/";

                if (!Directory.Exists(newSubFolderPath))
                    AssetDatabase.CreateFolder(newProjectName, subFolderName);
            }
        }

        /// <summary>
        /// Update JCSUnity
        /// </summary>
        [MenuItem("JCSUnity/Check for Update", false, 75)]
        private static void UpdateJCSUnity()
        {
            // TODO(jenchieh): check framework need to update or not?
            bool upToDate = true;
            const string title = "Check for Update - JCSUnity";

            if (upToDate)
            {
                EditorUtility.DisplayDialog(
                    title,
                    "Already up to date.",
                    "Close");
            }
            // TODO(jenchieh): not up to date...
            else
            {
                bool option = EditorUtility.DisplayDialog(
                    title,
                    "",
                    "Close");

                if (option)
                {

                }
                else
                {

                }
            }
        }

        /**
         * GUI
         */

        /// <summary>
        /// Create the cursor game object.
        /// </summary>
        private static GameObject Create2DCurosr()
        {
            const string setting_path = "JCSUnity_Resources/GUI/JCS_2DCursor";
            GameObject gameObj = CreateHierarchyObject(setting_path);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create 3D Cursor");

            gameObj.name = "_2DCursor (Created)";

            return gameObj;
        }

        /// <summary>
        /// Create the cursor game object.
        /// </summary>
        private static GameObject Create3DCurosr()
        {
            const string setting_path = "JCSUnity_Resources/GUI/JCS_3DCursor";
            GameObject gameObj = CreateHierarchyObject(setting_path);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create 3D Cursor");

            gameObj.name = "_3DCursor (Created)";

            return gameObj;
        }

        /// <summary>
        /// Create the clean base gui panel for JCSUnity 
        /// and add in under to the canvas.
        /// 
        /// Need:
        ///     1) JCS_Canvas
        /// in the scene before create base panel.
        /// </summary>
        private static GameObject CreateBasePanel()
        {
            JCS_Canvas jcsCanvas = (JCS_Canvas)FindObjectOfType(typeof(JCS_Canvas));
            if (jcsCanvas == null)
            {
                JCS_Debug.Log("Can't find JCS_Canvas in hierarchy. Plz create canvas before creating new panel.");
                return null;
            }

            const string setting_path = "JCSUnity_Resources/GUI/JCS_BasePanel";
            GameObject basePanel = CreateHierarchyObjectUnderCanvas(setting_path);

            Undo.RegisterCreatedObjectUndo(basePanel, "Create Base Panel");

            basePanel.transform.localScale = Vector3.one;
            basePanel.name = "_BasePanel (Created)";

            return basePanel;
        }

        /// <summary>
        /// Create the clean dialogue panel for JCSUnity.
        /// and add in under to the canvas.
        /// 
        /// Need:
        ///     1) JCS_Canvas
        /// in the scene before create dialogue panel.
        /// </summary>
        private static GameObject CreateDialoguePanel()
        {
            JCS_Canvas jcsCanvas = (JCS_Canvas)FindObjectOfType(typeof(JCS_Canvas));
            if (jcsCanvas == null)
            {
                JCS_Debug.Log("Can't find JCS_Canvas in hierarchy. Plz create canvas before creating new panel.");
                return null;
            }

            const string setting_path = "JCSUnity_Resources/GUI/JCS_DialoguePanel";
            GameObject dialoguePanel = CreateHierarchyObjectUnderCanvas(setting_path);

            Undo.RegisterCreatedObjectUndo(dialoguePanel, "Create Dialogue Panel");

            dialoguePanel.transform.localScale = Vector3.one;
            dialoguePanel.name = "_DialoguePanel (Created)";

            return dialoguePanel;
        }

        /// <summary>
        /// Create 9x9 slide panel.
        /// 
        /// Need:
        ///     1) JCS_Camera
        ///     2) JCS_Canvas
        /// in the scene before create 9 x 9 slide panel.
        /// </summary>
        private static void CreateSlidePanel()
        {
            JCS_Canvas jcsCanvas = (JCS_Canvas)FindObjectOfType(typeof(JCS_Canvas));
            if (jcsCanvas == null)
            {
                JCS_Debug.Log("Can't find JCS_Canvas in hierarchy. Plz create canvas before creating new panel.");
                return;
            }

            // find the camera in the scene.
            JCS_2DCamera cam = (JCS_2DCamera)FindObjectOfType(typeof(JCS_Camera));
            if (cam == null)
            {
                JCS_Debug.Log("Can't find JCS_Canvas in hierarchy. Plz create canvas before creating new panel.");
                return;
            }

            const string settingPath = "JCSUnity_Resources/LevelDesignUI/JCS_SlideScreenPanelHolder";

            // spawn the pane holder.
            JCS_SlideScreenPanelHolder panelHolder9x9 = CreateHierarchyObjectUnderCanvas(settingPath, jcsCanvas).GetComponent<JCS_SlideScreenPanelHolder>();

            // create the array of panel.
            panelHolder9x9.slidePanels = new RectTransform[9];

            int starting_pos_x = -1920;
            int starting_pos_y = 1080;

            const string slidePanelPath = "JCSUnity_Resources/LevelDesignUI/JCS_SlidePanel";

            int index = 0;

            // create all nine panel and assign to the slide panel.
            for (int row = 0; row < 3; ++row)
            {
                for (int column = 0; column < 3; ++column)
                {
                    // get the rect transform from the slide panel object.
                    RectTransform slidePanel = CreateHierarchyObjectUnderCanvas(slidePanelPath, jcsCanvas).GetComponent<RectTransform>();

                    // set the position into 9x9.
                    Vector3 slidePanelNewPos = slidePanel.localPosition;
                    slidePanelNewPos.x = starting_pos_x - (starting_pos_x * column);
                    slidePanelNewPos.y = starting_pos_y - (starting_pos_y * row);
                    slidePanel.localPosition = slidePanelNewPos;

                    // set scale to one.
                    slidePanel.localScale = Vector3.one;

                    Image panelImage = slidePanel.GetComponent<Image>();
                    if (panelImage != null)
                    {
                        panelImage.color = JCS_Random.RandomColor();
                    }

                    // assign to slide panel holder.
                    panelHolder9x9.slidePanels[index] = slidePanel;

                    ++index;
                }
            }

            const string slideScreenCameraPath = "JCSUnity_Resources/Camera/JCS_2DSlideScreenCamera";
            JCS_2DSlideScreenCamera slideScreenCamera = CreateHierarchyObject(slideScreenCameraPath).GetComponent<JCS_2DSlideScreenCamera>();

            Undo.RegisterCreatedObjectUndo(slideScreenCamera, "Create 2D Slide Screen Camera");

            slideScreenCamera.name = "_2DSlideScreenCamera (Created)";

            // set the panel holder.
            slideScreenCamera.PanelHolder = panelHolder9x9;

            slideScreenCamera.SetJCS2DCamera(cam);

            // set to default 2d.
            slideScreenCamera.UnityGUIType = JCS_UnityGUIType.uGUI_2D;
        }

        /// <summary>
        /// Create a tween panel.
        /// </summary>
        private static GameObject CreateTweenPanel()
        {
            JCS_Canvas jcsCanvas = (JCS_Canvas)FindObjectOfType(typeof(JCS_Canvas));
            if (jcsCanvas == null)
            {
                JCS_Debug.Log("Can't find JCS_Canvas in hierarchy. Plz create canvas before creating new panel.");
                return null;
            }

            const string setting_path = "JCSUnity_Resources/GUI/JCS_TweenPanel";
            GameObject tweenPanel = CreateHierarchyObjectUnderCanvas(setting_path);

            Undo.RegisterCreatedObjectUndo(tweenPanel, "Create Tween Panel");

            tweenPanel.transform.localScale = Vector3.one;
            tweenPanel.name = "_TweenPanel (Created)";

            return tweenPanel;
        }

        /// <summary>
        /// Create the undo redo system object.
        /// </summary>
        /// <returns></returns>
        private static GameObject CreateUndoRedoSystem()
        {
            const string setting_path = "JCSUnity_Resources/GUI/JCS_UndoRedoSystem";
            GameObject undoRedoSystem = CreateHierarchyObject(setting_path);

            Undo.RegisterCreatedObjectUndo(undoRedoSystem, "Create Undo Redo System");

            undoRedoSystem.name = "_UndoRedoSystem (Created)";

            return undoRedoSystem;
        }


        /**
         * 2D
         */

        /// <summary>
        /// Create camera for 2d game combine 
        /// with JCSUnity.
        /// </summary>
        private static void Create2DCamera()
        {
            const string camera_path = "JCSUnity_Resources/Camera/JCS_2DCamera";
            GameObject gameObj = CreateHierarchyObject(camera_path);

            // set camera depth to default -10.
            gameObj.transform.position = new Vector3(0, 0, -10);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create JCS 2D Camera");
        }

        /// <summary>
        /// Create a mix damage text pool.
        /// </summary>
        private static void CreateMixDamageTextPool()
        {
            const string setting_path = "JCSUnity_Resources/GUI/DamageText/JCS_MixDamageTextPool";
            GameObject gameObj = CreateHierarchyObject(setting_path);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create Min Damage Text Pool");
        }


        /**
         * 3D
         */

        /// <summary>
        /// Create camera for 3d game combine 
        /// with JCSUnity.
        /// </summary>
        private static void Create3DCamera()
        {
            const string camera_path = "JCSUnity_Resources/Camera/JCS_3DCamera";
            GameObject gameObj = CreateHierarchyObject(camera_path);

            Undo.RegisterCreatedObjectUndo(gameObj, "Create JCS 3D Camera");
        }

        /**
         * Utils
         */

        /// <summary>
        /// Create the Game Object during editing time.
        /// </summary>
        /// <returns></returns>
        private static GameObject CreateHierarchyObject(string settingPath)
        {
            // spawn the game object.
            GameObject hierarchyObj = JCS_Utility.SpawnGameObject(settingPath);

            // take away clone sign.
            hierarchyObj.name = hierarchyObj.name.Replace("(Clone)", "");

            return hierarchyObj;
        }

        /// <summary>
        /// Create the Game Object during the editing time and 
        /// put under JCS_Canvas object.
        /// </summary>
        /// <param name="settingPath"> path to spawn </param>
        /// <returns> object just spawned. </returns>
        private static GameObject CreateHierarchyObjectUnderCanvas(string settingPath)
        {
            // since this will be in the editing time.
            // so we don't worry to much about the performance.
            JCS_Canvas jcsCanvas = (JCS_Canvas)FindObjectOfType(typeof(JCS_Canvas));

            return CreateHierarchyObjectUnderCanvas(settingPath, jcsCanvas);
        }

        /// <summary>
        /// Create the Game Object during the editing time and 
        /// put under JCS_Canvas object.
        /// 
        /// Save O(n) time complexity during editing time.
        /// </summary>
        /// <param name="settingPath"> path to spawn </param>
        /// <param name="jcsCanvas"> canvas to set on. </param>
        /// <returns> object just spawned. </returns>
        private static GameObject CreateHierarchyObjectUnderCanvas(string settingPath, JCS_Canvas jcsCanvas)
        {
            if (jcsCanvas == null)
            {
                JCS_Debug.Log("Can't find JCS_Canvas in hierarchy. Plz create canvas before creating new panel.");
                return null;
            }

            // spawn the object first.
            GameObject hierarchyObj = CreateHierarchyObject(settingPath);

            // set the canvas as parent.
            hierarchyObj.transform.SetParent(jcsCanvas.transform);

            // init position.
            hierarchyObj.transform.localPosition = Vector3.zero;

            return hierarchyObj;
        }

        public string GetProjectName()
        {
            string[] s = Application.dataPath.Split('/');
            string projectName = s[s.Length - 2];
            return projectName;
        }
    }
}

#endif
