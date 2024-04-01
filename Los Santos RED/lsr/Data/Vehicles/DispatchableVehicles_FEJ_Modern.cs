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
    public List<DispatchableVehicle> MerryweatherPatrolVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> BobcatSecurityVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> GroupSechsVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> SecuroservVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> LNLVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> CHUFFVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> DowntownTaxiVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> PurpleTaxiVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> HellTaxiVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> ShitiTaxiVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> SunderedTaxiVehicles_FEJ_Modern { get; private set; }
    public List<DispatchableVehicle> LSLifeguardVehicles_FEJ_Modern { get; private set; }

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
        Security();
        Taxis();
    }
    private void LocalPolice()
    {
        UnmarkedVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(15,15,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(15,15,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(25,25,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(25,25,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(20,20,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(20,20,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(15,15,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(15,15,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(2, 2, 16, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(20,20,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(20,20,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceVSTR(5,5,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceVSTR(5,5,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceReblaGTS(1,1,3,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceRiata(1,1,7,true,PoliceVehicleType.Detective,-1,-1,-1,1,2,"","",4),
        };

        LSPDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresley(50,40,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(45,35,1,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(35,35,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(25,25,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(15,15,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,11,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
                   
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(1,1,1,false, PoliceVehicleType.Marked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(5,5,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(5,5,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(1,1,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),
            DispatchableVehicles_FEJ.Create_PoliceRiata(5,5,1,false,PoliceVehicleType.Marked,-1,-1,-1,1,2,"","",4),
            DispatchableVehicles_FEJ.Create_PoliceReblaGTS(1,1,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGresley(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(1,1,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceVSTR(1,1,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,3,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",40),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,0,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",20),
            DispatchableVehicles_FEJ.Create_PoliceVerus(1,0,1,false,PoliceVehicleType.Marked,-1,0,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),

            DispatchableVehicles_FEJ.Create_PoliceReblaGTS(0,15,0,false,PoliceVehicleType.Marked,-1,3,4,2,4,"",""),

        };
        EastLSPDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresley(40,40,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(30,30,3,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(15,15,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(25,25,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(15,15,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceFugitive(15,15,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(1,1,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),
            DispatchableVehicles_FEJ.Create_PoliceRiata(1,1,1,false,PoliceVehicleType.Marked,-1,-1,-1,1,2,"","",4),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
           
            DispatchableVehicles_FEJ.Create_PoliceGresley(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,3,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",40),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,0,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceVerus(1,0,1,false,PoliceVehicleType.Marked,-1,0,2,1,1,"DirtBike","DirtBike",10),
        };
        VWPDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {

            DispatchableVehicles_FEJ.Create_PoliceGresley(50,50,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(30,30,2,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(25,25,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(35,35,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(15,15,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(20,20,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(1,1,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),
            DispatchableVehicles_FEJ.Create_PoliceRiata(5,5,1,false,PoliceVehicleType.Marked,-1,-1,-1,1,2,"","",4),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(1,1,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(1,1,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),            
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            
            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,3,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",40),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,0,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceVerus(1,0,1,false,PoliceVehicleType.Marked,-1,0,2,1,1,"DirtBike","DirtBike",10),
        };

        RHPDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(55,60,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(50,50,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(25,25,5,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(15,15,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,20,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(5,5,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceReblaGTS(1,1,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),


            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(1,1,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),        
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),        
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),           
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceVSTR(1,1,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),


            DispatchableVehicles_FEJ.Create_PoliceReblaGTS(0,15,1,false,PoliceVehicleType.Marked,-1,3,4,2,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),
        };
        DPPDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(20,20,19,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(15,15,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(20,20,6,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(20,20,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(20,20,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceTerminus(5,5,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(10,10,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(3,3,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(1,1,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(1,1,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),           
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),         
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),                
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),
        };
    }
    private void StatePolice()
    {
        NYSPVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresley(50,50,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(50,50,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
        };
        NYSPVehicles_FEJ_Modern.ForEach(x => { x.ForcedPlateType = 5; x.MaxRandomDirtLevel = 15.0f; });

        LSIAPDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(25,25,12,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(15,15,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,21,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGauntlet(2,2,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(1,1,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),           
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),            
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),           
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceVSTR(1,1,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
        };
        LSPPVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresley(20,20,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,22,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(20,20,13,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(15,15,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(15,15,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGauntlet(2,2,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),         
            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),


            DispatchableVehicles_FEJ.Create_PoliceVindicator(15,10,4,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",45),
        };

        SAHPVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(50,50,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(75,75,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceGresley(35,35,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceGresley(35,35,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,4,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(25,25,4,false,PoliceVehicleType.SlicktopMarked,134,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),     
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(10,10,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(20,20,3,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(15,15,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(15,15,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            DispatchableVehicles_FEJ.Create_PoliceAleutian(15,15,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(15,15,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,14,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(15,15,16,true,PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "StandardSAHP", "StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(15,15,16,true,PoliceVehicleType.SlicktopMarked, -1, -1, -1, -1, -1, "StandardSAHP", "StandardSAHP"),     
            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),       
            DispatchableVehicles_FEJ.Create_PoliceCaracara(5,5,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(5,5,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(3,3,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(3,3,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            DispatchableVehicles_FEJ.Create_PoliceGresley(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"","StandardSAHP"),

            
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            DispatchableVehicles_FEJ.DispatchableVehicles.GauntletUndercoverSAHP,

            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            new DispatchableVehicle("frogger2",1,1) { RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 3, GroupName = "Helicopter",RequiredPedGroup = "Pilot",MaxWantedLevelSpawn = 2 },
            new DispatchableVehicle("frogger2",0,30) { RequiredGroupIsDriverOnly = true, RequiredLiveries = new List<int>() { 3 },MinOccupants = 3,MaxOccupants = 4, GroupName = "Helicopter",RequiredPedGroup = "Pilot",MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4 },

            new DispatchableVehicle("polmav", 1,1) { RequiredPedGroup = "Pilot", GroupName = "Helicopter",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 2 }, MaxWantedLevelSpawn = 2,MinOccupants = 2,MaxOccupants = 4 },
            new DispatchableVehicle("polmav", 0,30) { RequiredPedGroup = "Pilot", GroupName = "Helicopter",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 2 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },


            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,2,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceVindicator(55,20,0,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",65),
        };
    }
    private void LocalSheriff()
    {

        BCSOVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresley(15,15,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(20,20,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(10,10,0,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(15,15,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(15,15,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(5,5,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),
            DispatchableVehicles_FEJ.Create_PoliceRiata(10,10,0,false,PoliceVehicleType.Marked,-1,-1,-1,1,2,"","",4),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(2,2,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceCaracara(10,10,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(4,4,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
    
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),         
            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""), 
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),   
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),      
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,3,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceVindicator(15,10,1,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",40),

            DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),

            new DispatchableVehicle("polmav", 1, 150) { RequiredGroupIsDriverOnly = true, RequiredPedGroup = "Pilot",GroupName = "Helicopter", RequiredLiveries = new List<int>() { 10 }, MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
            new DispatchableVehicle("annihilator", 1, 150) { RequiredGroupIsDriverOnly = true, RequiredPedGroup = "Pilot",GroupName = "Helicopter",RequiredLiveries = new List<int>() { 5 }, MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },

        };
        BCSOVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        LSSDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresley(35,35,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(35,35,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(55,55,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(25,25,7,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(20,20,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(15,15,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(15,15,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(10,10,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceTerminus(3,3,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),
            DispatchableVehicles_FEJ.Create_PoliceRiata(7,7,2,false,PoliceVehicleType.Marked,-1,-1,-1,1,2,"","",4),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(2,2,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(1,1,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(1,1,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceVSTR(1,1,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,5,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",40),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,1,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceVerus(1,0,3,false,PoliceVehicleType.Marked,-1,0,2,1,1,"DirtBike","DirtBike",10),

            DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),

        };
        LSSDVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
        MajesticLSSDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresley(35,35,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(35,35,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(45,45,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(25,25,8,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(20,20,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(15,15,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(15,15,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(10,10,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
                  
            DispatchableVehicles_FEJ.Create_PoliceTerminus(5,5,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),
            DispatchableVehicles_FEJ.Create_PoliceRiata(7,7,2,false,PoliceVehicleType.Marked,-1,-1,-1,1,2,"","",4),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(1,1,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),     
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),           
            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),          
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),     
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),    
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),


            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,5,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",40),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,1,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceVerus(1,0,3,false,PoliceVehicleType.Marked,-1,0,2,1,1,"DirtBike","DirtBike",10),
        };
        MajesticLSSDVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        VWHillsLSSDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresley(35,35,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,9,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(25,25,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(20,20,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(20,20,18,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(20,20,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(15,15,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(10,10,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceTerminus(5,5,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),
            DispatchableVehicles_FEJ.Create_PoliceRiata(7,7,2,false,PoliceVehicleType.Marked,-1,-1,-1,1,2,"","",4),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(1,1,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),          
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),     
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),   
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,5,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",40),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,1,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceVerus(1,0,3,false,PoliceVehicleType.Marked,-1,0,2,1,1,"DirtBike","DirtBike",10),
        };
        VWHillsLSSDVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
        DavisLSSDVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresley(35,35,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(45,45,10,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(10,10,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(5,5,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(5,5,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(5,5,17,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
                        
            DispatchableVehicles_FEJ.Create_PoliceTerminus(1,1,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),
            DispatchableVehicles_FEJ.Create_PoliceRiata(1,1,2,false,PoliceVehicleType.Marked,-1,-1,-1,1,2,"","",4),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
                     
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),            

            DispatchableVehicles_FEJ.Create_PoliceVindicator(15,10,5,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",25),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,1,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            DispatchableVehicles_FEJ.Create_PoliceVerus(1,0,3,false,PoliceVehicleType.Marked,-1,0,2,1,1,"DirtBike","DirtBike",10),
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
            DispatchableVehicles_FEJ.Create_PoliceSanchez(10,10,8,false,PoliceVehicleType.Marked,134,-1,2,1,1,"OffRoad","OffRoad",10),
            DispatchableVehicles_FEJ.Create_PoliceVerus(10,10,4,false,PoliceVehicleType.Marked,-1,0,2,1,1,"OffRoad","OffRoad",10),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(25,25,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,20,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(20,20,20,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceRiata(15,15,3,false,PoliceVehicleType.Marked,134,-1,-1,1,2,"","",4),
        };
        ParkRangerVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        USNPSParkRangersVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,20,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBison(40,40,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(20,20,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",5),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(20,20,13,false,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"","",5),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(40,40,26,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(10,10,5,false,PoliceVehicleType.Marked,134,-1,2,1,1,"OffRoad","OffRoad",10),
            DispatchableVehicles_FEJ.Create_PoliceVerus(10,10,6,false,PoliceVehicleType.Marked,-1,0,2,1,1,"OffRoad","OffRoad",10),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(25,25,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,17,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(20,20,17,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceRiata(15,15,5,false,PoliceVehicleType.Marked,134,-1,-1,1,2,"","",4),
        };
        USNPSParkRangersVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        SADFWParkRangersVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceTerminus(20,20,14,false,PoliceVehicleType.Marked,51,-1,-1,-1,-1,"","",20),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(20,20,14,false,PoliceVehicleType.Unmarked,51,-1,-1,-1,-1,"","",20),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(40,40,27,false,PoliceVehicleType.MarkedWithColor,51,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(10,10,6,false,PoliceVehicleType.Marked,51,-1,2,1,1,"OffRoad","OffRoad",10),
            DispatchableVehicles_FEJ.Create_PoliceVerus(10,10,7,false,PoliceVehicleType.Marked,51,0,2,1,1,"OffRoad","OffRoad",10),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(25,25,14,false,PoliceVehicleType.Marked,51,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,18,false,PoliceVehicleType.Marked,51,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(20,20,18,false,PoliceVehicleType.Marked,51,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceRiata(15,15,6,false,PoliceVehicleType.Marked,51,-1,-1,1,2,"","",4),
        };
        SADFWParkRangersVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        LSDPRParkRangersVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceTerminus(20,20,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",20),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(20,20,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",20),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(40,40,28,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(10,10,7,false,PoliceVehicleType.Marked,134,-1,2,1,1,"OffRoad","OffRoad",10),
            DispatchableVehicles_FEJ.Create_PoliceVerus(10,10,5,false,PoliceVehicleType.Marked,-1,0,2,1,1,"OffRoad","OffRoad",10),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(25,25,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(20,20,19,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(20,20,19,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceRiata(15,15,4,false,PoliceVehicleType.Marked,134,-1,-1,1,2,"","",4),

        };
        LSDPRParkRangersVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 15.0f);

        LSLifeguardVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceVerus(20,20,0,false,PoliceVehicleType.Marked,-1,-1,-1,1,1,"ATV","ATV",10),
            new DispatchableVehicle("lguard", 50, 50),
            new DispatchableVehicle("blazer2",50,50)  { RequiredPedGroup = "ATV",GroupName = "ATV" },
            new DispatchableVehicle("freecrawler",5,5) { RequiredVariation = new VehicleVariation() { VehicleMods = new List<VehicleMod>() {new VehicleMod(48, 7) } } },
            new DispatchableVehicle("seashark2", 100, 100) { RequiredPedGroup = "Boat",GroupName = "Boat", RequiredLiveries = new List<int>() { 0,1 }, MaxOccupants = 1 },
            new DispatchableVehicle("frogger2",2,5) { RequiredLiveries = new List<int>() { 6 }, MinOccupants = 2,MaxOccupants = 3, GroupName = "Helicopter" },
            new DispatchableVehicle("polmav",2,5) { RequiredLiveries = new List<int>() { 5 }, MinOccupants = 2,MaxOccupants = 3, GroupName = "Helicopter" },
        };
        LSLifeguardVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
    }
    private void FederalPolice()
    {
        
        FIBVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,11,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceInterceptor(25,25,11,true,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(25,25,11,true,PoliceVehicleType.Detective,1,0,4,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,-1,false,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(15,15,-1,false,PoliceVehicleType.Detective,1,0,4,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(25,25,11,true,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(25,25,11,true,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600(25,25,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(25,25,11,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceAleutian(25,25,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(25,25,11,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceVSTR(15,15,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceVSTR(15,15,11,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),


            DispatchableVehicles_FEJ.Create_PoliceReblaGTS(15,15,3,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceReblaGTS(15,15,3,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),


            DispatchableVehicles_FEJ.Create_PoliceTerminus(0,1,12,false,PoliceVehicleType.Detective,1,0,4,-1,-1,"","",20),

            new DispatchableVehicle("frogger2", 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 0 } },

            new DispatchableVehicle("polmav", 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle("annihilator", 0, 30) { RequiredLiveries = new List<int>() { 2 },RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 1, RequiredPedGroup = "FIBHET", MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4 },

            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(0,15,-1,false,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(0,25,11,true,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(0,25,11,false,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(0,25,11,false,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(0,25,11,false,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(0,25,11,false,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PoliceVSTR(0,10,11,false,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PoliceReblaGTS(0,15,3,false,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),


            DispatchableVehicles_FEJ.Create_PoliceVerus(0,0,2,false,PoliceVehicleType.Unmarked,1,5,5,1,1,"FIBHET","FIBHET",25),


            new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 0, RequiredPedGroup = "FIBHET", ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 5,MaxWantedLevelSpawn = 6, },

            
        };
        FIBVehicles_FEJ_Modern.ForEach(x => x.MaxRandomDirtLevel = 2.0f);
        BorderPatrolVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(45,45,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(20,20,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,19,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(10,10,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",30),
            DispatchableVehicles_FEJ.Create_PoliceBison(10,10,19,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(5,5,25,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(15,15,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,14,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(1,1,16,false,PoliceVehicleType.Marked,111,0,3,-1,-1,"",""),   
            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,16,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            
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
            DispatchableVehicles_FEJ.Create_PoliceAleutian(5,5,14,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),


            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(0, 40, 13, false, PoliceVehicleType.Marked, 134, 4, 5, 3, 4, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(0,50,15,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(0,40,17,false,PoliceVehicleType.Marked,-1,4,5,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(0,15,23,false,PoliceVehicleType.Marked,-1,4,5,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(0,15,14,false,PoliceVehicleType.Marked,-1,3,4,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(0,15,14,false,PoliceVehicleType.Marked,-1,3,4,3,4,"",""),

            new DispatchableVehicle("polmav", 0, 100) { RequiredLiveries = new List<int>() { 8 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
            new DispatchableVehicle("annihilator", 0, 100) { RequiredLiveries = new List<int>() { 7 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
        };
        NOOSESEPVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,15,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(5,5,23,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(35,35,16,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(35,35,18,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGauntlet(1,1,17,false,PoliceVehicleType.Marked,111,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(1,1,11,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"","",10),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(1,1,18,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(25,25,15,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(20,20,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceReblaGTS(15,15,2,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceInterceptor(0,25,16,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresley(0,25,18,false,PoliceVehicleType.Marked,-1,4,5,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(0,35,15,false,PoliceVehicleType.Marked,-1,3,4,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(0,35,15,false,PoliceVehicleType.Marked,-1,3,4,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceReblaGTS(0,15,2,false,PoliceVehicleType.Marked,134,3,4,3,4,"",""),

            new DispatchableVehicle("polmav", 0, 100) { RequiredLiveries = new List<int>() { 9 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
            new DispatchableVehicle("annihilator", 0, 100) { RequiredLiveries = new List<int>() { 6 },MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
        };


        MarshalsServiceVehicles_FEJ_Modern = new List<DispatchableVehicle>
        {
            DispatchableVehicles_FEJ.Create_PoliceFugitive(5, 5, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptor(35, 35, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceLandstalkerXL(20, 20, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceTerminus(2, 2, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", "", 20),
            DispatchableVehicles_FEJ.Create_PoliceGresley(35, 35, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceCaracara(2, 2, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(35, 35, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTX(20,20,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600(15, 15, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(15, 15, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceVSTR(15, 15, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceReblaGTS(1,1,3,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceRiata(2,2,7,true,PoliceVehicleType.Unmarked,-1,-1,-1,1,2,"","",4),

            DispatchableVehicles_FEJ.Create_PoliceVerus(0,0,2,false,PoliceVehicleType.Unmarked,-1,-1,-1,1,1,"","",25),

        };


    }


    private void Security()
    {
        MerryweatherPatrolVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,5,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,6,false,ServiceVehicleType.Security,-1,-1,-1),
            DispatchableVehicles_FEJ.DispatchableVehicles.AleutianSecurityMW,
            DispatchableVehicles_FEJ.DispatchableVehicles.AsteropeSecurityMW,
            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,0,false,ServiceVehicleType.Security,-1,-1,-1),
        };
        BobcatSecurityVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_ServiceDilettante(20,20,8,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(20,20,9,false,ServiceVehicleType.Security,-1,-1,-1),
            DispatchableVehicles_FEJ.DispatchableVehicles.AleutianSecurityBobCat,
            DispatchableVehicles_FEJ.DispatchableVehicles.AsteropeSecurityBobCat,
            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,3,false,ServiceVehicleType.Security,-1,-1,-1),
        };
        GroupSechsVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_ServiceDilettante(20,20,7,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(20,20,8,false,ServiceVehicleType.Security,-1,-1,-1),
            DispatchableVehicles_FEJ.DispatchableVehicles.AleutianSecurityG6,
            DispatchableVehicles_FEJ.DispatchableVehicles.AsteropeSecurityG6,
            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,2,false,ServiceVehicleType.Security,-1,-1,-1),
        };
        SecuroservVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_ServiceDilettante(20,20,6,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(20,20,7,false,ServiceVehicleType.Security,-1,-1,-1),
            DispatchableVehicles_FEJ.DispatchableVehicles.AleutianSecuritySECURO,
            DispatchableVehicles_FEJ.DispatchableVehicles.AsteropeSecuritySECURO,
            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,1,false,ServiceVehicleType.Security,-1,-1,-1),
        };

        LNLVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,5,false,ServiceVehicleType.Security,-1,-1,-1),
        };

        CHUFFVehicles_FEJ_Modern = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,4,false,ServiceVehicleType.Security,-1,-1,-1),
        };

    }
    private void Taxis()
    {
        DowntownTaxiVehicles_FEJ_Modern = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 5, 5){ RequiredLiveries = new List<int>() { 0 } },
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,1,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,1,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,1,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,1,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,0,false,ServiceVehicleType.Taxi1,134,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,0,false,ServiceVehicleType.Taxi2,134,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,0,false,ServiceVehicleType.Taxi3,134,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,0,false,ServiceVehicleType.Taxi4,134,-1,-1),

            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,4,false,ServiceVehicleType.Taxi1,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,4,false,ServiceVehicleType.Taxi2,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,4,false,ServiceVehicleType.Taxi3,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,4,false,ServiceVehicleType.Taxi4,-1,-1,-1),


            DispatchableVehicles_FEJ.DispatchableVehicles.TaxiBroadWay,
            DispatchableVehicles_FEJ.DispatchableVehicles.TaxiEudora,
        };
        PurpleTaxiVehicles_FEJ_Modern = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 5, 5){ RequiredLiveries = new List<int>() { 1 } },
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,2,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,2,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,10,2,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,2,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,1,false,ServiceVehicleType.Taxi1,134,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,5,1,false,ServiceVehicleType.Taxi2,134,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,1,false,ServiceVehicleType.Taxi3,134,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,1,false,ServiceVehicleType.Taxi4,134,-1,-1),

            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,1,false,ServiceVehicleType.Taxi1,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,1,false,ServiceVehicleType.Taxi2,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,1,false,ServiceVehicleType.Taxi3,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,1,false,ServiceVehicleType.Taxi4,-1,-1,-1),

        };
        HellTaxiVehicles_FEJ_Modern = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 5, 5){ RequiredLiveries = new List<int>() { 2 } },
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,0,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,0,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,0,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,0,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,2,false,ServiceVehicleType.Taxi1,134,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,2,false,ServiceVehicleType.Taxi2,134,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,2,false,ServiceVehicleType.Taxi3,134,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,2,false,ServiceVehicleType.Taxi4,134,-1,-1),

            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,0,false,ServiceVehicleType.Taxi1,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,0,false,ServiceVehicleType.Taxi2,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,0,false,ServiceVehicleType.Taxi3,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,0,false,ServiceVehicleType.Taxi4,-1,-1,-1),

        };
        ShitiTaxiVehicles_FEJ_Modern = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 5, 5){ RequiredLiveries = new List<int>() { 3 } },
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,3,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,3,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,3,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,3,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,3,false,ServiceVehicleType.Taxi1,0,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,3,false,ServiceVehicleType.Taxi2,0,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,3,false,ServiceVehicleType.Taxi3,0,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,3,false,ServiceVehicleType.Taxi4,0,-1,-1),

            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,3,false,ServiceVehicleType.Taxi1,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,3,false,ServiceVehicleType.Taxi2,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,3,false,ServiceVehicleType.Taxi3,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,3,false,ServiceVehicleType.Taxi4,-1,-1,-1),

        };
        SunderedTaxiVehicles_FEJ_Modern = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 5, 5){ RequiredLiveries = new List<int>() { 4 } },
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,4,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,4,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,4,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(10,10,4,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,4,false,ServiceVehicleType.Taxi1,134,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,4,false,ServiceVehicleType.Taxi2,134,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,4,false,ServiceVehicleType.Taxi3,134,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceInterceptor(5,5,4,false,ServiceVehicleType.Taxi4,134,-1,-1),

            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,2,false,ServiceVehicleType.Taxi1,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,2,false,ServiceVehicleType.Taxi2,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,2,false,ServiceVehicleType.Taxi3,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivanite(35,35,2,false,ServiceVehicleType.Taxi4,-1,-1,-1),


        };
    }

}

