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
            switch (neighbor)
            {
                case TilingRuleOutput.Neighbor.This:
                    return (siblings.Contains(other)
                        || base.RuleMatch(neighbor, other));
                case TilingRuleOutput.Neighbor.NotThis:
                    return (!siblings.Contains(other)
                        && base.RuleMatch(neighbor, other));
            }
            return base.RuleMatch(neighbor, other);
        }
    }
}