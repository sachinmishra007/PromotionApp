using PromotionEvaulationApp._Class;
using PromotionEvaulationApp._Ennum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromotionEvaulationApp._mainProgram
{
    public class _mainProgram
    {
        // Unit Price SKU
        public Dictionary<char, double> _lstUnitPriceSKU = new Dictionary<char, double>();
        // Active Promotion Info
        public Dictionary<string, double> _lstActivePromotion = new Dictionary<string, double>();
        // Rule Engine
        public Dictionary<string, RuleEngine> _lstRuleEngine = new Dictionary<string, RuleEngine>();
        // Pending Order Info
        public List<OrderInfo> _lstPendingOrderInfo = new List<OrderInfo>();
        // Completed Order Info 
        public List<CompleteOrderInfo> _lstCompleteOrderInfo = new List<CompleteOrderInfo>();
        // List of Order Item SKU Unit
        public List<char> _lstOrderItems = new List<char>();
        public double _totalAmount = 0F;
        public void _getPendingOrderInfo(int orderedCount, char orderedUnit, CompleteOrderInfo _completeOrderInfo)
        {
            foreach (var _ruleInfo in _lstRuleEngine.Keys)
            {
                if (_lstRuleEngine[_ruleInfo]._promotionType == PromotionType.Single)
                {
                    if (_lstRuleEngine[_ruleInfo]._orderUnit != null && _lstRuleEngine[_ruleInfo]._orderUnit.ToString() == orderedUnit.ToString())
                    {
                        var _leftOrderCount = orderedCount % _lstRuleEngine[_ruleInfo]._orderCount;
                        if (_leftOrderCount != 0)
                        {
                            //Console.WriteLine(" Pending Order For : " + orderedUnit + " is " + _leftOrderCount);
                            _completeOrderInfo._appliedPromotionCount = orderedCount - _leftOrderCount;
                            _completeOrderInfo._appliedCountFact = (_completeOrderInfo._appliedPromotionCount / _lstRuleEngine[_ruleInfo]._orderCount);
                            _completeOrderInfo._appliedPromotionAmt = _completeOrderInfo._appliedCountFact * _lstRuleEngine[_ruleInfo]._promotionAmt;
                            _lstPendingOrderInfo.Add(new OrderInfo() { _orderCount = _leftOrderCount, _orderUnit = orderedUnit });
                        }
                        else if (_leftOrderCount == 0)
                        {
                            _completeOrderInfo._appliedPromotionCount = orderedCount - _leftOrderCount;
                            _completeOrderInfo._appliedCountFact = (_completeOrderInfo._appliedPromotionCount / _lstRuleEngine[_ruleInfo]._orderCount);
                            _completeOrderInfo._appliedPromotionAmt = _completeOrderInfo._appliedCountFact * _lstRuleEngine[_ruleInfo]._promotionAmt;
                        }

                        break;
                    }
                }
                else
                {
                    _lstPendingOrderInfo.Add(new OrderInfo() { _orderCount = orderedCount, _orderUnit = orderedUnit });
                    break;
                }
            }
        }

        public void _getComboOrderInfo()
        {
            for (int i = 0; i < _lstPendingOrderInfo.Count; i++)
            {
                var _pendingOrder = _lstPendingOrderInfo[i]._orderUnit + "" + _lstPendingOrderInfo[i]._orderCount;

                for (int j = i + 1; j < _lstPendingOrderInfo.Count; j++)
                {
                    var _comboOrder = _lstPendingOrderInfo[j]._orderUnit + "" + _lstPendingOrderInfo[j]._orderCount;
                    //Console.WriteLine(_pendingOrder + "" + _comboOrder);
                    if (_lstRuleEngine.ContainsKey(_pendingOrder + "" + _comboOrder))
                    {
                        //updating the main order details 

                        for (int _com = 0; _com < _lstCompleteOrderInfo.Count; _com++)
                        {
                            if (_lstCompleteOrderInfo[_com]._orderedUnit == _lstPendingOrderInfo[i]._orderUnit && _lstCompleteOrderInfo[_com]._orderedCount != 0)
                            {
                                _lstCompleteOrderInfo[_com]._orderedCount -= _lstPendingOrderInfo[i]._orderCount;
                            }
                            if (_lstCompleteOrderInfo[_com]._orderedUnit == _lstPendingOrderInfo[j]._orderUnit && _lstCompleteOrderInfo[_com]._orderedCount != 0)
                            {
                                _lstCompleteOrderInfo[_com]._orderedCount = _lstCompleteOrderInfo[_com]._orderedCount - _lstPendingOrderInfo[j]._orderCount;
                                _lstCompleteOrderInfo[_com]._appliedPromotionAmt = _lstRuleEngine[_pendingOrder + "" + _comboOrder]._promotionAmt;
                                _lstCompleteOrderInfo[_com]._appliedCountFact = 1;// Considering the Only One Combination of the Promotion will be in Rule Engine
                            }
                        }


                        _lstPendingOrderInfo[i]._orderCount = _lstPendingOrderInfo[i]._orderCount - _lstPendingOrderInfo[i]._orderCount;
                        _lstPendingOrderInfo[j]._orderCount = _lstPendingOrderInfo[j]._orderCount - _lstPendingOrderInfo[j]._orderCount;
                        if (_lstPendingOrderInfo[i]._orderCount == 0)
                        {
                            _lstPendingOrderInfo.RemoveAt(i);
                        }
                        if (_lstPendingOrderInfo[j - 1]._orderCount == 0)
                        {
                            _lstPendingOrderInfo.RemoveAt(j - 1);
                        }
                    }
                }
            }
        }
        public void _getSummaryOrderInfo()
        {
            _totalAmount = 0F;
            //Console.WriteLine("=========== Scenario ===============");
            for (int i = 0; i < _lstCompleteOrderInfo.Count; i++)
            {

                var penOrder = _lstPendingOrderInfo.Where(x => x._orderUnit == _lstCompleteOrderInfo[i]._orderedUnit).ToList();
                var unitPrice = _lstUnitPriceSKU.Where(x => x.Key == _lstCompleteOrderInfo[i]._orderedUnit).ToList()[0];

                if (penOrder != null && _lstCompleteOrderInfo[i]._appliedPromotionAmt != 0)
                {
                    string _appPromoDescritpion = string.Empty;
                    int _appCount = 0;
                    do
                    {
                        _appPromoDescritpion += _lstCompleteOrderInfo[i]._appliedPromotionAmt / _lstCompleteOrderInfo[i]._appliedCountFact + " + ";
                        _appCount++;
                    } while (_lstCompleteOrderInfo[i]._appliedCountFact > 1 && _lstCompleteOrderInfo[i]._appliedCountFact > _appCount);

                    if (_lstCompleteOrderInfo[i]._orderedCount != 0)
                    {

                        Console.WriteLine("{1} * {0}    {2} {3} * {4}", _lstCompleteOrderInfo[i]._orderedUnit, _lstCompleteOrderInfo[i]._orderedCount, _appPromoDescritpion, penOrder.Count == 0 ? 0 : penOrder[0]._orderCount, unitPrice.Value);
                    }
                    else
                    {
                        Console.WriteLine("{1} * {0}    {2} ", _lstCompleteOrderInfo[i]._orderedUnit, penOrder.Count == 0 ? 1 : penOrder[0]._orderCount, _lstCompleteOrderInfo[i]._appliedPromotionAmt);

                    }

                    _totalAmount += _lstCompleteOrderInfo[i]._appliedPromotionAmt + (penOrder.Count == 0 ? 0 : penOrder[0]._orderCount * unitPrice.Value);
                }
                else
                {
                    if (_lstCompleteOrderInfo[i]._orderedCount == 0 && _lstCompleteOrderInfo[i]._appliedCountFact == 0)
                    {
                        Console.WriteLine("{1} * {0}    {2}", _lstCompleteOrderInfo[i]._orderedUnit, _lstCompleteOrderInfo[i]._orderedCount, "-");

                    }
                    else
                    {
                        Console.WriteLine("{1} * {0}    {2}", _lstCompleteOrderInfo[i]._orderedUnit, _lstCompleteOrderInfo[i]._orderedCount, unitPrice.Value);

                    }


                    _totalAmount += _lstCompleteOrderInfo[i]._appliedPromotionAmt + (penOrder.Count == 0 ? 0 : penOrder[0]._orderCount * unitPrice.Value);

                }
            }
            Console.WriteLine("====================================");
            Console.WriteLine("Total           {0}", _totalAmount);


        }

        public void _getOrderInfo(bool isPredefined)
        {
            if (isPredefined)
            {
                _lstOrderItems.Add('A');
                _lstOrderItems.Add('A');
                _lstOrderItems.Add('A');
                _lstOrderItems.Add('A');
                _lstOrderItems.Add('A');
                _lstOrderItems.Add('B');
                _lstOrderItems.Add('B');
                _lstOrderItems.Add('B');
                _lstOrderItems.Add('B');
                _lstOrderItems.Add('B');
                _lstOrderItems.Add('C');
                _lstOrderItems.Add('D');
            }

        }

        public void _getUnitSKUPrice(bool isPredefined)
        {
            if (isPredefined)
            {
                _lstUnitPriceSKU.Add('A', 50);
                _lstUnitPriceSKU.Add('B', 30);
                _lstUnitPriceSKU.Add('C', 20);
                _lstUnitPriceSKU.Add('D', 15);
            }
        }

        public void _getActivePromotion(bool isPredefined)
        {
            if (isPredefined)
            {
                _lstActivePromotion.Add("A,A,A", 130);
                _lstActivePromotion.Add("B,B", 45);
                _lstActivePromotion.Add("C,D", 30);
            }
        }

        public void _getBasicSetup()
        {
            foreach (var item in _lstActivePromotion.Keys)
            {
                var _list = item.Split(',')
                    .GroupBy(x => new
                    {
                        _orderUnit = x,
                        _promotionAmt = _lstActivePromotion[item]
                    }).Select(x => new
                    {
                        _orderUnit = x.Key._orderUnit,
                        _orderCount = x.Count(),
                        _promotionAmt = x.Key._promotionAmt,
                    }).ToList();

                if (_list.Count <= 1)
                {
                    //Console.WriteLine("Promotion : " + _list[0]._orderUnit+ _list[0]._orderCount + " Amt: " + _list[0]._promotionAmt);
                    _lstRuleEngine.Add(_list[0]._orderUnit + _list[0]._orderCount, new RuleEngine()
                    {
                        _orderUnit = _list[0]._orderUnit,
                        _orderCount = _list[0]._orderCount,
                        _promotionAmt = _list[0]._promotionAmt,
                        _promotionType = PromotionType.Single
                    });

                }
                else
                {

                    string _orderStatus = String.Empty;
                    int i = 0;
                    do
                    {
                        _orderStatus += _list[i]._orderUnit + "" + _list[i]._orderCount;
                        i++;
                    } while (i < _list.Count);

                    //Console.WriteLine("Promotion : " + _orderStatus + " Amt: " + _list[0]._promotionAmt);
                    _lstRuleEngine.Add(_orderStatus, new RuleEngine()
                    {
                        _promotionAmt = _list[0]._promotionAmt,
                        _promotionType = PromotionType.Combo
                    });

                }
            }

            var _orderItems =
                    from _item in _lstOrderItems.AsEnumerable()
                    orderby _item
                    group _item by _item into g
                    select new
                    {
                        _orderedUnit = g.Key,
                        _orderedCount = g.Count(),
                        _appliedPromotionCount = 0,
                        _appliedPromotionAmt = 0,
                    };

            for (int i = 0; i < _orderItems.ToList().Count; i++)
            {
                _lstCompleteOrderInfo.Add(new CompleteOrderInfo()
                {
                    _appliedPromotionAmt = _orderItems.ToList()[i]._appliedPromotionAmt,
                    _appliedPromotionCount = _orderItems.ToList()[i]._appliedPromotionCount,
                    _orderedUnit = _orderItems.ToList()[i]._orderedUnit,
                    _orderedCount = _orderItems.ToList()[i]._orderedCount,
                });
            }

            for (int i = 0; i < _lstCompleteOrderInfo.Count; i++)
            {
                var orderedCount = _lstCompleteOrderInfo[i]._orderedCount;
                var orderedUnit = _lstCompleteOrderInfo[i]._orderedUnit;
                _getPendingOrderInfo(orderedCount, orderedUnit, _lstCompleteOrderInfo[i]);
            }

        }
        public void _entryProgram()
        {

            bool isPredefined = true;
            _getOrderInfo(isPredefined);
            _getUnitSKUPrice(isPredefined);
            _getActivePromotion(isPredefined);

            _getBasicSetup();
            _getComboOrderInfo();
            _getSummaryOrderInfo();
            Console.ReadLine();
        }
    }

}
