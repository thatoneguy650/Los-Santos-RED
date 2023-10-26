# Los-Santos-RED
[![Pic1](https://i.imgur.com/t3Oq0iD.png)](https://i.imgur.com/t3Oq0iD.png)

> Enhanced and Customizable Free-Roam

![](https://img.shields.io/github/last-commit/thatoneguy650/Los-Santos-RED)

![](https://img.shields.io/github/commit-activity/w/thatoneguy650/Los-Santos-RED)

  Bored with the vanilla GTA 5 free roam mode? Wish there was more interaction and things to do? Hate how the cops will only ever kill and never arrest? Confused why civilians don't care that you are carrying a rocket launcher? Los Santos RED hopes to enhance the default criminal free roam experience by: adding an entirely new script based dispatching system, enhancing police/gang/civilian AI, adding stores and locations to visit, allowing you to perform actions and consume items you buy, giving you the ability to swap to any ped on the street and become that persona, adding new crimes that civilians and cops can report/arrest you for, enhancing the carjacking and lockpicking activities, adding and expanding civilian hold ups and interactions, completely rewriting the scanner system to be more immersive, and much more. The mod also aims to be extremely customizable by exposing tons of settings for the player to get the experience they want. Additional configuration settings are also available in the user editable xml files for even more customization.

## Installation
- Place all RAGE Plugin Hook files in the root GTA V directory
  - RAGE Plugin Hook Version 1.100 or later required, see [RPH Discord #necessities](https://discord.gg/z8N5P9MCRx) to get it since the main site is outdated

+ Place all [RAGENativeUI](https://github.com/alexguirre/RAGENativeUI) files in the root GTA V directory
  + RageNativeUI Version 1.9 or later required, be sure to update if you haven't
+ Download the latest release and drag into the root GTA V directory (NAudio is included in the package with LSR)
+ Requires a custom Gameconfig, Packfile Limit Adjuster, and Heap Adjuster!
  + Download an install based on the instructions provided. Links: [Gameconfig](https://github.com/pnwparksfan/gameconfig/releases) [Packfile Limit Adjuster](https://www.gta5-mods.com/tools/packfile-limit-adjuster) [Heap Adjuster](https://www.gta5-mods.com/tools/heapadjuster/)
- Verify RAGE Plugin Hook is installed to the root GTA V directory
- Verify NAudio.dll and NAudio.xml along with RageNativeUI.dll and RageNativeUI.xml are in the root GTA V directory
- Verify the Los Santos RED.dll, Los Santos RED.pdb, and LosSantosRED folder  are in the plugins folder (in the root GTA V directory)

+ To update the mod, remove the Los Santos RED.dll, Los Santos RED.pdb, and LosSantosRED folder from your Plugins folder and replace with the newest version

## Features
Enhanced Free Roam Mode
  - Police Enhancements
    - Dynamic difficulty 
    - Script based
    - Create custom agencies
        - Set their patrolling and responding areas
        - Set their weapons and vehicles
        - Set the max threat they respond to
     - Redone police scanner audio
     - Can respond to civilian and player called in crimes 
     - Can call in backup to both civilian and player chases
     - Built in EUP support with pre-made configs for EUP Basic (AlternateConfigs\EUP).
            - Also includes an EUP + new lore friendly agency vehicles config (AlternateConfigs\FullExpandedJurisdiction) if you need some vehicles to go along with the EUP uniforms.
     - Police will deploy dynamic roadblocks Including spike strips. They can appear on any road with any agencies vehicles
  - Gang Enhancements
    - Create custom gangs or make changes to the vanilla gangs
        - Set their territory
        - Set their weapons and vehicles
        - Set the type of items they sell on the street at at their safehouses
        - Many new gangs have been added and given territory around LS
    - Reputation with the player
        - Gangs will now remember your interactions with them
        - Friendly relations with a gang can lead to opportunities
        - Hostile gangs territory should be avoided
        - Certain interactions will increase rep (purchasing, chatting, etc.) and others will decrease (bringing heat, attacking members, etc.)
    - Join a Gang
        - Ability to join a gang and recruit fellow members to help out.
        - Gang members have benefits and responsibilities.
  - Busted/Wasted Choose Your Start
    - Added "undie" after death to call a mulligan and continue the chase
    - Added bribing of police
    - Added surrendering to police with choice of respawn location
    - Added ability to chose jail/hospital to respawn at
  - Stores and Shopping
    - New store types have been added around Los Santos
    - Restaurants, Food Vendors, Convenience Stores, Liquor Shop, and more give you the ability to purchase foods, drinks, and smokables.
    - Hotels allow you to quickly advance time and help you expire BOLOs
    - Underground gun shops and gang dens allow you to purchase weapons not found in AmmuNation
    - Car dealers can be used to purchase vehicles legitimately
    - Pharmacies, Head Shops, and Gang Dens all allow purchase of narcotics, both legal and illegal
    - Scrap Yards to get rid of unwanted vehicles and make a quick buck
  - Tasking Enhancements
    - Police and civilians are more reactive
    - Police react more realistically to escalation
    - Civilians react with fight or flight response instead of ignoring you
    - Added ability to be busted at greater than 1 star
    - Cops will now give foot chase and use tasers
    - Cops will now apprehend and arrest peds that are committing crimes
    - Civilians can react and call in other civilians crimes
    - Civilians can commit random crimes
    - Civilians can now be arrested and transferred to the station
    - Cops will now say more based on the current chase state
  - Crimes
    - Added/Tweaked vanilla crimes
    - Added other common crimes
    - Added traffic offenses
        - Felony speeding, reckless driving, hit-and-run, etc.
    - Fully configurable
        - Set who can report, wanted level gained, etc.
  - Investigations
    - Police will respond and investigate areas called in by civilians
    - The more serious the crime the faster the response
  - Interactions
    - Added Talking with any NPC in the world
        - Ability to react positively and negatively, RDR2 style
        - Different Civilian/Police groups react differently to antagonism
    - Added mugging civilians to get extra spending money
    - Added buying and selling from peds on the street. Certain peds will have items that they either want to buy or sell
  - Activities
    - Drinking and Smoking are now possible without being stuck in one place
    - Drinking/Drugs will cause intoxication, with similar effects to vanilla
    - Police and Civilians are aware of you being too intoxicated
    - Ability to steal and replace license plates on cars
    - Ability to surrender or commit suicide to quick end a chase
    - Ability to manually close your car door
    - Cigarettes are hazardous to your health
  - Carjacking Enhancements
    - Carjacking can be done with any weapon now
    - Ability to quickly execute the target when doing an armed carjacking
  - Car Lock-Picking Enhancements
    - Most cars are now locked and require picking to enter along with hotwiring to start
    - Traditional smash-and-grab is still available however cops are now on the lookout for suspicious vehicles (broken windows, smashed headlights, etc.)
  - Ped Swapping
    - Become any ped you come across on the street
        - Auto generated name, criminal record, vehicle, and weapon
        - Spend money! Buy a soda! Go to Ammunation! The game will treat the ped as a main character and allow you to use money as expected
    - Become a completely customized ped
        - Customize Ped wizard allows you to create a ped from scratch 
        - Choose model, components, props, head data, name, money and more
        - Save and load your custom ped at will 
  - Sprinting
    - Can sprint for a short period of time to give you a slight speed boost over regular running
    - When running press Z to start sprinting
    - The red bar under your health shows your current sprint stamina
  - Intoxication
    - Certain purchased items that you ingest will cause intoxication
    - Effects are different from different substances
    - The blue bar under your armor will show current intoxication level
    - Cops and civilians are aware and will react if you are too intoxicated
  - Cell phone
    - Interact with gang members and others through the added contacts and text messages on your phone
    - Items also available on the Player Information Menu
  - Owned Vehicles
    - Purchased vehicles and vehicles you designate as "owned" will not be removed from the game world
    - They will always be unlocked for you and will have a custom blip on the map
    - You can add as many owned vehicles as you wish
    - Save vehicles and locations with the Save/Load feature on the main menu
  - Weapon Enhancements
    - Enhanced Recoil 
    - Added Sway
    - Added Fire Mode Selection
  - Vehicle Enhancements
    - Button press indicators
    - Button press ignition
    - Disabled auto vehicle start 
    - Auto window roll down when doing a drive-by
    - Manual door close
    - Forced first person on duck
    - Automatic radio tuning
- Fully Adjustable Settings
  - Configure almost any setting with the XML files
- Plus lots more

## Default Controls
- F10 - Main Menu
- F11 - Debug Menu
- Mouse4 - Action Wheel Menu
- Shift + E - Surrender
- Shift + X - Change Fire Mode Selection
- Z (When Running) - Start Sprinting
- Shift + E (In Car) - Right Blinker
- Shift + Q (In Car) - Left Blinker
- Shift + Space (In Car) - Hazards
- Shift + Z (In Car) - Toggle Engine
- LCtrl (In Car) - Manual Door Close
- Vehicle Enter (Occupied, Press, w/ Weapon) - Carjack With Weapon
- Vehicle Enter (Occupied, Hold) - Regular Carjack
- Vehicle Enter (Empty, Press) - Lockpick
- Vehicle Enter (Empty, Hold) - Smash Window
(others should have prompts)

## Discord
[Greskrendtregk Mods](https://discord.gg/7zaZhKervU)

## License
[![GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)

## Credits
- [alexguirre](https://github.com/alexguirre)
- [alloc8tor](https://github.com/alloc8or)
- [pongo1231](https://github.com/pongo1231)
- [Eddlm](https://github.com/Eddlm)
- [Foxunitone](https://www.gta5-mods.com/users/Foxunitone)
- [RAGE Plugin Hook Team](https://ragepluginhook.net/About.aspx)
- [GTA Forums](https://gtaforums.com/)
- [Bob74](https://github.com/Bob74)
- [DisapprovedByMe](https://github.com/DisapprovedByMe)
- [Xinerki](https://github.com/Xinerki)
- PeterBadoingy
- box

## Compatibility
- Incompatible with
  - Menyoo can cause item duplication
  - Any police script or wanted level changing mod
  - Can you make it work with XXX? No.
- Do not save and turn off autosave
  
## Complementary Mods
[Add On Plates (AlternateConfigs\AddOnPlates_Wildbrick142)](https://www.gta5-mods.com/paintjobs/new-license-plates-add-on)  
[True Realistic Driving V](https://www.gta5-mods.com/scripts/true-realistic-driving-v-realistic-mass-v0-1-beta)

## Issues
- Running without a custom gameconfig can lead to issues with vehicle and ped spawning.
- You will need to rebuy components if you remove them at the underground gunshops, gang dens, or drug dealers.
- Vanilla taxi service is not available with LSR active (Downtown Cab Co on the vanilla cell will show Busy forever) use the burner cell to call the LSR taxi service.
- Loading bodies in the trunk can fail or not attach properly.
