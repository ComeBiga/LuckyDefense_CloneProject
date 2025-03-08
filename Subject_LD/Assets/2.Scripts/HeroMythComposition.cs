using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMythComposition : MonoBehaviour
{
    [SerializeField]
    private Hero _hero;
    [SerializeField]
    private List<Hero> _materialHeroes = new List<Hero>();

    public bool IsComposable(SummonPoint[] summonPoints, out Dictionary<int, List<SummonPoint>> materialHeroMap)
    {
        materialHeroMap = new Dictionary<int, List<SummonPoint>>();

        for(int i = 0; i < _materialHeroes.Count; ++i)
        {
            Hero materialHero = _materialHeroes[i];

            materialHeroMap.Add(materialHero.ID, new List<SummonPoint>(5));

            for(int j = 0; j < summonPoints.Length; ++j)
            {
                SummonPoint summonPoint = summonPoints[j];

                if(summonPoint.Contains(materialHero.ID))
                {
                    materialHeroMap[materialHero.ID].Add(summonPoint);
                }
            }
        }

        foreach(var pair in materialHeroMap)
        {
            List<SummonPoint> matchedPoints = pair.Value;

            if(matchedPoints.Count <= 0)
            {
                return false;
            }
        }

        return true;
    }
}
