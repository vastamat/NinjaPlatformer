using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
		public LayerMask collisionMask;
		public int horizontalRayCount = 4;
		public int verticalRayCount = 4;

		protected const float skinWidth = 0.015f;

		protected float horizontalRaySpacing;
		protected float verticalRaySpacing;

		private BoxCollider2D capsuleCol;
		protected RaycastOrigins raycastOrigins;

		// Use this for initialization
		public virtual void Start()
		{
				capsuleCol = GetComponent<BoxCollider2D>();
				CalculateRaySpacing();
		}

		public void UpdateRaycastOrigins()
		{
				Bounds bounds = GetShrunkenBounds();

				raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
				raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
				raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
				raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
		}

		public void CalculateRaySpacing()
		{
				Bounds bounds = GetShrunkenBounds();

				horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
				verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

				horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
				verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
		}

		Bounds GetShrunkenBounds()
		{
				Bounds bounds = capsuleCol.bounds;

				bounds.Expand(skinWidth * -2);

				return bounds;
		}

		public struct RaycastOrigins
		{
				public Vector2 topLeft, topRight;
				public Vector2 bottomLeft, bottomRight;
		}
}
