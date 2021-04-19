#if UNITY_EDITOR
/**
    Source: http://answers.unity3d.com/questions/444312/having-text-or-notes-in-the-inspector.html
    by Loius
*/
using UnityEngine;
using UnityEditor;

public class CommentAttribute : PropertyAttribute
{
    public readonly string tooltip;
    public readonly string comment;

    public CommentAttribute(string comment, string tooltip)
    {
        this.tooltip = tooltip;
        this.comment = comment;
    }
}

[CustomPropertyDrawer(typeof(CommentAttribute))]
public class CommentDrawer : PropertyDrawer
{
    const int textHeight = 20;

    CommentAttribute commentAttribute { get { return (CommentAttribute)attribute; } }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        return textHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.LabelField(position, new GUIContent(commentAttribute.comment, commentAttribute.tooltip));
    }
}

public class PropDraw : MonoBehaviour
{
    [Comment("This is the description", "And this is the tooltip")]
    public int thisIsADummy;
}

#endif