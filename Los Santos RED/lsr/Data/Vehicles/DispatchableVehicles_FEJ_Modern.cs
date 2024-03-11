using NAudio.Mixer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DispatchableVehicles_FEJ_Modern
{
    private DispatchableVehicles_FEJ DispatchableVehicles_FEJ;

    public List<DispatchableVehicle> UnmarkedVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> LSPDVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> EastLSPDVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> VWPDVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> RHPDVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> DPPDVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> NYSPVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> LSIAPDVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> LSPPVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> SAHPVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> BCSOVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> LSSDVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> MajesticLSSDVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> VWHillsLSSDVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> DavisLSSDVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> ParkRangerVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> USNPSParkRangersVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> SADFWParkRangersVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> LSDPRParkRangersVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> FIBVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> BorderPatrolVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> NOOSEPIAVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> NOOSESEPVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> MarshalsServiceVehicles_FEJ_Modern { get; private set; }

    public DispatchableVehicles_FEJ_Modern(DispatchableVehicles_FEJ dispatchableVehicles_FEJ)
    {
        DispatchableVehicles_FEJ = dispatchableVehicles_FEJ;
    }
    public void DefaultConfig()
    {
        LocalPolice();
        StatePolice();
        LocalSheriff();
        ParkRangers();
        FederalPolice();
    }
    private void LocalPolice()
    {
        UnmarkedVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceFugitive(25,25,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(25,25,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(50, 50, 16, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(25,25,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(25,25,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(5,5,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(5,5,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(5,5,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(5,5,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(15,15,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(15,15,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
        };

        LSPDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,11,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(10,5,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(48,35,1,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(2,2,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(2,2,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(48,35,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(25,25,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceCaracara(5,5,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600(25,25,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(25, 20, 1, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(5,5,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceTerminus(1,1,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),

            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,3,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,0,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),

        };
        EastLSPDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(5,5,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(5, 5, 3, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(10,10,3,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(10,10,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(1,1,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),

            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(15,15,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceCaracara(5,5,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600(25,25,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,3,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,0,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
        };
        VWPDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(25, 25, 2, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(50,50,2,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(50,50,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(1,1,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(20,20,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(5,5,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,3,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,0,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),

        };

        RHPDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,20,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(50, 50, 5, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(25,25,5,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(5,5,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(25,25,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(5,5,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(20,15,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),
        };
        DPPDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,19,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(20, 20, 6, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(50,50,6,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(50,50,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(25,25,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(15,15,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(3,3,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(15,10,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(3,3,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),
        };
    }
    private void StatePolice()
    {
        NYSPVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresley(10,10,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(25,25,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
        };
        NYSPVehicles_FEJ_Modern.ForEach(x => { x.ForcedPlateType = 5; x.MaxRandomDirtLevel = 15.0f; });

        LSIAPDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(5,5,21,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(15, 15, 11, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,12,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(10,10,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(2,2,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
        };
        LSPPVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(5,5,22,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(5, 5, 12, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(10,10,13,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(10,10,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(5,5,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(2,2,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceVindicator(15,10,4,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
        };

        SAHPVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(1,1,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(1,1,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(5, 5, 16, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "StandardSAHP", "StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,14,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(45,55,4,false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "StandardSAHP", "StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(20,20,4,false, PoliceVehicleType.SlicktopMarked, 134, -1, -1, -1, -1, "StandardSAHP", "StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,4,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,4,false,PoliceVehicleType.SlicktopMarked,134,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceGresley(10,10,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceGresley(10,10,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(1,1,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(10,10,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(10,10,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            DispatchableVehicles_FEJ.DispatchableVehicles.GauntletUndercoverSAHP,

            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(35,35,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(35,35,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),


            DispatchableVehicles_FEJ.Create_PoliceCaracara(5,5,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(5,5,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),


            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            

            new DispatchableVehicle("frogger2",1,1) { RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 3, GroupName = "Helicopter",RequiredPedGroup = "Pilot",MaxWantedLevelSpawn = 2 },
            new DispatchableVehicle("frogger2",0,30) { RequiredGroupIsDriverOnly = true, RequiredLiveries = new List<int>() { 3 },MinOccupants = 3,MaxOccupants = 4, GroupName = "Helicopter",RequiredPedGroup = "Pilot",MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4 },

            new DispatchableVehicle("polmav", 1,1) { RequiredPedGroup = "Pilot", GroupName = "Helicopter",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 2 }, MaxWantedLevelSpawn = 2,MinOccupants = 2,MaxOccupants = 4 },
            new DispatchableVehicle("polmav", 0,30) { RequiredPedGroup = "Pilot", GroupName = "Helicopter",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 2 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },


            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,2,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceVindicator(45,20,0,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
        };
    }
    private void LocalSheriff()
    {

        BCSOVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(20,20,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(7, 7, 0, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(10,10,0,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(15,15,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(15,15,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(1,1,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(4,4,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceTerminus(3,3,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),

            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(25,25,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceCaracara(35,35,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,3,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceVindicator(15,10,1,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),

            DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),

            new DispatchableVehicle("polmav", 1, 150) { RequiredGroupIsDriverOnly = true, RequiredPedGroup = "Pilot",GroupName = "Helicopter", RequiredLiveries = new List<int>() { 10 }, MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
            new DispatchableVehicle("annihilator", 1, 150) { RequiredGroupIsDriverOnly = true, RequiredPedGroup = "Pilot",GroupName = "Helicopter",RequiredLiveries = new List<int>() { 5 }, MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },

        };
        BCSOVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        LSSDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(15, 15, 7, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,7,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(10,10,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(2,2,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(25,25,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceCaracara(25,25,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,5,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,1,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(3,3,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),
            DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),

        };
        LSSDVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
        MajesticLSSDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(25, 25, 8, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(25,25,8,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(10,10,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(25,25,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceCaracara(25,25,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,5,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,1,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(3,3,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),
        };
        MajesticLSSDVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
        VWHillsLSSDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,18,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(15, 15, 9, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,9,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(20,20,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(7,7,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(25,25,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceCaracara(25,25,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,5,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,1,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(3,3,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),
        };
        VWHillsLSSDVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
        DavisLSSDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,17,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(10, 10, 10, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,10,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(10,10,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(5,5,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(25,25,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceCaracara(5,5,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600(15,15,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceVindicator(15,10,5,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,1,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(3,3,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),

        };
        DavisLSSDVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
    }
    private void ParkRangers()
    {
        ParkRangerVehicles_FEJ_Modern = new List<DispatchableVehicle>()//San Andreas State Parks
        {
            DispatchableVehicles_FEJ.Create_PoliceTerminus(20,20,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",20),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(20,20,16,false,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"","",20),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(40,40,29,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(10,10,8,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(25,25,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,20,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
        };
        ParkRangerVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        USNPSParkRangersVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,20,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(40,40,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(20,20,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",5),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(20,20,13,false,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"","",5),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(40,40,26,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(10,10,5,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(25,25,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,17,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
        };
        USNPSParkRangersVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        SADFWParkRangersVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceTerminus(20,20,14,false,PoliceVehicleType.Marked,51,-1,-1,-1,-1,"","",20),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(20,20,14,false,PoliceVehicleType.Unmarked,51,-1,-1,-1,-1,"","",20),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(40,40,27,false,PoliceVehicleType.MarkedWithColor,51,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(10,10,6,false,PoliceVehicleType.Marked,51,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(25,25,14,false,PoliceVehicleType.Marked,51,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,18,false,PoliceVehicleType.Marked,51,-1,-1,-1,-1,"",""),
        };
        SADFWParkRangersVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        LSDPRParkRangersVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceTerminus(20,20,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",20),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(20,20,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",20),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(40,40,28,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(10,10,7,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(25,25,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,19,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
        };
        LSDPRParkRangersVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
    }
    private void FederalPolice()
    {
        
        FIBVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(10,10,16,false,PoliceVehicleType.Unmarked,1,0,4,-1,-1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,11,false,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,11,false,PoliceVehicleType.Detective,1,0,4,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(5,5,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(5,5,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Detective,1,0,4,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(5,5,-1,false,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(5,5,-1,false,PoliceVehicleType.Detective,1,0,4,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(5,5,12,false,PoliceVehicleType.Detective,1,0,4,-1,-1,"","",20),
            DispatchableVehicles_FEJ.Create_PoliceGresley(5,5,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(5,5,11,true,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(5,5,11,true,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),

            new DispatchableVehicle("frogger2", 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 0 } },

            new DispatchableVehicle("polmav", 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle("annihilator", 0, 30) { RequiredLiveries = new List<int>() { 2 },RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 1, RequiredPedGroup = "FIBHET", MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4 },

            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(0,20,-1,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(0,10,11,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(0,35,16,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,DispatchableVehicles_FEJ.PIBlankLivID,true,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(0,15,11,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(0,15,11,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(0,20,11,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),

            new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 0, RequiredPedGroup = "FIBHET", ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 5,MaxWantedLevelSpawn = 6, },


        };
        FIBVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 2.0f);
        BorderPatrolVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(20, 20, 15, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,14,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(40,40,19,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(1,1,16,false,PoliceVehicleType.Marked,111,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(35,35,19,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,16,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(5,5,25,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            
            DispatchableVehicles_FEJ.Create_PoliceTerminus(3,3,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",30),

            DispatchableVehicles_FEJ.Create_PoliceCaracara(25,25,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600(25,25,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            
            new DispatchableVehicle("polmav", 0, 100) { RequiredLiveries = new List<int>() { 7 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
            new DispatchableVehicle("annihilator", 0, 100) { RequiredLiveries = new List<int>() { 8 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
        };
        BorderPatrolVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        NOOSEPIAVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,14,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(5,5,23,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(35, 35, 13, false, PoliceVehicleType.Marked, 134, 0, 3, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(70,70,15,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(70,70,17,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(1,1,18,false,PoliceVehicleType.Marked,111,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(1,1,10,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"","",10),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(5,5,17,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(5,5,14,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            

            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(0, 40, 13, false, PoliceVehicleType.Marked, 134, 4, 5, 3, 4, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(0,50,15,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(0,40,17,false,PoliceVehicleType.Marked,-1,4,5,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(0,15,23,false,PoliceVehicleType.Marked,-1,4,5,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(0,15,14,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(0,15,14,false,PoliceVehicleType.Marked,-1,3,4,3,4,"",""),


            new DispatchableVehicle("polmav", 0, 100) { RequiredLiveries = new List<int>() { 8 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
            new DispatchableVehicle("annihilator", 0, 100) { RequiredLiveries = new List<int>() { 7 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
        };
        NOOSESEPVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,15,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(5,5,23,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(15,15,14,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(70,70,16,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(30,30,18,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(1,1,17,false,PoliceVehicleType.Marked,111,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(1,1,11,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"","",10),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(5,5,18,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(5,5,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(25, 20, 14, false, PoliceVehicleType.Marked, 134, 4, 5, 3, 4, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(0,50,16,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(0,40,18,false,PoliceVehicleType.Marked,-1,4,5,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(0,15,15,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(0,15,15,false,PoliceVehicleType.Marked,-1,3,4,3,4,"",""),

            new DispatchableVehicle("polmav", 0, 100) { RequiredLiveries = new List<int>() { 9 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
            new DispatchableVehicle("annihilator", 0, 100) { RequiredLiveries = new List<int>() { 6 },MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
        };


        MarshalsServiceVehicles_FEJ_Modern = new List<DispatchableVehicle>
        {
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(25, 25, 16, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(15, 15, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15, 15, DispatchableVehicles_FEJ.PIBlankLivID, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(10, 10, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(5, 5, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", "", 20),
            DispatchableVehicles_FEJ.Create_PoliceGresley(15, 15, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(10, 10, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(10, 10, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", "")
        };
    }
}

