using ExtensionsMethods;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class RouletteWheel
    {
        private RouletteGame RouletteGame;
        public List<RoulettePocket> PocketsList { get; private set; } = new List<RoulettePocket>();
        public RoulettePocket SelectedPocket { get; private set; }
        public bool IsDoubleZero { get; set; } = true;
        public bool IsTripleZero { get; set; } = false;
        public RouletteWheel(RouletteGame rouletteGame)
        {
            RouletteGame = rouletteGame;
        }
        public RoulettePocket GetPocket(int pocketId) => PocketsList.Where(x=> x.PocketID == pocketId).FirstOrDefault();
        public void Setup()
        {
            PocketsList = new List<RoulettePocket>();
            for (int pocketID = 0; pocketID <= 36; pocketID++)
            {
                RoulettePocket rp = new RoulettePocket(pocketID);
                rp.Setup();
                PocketsList.Add(rp);
            }
            if(IsDoubleZero)
            {
                RoulettePocket rp = new RoulettePocket(-1);
                rp.Setup();
                PocketsList.Add(rp);
            }
            if (IsTripleZero)
            {
                RoulettePocket rp = new RoulettePocket(-2);
                rp.Setup();
                PocketsList.Add(rp);
            }
        }
        public void Spin()
        {
            SelectedPocket = PocketsList.PickRandom();
        }

    }
}
