using UnityEngine;
using UXT;
using UXT.Attr;
using Sf = UnityEngine.SerializeField;

namespace Progressor
{
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class ObliqueCamera : MonoBehaviour
    {
        //! EXPERIMENTAL. DOES NOT WORK PROPERLY ...
        //... When camera projection is assigned (even without any changes)
        // the operation throws off other parameters resulting
        // in objects rendered by the camera no longer retaining their own proportions
        // but instead being affected by the camera's aspect.
        // Some other parameters [unknown] need to be adjusted to prevent that

        // ≡ Sf

        [SettingsHeader]
        [Sf] float  ShearX  = 0;
        [Sf] float  ShearY  = 0;

        // ╍ Private

        private Camera  Camera;

        // ObliqueCamera

        [ContextMenu("Apply Oblique Projection")]
        private void ApplyObliqueProjection ()
        {
            var obliqueProjectionMatrix = Camera.projectionMatrix;

            obliqueProjectionMatrix.m02 = ShearX;
            obliqueProjectionMatrix.m12 = ShearY;

            Camera.projectionMatrix = obliqueProjectionMatrix;
        }

        [ContextMenu("Reset Camera Projection")]
        private void ResetCameraProjection ()
        {
            Camera.ResetProjectionMatrix();
        }

        // ↑ MonoBehaviour

        private void Update ()
        {
            // ApplyObliqueProjection();
        }

        private void Awake ()
        {
            Camera = GetComponent<Camera>();

            ApplyObliqueProjection();
        }
    }
}
