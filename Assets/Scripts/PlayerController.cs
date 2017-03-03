using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
		public LayerMask collisionMask;
		public int horizontalRayCount = 4;
		public int verticalRayCount = 4;

		private float horizontalRaySpacing;
		private float verticalRaySpacing;

		private const float skinWidth = 0.015f;
		private BoxCollider2D capsuleCol;
		private RaycastOrigins raycastOrigins;
		private CollisionInfo collisions;

		void Start()
		{
				capsuleCol = GetComponent<BoxCollider2D>();
				CalculateRaySpacing();
		}

		public void Move(Vector2 _velocity)
		{
				UpdateRaycastOrigins();
				collisions.Reset();

				if (_velocity.x != 0.0f)
				{
						HorizontalCollisions(ref _velocity);
				}
				if (_velocity.y != 0.0f)
				{
						VerticalCollisions(ref _velocity);
				}

				transform.Translate(_velocity);
		}

		void HorizontalCollisions(ref Vector2 _velocity)
		{
				float directionX = Mathf.Sign(_velocity.x);
				float rayLength = Mathf.Abs(_velocity.x) + skinWidth;

				for (int i = 0; i < horizontalRayCount; i++)
				{
						Vector2 rayOrigin = (directionX == -1) ? rayOrigin = raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
						rayOrigin += Vector2.up * (horizontalRaySpacing * i);

						RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

						Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

						if (hit)
						{
								_velocity.x = (hit.distance - skinWidth) * directionX;
								rayLength = hit.distance;

								collisions.left = (directionX == -1);
								collisions.right = (directionX == 1);

						}
				}
		}

		void VerticalCollisions(ref Vector2 _velocity)
		{
				float directionY = Mathf.Sign(_velocity.y);
				float rayLength = Mathf.Abs(_velocity.y) + skinWidth;

				for (int i = 0; i < verticalRayCount; i++)
				{
						Vector2 rayOrigin = (directionY == -1) ? rayOrigin = raycastOrigins.bottomLeft : raycastOrigins.topLeft;
						rayOrigin += Vector2.right * (verticalRaySpacing * i + _velocity.x);

						RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

						Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

						if (hit)
						{
								_velocity.y = (hit.distance - skinWidth) * directionY;
								rayLength = hit.distance;

								collisions.below = (directionY == -1);
								collisions.above = (directionY == 1);
						}
				}
		}

		void UpdateRaycastOrigins()
		{
				Bounds bounds = GetShrunkenBounds();

				raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
				raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
				raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
				raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

		}

		void CalculateRaySpacing()
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

		public CollisionInfo GetCollisionInfo()
		{
				return collisions;
		}

		struct RaycastOrigins
		{
				public Vector2 topLeft, topRight;
				public Vector2 bottomLeft, bottomRight;
		}

		public struct CollisionInfo
		{
				public bool above, below, left, right;

				public void Reset()
				{
						above = below = left = right = false;
				}
		}
}
