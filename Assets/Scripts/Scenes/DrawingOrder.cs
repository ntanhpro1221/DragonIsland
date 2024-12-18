namespace Scenes {
    /// <summary>
    /// Drawing order of element in scene
    /// </summary>
    enum DrawingOrder
    {
        /// <summary>
        /// Deepest layer
        /// </summary>
        Background = 1,

        /// <summary>
        /// This layer is drawn behind the middle layer
        /// </summary>
        Back = 2,

        /// <summary>
        /// Middle layer
        /// </summary>
        Mid = 3,

        /// <summary>
        /// This layer is drawn in front of the middle layer
        /// </summary>
        Fore = 4,

        /// <summary>
        /// Highest layer
        /// </summary>
        Foreground = 5,
    }
}