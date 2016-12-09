namespace Igneel
{
    public static class ValueTuple
    {
        public static Tuple2D<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
        {
            return new Tuple2D<T1, T2> { Item1 = item1, Item2 = item2 };
        }

        public static Tuple3D<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
        {
            return new Tuple3D<T1, T2, T3> { Item1 = item1, Item2 = item2, Item3 = item3 };
        }
    }
}