using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public class MakeEmpty : EditorWindow
{
    static bool @checked;

    [SerializeField]
    static GameObject gameObject;
    [SerializeField]
    static Rect buttonRect;
    [SerializeField]
    static int index = 0;

    [SerializeField]
    const int BUTTONSIZE = 32;
    [SerializeField]
    const int INDEX_INC = 1;


    [MenuItem("AyahaTools/MakeEmpty")]
    static void Check()
    {
        var menuPath = "AyahaTools/MakeEmpty";
        @checked = Menu.GetChecked(menuPath);
        Menu.SetChecked(menuPath, !@checked);
    }

    [InitializeOnLoadMethod]
    static void CreateUI()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnGUI;
    }

    static void OnGUI(int instanceID, Rect selectionRect)
    {
        gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        // Sceneはreturn
        if (gameObject == null)return;

        // ボタンの位置調整
        selectionRect.xMin += selectionRect.width - 80;
        buttonRect = selectionRect;
        buttonRect.width = BUTTONSIZE;


        // ここから描画内容＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
        if (@checked == false)
        {
            // 子の空のオブジェクト生成
            if (GUI.Button(buttonRect, "+"))
            {           
                //Debug.Log("MovePlus");
                GameObject emptyGameObject = new GameObject("GameObject");
                emptyGameObject.transform.SetParent(gameObject.transform);
                emptyGameObject.transform.localPosition = Vector3.zero;
                Undo.RegisterCreatedObjectUndo(emptyGameObject, "Create Empty GameObject (child)");
            }

            buttonRect.x += BUTTONSIZE;

            // オブジェクトの下に空のオブジェクト生成
            if(GUI.Button(buttonRect, "V"))
            {
                //Debug.Log("MoveButton");
                GameObject emptyGameObject = new GameObject("GameObject");
                index = gameObject.transform.GetSiblingIndex();
                emptyGameObject.transform.SetParent(gameObject.transform.parent);
                emptyGameObject.transform.localPosition = Vector3.zero;
                emptyGameObject.transform.SetSiblingIndex(index + INDEX_INC);
                Undo.RegisterCreatedObjectUndo(emptyGameObject, "Create Empty GameObject (Under)");
            }

            buttonRect.x += BUTTONSIZE;

            // 空の親オブジェクトを生成
            if (GUI.Button(buttonRect, "<>"))
            {
                GameObject emptyGameObject = new GameObject("GameObject");
                index = gameObject.transform.GetSiblingIndex();
                emptyGameObject.transform.SetParent(gameObject.transform.parent);
                emptyGameObject.transform.localPosition = Vector3.zero;
                emptyGameObject.transform.SetSiblingIndex(index);

                Undo.RegisterFullObjectHierarchyUndo(gameObject, "full object hierarchy change");
                gameObject.transform.SetParent(emptyGameObject.transform);
                Undo.RegisterCreatedObjectUndo(emptyGameObject, "Create Empty Parent");

            }
        }
    }
}
