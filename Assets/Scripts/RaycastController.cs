using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
		/** Layer mask for collision detection using raycasts (will only collide with objects of set layer) */
		public LayerMask collisionMask;

		/** skinWidth to specify a little inwards offset from the bounds for raycasts */
		protected const float skinWidth = 0.015f;
		/** the desired distance between raycasts */
		private const float distBetweenRays = 0.25f;

		/** number of horizontal rays casted */
		protected int horizontalRayCount;
		/** number of vertical rays casted */
		protected int verticalRayCount;
		/** distance between horizontal rays */
		protected float horizontalRaySpacing;
		/** distance between vertical rays */
		protected float verticalRaySpacing;

		/** reference to the box collider */
		private BoxCollider2D boxCol;
		/** origin positions for the raycasts */
		protected RaycastOrigins raycastOrigins;

		// Use this for initialization
		public virtual void Awake()
		{
				//get the reference to the box collider
				boxCol = GetComponent<BoxCollider2D>();
		}

		public virtual void Start()
		{
				//calculate the ray spacing
				CalculateRaySpacing();
		}

		public void UpdateRaycastOrigins()
		{
				//get the inwards scaled bounds
				Bounds bounds = GetShrunkenBounds();

				//set the raycast origins
				raycastOrigins.bottomLeft  = new Vector2(bounds.min.x, bounds.min.y);
				raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
				raycastOrigins.topLeft				 = new Vector2(bounds.min.x, bounds.max.y);
				raycastOrigins.topRight			 = new Vector2(bounds.max.x, bounds.max.y);
		}

		public void CalculateRaySpacing()
		{
				//get the inwards scaled bounds
				Bounds bounds = GetShrunkenBounds();

				//get the width and height of the bounds
				float boundsWidth = bounds.size.x;
				float boundsHeight = bounds.size.y;

				//calculate the cound of the raycasts required to fill the bounds with the set spacing
				horizontalRayCount = Mathf.RoundToInt(boundsHeight / distBetweenRays);
				verticalRayCount = Mathf.RoundToInt(boundsWidth / distBetweenRays);

				//calculate the spacing between the rays
				horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
				verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
		}

		Bounds GetShrunkenBounds()
		{
				//get the current collider bounds
				Bounds bounds = boxCol.bounds;

				//expand them inwards
				bounds.Expand(skinWidth * -2.0f);

				//return the result
				return bounds;
		}

		public struct RaycastOrigins
		{
				//origin positions for the raycasts
				public Vector2 topLeft, topRight;
				public Vector2 bottomLeft, bottomRight;
		}
}
