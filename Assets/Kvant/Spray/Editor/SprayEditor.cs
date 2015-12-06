//
// Custom editor class for Spray
//
using UnityEngine;
using UnityEditor;

namespace Kvant
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Spray))]
    public class SprayEditor : Editor
    {
        SerializedProperty _maxParticles;
        SerializedProperty _emitterCenter;
        SerializedProperty _emitterSize;
        SerializedProperty _throttle;

        SerializedProperty _minLife;
        SerializedProperty _maxLife;

        SerializedProperty _initialVelocity;
        SerializedProperty _directionSpread;
        SerializedProperty _speedRandomness;

        SerializedProperty _acceleration;
        SerializedProperty _drag;

        SerializedProperty _spin;
        SerializedProperty _speedToSpin;
        SerializedProperty _spinRandomness;

        SerializedProperty _noiseAmplitude;
        SerializedProperty _noiseFrequency;
        SerializedProperty _noiseMotion;

        SerializedProperty _shapes;
        SerializedProperty _minScale;
        SerializedProperty _maxScale;
        SerializedProperty _material;
        SerializedProperty _castShadows;
        SerializedProperty _receiveShadows;

        SerializedProperty _randomSeed;
        SerializedProperty _debug;

        static GUIContent _textCenter    = new GUIContent("Center");
        static GUIContent _textSize      = new GUIContent("Size");
        static GUIContent _textLife      = new GUIContent("Life");
        static GUIContent _textMotion    = new GUIContent("Motion");
        static GUIContent _textAmplitude = new GUIContent("Amplitude");
        static GUIContent _textFrequency = new GUIContent("Frequency");
        static GUIContent _textScale     = new GUIContent("Scale");

        void OnEnable()
        {
            _maxParticles  = serializedObject.FindProperty("_maxParticles");
            _emitterCenter = serializedObject.FindProperty("_emitterCenter");
            _emitterSize   = serializedObject.FindProperty("_emitterSize");
            _throttle      = serializedObject.FindProperty("_throttle");

            _minLife = serializedObject.FindProperty("_minLife");
            _maxLife = serializedObject.FindProperty("_maxLife");

            _initialVelocity = serializedObject.FindProperty("_initialVelocity");
            _directionSpread = serializedObject.FindProperty("_directionSpread");
            _speedRandomness = serializedObject.FindProperty("_speedRandomness");

            _acceleration = serializedObject.FindProperty("_acceleration");
            _drag         = serializedObject.FindProperty("_drag");

            _spin           = serializedObject.FindProperty("_spin");
            _speedToSpin    = serializedObject.FindProperty("_speedToSpin");
            _spinRandomness = serializedObject.FindProperty("_spinRandomness");

            _noiseAmplitude = serializedObject.FindProperty("_noiseAmplitude");
            _noiseFrequency = serializedObject.FindProperty("_noiseFrequency");
            _noiseMotion    = serializedObject.FindProperty("_noiseMotion");

            _shapes         = serializedObject.FindProperty("_shapes");
            _minScale       = serializedObject.FindProperty("_minScale");
            _maxScale       = serializedObject.FindProperty("_maxScale");
            _material       = serializedObject.FindProperty("_material");
            _castShadows    = serializedObject.FindProperty("_castShadows");
            _receiveShadows = serializedObject.FindProperty("_receiveShadows");

            _randomSeed = serializedObject.FindProperty("_randomSeed");
            _debug      = serializedObject.FindProperty("_debug");
        }

        public override void OnInspectorGUI()
        {
            var targetSpray = target as Spray;

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_maxParticles);
            if (!_maxParticles.hasMultipleDifferentValues) {
                var note = "Allocated: " + targetSpray.maxParticles;
                EditorGUILayout.LabelField(" ", note, EditorStyles.miniLabel);
            }

            if (EditorGUI.EndChangeCheck())
                targetSpray.NotifyConfigChange();

            EditorGUILayout.LabelField("Emitter", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_emitterCenter, _textCenter);
            EditorGUILayout.PropertyField(_emitterSize, _textSize);
            EditorGUILayout.PropertyField(_throttle);

            EditorGUILayout.Space();

            MinMaxSlider(_textLife, _minLife, _maxLife, 0.1f, 5.0f);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Velocity", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_initialVelocity);
            EditorGUILayout.PropertyField(_directionSpread);
            EditorGUILayout.PropertyField(_speedRandomness);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_acceleration);
            EditorGUILayout.PropertyField(_drag);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Rotation", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_spin);
            EditorGUILayout.PropertyField(_speedToSpin);
            EditorGUILayout.PropertyField(_spinRandomness);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Turbulent Noise", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_noiseAmplitude, _textAmplitude);
            EditorGUILayout.PropertyField(_noiseFrequency, _textFrequency);
            EditorGUILayout.PropertyField(_noiseMotion, _textMotion);

            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_shapes, true);

            if (EditorGUI.EndChangeCheck())
                targetSpray.NotifyConfigChange();

            MinMaxSlider(_textScale, _minScale, _maxScale, 0.01f, 2.0f);

            EditorGUILayout.PropertyField(_material);
            EditorGUILayout.PropertyField(_castShadows);
            EditorGUILayout.PropertyField(_receiveShadows);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_randomSeed);
            EditorGUILayout.PropertyField(_debug);

            serializedObject.ApplyModifiedProperties();
        }

        void MinMaxSlider(GUIContent label, SerializedProperty propMin, SerializedProperty propMax, float minLimit, float maxLimit)
        {
            var min = propMin.floatValue;
            var max = propMax.floatValue;

            EditorGUI.BeginChangeCheck();

            // Min-max slider.
            EditorGUILayout.MinMaxSlider(label, ref min, ref max, minLimit, maxLimit);

            var prevIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Float value boxes.
            var rect = EditorGUILayout.GetControlRect();
            rect.x += EditorGUIUtility.labelWidth;
            rect.width = (rect.width - EditorGUIUtility.labelWidth) / 2 - 2;

            if (EditorGUIUtility.wideMode)
            {
                EditorGUIUtility.labelWidth = 28;
                min = Mathf.Clamp(EditorGUI.FloatField(rect, "min", min), minLimit, max);
                rect.x += rect.width + 4;
                max = Mathf.Clamp(EditorGUI.FloatField(rect, "max", max), min, maxLimit);
                EditorGUIUtility.labelWidth = 0;
            }
            else
            {
                min = Mathf.Clamp(EditorGUI.FloatField(rect, min), minLimit, max);
                rect.x += rect.width + 4;
                max = Mathf.Clamp(EditorGUI.FloatField(rect, max), min, maxLimit);
            }

            EditorGUI.indentLevel = prevIndent;

            if (EditorGUI.EndChangeCheck()) {
                propMin.floatValue = min;
                propMax.floatValue = max;
            }
        }
    }
}
