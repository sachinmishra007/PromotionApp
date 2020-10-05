namespace PromotionEvaulationApp
{
    public class CompleteOrderInfo
    {
        public char _orderedUnit { get; set; }
        public int _orderedCount { get; set; }
        public int _appliedPromotionCount { get; set; }
        public double _appliedPromotionAmt { get; set; }

        public int _appliedCountFact { get; set; }
    }
}