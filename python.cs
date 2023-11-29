using System;
using System.Collections.Generic;

class Player
{
    static Zone warm, cold, current;
    static int width, height;
    static int x, y;
    static int lastX, lastY;
    static bool foundX = false, firstChance = true, outside = false;
    static char bombDir = 'U';

    static void Main(string[] args)
    {
        string[] inputs = Console.ReadLine().Split(' ');
        width = int.Parse(inputs[0]);
        height = int.Parse(inputs[1]);
        Init();
        int rounds = int.Parse(Console.ReadLine());
        inputs = Console.ReadLine().Split(' ');
        x = int.Parse(inputs[0]);
        y = int.Parse(inputs[1]);

        while (true)
        {
            bombDir = Console.ReadLine()[0];
            Move();
            Console.WriteLine(x + " " + y);
        }
    }

    public static void Init()
    {
        current = new Zone(0, width - 1);
        cold = new Zone(0, width - 1);
        warm = new Zone(0, width - 1);
        x = y = lastX = lastY = 0;
    }

    public static void Move()
    {
        int tmpX = x;
        int tmpY = y;
        switch (bombDir)
        {
            case 'W':
                current.Update(warm);
                break;
            case 'C':
                current.Update(cold);
                break;
            case 'S':
                if (!firstChance)
                {
                    if (!Found())
                        return;
                }
                break;
        }
        if (current.Low >= current.High)
        {
            if (!Found())
                return;
        }
        firstChance = false;
        if (foundX)
            y = Get(y, height - 1);
        else
            x = Get(x, width - 1);
        lastX = tmpX;
        lastY = tmpY;
    }

    public static bool Found()
    {
        int tmpX = x;
        int tmpY = y;
        if (foundX)
        {
            y = (current.Low + current.High) / 2;
            foundX = false;
        }
        else
        {
            x = (current.Low + current.High) / 2;
            foundX = true;
            current.Update(0, height - 1);
            warm.Update(current);
            cold.Update(current);
        }
        firstChance = true;
        return (x == tmpX && y == tmpY);
    }

    public static int Get(int value, int limit)
    {
        int low = current.Low;
        int high = current.High;
        int give = low + high - value;
        if (outside)
        {
            if (value == 0) give = (give - 0) / 2;
            else if (value == limit) give = (limit + give) / 2;
        }
        outside = false;
        if (give == value) give = value + 1;
        if (give < 0)
        {
            give = 0;
            outside = true;
        }
        else if (give > limit)
        {
            give = limit;
            outside = true;
        }
        int middle = (give + value) / 2;
        int lower = (give + value - 1) / 2;
        int higher = (give + value + 1) / 2;
        if (give > value)
        {
            warm.Update(higher, high);
            cold.Update(low, lower);
        }
        else if (give < value)
        {
            warm.Update(low, lower);
            cold.Update(higher, high);
        }
        return give;
    }

    public class Zone
    {
        public int Low, High;
        public Zone(int low, int high)
        {
            Low = low;
            High = high;
        }

        public void Update(Zone z)
        {
            Low = z.Low;
            High = z.High;
        }

        public void Update(int low, int high)
        {
            Low = low;
            High = high;
        }
    }
}
