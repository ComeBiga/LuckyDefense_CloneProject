using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SummonPointManager : MonoBehaviour
{
    public SummonPoint[] SummonPoints => mSummonPoints;
    public SummonPoint SelectedSummonPoint => mSelectedSummonPoint;

    public bool interactable = true;

    [SerializeField]
    private Transform _summonPointParent;
    [SerializeField]
    private Transform _trSummonPointCanvas;
    [SerializeField]
    private Transform _trHeroAttackRange;

    private SummonPoint[] mSummonPoints;

    private SummonPoint mSelectedSummonPoint = null;
    private SummonPoint mHoldingSummonPoint = null;
    private SummonPoint mGoalSummonPoint = null;

    public SummonPoint FindSummonPoint(int heroID, bool withoutFull = true)
    {
        for(int i = 0; i < mSummonPoints.Length; ++i)
        {
            if(mSummonPoints[i].TryGetHero(out Hero summonPointHero))
            {
                if(heroID == summonPointHero.ID)
                {
                    return mSummonPoints[i];
                }
            }
        }

        return null;
    }
    public SummonPoint FindSummonPointWithoutFull(int heroID)
    {
        for(int i = 0; i < mSummonPoints.Length; ++i)
        {
            if(mSummonPoints[i].IsFull)
            {
                continue;
            }

            if(mSummonPoints[i].TryGetHero(out Hero summonPointHero))
            {
                if(heroID == summonPointHero.ID)
                {
                    return mSummonPoints[i];
                }
            }
        }

        return null;
    }

    public SummonPoint FindFullSummonPoint(bool withMythHero = false)
    {
        for (int i = 0; i < mSummonPoints.Length; ++i)
        {
            if (mSummonPoints[i].IsFull)
            {
                if(!withMythHero)
                {
                    mSummonPoints[i].TryGetHero(out Hero hero);

                    if(hero.Grade == Hero.EGrade.Myth)
                    {
                        continue;
                    }
                }

                return mSummonPoints[i];
            }
        }

        return null;
    }

    public SummonPoint GetEmptySummonPoint()
    {
        for (int i = 0; i < mSummonPoints.Length; ++i)
        {
            if (!mSummonPoints[i].TryGetHero(out Hero summonPointHero))
            {
                return mSummonPoints[i];
            }
        }

        return null;
    }

    public void UnSelect()
    {
        mSelectedSummonPoint.UnSelect();
        mSelectedSummonPoint = null;

        _trSummonPointCanvas.gameObject.SetActive(false);
        hideHeroAttackRange();
    }

    private void Start()
    {
        mSummonPoints = findSummonPoints();
    }

    private void Update()
    {
        if(!interactable)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // UI 위라면 Raycast 안 함
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (tryRaycastSummonPoint(out SummonPoint hitSummonPoint))
            {
                if(mSelectedSummonPoint != null && hitSummonPoint != mSelectedSummonPoint)
                {
                    UnSelect();
                }

                if (!hitSummonPoint.IsEmpty)
                {
                    mHoldingSummonPoint = hitSummonPoint;

                    mHoldingSummonPoint.UnSelect();
                    mHoldingSummonPoint.Hold();
                }
            }
            else
            {
                if (mSelectedSummonPoint != null)
                {
                    UnSelect();
                }
            }
        }

        if(Input.GetMouseButton(0))
        {
            if (mHoldingSummonPoint != null)
            {
                if (tryRaycastSummonPoint(out SummonPoint hitSummonPoint))
                {
                    if (hitSummonPoint == mHoldingSummonPoint)
                    {

                    }
                    else if (hitSummonPoint != mGoalSummonPoint)
                    {
                        mGoalSummonPoint?.UnGoal();

                        mGoalSummonPoint = hitSummonPoint;
                        mGoalSummonPoint.Goal();
                    }
                }
            }
        }

        if (mHoldingSummonPoint != null && Input.GetMouseButtonUp(0))
        {
            if(tryRaycastSummonPoint(out SummonPoint hitSummonPoint))
            {
                // 합성 및 판매 UI 표시
                if(hitSummonPoint == mHoldingSummonPoint)
                {
                    mSelectedSummonPoint = hitSummonPoint;

                    mSelectedSummonPoint.UnHold();
                    mSelectedSummonPoint.Select();

                    _trSummonPointCanvas.transform.position = mSelectedSummonPoint.transform.position;
                    _trSummonPointCanvas.gameObject.SetActive(true);

                    showHeroAttackRange(mSelectedSummonPoint);
                }
                else
                {
                    mHoldingSummonPoint.UnHold();
                    mHoldingSummonPoint.UnSelect();

                    mGoalSummonPoint.UnGoal();

                    // 영웅 이동
                    changeHeroes(mHoldingSummonPoint, mGoalSummonPoint);
                }
            }

            mHoldingSummonPoint = null;
            mGoalSummonPoint = null;
        }
    }

    private SummonPoint[] findSummonPoints()
    {
        SummonPoint[] summonPoints = _summonPointParent.GetComponentsInChildren<SummonPoint>();

        return summonPoints;
    }

    private bool tryRaycastSummonPoint(out SummonPoint hitSummonPoint)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0f, LayerMask.GetMask("SummonPoint"));

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("SummonPoint"))
            {
                hitSummonPoint = hit.collider.GetComponent<SummonPoint>();

                return true;
            }
            else
            {
                hitSummonPoint = null;
                return false;
            }
        }
        else
        {
            hitSummonPoint = null;
            return false;
        }
    }

    private void changeHeroes(SummonPoint summonPoint1, SummonPoint summonPoint2)
    {
        var summonPoint1Heroes = new List<Hero>(summonPoint1.Heroes);
        var summonPoint2Heroes = new List<Hero>(summonPoint2.Heroes);

        summonPoint1.Clear();
        summonPoint2.Clear();

        summonPoint1.AddHeroes(summonPoint2Heroes);
        summonPoint2.AddHeroes(summonPoint1Heroes);
    }

    private void showHeroAttackRange(SummonPoint summonPoint)
    {
        summonPoint.TryGetHero(out Hero hero);
        var heroAttack = hero.GetComponent<HeroAttack>();

        _trHeroAttackRange.position = summonPoint.transform.position;
        _trHeroAttackRange.localScale = Vector3.one * heroAttack.AttackRange * 2f;

        _trHeroAttackRange.gameObject.SetActive(true);
    }

    private void hideHeroAttackRange()
    {
        _trHeroAttackRange.gameObject.SetActive(false);
    }
}
