using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float MoveSpeed => _moveSpeed;

    [SerializeField]
    private float _moveSpeed = 1f;
    [SerializeField]
    private Vector3 _moveDirection = Vector3.zero;

    private int mWayPointCount;
    private Transform[] mWayPoints;
    private int mCurrentIndex = 0;

    public void SetWayPoints(Transform[] wayPoints)
    {
        mWayPointCount = wayPoints.Length;
        // mWayPoints = new Transform[mWayPointCount];
        mWayPoints = wayPoints;
        
        transform.position = wayPoints[mCurrentIndex].position;
    }

    public void StartMove()
    {
        StartCoroutine(eStartMove());
    }

    public void SetDirection(Vector3 direction)
    {
        _moveDirection = direction;
    }

    private void Update()
    {
        transform.position += _moveDirection * _moveSpeed * Time.deltaTime;
    }

    private IEnumerator eStartMove()
    {
        nextWayPoint();

        while(true)
        {
            Transform goalWayPoint = mWayPoints[mCurrentIndex];

            if (Vector3.Distance(transform.position, goalWayPoint.position) < .02f * _moveSpeed)
            {
                nextWayPoint();
            }

            yield return null;
        }
    }

    private bool nextWayPoint()
    {
        Transform lastWayPoint = mWayPoints[mCurrentIndex];
        transform.position = lastWayPoint.position;

        mCurrentIndex = ++mCurrentIndex % mWayPointCount;
        Transform goalWayPoint = mWayPoints[mCurrentIndex];
        Vector3 direction = (goalWayPoint.position - lastWayPoint.position).normalized;
        SetDirection(direction);

        return true;
    }
}
