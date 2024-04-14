using System.Text.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        ReadWriteConfig rwconf = new ReadWriteConfig();

        rwconf.UbahSatuan();

        Console.Write($"Berapa suhu badan anda saat ini? Dalam nilai {rwconf.config.satuan_suhu} : ");
        double suhu = Convert.ToDouble(Console.ReadLine());

        Console.Write($"Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala demam? : ");
        int terakhir_demam = Convert.ToInt32(Console.ReadLine());

        if (
            ((rwconf.config.satuan_suhu == "celcius" && suhu >= 36.5 && suhu <= 37.5) ||
            (rwconf.config.satuan_suhu == "fahrenheit" && suhu >= 97.7 && suhu <= 99.5)) &&
            terakhir_demam < rwconf.config.batas_hari_demam
        )
        {
            Console.WriteLine(rwconf.config.pesan_diterima);
        }
        else
        {
            Console.WriteLine(rwconf.config.pesan_ditolak);
        }
    }
}

class CovidConfig
{
    public String satuan_suhu { get; set; }
    public int batas_hari_demam { get; set; }
    public String pesan_ditolak { get; set; }
    public String pesan_diterima { get; set; }

    public CovidConfig() { }

    public CovidConfig(string satuan_suhu, int batas_hari_demam, string pesan_ditolak, string pesan_diterima)
    {
        this.satuan_suhu = satuan_suhu;
        this.batas_hari_demam = batas_hari_demam;
        this.pesan_ditolak = pesan_ditolak;
        this.pesan_diterima = pesan_diterima;
    }
}

class ReadWriteConfig
{
    public CovidConfig config;

    public const String filePath = "../../../covid_config.json";

    public ReadWriteConfig()
    {
        try
        {
            ReadConfigFile();
        }
        catch (Exception)
        {
            SetDefault();
            WriteNewConfigFile();
        }
    }

    private CovidConfig ReadConfigFile()
    {
        String configJsonData = File.ReadAllText(filePath);
        config = JsonSerializer.Deserialize<CovidConfig>(configJsonData);
        return config;
    }

    private void SetDefault()
    {
        config = new CovidConfig("celcius", 14, "Anda tidak diperbolehkan masuk ke gedung ini", "Anda dipersilakan untuk masuk ke gedung ini");
    }

    private void WriteNewConfigFile()
    {
        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            WriteIndented = true,
        };

        String jsonString = JsonSerializer.Serialize(config, options);
        File.WriteAllText(filePath, jsonString);
    }

    public void UbahSatuan()
    {
        if (config.satuan_suhu == "celcius")
        {
            config.satuan_suhu = "fahrenheit";
        }
        else
        {
            config.satuan_suhu = "celcius";
        }

        String jsonString = JsonSerializer.Serialize(config);
        File.WriteAllText(filePath, jsonString);
    }
}