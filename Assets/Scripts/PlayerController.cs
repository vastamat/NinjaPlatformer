using UnityEngine;

public class PlayerController : RaycastController
{
		public float maxClimpAngle = 80.0f;
		public float maxDescendAngle = 75.0f;

		private CollisionInfo collisions;

		public override void Start()
		{
				base.Start();
				collisions.faceDir = 1;
		}

		public void Move(Vector2 _velocity)
		{
				UpdateRaycastOrigins();
				collisions.Reset();

				collisions.velocityOld = _velocity;

				if (_velocity.x != 0.0f)
				{
						collisions.faceDir = (int)Mathf.Sign(_velocity.x);
				}

				if (_velocity.y < 0.0f)
				{
						DescendSlope(ref _velocity);
				}

				HorizontalCollisions(ref _velocity);

				if (_velocity.y != 0.0f)
				{
						VerticalCollisions(ref _velocity);
				}

				transform.Translate(_velocity);
		}

		void HorizontalCollisions(ref Vector2 _velocity)
		{
				float directionX = collisions.faceDir;
				float rayLength = Mathf.Abs(_velocity.x) + skinWidth;

				if (Mathf.Abs(_velocity.x) < skinWidth)
				{
						rayLength = 2.0f * skinWidth;
				}

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

		public CollisionInfo GetCollisionInfo()
		{
				return collisions;
		}

		public struct CollisionInfo
		{
				public bool above, below, left, right;
				public bool climbingSlope;
				public bool descendingSlope;

				public float slopeAngle, slopeAngleOld;

				public int faceDir;

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
