#if (UNITY_EDITOR)
using System.IO;
using System.Threading;

public class CpuInfo
{
    /* Variables */

    public static readonly string statPath = "/proc/stat";

    /* Setter & Getter */

    /* Functions */

    public static string[] GetStatInfoArray()
    {
        return File.ReadAllLines(statPath);
    }

    public static float cpuUsageRate = 0;

    public static void Update()
    {
        new Thread(Update2).Start();
    }

    public static void Update2()
    {
        string line;
        string[] values;
        float cpu1, cpu2, idle1, idle2;

        //------------------------

        line = GetStatInfoArray()[0];
        values = Split(line);
        cpu1 = int.Parse(values[1]) + int.Parse(values[2]) + int.Parse(values[3]) + int.Parse(values[5]) + int.Parse(values[6]) + int.Parse(values[7]) + int.Parse(values[8]);
        idle1 = int.Parse(values[4]);

        //------------------------

        Thread.Sleep(10);

        line = GetStatInfoArray()[0];
        values = Split(line);
        cpu2 = int.Parse(values[1]) + int.Parse(values[2]) + int.Parse(values[3]) + int.Parse(values[5]) + int.Parse(values[6]) + int.Parse(values[7]) + int.Parse(values[8]);
        idle2 = int.Parse(values[4]);

        //------------------------

        cpuUsageRate = (cpu2 - cpu1) / ((cpu2 + idle2) - (cpu1 + idle1)) * 100;
        //      cpuUsageRate = (cpu2 - cpu1 - (idle2 - idle1)) / (cpu2 - cpu1) * 100 ;
        //      http://blog.csdn.net/jk110333/article/details/8683478
    }

    private static string[] Split(string data)
    {
        data = data.Replace(" ", " ");
        data = data.Replace(" ", " ");
        data = data.Replace(" ", " ");
        return data.Split(' ');
    }
}
#endif
