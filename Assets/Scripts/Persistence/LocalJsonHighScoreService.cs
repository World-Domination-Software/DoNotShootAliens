using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZombieRescue.Scoring;

namespace ZombieRescue.Persistence
{
    /// <summary>
    /// Persists match results as a JSON file in the device's persistent data folder.
    /// </summary>
    public class LocalJsonHighScoreService : MonoBehaviour, IHighScoreService
    {
        [SerializeField] private string fileName = "highscores.json";

        private string FilePath => Path.Combine(Application.persistentDataPath, fileName);

        // ── IHighScoreService ──────────────────────────────────────────────────
        public void SaveResult(MatchResults result)
        {
            var data = ReadFromDisk();
            data.entries.Add(result);
            WriteToDisk(data);
            Debug.Log($"[HighScore] Saved result to {FilePath}");
        }

        public List<MatchResults> LoadResults()
        {
            return ReadFromDisk().entries;
        }

        public void ClearResults()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
            Debug.Log("[HighScore] Scores cleared.");
        }

        // ── Disk helpers ───────────────────────────────────────────────────────
        private HighScoreData ReadFromDisk()
        {
            if (!File.Exists(FilePath))
                return new HighScoreData();

            try
            {
                string json = File.ReadAllText(FilePath);
                return JsonUtility.FromJson<HighScoreData>(json) ?? new HighScoreData();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[HighScore] Failed to read file: {e.Message}");
                return new HighScoreData();
            }
        }

        private void WriteToDisk(HighScoreData data)
        {
            try
            {
                File.WriteAllText(FilePath, JsonUtility.ToJson(data, prettyPrint: true));
            }
            catch (Exception e)
            {
                Debug.LogError($"[HighScore] Failed to write file: {e.Message}");
            }
        }

        // ── Serialisation wrapper ──────────────────────────────────────────────
        [Serializable]
        private class HighScoreData
        {
            public List<MatchResults> entries = new List<MatchResults>();
        }
    }
}
