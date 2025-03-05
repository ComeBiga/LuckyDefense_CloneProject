using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    private List<Hero> _heroPrefabs;
    [SerializeField]
    private SummonPointManager _summonPointManager;

    private SummonPoint mSelectedSummonPoint;

    public bool TrySummonHero(int heroID)
    {
        SummonPoint targetSummonPoint = _summonPointManager.FindSummonPointWithoutFull(heroID);

        if(targetSummonPoint == null || targetSummonPoint.IsFull)
        {
            targetSummonPoint = _summonPointManager.GetEmptySummonPoint();
        }

        summonHero(heroID, targetSummonPoint);

        return true;
    }

    public void SummonRandomHero()
    {
        TrySummonHero(UnityEngine.Random.Range(0, 1));
    }

    public void ComposeHero()
    {
        if(mSelectedSummonPoint == null)
        {
            return;
        }

        if(mSelectedSummonPoint.PositionType != SummonPoint.EPositionType.Tripple)
        {
            return;
        }

        var composedHeroes = new List<Hero>(mSelectedSummonPoint.Heroes);
        mSelectedSummonPoint.Clear();

        for(int i = composedHeroes.Count - 1; i >= 0; i--)
        {
            Destroy(composedHeroes[i].gameObject);
        }

        summonHero(UnityEngine.Random.Range(100, 101), mSelectedSummonPoint);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0f, LayerMask.GetMask("SummonPoint"));

            if (hit.collider != null)
            {
                if(hit.collider.CompareTag("SummonPoint"))
                {
                    var summonPoint = hit.collider.GetComponent<SummonPoint>();

                    mSelectedSummonPoint = summonPoint;
                }
            }
        }
    }

    private void summonHero(int heroID, SummonPoint summonPoint)
    {
        Hero targetPrefab = _heroPrefabs.Find(heroPrefab => heroPrefab.ID == heroID);
        Hero summonedHero = Instantiate(targetPrefab);
        summonPoint.AddHero(summonedHero);

        var summonedHeroAttack = summonedHero.GetComponent<HeroAttack>();
        summonedHeroAttack.StartAttack();
    }
}
