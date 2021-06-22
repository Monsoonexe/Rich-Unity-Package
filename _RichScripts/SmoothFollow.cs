using UnityEngine;

namespace UnityStandardAssets.Utility
{
	public class SmoothFollow : RichMonoBehaviour
	{
		// The target we are following
		public Transform target;
		// The distance in the x-z plane to the target
		[SerializeField]
		private float distance = 10.0f;
		// the height we want the camera to be above the target
		[SerializeField]
		private float height = 5.0f;

		[SerializeField]
		private float rotationDamping;
		[SerializeField]
		private float heightDamping;

		// Update is called once per frame
		void LateUpdate()
		{
			// Early out if we don't have a target
			if (!target) return;

			// Calculate the current rotation angles
			var wantedRotationAngle = target.eulerAngles.y;
			var wantedHeight = target.position.y + height;

			var currentRotationAngle = transform.eulerAngles.y;
			var currentHeight = transform.position.y;

            var deltaTime = Time.deltaTime;

			// Damp the rotation around the y-axis
			currentRotationAngle = Mathf.LerpAngle(
                currentRotationAngle, wantedRotationAngle, 
                rotationDamping * deltaTime);

			// Damp the height
			currentHeight = Mathf.Lerp(currentHeight, 
                wantedHeight, heightDamping * deltaTime);

			// Convert the angle into a rotation
			var currentRotation = Quaternion.Euler(0, 
                currentRotationAngle, 0);

			// Set the position of the camera on the x-z plane to:
			// distance meters behind the target
			var targetPos = target.position - currentRotation 
                * Vector3.forward * distance;

            // Set the height of the camera
            transform.position = targetPos.WithY(currentHeight);

			// Always look at the target
			transform.LookAt(target);
		}

        public void SnapUpdate()
        {
            // Early out if we don't have a target
            if (!target) return;

            // Calculate the current rotation angles
            var wantedRotationAngle = target.eulerAngles.y;
            var wantedHeight = target.position.y + height;

            var currentRotationAngle = transform.eulerAngles.y;
            var currentHeight = transform.position.y;

            currentRotationAngle = wantedRotationAngle;

            // Damp the height
            currentHeight = wantedHeight;

            // Convert the angle into a rotation
            var currentRotation = Quaternion.Euler(0,
                currentRotationAngle, 0);

            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target
            var targetPos = target.position - currentRotation
                * Vector3.forward * distance;

            // Set the height of the camera
            transform.position = targetPos.WithY(currentHeight);

            // Always look at the target
            transform.LookAt(target);
        }
	}
}
