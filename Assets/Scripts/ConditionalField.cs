using UnityEditor;
using UnityEngine;

namespace Game.Utility
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ConditionalFieldAttribute : PropertyAttribute
    {
        public readonly string fieldName;
        public readonly bool inverse;

        public ConditionalFieldAttribute(string fieldName, bool inverse = false)
        {
            this.fieldName = fieldName;
            this.inverse = inverse;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
    public class ConditionalFieldAttributeDrawer : PropertyDrawer
    {
        private bool _isVisible = true;
        private ConditionalFieldAttribute _attribute;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (_attribute == null)
            {
                _attribute = this.attribute as ConditionalFieldAttribute;
            }

            if (_attribute == null)
            {
                return 0f;
            }

            var conditionalProperty = this.FindRelativeProperty(property, _attribute.fieldName);

            _isVisible = conditionalProperty == null
                || conditionalProperty.propertyType != SerializedPropertyType.Boolean
                || conditionalProperty.boolValue != _attribute.inverse;

            if (!_isVisible)
            {
                return 0f;
            }

            return EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!_isVisible)
            {
                return;
            }

            EditorGUI.PropertyField(position, property, label, true);
        }

        private SerializedProperty FindRelativeProperty(SerializedProperty property, string propertyName)
        {
            if (property.depth == 0)
            {
                return property.serializedObject.FindProperty(propertyName);
            }

            var path = property.propertyPath.Split('.');
            var target = property.serializedObject.FindProperty(path[0]);
            for (int i = 1, c = path.Length - 1; i < c && target != null; ++i)
            {
                target = target.FindPropertyRelative(path[i]);
            }
            return target != null ? target.FindPropertyRelative(propertyName) : null;
        }
    }
#endif
}

