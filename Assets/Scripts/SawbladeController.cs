using System;
using UnityEngine;

public class SawbladeController : MonoBehaviour
{
		/** The ease amount of the sawblades (how linearly they will go through their waypoints)*/
		[Range(0.0f, 2.0f)]
		public float easeAmount = 0.0f;
		/** How much will the sawblades wait after reaching their waypoint */
		public float waitTime = 1.0f;
		/** Movement speed of the saws */
		public float speed = 5.0f;
		/** flag whether the saws are cyclic or not (if upon reaching the last waypoint they go to the first one or just backwards) */
		public bool cyclic = true;
		/** The waypoints in local space */
		public Vector2[] localWaypoints;

		/** The waypoints in global space */
		private Vector2[] globalWaypoints;
		/** The index of the current waypoint the saw is at */
		private int fromWaypointIndex;
		/** The precent of how much distance the saw has traveled from its current waypoint to the next */
		private float percentBetweenWayoints;
		/** The time to make the next move (start going to the next waypoint) */
		private float nextMoveTime;
		// Use this for initialization
		void Start()
		{
				globalWaypoints = new Vector2[localWaypoints.Length];

				for (int i = 0; i < localWaypoints.Length; i++)
				{
						//calculate the global positions of the waypoints
						globalWaypoints[i] = localWaypoints[i] + new Vector2(transform.position.x, transform.position.y);
				}
		}

		// Update is called once per frame
		void Update()
		{
				if (globalWaypoints.Length != 0)
				{
						//calculate the velocity of the saw
						Vector2 velocity = CalculateMovement();

						//temporality store it's current orientation
						Quaternion rotation = transform.rotation;

						//set the orientation to identity for correct translation
						transform.rotation = Quaternion.identity;
						//translate the saw
						transform.Translate(velocity);
						//set back its orientation
						transform.rotation = rotation;
				}
		}

		Vector2 CalculateMovement()
		{
				if (Time.time < nextMoveTime)
				{
						return Vector2.zero;
				}

				//get the current waypoint
				fromWaypointIndex %= globalWaypoints.Length;
				//get the next waypoint
				int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;

				float distanceBetweenWaypoints = Vector2.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
				percentBetweenWayoints += Time.deltaTime * speed / distanceBetweenWaypoints;
				percentBetweenWayoints = Mathf.Clamp01(percentBetweenWayoints);
				float easedPercentBetweenWaypoints = Ease(percentBetweenWayoints);

				Vector2 newPos = Vector2.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

				if (percentBetweenWayoints >= 1.0f)
				{
						percentBetweenWayoints = 0.0f;
						fromWaypointIndex++;

						if (!cyclic)
						{
								if (fromWaypointIndex >= globalWaypoints.Length - 1)
								{
										fromWaypointIndex = 0;
										Array.Reverse(globalWaypoints);
								}
						}
						nextMoveTime = Time.time + waitTime;
				}

				return newPos - new Vector2(transform.position.x, transform.position.y);
		}

		float Ease(float _percentBetweenWayoints)
		{
				float eased = easeAmount + 1;

				return Mathf.Pow(_percentBetweenWayoints, eased) /
						(Mathf.Pow(_percentBetweenWayoints, eased) + Mathf.Pow(1 - _percentBetweenWayoints, eased));
		}

		void OnDrawGizmos()
		{
				if (localWaypoints != null)
				{
						Gizmos.color = Color.red;
						float size = 0.3f;

						for (int i = 0; i < localWaypoints.Length; i++)
						{
								Vector2 globalWaypointsPos = (Application.isPlaying) ? globalWaypoints[i] :
										localWaypoints[i] + new Vector2(transform.position.x, transform.position.y);

								Gizmos.DrawLine(globalWaypointsPos - Vector2.up * size, globalWaypointsPos + Vector2.up * size);
								Gizmos.DrawLine(globalWaypointsPos - Vector2.left * size, globalWaypointsPos + Vector2.left * size);
						}
				}
		}
}
