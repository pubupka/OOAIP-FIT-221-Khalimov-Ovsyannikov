public class Vector
{
    private readonly int[] array;
    public Vector(int[] array)
    {
        this.array = array;
    }

    public static Vector operator +(Vector v1, Vector v2)
    {
        var arr = v1.array.Select((value, index) => value + v2.array[index]).ToArray();

        return new Vector(arr);
    }

    // == сравнивает ссылки, а equals объекты
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        return obj.GetType() == typeof(Vector) && Enumerable.SequenceEqual(((Vector)obj).array, array);
    }

    public override int GetHashCode()
    {
        return array.GetHashCode();
    }

    public static bool operator ==(Vector v1, Vector v2)
    {
        return v1.array == v2.array;
    }

    public static bool operator !=(Vector v1, Vector v2)
    {
        return !(v1 == v2);
    }
}
