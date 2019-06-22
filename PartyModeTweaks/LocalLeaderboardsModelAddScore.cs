using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Harmony;

namespace PartyModeTweaks
{

    [HarmonyPatch(typeof(LocalLeaderboardsModel), new Type[] { typeof(string), typeof(string), typeof(int), typeof(bool)})]
    [HarmonyPatch("AddScore", MethodType.Normal)]
    class LocalLeaderboardsModelAddScore
    {
        static bool Prefix(ref LocalLeaderboardsModel __instance, string leaderboardId, string playerName, int score, bool fullCombo)
        {
            List<LocalLeaderboardsModel.ScoreData> allTimeScores = __instance.GetScores(leaderboardId, LocalLeaderboardsModel.LeaderboardType.AllTime);
            if (!RemoveOldHighScoreFromLeaderboard(__instance, leaderboardId, playerName, score, LocalLeaderboardsModel.LeaderboardType.AllTime)) return false;
            if (!RemoveOldHighScoreFromLeaderboard(__instance, leaderboardId, playerName, score, LocalLeaderboardsModel.LeaderboardType.Daily)) return false;
            return true;
        }

        static bool RemoveOldHighScoreFromLeaderboard(LocalLeaderboardsModel instance, string leaderboardId, string playerName, int score, LocalLeaderboardsModel.LeaderboardType type)
        {
            List<LocalLeaderboardsModel.ScoreData> allTimeScores = instance.GetScores(leaderboardId, type);
            if (allTimeScores == null) return true;
            foreach (LocalLeaderboardsModel.ScoreData data in allTimeScores)
            {
                if (data._playerName != playerName) continue;
                if (data._score < score) allTimeScores.Remove(data);
                else return false;
            }
            return true;
        }
    }
}
