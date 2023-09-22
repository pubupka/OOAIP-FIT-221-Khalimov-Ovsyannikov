public class Vector
{
    int[] array;
    public Vector(int[] array)
    {
        this.array = array;
    }

    public static Vector operator+(Vector v1, Vector v2)
    {
        int[] array = new int[v1.array.Length];

        for (int i=0; i<array.Length; i++)
            array[i] = v1.array[i] + v2.array[i];

        return new Vector(array);
    }
}