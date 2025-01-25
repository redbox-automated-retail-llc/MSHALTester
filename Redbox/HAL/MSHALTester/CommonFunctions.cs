using Redbox.HAL.Component.Model.Extensions;
using System;
using System.Xml;


namespace Redbox.HAL.MSHALTester
{
    public static class CommonFunctions
    {
        private const int SellThruSlotWidth = 466;

        public static bool ComputeQuadrants(
          Decimal? startOffset,
          int? numberOfQuadrants,
          int? sellThruSlots,
          int? sellThruOffset,
          int? slotsPerQuadrant,
          Decimal? slotWidth,
          XmlNode deckNode)
        {
            Decimal? nullable1 = startOffset;
            int i = 0;
            while (true)
            {
                int num1 = i;
                int? nullable2 = numberOfQuadrants;
                int valueOrDefault = nullable2.GetValueOrDefault();
                if (num1 < valueOrDefault & nullable2.HasValue)
                {
                    deckNode.ChildNodes[i].SetAttributeValue<int>("Offset", (int)nullable1.Value);
                    Decimal? nullable3;
                    Decimal num2 = 0M;
                    Decimal num3 = num2;
                    nullable3 = new Decimal?(num3);
                    Decimal? nullable4;
                    Decimal? nullable5;
                    Decimal? nullable6;
                    if (sellThruSlots.HasValue && sellThruOffset.HasValue)
                    {
                        nullable2 = slotsPerQuadrant;
                        int? nullable7;
                        int? nullable8;
                        if (!nullable2.HasValue)
                        {
                            nullable7 = new int?();
                            nullable8 = nullable7;
                        }
                        else
                            nullable8 = new int?(nullable2.GetValueOrDefault() - 1);
                        nullable7 = nullable8;
                        num2 = (Decimal)nullable7.Value;
                        nullable4 = slotWidth;
                        Decimal? nullable9;
                        if (!nullable4.HasValue)
                        {
                            nullable5 = new Decimal?();
                            nullable9 = nullable5;
                        }
                        else
                            nullable9 = new Decimal?(num2 * nullable4.GetValueOrDefault());
                        nullable4 = nullable9;
                        num2 = 0.5M;
                        Decimal? nullable10;
                        if (!nullable4.HasValue)
                        {
                            nullable5 = new Decimal?();
                            nullable10 = nullable5;
                        }
                        else
                            nullable10 = new Decimal?(nullable4.GetValueOrDefault() + num2);
                        nullable4 = nullable10;
                        num2 = (Decimal)466;
                        Decimal? nullable11;
                        if (!nullable4.HasValue)
                        {
                            nullable5 = new Decimal?();
                            nullable11 = nullable5;
                        }
                        else
                            nullable11 = new Decimal?(nullable4.GetValueOrDefault() + num2);
                        nullable3 = nullable11;
                    }
                    else
                    {
                        num2 = (Decimal)slotsPerQuadrant.Value;
                        nullable6 = slotWidth;
                        nullable4 = nullable6.HasValue ? new Decimal?(num2 * nullable6.GetValueOrDefault()) : new Decimal?();
                        nullable5 = slotWidth;
                        Decimal? nullable12;
                        if (!(nullable4.HasValue & nullable5.HasValue))
                        {
                            nullable6 = new Decimal?();
                            nullable12 = nullable6;
                        }
                        else
                            nullable12 = new Decimal?(nullable4.GetValueOrDefault() + nullable5.GetValueOrDefault());
                        nullable3 = nullable12;
                    }
                    nullable5 = nullable1;
                    nullable4 = nullable3;
                    Decimal? nullable13;
                    if (!(nullable5.HasValue & nullable4.HasValue))
                    {
                        nullable6 = new Decimal?();
                        nullable13 = nullable6;
                    }
                    else
                        nullable13 = new Decimal?(nullable5.GetValueOrDefault() + nullable4.GetValueOrDefault());
                    nullable1 = nullable13;
                    ++i;
                }
                else
                    break;
            }
            return true;
        }
    }
}
