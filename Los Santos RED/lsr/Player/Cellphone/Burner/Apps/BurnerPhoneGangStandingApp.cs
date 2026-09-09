using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Your standing with the gang, and the people who work for you.
///
/// This began as a development instrument behind #if DEBUG and earned its way into the
/// build. The reason is the event log: standing and goodwill are totals, and a total tells
/// you nothing about WHY it moved. Every design question this feature has raised was
/// answered by reading a stream of reasons — and a player wants that for the same reason a
/// developer did.
///
/// Modelled on BurnerPhoneMapsApp, the one app that drives RAGENativeUI rather than the
/// phone scaleform directly.
/// </summary>
public class BurnerPhoneGangStandingApp : BurnerPhoneApp
{
    private MenuPool MenuPool;
    private UIMenu StandingMenu;
    private UIMenu CrewMenu;
    private readonly Dictionary<int, UIMenu> MemberMenus = new Dictionary<int, UIMenu>();

    public BurnerPhoneGangStandingApp(BurnerPhone burnerPhone, ICellPhoneable player, ITimeReportable time, ISettingsProvideable settings, int index)
        : base(burnerPhone, player, time, settings, index, "Crew", 5)
    {
        MenuPool = new MenuPool();
        StandingMenu = new UIMenu("Crew", "Where you stand, and who stands with you");
        MenuPool.Add(StandingMenu);
    }

    public override void Open(bool Reset)
    {
        BuildMenu();
        StandingMenu.Visible = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                while (MenuPool.IsAnyMenuOpen())
                {
                    GameFiber.Yield();
                }
                Player.CellPhone.Close(250);
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "GangStandingApp");
    }

    public override void Update()
    {
        if (MenuPool != null)
        {
            MenuPool.ProcessMenus();
        }
    }

    private Gang CurrentGang => Player?.RelationshipManager?.GangRelationships?.CurrentGang;

    private void BuildMenu()
    {
        StandingMenu.Clear();
        MemberMenus.Clear();

        if (Player?.GangProgressionManager == null
            || Settings?.SettingsManager?.GangProgressionSettings?.EnableGangProgression != true)
        {
            StandingMenu.AddItem(Readonly("Unavailable", "Gang progression is switched off."));
            return;
        }
        Gang gang = CurrentGang;
        if (gang == null)
        {
            StandingMenu.AddItem(Readonly("Not a member", "You aren't running with anybody."));
            return;
        }

        AddStatus(gang);
        AddCrewSection(gang);
        AddEventLog();
    }

    private void AddStatus(Gang gang)
    {
        GangStandingLevel level = Player.GangProgressionManager.GetLevel(gang);
        GangStandingLevel next = Player.GangProgressionManager.GetNextLevel(gang);
        int toNext = Player.GangProgressionManager.GetStandingToNextLevel(gang);

        StandingMenu.AddItem(Readonly("Gang", gang.ShortName ?? gang.ID));
        StandingMenu.AddItem(Readonly("Rank", level == null ? "-" : level.Name));
        StandingMenu.AddItem(Readonly("Standing", next == null
            ? $"{Player.GangProgressionManager.GetStanding(gang)} (top)"
            : $"{Player.GangProgressionManager.GetStanding(gang)} — {toNext} to {next.Name}"));
        StandingMenu.AddItem(Readonly("Goodwill", Player.GangProgressionManager.GetGoodwill(gang).ToString()));
    }
    /// <summary>
    /// The roster, with a submenu per man showing what he has been through.
    ///
    /// Dead men stay listed. That is the point of permadeath — a name you can no longer
    /// call, with the history that got him killed still attached, says more than quietly
    /// removing the row.
    /// </summary>
    private void AddCrewSection(Gang gang)
    {
        GangCrewManager crew = Player.GangCrewManager;
        if (crew == null || !crew.IsEnabled)
        {
            return;
        }
        // The dead have their own screen. Mixing them into the roster made the living list
        // longer every time somebody died, which is backwards — a loss should shorten the
        // list you work from, not lengthen it.
        List<GangCrewMember> roster = crew.AllMembers
            .Where(x => x != null && x.GangID == gang.ID && !x.IsDead)
            .OrderByDescending(x => x.Experience)
            .ToList();

        AddMemorialSection(gang);

        if (!roster.Any())
        {
            StandingMenu.AddItem(Readonly("Crew", "Nobody yet"));
            return;
        }

        DateTime now = Time.CurrentDateTime;
        int available = roster.Count(x => x.IsAvailableAt(now));
        StandingMenu.AddItem(Readonly("Crew", $"{available} around"));

        foreach (GangCrewMember member in roster)
        {
            string status = member.IsJailedAt(now) ? "~o~inside~s~" : "~g~around~s~";
            UIMenu memberMenu = MenuPool.AddSubMenu(StandingMenu, member.Name);
            StandingMenu.MenuItems[StandingMenu.MenuItems.Count() - 1].RightLabel = status;
            StandingMenu.MenuItems[StandingMenu.MenuItems.Count() - 1].Description = $"{member.Rank}, out with you {member.TimesDispatched}x";
            memberMenu.RemoveBanner();
            BuildMemberMenu(memberMenu, member, now);
            MemberMenus[member.ID] = memberMenu;
        }
    }

    /// <summary>
    /// The men you lost.
    ///
    /// Its own screen rather than red entries in the roster, and capped: a memorial that
    /// grows without limit stops being a memorial and becomes a spreadsheet. Beyond the cap
    /// the oldest unpinned man is dropped, but TotalCrewLost is persisted separately, so the
    /// number never lies even when the names are gone.
    ///
    /// The player chooses who stays. That is the whole point — the men worth remembering are
    /// not necessarily the most recent ones.
    /// </summary>
    private void AddMemorialSection(Gang gang)
    {
        GangCrewManager crew = Player.GangCrewManager;
        if (crew == null || !crew.MemorialEnabled)
        {
            return;
        }
        List<GangCrewMember> fallen = crew.TheFallen().Where(x => x.GangID == gang.ID).ToList();
        if (crew.TotalCrewLost <= 0 && !fallen.Any())
        {
            return;
        }

        UIMenu memorialMenu = MenuPool.AddSubMenu(StandingMenu, "The Fallen");
        StandingMenu.MenuItems[StandingMenu.MenuItems.Count() - 1].RightLabel = $"~r~{crew.TotalCrewLost}~s~";
        StandingMenu.MenuItems[StandingMenu.MenuItems.Count() - 1].Description = "The ones who didn't come back.";
        memorialMenu.RemoveBanner();

        memorialMenu.AddItem(Readonly("Buried", crew.TotalCrewLost.ToString()));
        if (!fallen.Any())
        {
            memorialMenu.AddItem(Readonly("No names left", "Been too long."));
            return;
        }

        foreach (GangCrewMember member in fallen)
        {
            UIMenu graveMenu = MenuPool.AddSubMenu(memorialMenu, member.Name);
            memorialMenu.MenuItems[memorialMenu.MenuItems.Count() - 1].RightLabel =
                member.IsRemembered ? "~y~kept~s~" : $"{member.DiedDate:M/d}";
            memorialMenu.MenuItems[memorialMenu.MenuItems.Count() - 1].Description =
                $"{member.Rank}, {member.JobsCompleted} jobs with you";
            graveMenu.RemoveBanner();
            BuildGraveMenu(graveMenu, member, crew);
        }
    }

    private void BuildGraveMenu(UIMenu graveMenu, GangCrewMember member, GangCrewManager crew)
    {
        graveMenu.AddItem(Readonly("Reputation", member.Rank));
        graveMenu.AddItem(Readonly("Jobs", member.JobsCompleted.ToString()));
        graveMenu.AddItem(Readonly("Times out", member.TimesDispatched.ToString()));
        graveMenu.AddItem(Readonly("Brought in", $"${member.TributeEarnedLifetime}"));
        graveMenu.AddItem(Readonly("Died", member.DiedDate == DateTime.MinValue ? "-" : $"{member.DiedDate:MMM d}"));
        if (!string.IsNullOrEmpty(member.DeathCause))
        {
            graveMenu.AddItem(Readonly("How", member.DeathCause));
        }
        if (!string.IsNullOrEmpty(member.DeathZone))
        {
            graveMenu.AddItem(Readonly("Where", member.DeathZone));
        }
        if (member.FirstMetDate != DateTime.MinValue)
        {
            graveMenu.AddItem(Readonly("Since", $"{member.FirstMetDate:MMM d}"));
        }

        UIMenuItem keep = new UIMenuItem(member.IsRemembered ? "Let him go" : "Don't forget him",
            member.IsRemembered ? "You'll remember this one." : "Keep his name when the rest fade.");
        keep.Activated += (sender, selected) =>
        {
            string result = crew.ToggleRemembered(member);
            keep.Text = member.IsRemembered ? "Let him go" : "Don't forget him";
            keep.Description = member.IsRemembered ? "You'll remember this one." : "Keep his name when the rest fade.";
            if (!string.IsNullOrEmpty(result))
            {
                Game.DisplaySubtitle(result);
            }
        };
        graveMenu.AddItem(keep);
    }

    /// <summary>
    /// What a reputation buys: a name, and then a face.
    ///
    /// Both items stay VISIBLE and refuse with a reason when locked, following the den's
    /// idiom rather than hiding them — a player who cannot see that naming a man is possible
    /// has no reason to level one up, and the whole point of this is to be an incentive.
    ///
    /// The keyboard call blocks the fiber until the player is done typing, so the menu is
    /// closed first; leaving it drawn while the on-screen keyboard owns input strands both.
    /// </summary>
    private void AddCustomisationItems(UIMenu memberMenu, GangCrewMember member)
    {
        GangCrewManager crew = Player.GangCrewManager;
        if (crew == null || !crew.CustomisationEnabled || member == null || member.IsDead)
        {
            return;
        }

        bool canRename = crew.CanRename(member);
        UIMenuItem rename = new UIMenuItem("Give him a name",
            canRename ? "Whatever you call him is what sticks." : crew.RenameLockedReason());
        rename.Enabled = canRename;
        rename.Activated += (sender, selected) =>
        {
            memberMenu.Visible = false;
            string entered = NativeHelper.GetKeyboardInput(member.Name ?? "");
            Game.DisplaySubtitle(crew.Rename(member, entered));
        };
        memberMenu.AddItem(rename);

        bool canReskin = crew.CanReskin(member);
        List<string> models = canReskin ? crew.AvailableModelsFor(member) : new List<string>();
        if (!canReskin || models.Count <= 1)
        {
            UIMenuItem locked = new UIMenuItem("Change his look",
                !canReskin ? crew.ReskinLockedReason() : "Everybody round here looks the same.");
            locked.Enabled = false;
            memberMenu.AddItem(locked);
            return;
        }

        UIMenuListScrollerItem<string> faceScroller = new UIMenuListScrollerItem<string>("His look",
            "Only faces the outfit actually puts on the street.", models);
        int current = models.FindIndex(x => x.Equals(member.ModelName, StringComparison.OrdinalIgnoreCase));
        if (current >= 0)
        {
            faceScroller.Index = current;
        }
        memberMenu.AddItem(faceScroller);

        UIMenuItem applyFace = new UIMenuItem("Change his look", "He'll show up like that next time.");
        Action refreshFace = () =>
        {
            string picked = faceScroller.SelectedItem;
            GangCrewMember clash = crew.OtherMemberWearing(member, picked);
            applyFace.Description = clash != null
                ? $"~o~{clash.Name} already looks like that~s~ - only one of them turns up."
                : "He'll show up like that next time.";
        };
        faceScroller.IndexChanged += (sender, oldIndex, newIndex) => refreshFace();
        refreshFace();
        applyFace.Activated += (sender, selected) =>
        {
            Game.DisplaySubtitle(crew.Reskin(member, faceScroller.SelectedItem));
        };
        memberMenu.AddItem(applyFace);
    }

    private void BuildMemberMenu(UIMenu memberMenu, GangCrewMember member, DateTime now)
    {
        memberMenu.AddItem(Readonly("Reputation", member.Rank));
        memberMenu.AddItem(Readonly("Experience", member.Experience.ToString()));
        int fightingPercent = Player.GangCrewManager == null ? 100 : Player.GangCrewManager.FightingPercentFor(member);
        memberMenu.AddItem(Readonly("Trusted", fightingPercent >= 100 ? "All the way" : "Not all the way"));
        memberMenu.AddItem(Readonly("Holding", $"${member.Tribute}"));
        memberMenu.AddItem(Readonly("Times out", member.TimesDispatched.ToString()));
        memberMenu.AddItem(Readonly("Since", member.FirstMetDate == DateTime.MinValue ? "-" : member.FirstMetDate.ToString("MMM d")));
        memberMenu.AddItem(Readonly("Status", member.IsDead ? "Dead"
            : member.IsJailedAt(now) ? $"Inside til {member.JailedUntilDate:M/d}"
            : "Around"));

        AddCustomisationItems(memberMenu, member);

        if (member.Events == null || !member.Events.Any())
        {
            memberMenu.AddItem(Readonly("History", "Nothing yet."));
            return;
        }
        memberMenu.AddItem(Readonly("History", $"{member.Events.Count}"));
        foreach (GangCrewEvent crewEvent in member.Events)
        {
            // Title carries the text alone. Prefixing the timestamp cost 16 of the ~30
            // characters that render before a line runs into its own right label.
            memberMenu.AddItem(Readonly($"  {crewEvent.Text}", $"{crewEvent.When:M/d}"));
        }
    }

    private void AddEventLog()
    {
        List<GangStandingEvent> events = Player.GangProgressionManager.EventLog;
        if (events == null || !events.Any())
        {
            StandingMenu.AddItem(Readonly("Recent", "Nothing yet."));
            return;
        }
        StandingMenu.AddItem(Readonly("Recent", $"{events.Count}"));
        foreach (GangStandingEvent standingEvent in events)
        {
            string delta = standingEvent.StandingDelta == 0
                ? (standingEvent.GoodwillDelta >= 0 ? $"+{standingEvent.GoodwillDelta} gw" : $"{standingEvent.GoodwillDelta} gw")
                : (standingEvent.StandingDelta > 0 ? $"+{standingEvent.StandingDelta}" : standingEvent.StandingDelta.ToString());
            StandingMenu.AddItem(Readonly($"  {standingEvent.GameTime:HH:mm} {standingEvent.Reason}", delta));
        }
    }

    private static UIMenuItem Readonly(string text, string rightLabel)
    {
        return new UIMenuItem(text) { RightLabel = rightLabel, Enabled = false };
    }
}
