using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class UnityEditorTools
{
    public static Rect CutHeight(this ref Rect rect, float height, float spacing = 0f)
    {
        var r = new Rect(
            rect.x,
            rect.y,
            rect.width,
            height
        );

        float shift = height + spacing;
        rect.y += shift;
        rect.height -= shift;
        return r;
    }

    public static Rect CutLine(this ref Rect rect, bool skipSpace = true)
    {
        return rect.CutHeight(EditorGUIUtility.singleLineHeight, skipSpace ? EditorGUIUtility.standardVerticalSpacing : 0f);
    }

    public static Rect PercentFixed(this Rect rect, float width, float from)
    {
        from = Mathf.Clamp01(from);
        return new Rect()
        {
            x = rect.position.x + from * rect.width,
            y = rect.position.y,
            width = width,
            height = rect.height
        };
    }

    public static Rect Percent(this Rect rect, float from, float to)
    {
        from = Mathf.Clamp01(from);
        to = Mathf.Clamp01(to);
        return new Rect(
            rect.position.x + from * rect.width,
            rect.y,
            rect.width * (to - from),
            rect.height
        );
    }
}
