using PromotionEvaulationApp._Ennum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PromotionEvaulationApp
{

    public class RuleEngine
    {
        public string _orderUnit { get; set; }
        public int _orderCount { get; set; }
        public PromotionType _promotionType { get; set; }
        public double _promotionAmt { get; set; }
    }
}
