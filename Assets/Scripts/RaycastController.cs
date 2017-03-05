using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
		public LayerMask collisionMask;

		protected const float skinWidth = 0.015f;
		private const float distBetweenRays = 0.25f;

		protected int horizontalRayCount;
		protected int verticalRayCount;
		protected float horizontalRaySpacing;
		protected float verticalRaySpacing;

		private BoxCollider2D boxCol;
		protected RaycastOrigins raycastOrigins;

		// Use this for initialization
		public virtual void Awake()
		{
				boxCol = GetComponent<BoxCollider2D>();
		}

		public virtual void Start()
		{
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

				float boundsWidth = bounds.size.x;
				float boundsHeight = bounds.size.y;

				horizontalRayCount = Mathf.RoundToInt(boundsHeight / distBetweenRays);
				verticalRayCount = Mathf.RoundToInt(boundsWidth / distBetweenRays);

				horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
				verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
		}

		Bounds GetShrunkenBounds()
		{
				Bounds bounds = boxCol.bounds;

				bounds.Expand(skinWidth * -2.0f);

				return bounds;
		}

		public struct RaycastOrigins
		{
				public Vector2 topLeft, topRight;
				public Vector2 bottomLeft, bottomRight;
		}
}
