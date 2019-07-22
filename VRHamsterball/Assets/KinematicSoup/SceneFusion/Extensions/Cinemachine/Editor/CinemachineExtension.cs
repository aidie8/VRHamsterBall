//Uncomment the preprocessor definitions below to sync those properties.
//#define SYNC_SOLO_CAMERA
//#define SYNC_SHOW_IN_GAME_GUIDE
//#define SYNC_SAVE_DURING_PLAY

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;

using UnityEngine;
using UnityEditor;
#if SF_CINEMACHINE
using Cinemachine;
#endif


using KS.SceneFusion.API;
using KS.Reactor;

namespace KS.SceneFusion.Extensions.Editor
{
    /**
     * Syncs Cinemachine's component pipeline.
     * 
     * NOTE: If you get compilation errors because CinemachineVirtualCamera is not defined, go to
     * Project Settings > Player > Scripting Define Symbols, remove SF_CINEMACHINE and press enter.
     */
    [InitializeOnLoad]
    public class CinemachineExtension
    {
        private const string CHANNEL = "CinemachineExtension";

#if SF_CINEMACHINE
        private static FieldInfo m_componentOwnerField = null;
        private static bool m_oldShowHiddenObjects = false;

        private const string SHOW_IN_GAME_GUIDE_KEY = "CNMCN_Core_ShowInGameGuides";
        private const string PREFIX = "SceneFusion_Cinemachine_";
        private const string SOLO_CAMERA = PREFIX + "SoloCamera";
        private const string SHOW_IN_GAME_GUIDE = PREFIX + "ShowInGameGuide";
        private const string SAVE_DURING_PLAY = PREFIX + "SaveDuringPlay";

#if SYNC_SOLO_CAMERA
        private static ICinemachineCamera m_soloCamera = null;
        private static bool m_needSetSoloCamera = false;
#endif

#if SYNC_SHOW_IN_GAME_GUIDE
        private static bool m_showInGameGuides;
#endif

#if SYNC_SAVE_DURING_PLAY
        private static bool m_saveDuringPlay;
#endif

        /**
         * Initialization
         */
        static CinemachineExtension()
        {
            EditorApplication.update += Update;

            sfUtility.OnConnect += OnConnect;
            sfUtility.OnDisconnect += OnDisconnect;
            sfUtility.OnLoadingComplete += OnLoadingComplete;

            // Register on spawn request handler
            sfObjectUtility.OnSpawn += OnSpawn;

            // Register custom property event handlers
            sfCustomProperties.OnCreateCustomProperty += ApplyCustomProperty;
            sfCustomProperties.OnChangeCustomProperty += ApplyCustomProperty;
            sfCustomProperties.OnChangeCustomPropertyFailed += ApplyCustomProperty;

            m_componentOwnerField = typeof(CinemachineVirtualCamera).GetField("m_ComponentOwner",
                BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /**
         * On connect. Sets CinemachineCore.sShowHiddenObjects to show the hidden component owner game object,
         * so that Scene Fusion can sync the component pipeline.
         */
        private static void OnConnect()
        {
            m_oldShowHiddenObjects = CinemachineCore.sShowHiddenObjects;
            CinemachineCore.sShowHiddenObjects = true;

#if SYNC_SOLO_CAMERA
            m_soloCamera = CinemachineBrain.SoloCamera;
            m_needSetSoloCamera = false;
#endif

#if SYNC_SHOW_IN_GAME_GUIDE
            m_showInGameGuides = EditorPrefs.GetBool(SHOW_IN_GAME_GUIDE_KEY);
#endif

#if SYNC_SAVE_DURING_PLAY
            m_saveDuringPlay = SaveDuringPlay.SaveDuringPlay.Enabled;
#endif

        }

        /**
         * On disconnect. Set CinemachineCore.sShowHiddenObjects back.
         */
        private static void OnDisconnect()
        {
            CinemachineCore.sShowHiddenObjects = m_oldShowHiddenObjects;
        }

        /**
         * On loading complete. Create global properties.
         */
        private static void OnLoadingComplete()
        {
#if SYNC_SOLO_CAMERA
            if (!sfCustomProperties.HasProperty(SOLO_CAMERA))
            {
                GameObject gameObject = null;
                if (m_soloCamera != null)
                {
                    gameObject = m_soloCamera.VirtualCameraGameObject;
                }
                uint id = sfObjectUtility.GetGameObjectId(gameObject);
                sfCustomProperties.CreateCustomProperty(SOLO_CAMERA, BitConverter.GetBytes(id), false);
            }
#endif

#if SYNC_SHOW_IN_GAME_GUIDE
            if (!sfCustomProperties.HasProperty(SHOW_IN_GAME_GUIDE))
            {
                sfCustomProperties.CreateCustomProperty(
                    SHOW_IN_GAME_GUIDE,
                    BitConverter.GetBytes(m_showInGameGuides),
                    false);
            }
#endif

#if SYNC_SAVE_DURING_PLAY
            if (!sfCustomProperties.HasProperty(SAVE_DURING_PLAY))
            {
                sfCustomProperties.CreateCustomProperty(
                    SAVE_DURING_PLAY,
                    BitConverter.GetBytes(m_saveDuringPlay),
                    false);
            }
#endif
        }

        /**
         * Send changes for the component owner game object if the parent virtual camera game object is selected.
         * Send Cinemachine global property changes.
         */
        private static void Update()
        {
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                CinemachineVirtualCamera virtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
                if (gameObject.activeInHierarchy && virtualCamera != null)
                {
                    Transform componentOwner = m_componentOwnerField.GetValue(virtualCamera) as Transform;
                    if (componentOwner != null && !Selection.Contains(componentOwner.gameObject))
                    {
                        sfObjectUtility.SendChanges(componentOwner.gameObject);
                    }
                }
            }

#if SYNC_SOLO_CAMERA
            if (sfCustomProperties.HasProperty(SOLO_CAMERA) && m_soloCamera != CinemachineBrain.SoloCamera)
            {
                m_soloCamera = CinemachineBrain.SoloCamera;
                GameObject gameObject = null;
                if (m_soloCamera != null)
                {
                    gameObject = m_soloCamera.VirtualCameraGameObject;
                }
                uint id = sfObjectUtility.GetGameObjectId(gameObject);
                sfCustomProperties.SetCustomProperty(SOLO_CAMERA, BitConverter.GetBytes(id));
            }
            
            if (m_needSetSoloCamera)
            {
                ApplyCustomProperty(SOLO_CAMERA);
            }
#endif

#if SYNC_SHOW_IN_GAME_GUIDE
            if (sfCustomProperties.HasProperty(SHOW_IN_GAME_GUIDE))
            {
                bool showInGameGuides = EditorPrefs.GetBool(SHOW_IN_GAME_GUIDE_KEY);
                if (m_showInGameGuides != showInGameGuides)
                {
                    m_showInGameGuides = showInGameGuides;
                    sfCustomProperties.SetCustomProperty(SHOW_IN_GAME_GUIDE,
                        BitConverter.GetBytes(m_showInGameGuides));
                }
            }
#endif

#if SYNC_SAVE_DURING_PLAY
            if (sfCustomProperties.HasProperty(SAVE_DURING_PLAY) &&
                m_saveDuringPlay != SaveDuringPlay.SaveDuringPlay.Enabled)
            {
                m_saveDuringPlay = SaveDuringPlay.SaveDuringPlay.Enabled;
                sfCustomProperties.SetCustomProperty(SAVE_DURING_PLAY, BitConverter.GetBytes(m_saveDuringPlay));
            }
#endif
        }

        /**
         * Called when spawning a game object. If it is a child of a virtual camera, remove duplicate component owner
         * game objects and set the virtual camera's component owner to be the given game object.
         * 
         * @param   GameObject gameObject that was spawned.
         */
        private static void OnSpawn(GameObject gameObject)
        {
            if (gameObject == null || gameObject.transform.parent == null)
            {
                return;
            }

            GameObject parent = gameObject.transform.parent.gameObject;
            CinemachineVirtualCamera virtualCamera = parent.GetComponent<CinemachineVirtualCamera>();
            if (virtualCamera != null)
            {
                // Set component owner
                Transform componentOwner = m_componentOwnerField.GetValue(virtualCamera) as Transform;
                if (componentOwner != gameObject.transform)
                {
                    if (m_componentOwnerField != null)
                    {
                        m_componentOwnerField.SetValue(virtualCamera, gameObject.transform);
                    }

                    // Remove duplicates
                    if (componentOwner != null)
                    {
                        GameObject.DestroyImmediate(componentOwner.gameObject);
                    }
                }
            }
        }

        /**
         * Apply a custom property change from server to unity.
         * 
         * @param   string id of custom property
         */
        private static void ApplyCustomProperty(string id)
        {
            if (!id.StartsWith(PREFIX))
            {
                return;
            }

            byte[] data = null;
            if (sfCustomProperties.TryGetCustomProperty(id, ref data) && data != null)
            {
#if SYNC_SOLO_CAMERA
                if (id == SOLO_CAMERA)
                {
                    uint objectId = BitConverter.ToUInt32(data, 0);
                    if (objectId == 0)
                    {
                        m_soloCamera = null;
                        CinemachineBrain.SoloCamera = null;
                        return;
                    }

                    m_needSetSoloCamera = true;
                    GameObject gameObject = sfObjectUtility.GetGameObjectById(objectId);
                    if (gameObject == null)
                    {
                        return;
                    }

                CinemachineVirtualCamera virtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
                    if (virtualCamera == null)
                    {
                        return;
                    }

                    Transform componentOwner = m_componentOwnerField.GetValue(virtualCamera) as Transform;
                    if (componentOwner != null &&
                        sfObjectUtility.GetGameObjectId(componentOwner.gameObject) != 0)
                    {
                        m_needSetSoloCamera = false;
                        m_soloCamera = virtualCamera;
                        CinemachineBrain.SoloCamera = virtualCamera;
                    }
                    return;
                }
#endif

#if SYNC_SHOW_IN_GAME_GUIDE
                if (id == SHOW_IN_GAME_GUIDE)
                {
                    m_showInGameGuides = BitConverter.ToBoolean(data, 0);
                    EditorPrefs.SetBool(SHOW_IN_GAME_GUIDE_KEY, m_showInGameGuides);
                    UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                    return;
                }
#endif

#if SYNC_SAVE_DURING_PLAY
                if (id == SAVE_DURING_PLAY)
                {
                    m_saveDuringPlay = BitConverter.ToBoolean(data, 0);
                    SaveDuringPlay.SaveDuringPlay.Enabled = m_saveDuringPlay;
                    return;
                }
#endif
            }
        }
#else
        /**
         * Initialization
         */
        static CinemachineExtension()
        {
            // Set SF_CINEMACHINE define to compile Cinemachine-dependant code if Cinemachine is detected.
            if (DetectCinemachine())
            {
                sfUtility.SetDefineSymbol("SF_CINEMACHINE");

#if UNITY_2018_3_OR_NEWER
                // Add references to plugin assesmblies
                sfUtility.AddPluginReferences(
                    "Cinemachine",
                    new string[]
                    {
                        "Cinemachine"
                    });
                return;
#endif
            }
#if UNITY_2018_3_OR_NEWER
                // Remove references to plugin assesmblies
                sfUtility.RemovePluginReferences(
                    "Cinemachine",
                    new string[]
                    {
                        "Cinemachine"
                    });
                return;
#endif
        }

        /**
         * Detects if Cinemachine is in the project.
         * 
         * @return  bool true if Cinemachine was detected.
         */
        private static bool DetectCinemachine()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type cinemachineVirtualCamera = assembly.GetType("Cinemachine.CinemachineVirtualCamera");
                if (cinemachineVirtualCamera == null)
                {
                    continue;
                }
                return true;
            }
            return false;
        }
#endif
    }
}