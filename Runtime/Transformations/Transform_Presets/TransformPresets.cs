using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Shared
{
   [DisallowMultipleComponent]
   [AddComponentMenu("Shared/Transform Presets")]
   public sealed class TransformPresets : MonoBehaviour
   {
   #region Fields
      [SerializeField, Range(0, 10)] private int _selected = 0;
      [SerializeField, ReadOnlyInInspector] private string _selectedName = "";

      [Space(10f)]
      public bool Control = false;
      public Space Space;

      [Space(10f)]
      [SerializeField] private LeanTweenClip DefaultTweenClip;
      
      [Space(10f)]
      public Axis PositionAxis;
      public bool EffectPosition = true;
      public bool EffectRotation = true;
      public bool EffectScale = true;
      public TransformPreset[] Presets;

      private bool _transitioning = false;
   #endregion Fields


   #region Properties
      public int Selected { get => _selected; }
      public bool Transitioning { get => _transitioning; }
   #endregion


      private void OnValidate ()
      {
         SetTargetPreset(_selected);
      }

      public void AddPresets (List<TransformPreset> presets)
      {
         Presets = presets.ToArray();
      }


   #region Set Target Preset
      public void SetTargetPreset (int index, bool overrideSnap = false)
      {
         if (Presets == null) return;
         if (Presets.Length == 0) return;
         
         _selected = index;
         _selected = Math.Clamp(_selected, 0, Presets.Length - 1);

         if (Control) BeginTransition(Presets[_selected], overrideSnap);
      }

      public void SetTargetPreset (string name_, bool overrideSnap = false)
      {
         int i = 0 ;
         foreach (TransformPreset preset in Presets)
         {
            if (preset.Name == name_) {
               SetTargetPreset(i, overrideSnap);
               return;
            }

            i++;
         }

         Debug.LogError("No Transform Preset Could be found with name '" + name_ + "'.", gameObject);
         return;
      }
   #endregion Set Target


   #region Transition
      private void BeginTransition (TransformPreset targetPreset, bool overrideSnap)
      {
         if (!Control) return;

         _selectedName = targetPreset.Name;
         
         bool snap = !Application.isPlaying || overrideSnap;
         _transitioning = !snap;

      #region Position
         Vector3 targetPosition;
         if (Space == Space.World) targetPosition = transform.position;
         else targetPosition = transform.localPosition;

         if (PositionAxis == Axis.X || PositionAxis == Axis.Everything) targetPosition.x = targetPreset.Position.x;
         if (PositionAxis == Axis.Y || PositionAxis == Axis.Everything) targetPosition.y = targetPreset.Position.y;
         if (PositionAxis == Axis.Z || PositionAxis == Axis.Everything) targetPosition.z = targetPreset.Position.z;
      #endregion Position


      #region Tween Clip Override
         LeanTweenClip tweeningClip = new();

         if (targetPreset.OverrideDefaultClip) tweeningClip = targetPreset.TweenClip;
         else tweeningClip = DefaultTweenClip;
      #endregion Tween Clip Override

         if (Space == Space.World) TransitionWorldSpace();
         else TransitionLocalSpace();
         TransitionScale();

         void TransitionWorldSpace ()
         {
            if (EffectPosition) {
               if (snap) {
                  transform.position = targetPosition;
               } else {
                  tweeningClip.Effect
                  (
                     LeanTween.move(gameObject, targetPosition, 1)
                     .setOnComplete(OnTweenFinish)
                  );
               }
            }

            if (EffectRotation) {
               if (snap) {
                  transform.eulerAngles = targetPreset.EulerAngles;
               } else {
                  tweeningClip.Effect
                  (
                     LeanTween.rotate(gameObject, targetPreset.Rotation.eulerAngles, 1f)
                     .setOnComplete(OnTweenFinish)
                  );
               }
            }
         }

         void TransitionLocalSpace ()
         {
            if (EffectPosition) {
               if (snap) {
                  transform.localPosition = targetPosition;
               } else {
                  tweeningClip.Effect
                  (
                     LeanTween.moveLocal(gameObject, targetPosition, 1f)
                     .setOnComplete(OnTweenFinish)
                  );
               }
            }

            if (EffectRotation) {
               if (snap) {
                  transform.localEulerAngles = targetPreset.EulerAngles;
               } else {
                  tweeningClip.Effect
                  (
                     LeanTween.rotateLocal(gameObject, targetPreset.Rotation.eulerAngles, 1f)
                     .setOnComplete(OnTweenFinish)
                  );
               }
            }
         }

         void TransitionScale ()
         {
            if (EffectScale) {
               if (snap) {
                  transform.localScale = targetPreset.Scale;
               } else {
                  tweeningClip.Effect
                  (
                     transform.LeanScale(targetPreset.Scale, 1).setOnComplete(OnTweenFinish)
                  );
               }
            }
         }
      }

      private void OnTweenFinish ()
      {
         _transitioning = false;
      }
   #endregion Transition

   }
}