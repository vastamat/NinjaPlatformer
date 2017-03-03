using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
		public LayerMask collisionMask;
		public int horizontalRayCount = 4;
		public int verticalRayCount = 4;

		public float maxClimpAngle = 80.0f;
		public float maxDescendAngle = 75.0f;

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

				collisions.velocityOld = _velocity;

				if (_velocity.y < 0.0f)
				{
						DescendSlope(ref _velocity);
				}
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
								float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

								if (i == 0 && slopeAngle <= maxClimpAngle)
								{
										if (collisions.descendingSlope)
										{
												collisions.descendingSlope = false;
												_velocity = collisions.velocityOld;
										}
										float distanceToSlipeStart = 0.0f;
										if (slopeAngle != collisions.slopeAngleOld)
										{
												distanceToSlipeStart = hit.distance - skinWidth;
												_velocity.x -= distanceToSlipeStart * directionX;
										}
										ClimbSlope(ref _velocity, slopeAngle);
										_velocity.x += distanceToSlipeStart * directionX;
								}

								if (!collisions.climbingSlope || slopeAngle > maxClimpAngle)
								{
										_velocity.x = (hit.distance - skinWidth) * directionX;
										rayLength = hit.distance;

										if (collisions.climbingSlope)
										{
												_velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(_velocity.x);
										}

										collisions.left = (directionX == -1);
										collisions.right = (directionX == 1);
								}

						}

				}
		}

		void ClimbSlope(ref Vector2 _velocity, float _slopeAngle)
		{
				float moveDistance = Mathf.Abs(_velocity.x);
				float climbVelocityY = Mathf.Sin(_slopeAngle * Mathf.Deg2Rad) * moveDistance;

				if (_velocity.y <= climbVelocityY)
				{
						_velocity.y = climbVelocityY;
						_velocity.x = Mathf.Cos(_slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(_velocity.x);
						collisions.below = true;
						collisions.climbingSlope = true;
						collisions.slopeAngle = _slopeAngle;
				}
		}

		void DescendSlope(ref Vector2 _velocity)
		{
				float directionX = Mathf.Sign(_velocity.x);
				Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);

				if (hit)
				{
						float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

						if (slopeAngle != 0.0f && slopeAngle <= maxDescendAngle)
						{
								if (Mathf.Sign(hit.normal.x) == directionX)
								{
										if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(_velocity.x))
										{
												float moveDistance = Mathf.Abs(_velocity.x);
												float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
												_velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(_velocity.x);
												_velocity.y -= descendVelocityY;

												collisions.slopeAngle = slopeAngle;
												collisions.descendingSlope = true;
												collisions.below = true;
										}
								}
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

								if (collisions.climbingSlope)
								{
										_velocity.x = _velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(_velocity.x);
								}

								collisions.below = (directionY == -1);
								collisions.above = (directionY == 1);
						}
				}

				if (collisions.climbingSlope)
				{
						float directionX = Mathf.Sign(_velocity.x);
						rayLength = Mathf.Abs(_velocity.x) + skinWidth;
						Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * _velocity.y;
						RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
						if (hit)
						{
								float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
								if (slopeAngle != collisions.slopeAngle)
								{
										//collided with a new slope
										_velocity.x = (hit.distance - skinWidth) * directionX;
										collisions.slopeAngle = slopeAngle;
								}
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
				public bool climbingSlope;
				public bool descendingSlope;

				public float slopeAngle, slopeAngleOld;

				public Vector3 velocityOld;

				public void Reset()
				{
						above = below = left = right = false;
						climbingSlope = false;
						descendingSlope = false;

						slopeAngleOld = slopeAngle;
						slopeAngle = 0.0f;
				}
		}
}
