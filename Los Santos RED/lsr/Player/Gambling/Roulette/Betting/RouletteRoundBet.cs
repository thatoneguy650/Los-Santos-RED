using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class RouletteRoundBet
    {
        public RouletteRoundBet()
        {
        }
        public List<RouletteBet> RouletteBets
        {
            get
            {
                List<RouletteBet> toReturn = new List<RouletteBet>();
                toReturn.AddRange(SingleBets.ToList());
                toReturn.AddRange(ColorBets.ToList());
                toReturn.AddRange(EvenOddBets.ToList());
                toReturn.AddRange(DozenBets.ToList());
                toReturn.AddRange(ColumnBets.ToList());
                toReturn.AddRange(TopLineBets.ToList());
                toReturn.AddRange(RowBets.ToList());
                return toReturn;
            }
        }
        public List<SingleBet> SingleBets { get; set; } = new List<SingleBet>();
        public List<ColorBet> ColorBets { get; set; } = new List<ColorBet>();
        public List<EvenOddBet> EvenOddBets { get; set; } = new List<EvenOddBet>();
        public List<DozenBet> DozenBets { get; set; } = new List<DozenBet>();
        public List<ColumnBet> ColumnBets { get; set; } = new List<ColumnBet>();
        public List<TopLineBet> TopLineBets { get; set; } = new List<TopLineBet>();
        public List<RowBet> RowBets { get; set; } = new List<RowBet>();
    }
}
