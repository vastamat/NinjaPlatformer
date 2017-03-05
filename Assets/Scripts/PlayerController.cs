using UnityEngine;

public class PlayerController : RaycastController
{
		/** The maximum angle of slope the player can climb */
		public float maxSlopeAngle = 80.0f;

		/** Collision info of collisions with the player per-frame */
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

				if (_deltaMove.y < 0.0f)
				{
						DescendSlope(ref _deltaMove);
				}

				if (_deltaMove.x != 0.0f)
				{
						collisions.faceDir = (int)Mathf.Sign(_deltaMove.x);
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

				for (int i = 0; i < horizontalRayCount; ++i)
				{
						Vector2 rayOrigin = (directionX == -1) ? rayOrigin = raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
						rayOrigin += Vector2.up * (horizontalRaySpacing * i);

						RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

						Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

						if (hit)
						{
								if (hit.collider.CompareTag("Fatal") || hit.collider.CompareTag("FatalRefreshable"))
								{
										collisions.fatalCollision = true;
								}

								if (hit.distance == 0.0f)
								{
										continue;
								}

								float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

								if (i == 0 && slopeAngle <= maxSlopeAngle)
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
										ClimbSlope(ref _deltaMove, slopeAngle, hit.normal);
										_deltaMove.x += distanceToSlipeStart * directionX;
								}

								if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle)
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

				for (int i = 0; i < verticalRayCount; ++i)
				{
						Vector2 rayOrigin = (directionY == -1) ? rayOrigin = raycastOrigins.bottomLeft : raycastOrigins.topLeft;
						rayOrigin += Vector2.right * (verticalRaySpacing * i + _deltaMove.x);

						RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

						Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

						if (hit)
						{
								if (hit.collider.CompareTag("Fatal") || hit.collider.CompareTag("FatalRefreshable"))
								{
										collisions.fatalCollision = true;
								}

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
										collisions.slopeNormal = hit.normal;
								}
						}
				}
		}

		void ClimbSlope(ref Vector2 _deltaMove, float _slopeAngle, Vector2 _slopeNormal)
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
						collisions.slopeNormal = _slopeNormal;
				}
		}

		void DescendSlope(ref Vector2 _deltaMove)
		{
				RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(raycastOrigins.bottomLeft,
						Vector2.down, Mathf.Abs(_deltaMove.y) + skinWidth, collisionMask);

				RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(raycastOrigins.bottomRight,
						Vector2.down, Mathf.Abs(_deltaMove.y) + skinWidth, collisionMask);

				if (maxSlopeHitLeft ^ maxSlopeHitRight)
				{
						SlideDownMaxSlope(maxSlopeHitLeft, ref _deltaMove);
						SlideDownMaxSlope(maxSlopeHitRight, ref _deltaMove);
				}

				if (!collisions.slidingDownMaxSlope)
				{
						float directionX = Mathf.Sign(_deltaMove.x);
						Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
						RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);

						if (hit)
						{
								float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

								if (slopeAngle != 0.0f && slopeAngle <= maxSlopeAngle)
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
														collisions.slopeNormal = hit.normal;
												}
										}
								}
						}
				}
		}

		private void SlideDownMaxSlope(RaycastHit2D _hit, ref Vector2 _deltaMove)
		{
				if (_hit)
				{
						float slopeAngle = Vector2.Angle(_hit.normal, Vector2.up);

						if (slopeAngle > maxSlopeAngle)
						{
								_deltaMove.x = Mathf.Sign(_hit.normal.x) * (Mathf.Abs(_deltaMove.y) - _hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

								collisions.slopeAngle = slopeAngle;
								collisions.slidingDownMaxSlope = true;
								collisions.slopeNormal = _hit.normal;
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
				public bool slidingDownMaxSlope;
				public bool fatalCollision;

				public float slopeAngle, slopeAngleOld;

				public int faceDir;

				public Vector2 velocityOld;
				public Vector2 slopeNormal;

				public void Reset()
				{
						above = below = left = right = false;
						climbingSlope = false;
						descendingSlope = false;
						slidingDownMaxSlope = false;
						fatalCollision = false;

						slopeAngleOld = slopeAngle;
						slopeAngle = 0.0f;

						slopeNormal = Vector2.zero;
				}
		}
}
