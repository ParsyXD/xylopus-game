using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MyGrabber3D
{
    [CustomEditor(typeof(MyGrabber))]
    public class MyGrabberEditor : Editor
    {
        private MyGrabber myGrabber;
        Texture2D logoTex;
        Texture2D backGroundTex;

        public SerializedProperty grabType, forceGrab, velocityGrab, objectRotationSpeed, grabPos, maintainObjectOffset, returnToParent, grabPosOffset, breakDistance;

        void OnEnable()
        {
            myGrabber = (MyGrabber)target;

            logoTex = (Texture2D)EditorGUIUtility.Load("Assets/MyGrabber/Scripts/Editor/Images/GrabberHeader.png");
            backGroundTex = (Texture2D)EditorGUIUtility.Load("Assets/MyGrabber/Scripts/Editor/Images/GrabberBg.png");

            grabType = serializedObject.FindProperty("grabType");
            forceGrab = serializedObject.FindProperty("forceGrab");
            velocityGrab = serializedObject.FindProperty("velocityGrab");
            objectRotationSpeed = serializedObject.FindProperty("objectRotationSpeed");
            grabPos = serializedObject.FindProperty("grabPos");
            maintainObjectOffset = serializedObject.FindProperty("maintainObjectOffset");
            returnToParent = serializedObject.FindProperty("returnToParent");
            grabPosOffset = serializedObject.FindProperty("grabPosOffset");
            breakDistance = serializedObject.FindProperty("breakDistance");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUI.color = Color.white;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            DrawBackground();
            DrawLogo();
            DrawProperties();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawProperties()
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(grabType, new GUIContent("Grab Type"));

            switch (myGrabber.grabType)
            {
                case GrabType.Force:
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(forceGrab, new GUIContent("Force Grab"));
                    EditorGUILayout.Space();
                    break;
                case GrabType.Velocity:
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(velocityGrab, new GUIContent("Velocity Grab"));
                    EditorGUILayout.Space();
                    break;
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(objectRotationSpeed, new GUIContent("Object Rotation Speed"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(grabPos, new GUIContent("Grab Pos"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(maintainObjectOffset, new GUIContent("Maintain Object Offset"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(returnToParent, new GUIContent("Return to Parent"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(grabPosOffset, new GUIContent("Grab Pos Offset"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(breakDistance, new GUIContent("Break Distance"));
            EditorGUILayout.Space();
        }

        private void DrawLogo()
        {
            Rect rect = GUILayoutUtility.GetLastRect();
            GUI.DrawTexture(new Rect(0, rect.yMin + 20, EditorGUIUtility.currentViewWidth, 130), logoTex, ScaleMode.ScaleToFit);
            GUILayout.Space(200);
        }

        private void DrawBackground()
        {
            Rect rect = GUILayoutUtility.GetLastRect();
            GUI.color = GUI.color = Color.white;
            GUI.DrawTexture(new Rect(0, rect.yMin, EditorGUIUtility.currentViewWidth, 500), backGroundTex);

        }
    }
}