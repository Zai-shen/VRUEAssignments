using System;

namespace VRUEAssignments.Managers
{
    public static class GameStatistics
    {
        public static DateTime StartTime;
        public static float TimeElapsed = 0f;
        public static int HolesHit = 0;
        public static int CueHits = 0;


        public static void Reset()
        {
            StartTime = DateTime.UtcNow;
            TimeElapsed = 0f;
            HolesHit = 0;
            CueHits = 0;
        }
    }
}