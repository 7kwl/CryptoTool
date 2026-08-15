using System;
using Org.BouncyCastle.Asn1.X9;

class Program
{
    static void Main()
    {
        string[] names = { "K-163", "K-233", "K-283", "K-409", "K-571", "P-256", "B-571", "secp256k1" };
        foreach (var n in names)
        {
            var curve = ECNamedCurveTable.GetByName(n);
            Console.WriteLine((curve == null ? "MISSING: " : "OK:      ") + n);
        }
        // 验证完整 Names 列表包含 K 系列
        bool hasK = false;
        foreach (var name in ECNamedCurveTable.Names)
        {
            if (name.StartsWith("K-")) { hasK = true; Console.WriteLine("NAME: " + name); }
        }
        Console.WriteLine("HAS-K-PREFIX: " + hasK);
    }
}
