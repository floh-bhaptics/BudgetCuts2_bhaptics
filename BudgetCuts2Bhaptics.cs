using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using MyBhapticsTactsuit;

/*
CrushingtBulbStateAdamBC2
FightingStateAdamBC2
HidingStateAdamBC2
InspectingStateAdamBC2
Interactable
NPC
NPCAdamBC2
Player
PlayerHand
SearchingStateAdamBC2
TrainWind.Outdoors
BombHitGroundTrigger.BombHitGroundTrigger
NPCAdamNoLegs.GetCrushed
NPCAdamBC2.FallIntoShredder
AdamBC2State.SelectAggressiveState
 */
[assembly: MelonInfo(typeof(BudgetCuts2Bhaptics.BudgetCuts2Bhaptics), "BudgetCuts2Bhaptics", "1.0.1", "Florian Fahrenberger")]
[assembly: MelonGame("Neat Corporation", "Budget Cuts 2")]


namespace BudgetCuts2Bhaptics
{
    public class BudgetCuts2Bhaptics : MelonMod
    {
        public static TactsuitVR tactsuitVr = null!;

        public override void OnInitializeMelon()
        {
            //base.OnApplicationStart();
            tactsuitVr = new TactsuitVR();
            tactsuitVr.PlaybackHaptics("HeartBeat");
        }
        /*
                [HarmonyPatch(typeof(BombHitGroundTrigger), "BombHitGroundTrigger")]
                public class bhaptics_BombHitGroundTrigger
                {
                    [HarmonyPostfix]
                    public static void Postfix()
                    {
                        tactsuitVr.PlaybackHaptics("Grenade");
                    }
                }
        */
        [HarmonyPatch(typeof(Player), "OnCrushed")]
        public class bhaptics_PlayerStrangled
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopAllHapticFeedback();
                tactsuitVr.PlaybackHaptics("CaughtByAdam");
            }
        }

        [HarmonyPatch(typeof(Player), "OnStrangle")]
        public class bhaptics_PlayerCrushed
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopAllHapticFeedback();
                tactsuitVr.PlaybackHaptics("CaughtByAdam");
            }
        }

        [HarmonyPatch(typeof(LevelTriggerCheckpoint), "TriggerEnter")]
        public class bhaptics_LevelCheckpoint
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopHeartBeat();
                tactsuitVr.StopNeckTingle();
                //tactsuitVr.PlaybackHaptics("StrangleHold");
            }
        }

        [HarmonyPatch(typeof(TutorialObjectBC2), "OnFinishedTutorial")]
        public class bhaptics_FinishedTutorial
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopHeartBeat();
                tactsuitVr.StopNeckTingle();
                //tactsuitVr.PlaybackHaptics("StrangleHold");
            }
        }

        [HarmonyPatch(typeof(NPCAdamNoLegs), "GetCrushed")]
        public class bhaptics_AdamDies
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopHeartBeat();
                tactsuitVr.StopNeckTingle();
            }
        }

        [HarmonyPatch(typeof(ChasingStateAdamBC2), "EnterState")]
        public class bhaptics_AdamChasingStart
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StartHeartBeat();
            }
        }

        [HarmonyPatch(typeof(RidingElevatorStateAdamBC2), "EnterState")]
        public class bhaptics_AdamElevatorStart
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StartHeartBeat();
            }
        }

        [HarmonyPatch(typeof(SearchingStateAdamBC2), "EnterState")]
        public class bhaptics_AdamSearchingStart
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopHeartBeat();
                tactsuitVr.StartNeckTingle();
            }
        }

        [HarmonyPatch(typeof(SearchingStateAdamBC2), "ExitState")]
        public class bhaptics_AdamSearchingStop
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopNeckTingle();
            }
        }

        [HarmonyPatch(typeof(FabLightBulb), "OnDestroy")]
        public class bhaptics_AdamCrushingBulb
        {
            [HarmonyPostfix]
            public static void Postfix(FabLightBulb __instance)
            {
                if ((__instance.applicationQuitting)|(!__instance.lit)) {
                    return;
                }
                tactsuitVr.PlaybackHaptics("Stomp");
            }
        }

        [HarmonyPatch(typeof(TimedBomb), "DetectedTampering")]
        public class bhaptics_BombTimer
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StartHeartBeat();
            }
        }

        [HarmonyPatch(typeof(TimedBomb), "BombNowOutOfReachOfPlayer")]
        public class bhaptics_BombOutOfReach
        {
            [HarmonyPostfix]
            public static void Postfix(TimedBomb __instance)
            {
                tactsuitVr.StopHeartBeat();
                tactsuitVr.StopNeckTingle();
                // bool playerSafe = false;
                // try { playerSafe = __instance.isPlayerSafeFromExplosion;  tactsuitVr.LOG("Playxersafe? " + playerSafe); } catch { }
                // if (playerSafe) { tactsuitVr.PlaybackHaptics("Stomp"); }
                // else { tactsuitVr.PlaybackHaptics("Grenade"); }
            }
        }

        [HarmonyPatch(typeof(VaultDoor), "OpenSesame")]
        public class bhaptics_OpenVaultDoor
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.LOG("OpenSesame");
                tactsuitVr.PlaybackHaptics("Stomp", 1.0f, 3.0f);
            }
        }

        [HarmonyPatch(typeof(ChasingStateBSB), "EnterState")]
        public class bhaptics_BSBChasingStart
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StartNeckTingle();
            }
        }


        [HarmonyPatch(typeof(ChasingStateBSB), "ExitState")]
        public class bhaptics_BSBChasingStop
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopNeckTingle();
            }
        }

        [HarmonyPatch(typeof(ChasingStateHornet), "EnterState")]
        public class bhaptics_HornetChasingStart
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StartNeckTingle();
            }
        }


        [HarmonyPatch(typeof(ChasingStateHornet), "ExitState")]
        public class bhaptics_HornetChasingStop
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopNeckTingle();
            }
        }

        [HarmonyPatch(typeof(ChasingStateRSB), "EnterState")]
        public class bhaptics_RSBChasingStart
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StartNeckTingle();
            }
        }


        [HarmonyPatch(typeof(ChasingStateRSB), "ExitState")]
        public class bhaptics_RSBChasingStop
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopNeckTingle();
            }
        }

        [HarmonyPatch(typeof(Bow), "StartNocking")]
        public class bhaptics_BowStartNocking
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("BowTension");
            }
        }


        [HarmonyPatch(typeof(Bow), "StopNocking")]
        public class bhaptics_BowStopNocking
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopHapticFeedback("BowTension");
            }
        }


        [HarmonyPatch(typeof(ToolSelector), "OnEnable")]
        public class bhaptics_OpenTools
        {
            [HarmonyPostfix]
            public static void Postfix(ToolSelector __instance)
            {
                bool right_hand = false;
                // On Startup and new levels, ToolSelector will be enabled, but there is no instance yet
                try { right_hand = __instance.handle.IsRightHand; } catch { return; }
                if (right_hand) { tactsuitVr.PlaybackHaptics("HandSelector_R"); }
                else { tactsuitVr.PlaybackHaptics("HandSelector_L"); }
                
            }
        }

        [HarmonyPatch(typeof(ToolSelector), "OnDisable")]
        public class bhaptics_CloseTools
        {
            [HarmonyPostfix]
            public static void Postfix(ToolSelector __instance)
            {
                if (__instance.handle.IsRightHand) { tactsuitVr.StopHapticFeedback("HandSelector_R"); }
                else { tactsuitVr.StopHapticFeedback("HandSelector_L"); }
            }
        }


        [HarmonyPatch(typeof(Inventory), "OnEnable")]
        public class bhaptics_OpenInventory
        {
            [HarmonyPostfix]
            public static void Postfix(Inventory __instance)
            {
                bool right_hand = false;
                // On Startup and new levels, ToolSelector will be enabled, but there is no instance yet
                try { right_hand = __instance.handle.IsRightHand; } catch { return; }
                if (right_hand) { tactsuitVr.PlaybackHaptics("HandSelector_R"); }
                else { tactsuitVr.PlaybackHaptics("HandSelector_L"); }
            }
        }

        [HarmonyPatch(typeof(Inventory), "Close")]
        public class bhaptics_CloseInventory
        {
            [HarmonyPostfix]
            public static void Postfix(Inventory __instance)
            {
                if (__instance.handleLastUsed.IsRightHand) { tactsuitVr.StopHapticFeedback("HandSelector_R"); }
                else { tactsuitVr.StopHapticFeedback("HandSelector_L"); }
            }
        }

        [HarmonyPatch(typeof(Bow), "LaunchArrow")]
        public class bhaptics_BowLaunchArrow
        {
            [HarmonyPostfix]
            public static void Postfix(Bow __instance, float stretchAmount)
            {
                tactsuitVr.StopHapticFeedback("BowTension");
                if (__instance.handle.IsRightHand) { tactsuitVr.PlaybackHaptics("BowRelease_R"); }
                else { tactsuitVr.PlaybackHaptics("BowRelease_L"); }
            }
        }


        [HarmonyPatch(typeof(Player), "OnHitInTheFace")]
        public class bhaptics_HitInTheFace
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopHeartBeat();
                tactsuitVr.StopNeckTingle();
                if (bHapticsLib.bHapticsManager.IsDeviceConnected(bHapticsLib.PositionID.Head))
                { tactsuitVr.PlaybackHaptics("HitInTheFace"); }
                else { tactsuitVr.PlaybackHaptics("BulletHit"); }
            }
        }

        [HarmonyPatch(typeof(Player), "OnHitByDeadlyObject")]
        public class bhaptics_HitByDeadlyObject
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopHeartBeat();
                tactsuitVr.StopNeckTingle();
                tactsuitVr.PlaybackHaptics("Dying");
            }
        }

        [HarmonyPatch(typeof(Player), "Die")]
        public class bhaptics_Dying
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopHeartBeat();
                tactsuitVr.StopNeckTingle();
                tactsuitVr.PlaybackHaptics("Dying");
            }
        }

        [HarmonyPatch(typeof(Grenade), "OnHitByGrenade")]
        public class bhaptics_HitByGrenade
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopHeartBeat();
                tactsuitVr.StopNeckTingle();
                tactsuitVr.PlaybackHaptics("Grenade");
            }
        }



        [HarmonyPatch(typeof(Gun), "Fire")]
        public class bhaptics_GunFired
        {
            [HarmonyPostfix]
            public static void Postfix(Gun __instance)
            {
                if (__instance.handle.IsRightHand)
                {
                    tactsuitVr.PlaybackHaptics("Recoil_R");
                    tactsuitVr.PlaybackHaptics("RecoilVest_R");
                }
                else
                {
                    tactsuitVr.PlaybackHaptics("Recoil_L");
                    tactsuitVr.PlaybackHaptics("RecoilVest_R");
                }
                //collisionDispatch.bodyPart.ToString();
            }
        }

        [HarmonyPatch(typeof(WeaponRevolver), "ActuallyFireGun")]
        public class bhaptics_RevolverFired
        {
            [HarmonyPostfix]
            public static void Postfix(WeaponRevolver __instance)
            {
                if (__instance.grabberHoldingThis.handle.IsRightHand)
                {
                    tactsuitVr.PlaybackHaptics("Recoil_R");
                    tactsuitVr.PlaybackHaptics("RecoilVest_R");
                }
                else
                {
                    tactsuitVr.PlaybackHaptics("Recoil_L");
                    tactsuitVr.PlaybackHaptics("RecoilVest_R");
                }
                //collisionDispatch.bodyPart.ToString();
            }
        }

        [HarmonyPatch(typeof(Portal), "OpenCor")]
        public class bhaptics_OpenCor
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("PortalPullLong");
            }
        }

        [HarmonyPatch(typeof(Portal), "CloseCor")]
        public class bhaptics_CloseCor
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopHapticFeedback("PortalPullLong");
            }
        }

        [HarmonyPatch(typeof(Portal), "TranslocateCor")]
        public class bhaptics_TranslocateCor
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("GoThroughPortal");
            }
        }

    }
}
