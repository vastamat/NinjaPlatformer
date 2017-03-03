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

		public void Move(Vector2 _deltaMove)
		{
				UpdateRaycastOrigins();
				collisions.Reset();

				collisions.velocityOld = _deltaMove;

				if (_deltaMove.x != 0.0f)
				{
						collisions.faceDir = (int)Mathf.Sign(_deltaMove.x);
				}

				if (_deltaMove.y < 0.0f)
				{
						DescendSlope(ref _deltaMove);
				}

				HorizontalCollisions(ref _deltaMove);

				if (_deltaMove.y != 0.0f)
				{
						VerticalCollisions(ref _deltaMove);
				}

				transform.Translate(_deltaMove);
		}

		void HorizontalCollisions(ref Vector2 _deltaMove)
		{
				float directionX = collisions.faceDir;
				float rayLength = Mathf.Abs(_deltaMove.x) + skinWidth;

				if (Mathf.Abs(_deltaMove.x) < skinWidth)
				{
						rayLength = 2.0f * skinWidth;
				}

				for (int i = 0; i < horizontalRayCount; i++)
				{
						Vector2 rayOrigin = (directionX == -1) ? rayOrigin = raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
						rayOrigin += Vector2.up * (horizontalRaySpacing * i);

						RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

						Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

						if (hit)
						{
								float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

								if (i == 0 && slopeAngle <= maxClimpAngle)
								{
										if (collisions.descendingSlope)
										{
												collisions.descendingSlope = false;
												_deltaMove = collisions.velocityOld;
										}
										float distanceToSlipeStart = 0.0f;
										if (slopeAngle != collisions.slopeAngleOld)
										{
												distanceToSlipeStart = hit.distance - skinWidth;
												_deltaMove.x -= distanceToSlipeStart * directionX;
										}
										ClimbSlope(ref _deltaMove, slopeAngle);
										_deltaMove.x += distanceToSlipeStart * directionX;
								}

								if (!collisions.climbingSlope || slopeAngle > maxClimpAngle)
								{
										_deltaMove.x = (hit.distance - skinWidth) * directionX;
										rayLength = hit.distance;

										if (collisions.climbingSlope)
										{
												_deltaMove.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(_deltaMove.x);
										}

										collisions.left = (directionX == -1);
										collisions.right = (directionX == 1);
								}

						}

				}
		}

		void VerticalCollisions(ref Vector2 _deltaMove)
		{
				float directionY = Mathf.Sign(_deltaMove.y);
				float rayLength = Mathf.Abs(_deltaMove.y) + skinWidth;

				for (int i = 0; i < verticalRayCount; i++)
				{
						Vector2 rayOrigin = (directionY == -1) ? rayOrigin = raycastOrigins.bottomLeft : raycastOrigins.topLeft;
						rayOrigin += Vector2.right * (verticalRaySpacing * i + _deltaMove.x);

						RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

						Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

						if (hit)
						{
								_deltaMove.y = (hit.distance - skinWidth) * directionY;
								rayLength = hit.distance;

								if (collisions.climbingSlope)
								{
										_deltaMove.x = _deltaMove.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(_deltaMove.x);
								}

								collisions.below = directionY == -1;
								collisions.above = directionY == 1;
						}
				}

				if (collisions.climbingSlope)
				{
						float directionX = Mathf.Sign(_deltaMove.x);
						rayLength = Mathf.Abs(_deltaMove.x) + skinWidth;
						Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * _deltaMove.y;
						RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
						if (hit)
						{
								float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
								if (slopeAngle != collisions.slopeAngle)
								{
										//collided with a new slope
										_deltaMove.x = (hit.distance - skinWidth) * directionX;
										collisions.slopeAngle = slopeAngle;
								}
						}
				}
		}

		void ClimbSlope(ref Vector2 _deltaMove, float _slopeAngle)
		{
				float moveDistance = Mathf.Abs(_deltaMove.x);
				float climbVelocityY = Mathf.Sin(_slopeAngle * Mathf.Deg2Rad) * moveDistance;

				if (_deltaMove.y <= climbVelocityY)
				{
						_deltaMove.y = climbVelocityY;
						_deltaMove.x = Mathf.Cos(_slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(_deltaMove.x);
						collisions.below = true;
						collisions.climbingSlope = true;
						collisions.slopeAngle = _slopeAngle;
				}
		}

		void DescendSlope(ref Vector2 _deltaMove)
		{
				float directionX = Mathf.Sign(_deltaMove.x);
				Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);

				if (hit)
				{
						float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

						if (slopeAngle != 0.0f && slopeAngle <= maxDescendAngle)
						{
								if (Mathf.Sign(hit.normal.x) == directionX)
								{
										if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(_deltaMove.x))
										{
												float moveDistance = Mathf.Abs(_deltaMove.x);
												float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
												_deltaMove.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(_deltaMove.x);
												_deltaMove.y -= descendVelocityY;

												collisions.slopeAngle = slopeAngle;
												collisions.descendingSlope = true;
												collisions.below = true;
										}
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
