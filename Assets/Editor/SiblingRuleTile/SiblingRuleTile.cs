using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace UnityEngine
{
    [CreateAssetMenu(fileName = "New Sibling Rule Tile", menuName = "2D/Tiles/Sibling Rule Tile")]
    public class SiblingRuleTile : RuleTile
    {
        public List<TileBase> siblings;

        public override bool RuleMatch(int neighbor, TileBase other)
        {
            if (siblings.Contains(other))
            {
                return true;
            }

            return base.RuleMatch(neighbor, other);
        }
    }
}