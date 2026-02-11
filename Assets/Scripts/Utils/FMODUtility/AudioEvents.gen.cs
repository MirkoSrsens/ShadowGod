// Auto generated script, do not modify!
// 
// Asset: "/Assets/Scripts/Utils/FMODUtility/"
// Unity: 6000.1.3f1
// .NET: 4.0.30319.42000

using UnityEngine;
using FMODUnity;

namespace Razorhead.Core
{
    public static class AudioEvents
    {
        private static EventReference Create(int a, int b, int c, int d)
        {
            return new() { Guid = new FMOD.GUID() { Data1 = a, Data2 = b, Data3 = c, Data4 = d } };
        }

        public static class Music
        {
            public static class Background
            {
                /// <summary>event:/Music/Background/BackgroundLoopDefault</summary>
                public static readonly EventReference @BackgroundLoopDefault = Create(55806069, 1258512533, 1792181644, -1147817040);

            }
        }

        public static class SFX
        {
            public static class Heroes
            {
                public static class Hades
                {
                    /// <summary>event:/SFX/Heroes/Hades/HadesActiveAbilityDefault</summary>
                    public static readonly EventReference @HadesActiveAbilityDefault = Create(-1849902325, 1275458655, -1899641690, 938670769);

                    /// <summary>event:/SFX/Heroes/Hades/HadesSpecialTilesDefault</summary>
                    public static readonly EventReference @HadesSpecialTilesDefault = Create(1968772218, 1338378458, 1241949857, 1541041645);

                }

                public static class Rapunzel
                {
                    /// <summary>event:/SFX/Heroes/Rapunzel/RapunzelPassiveAbilityDefault</summary>
                    public static readonly EventReference @RapunzelPassiveAbilityDefault = Create(-76364701, 1274762259, -1830774092, -174445647);

                    /// <summary>event:/SFX/Heroes/Rapunzel/RapunzelActiveAbilityDefault</summary>
                    public static readonly EventReference @RapunzelActiveAbilityDefault = Create(778148590, 1228501680, -1421189721, -1276302687);

                }

                public static class Beast
                {
                    /// <summary>event:/SFX/Heroes/Beast/BeastActiveAbilityDefault</summary>
                    public static readonly EventReference @BeastActiveAbilityDefault = Create(-1339682391, 1180592860, 202477984, -15953460);

                }

                public static class WallE
                {
                    /// <summary>event:/SFX/Heroes/WallE/WallEActiveAbilityDefault</summary>
                    public static readonly EventReference @WallEActiveAbilityDefault = Create(-173816146, 1337575718, -706974292, 1282080337);

                }

                public static class Stitch
                {
                    /// <summary>event:/SFX/Heroes/Stitch/StitchAtiveAbilityDefault</summary>
                    public static readonly EventReference @StitchAtiveAbilityDefault = Create(-630804491, 1275344412, -1497427071, -582551867);

                }
            }

            public static class Biomes
            {
                public static class Default
                {
                    /// <summary>event:/SFX/Biomes/Default/Match5Default</summary>
                    public static readonly EventReference @Match5Default = Create(968262931, 1268973668, -2145967948, 1050959092);

                    /// <summary>event:/SFX/Biomes/Default/Match4Default</summary>
                    public static readonly EventReference @Match4Default = Create(-301282029, 1325938672, 1269742740, 2134812645);

                    /// <summary>event:/SFX/Biomes/Default/ExtraTurnDefault</summary>
                    public static readonly EventReference @ExtraTurnDefault = Create(1254700852, 1138166789, 266247304, 1308667518);

                    /// <summary>event:/SFX/Biomes/Default/RowShatterDefault</summary>
                    public static readonly EventReference @RowShatterDefault = Create(-2048002245, 1324657665, 1497627820, -92142769);

                    /// <summary>event:/SFX/Biomes/Default/EnemyDamageDefault</summary>
                    public static readonly EventReference @EnemyDamageDefault = Create(1351172692, 1149099804, -64638560, 1699275738);

                    /// <summary>event:/SFX/Biomes/Default/PlayerDamageDefault</summary>
                    public static readonly EventReference @PlayerDamageDefault = Create(1609898415, 1075693050, 125314181, -1689581424);

                    /// <summary>event:/SFX/Biomes/Default/Match3Default</summary>
                    public static readonly EventReference @Match3Default = Create(-1681735496, 1234241085, 505839237, -854806625);

                    /// <summary>event:/SFX/Biomes/Default/InvalidSwapDefault</summary>
                    public static readonly EventReference @InvalidSwapDefault = Create(1975724009, 1149214187, -1348066658, -977645056);

                }

                public static class Ability
                {
                    /// <summary>event:/SFX/Biomes/Ability/ActionPointGainDefault</summary>
                    public static readonly EventReference @ActionPointGainDefault = Create(-2020353585, 1099170760, -1887165763, 1041005803);

                }
            }

            public static class Gameboard
            {
                /// <summary>event:/SFX/Gameboard/LevelStartDefault</summary>
                public static readonly EventReference @LevelStartDefault = Create(187409744, 1083838931, -246223974, 1093367101);

                /// <summary>event:/SFX/Gameboard/RewardGrantedDefault</summary>
                public static readonly EventReference @RewardGrantedDefault = Create(-848566430, 1263977458, -1810849663, -1828861653);

                /// <summary>event:/SFX/Gameboard/LevelSuccessDefault</summary>
                public static readonly EventReference @LevelSuccessDefault = Create(694856648, 1206657647, -1050867574, -1284053346);

                /// <summary>event:/SFX/Gameboard/LevelFailedDefault</summary>
                public static readonly EventReference @LevelFailedDefault = Create(1682855648, 1176545101, -513030992, 123101977);

            }

            public static class UI
            {
                /// <summary>event:/SFX/UI/ButtonTapDefault</summary>
                public static readonly EventReference @ButtonTapDefault = Create(-1243561327, 1303269961, 802415537, -3874436);

            }
        }
    }
}
