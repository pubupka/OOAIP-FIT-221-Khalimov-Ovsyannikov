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

        for (int i=0; i < array.Length; i++)
            array[i] = v1.array[i] + v2.array[i];

        return new Vector(array);
    }

    // == сравнивает ссылки, а equals объекты
    public override bool Equals(object? obj)
    {
        
        if (obj == null || this.GetType() != obj.GetType())
        {
            return false;
        }
        
        return base.Equals(obj);
    }
    
    public override int GetHashCode()
    {
        return this.array.GetHashCode();
    }

    public static bool operator==(Vector v1, Vector v2)
    {
        for (int i=0; i < v1.array.Length; i++)
        {
            if (v1.array[i] != v2.array[i])
                return false;
        }
        return true;
    }

    public static bool operator!=(Vector v1, Vector v2)
    {
        return !(v1 == v2);
    }
}