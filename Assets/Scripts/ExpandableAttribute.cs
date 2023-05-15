using System;
using UnityEditor;
using UnityEngine;

namespace Game.Utility
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ExpandableAttribute : PropertyAttribute
    {

    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ExpandableAttribute))]
    public class ExpandablePropertyDrawer : PropertyDrawer
    {
        private bool _isValid;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            _isValid = false;
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                return EditorGUI.GetPropertyHeight(property, true);
            }

            if (property.objectReferenceValue == null)
            {
                return base.GetPropertyHeight(property, label);
            }

            var propertyType = property.objectReferenceValue.GetType();
            if (!typeof(ScriptableObject).IsAssignableFrom(propertyType) )
            {
                return base.GetPropertyHeight(property, label);
            }

            _isValid = true;

            if (!property.isExpanded)
            {
                return base.GetPropertyHeight(property, label);
            }

            float totalHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            using (var so = new SerializedObject(property.objectReferenceValue))
            {
                using (var iterator = so.GetIterator())
                {
                    if (iterator.NextVisible(true))
                    {
                        do
                        {
                            totalHeight += EditorGUI.GetPropertyHeight(iterator, true);
                            totalHeight += EditorGUIUtility.standardVerticalSpacing;
                        } while (iterator.NextVisible(false));
                    }
                    
                }
            }
            return totalHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (!_isValid)
            {
                EditorGUI.PropertyField(position, property, label, true);
                EditorGUI.EndProperty();
                return;
            }

            var propertyRect = position.CutLine();

            var foldoutRect = propertyRect.PercentFixed(EditorGUIUtility.labelWidth, 0f);

            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, toggleOnLabelClick: true);

            EditorGUI.PropertyField(propertyRect, property, label, false);

            if (property.isExpanded)
            {
                Rect boxRect = new Rect()
                {
                    x = 0.0f,
                    y = position.y,
                    width = position.width * 2.0f,
                    height = position.height
                };

                GUI.Box(boxRect, GUIContent.none);

                using (new EditorGUI.IndentLevelScope())
                {
                    using (var so = new SerializedObject(property.objectReferenceValue))
                    {
                        so.Update();

                        using (var iterator = so.GetIterator())
                        {
                            if (iterator.NextVisible(true))
                            {
                                do
                                {
                                    float childHeight = EditorGUI.GetPropertyHeight(iterator, true);
                                    using (new EditorGUI.DisabledScope(iterator.name.Equals("m_Script", System.StringComparison.Ordinal)))
                                    {
                                        EditorGUI.PropertyField(
                                            position.CutHeight(childHeight, EditorGUIUtility.standardVerticalSpacing),
                                            iterator,
                                            true
                                        );
                                    }
                                } while (iterator.NextVisible(false));
                            }
                        }

                        so.ApplyModifiedProperties();
                    }
                }
            }


            EditorGUI.EndProperty();
        }
    }
#endif
}
