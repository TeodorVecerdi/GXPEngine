using System;
using GXPEngine.OpenGL;

namespace GXPEngine
{
    /// <summary>
    ///     Contains various time related functions.
    /// </summary>
    public class Time
    {
        private static int previousTime;

        static Time()
        {
        }

        /// <summary>
        ///     Returns the current system time in milliseconds
        /// </summary>
        public static int now => Environment.TickCount;

        /// <summary>
        ///     Returns this time in milliseconds since the program started
        /// </summary>
        /// <value>
        ///     The time.
        /// </value>
        public static int time => (int)(GL.glfwGetTime() * 1000);
        /// <summary>
        ///     Returns the time in milliseconds that has passed since the previous frame
        /// </summary>
        /// <value>
        ///     The delta time.
        /// </value>
        private static int previousFrameTime;
        public static int deltaTimeMs
        {
            get
            {
                return previousFrameTime;
            }
        }

        public static float deltaTime => deltaTimeMs / 1000f;

        internal static void newFrame()
        {
            previousFrameTime = time - previousTime;
            previousTime = time;
        }
    }
}