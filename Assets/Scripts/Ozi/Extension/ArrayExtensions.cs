namespace Ozi.Extension {
    public static class ArrayExtensions {
        public static int PreviousIndex<T>(this T[] array, int index) {
            int previous_index = index - 1;

            if (previous_index < 0) {
                return array.Length - 1;
            }

            return previous_index;
        }
        public static int NextIndex<T>(this T[] array, int index) {
            int next_index = index + 1;

            if (next_index >= array.Length) {
                next_index = 0;
            }

            return next_index;
        }
        public static bool IsVaildIndex<T>(this T[] array, int index) => 0 <= index && index < array.Length;
    }
}