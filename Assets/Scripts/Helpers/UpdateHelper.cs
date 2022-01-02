using UnityEngine;

namespace Helpers
{
    public static class UpdateHelper
    {
        private const int Frequency = 30;

        public static bool IsUpdatingFrame()
        {
            return Time.frameCount % Frequency == 0;
        }
    }
}