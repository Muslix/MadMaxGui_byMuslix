using Domain;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataAccessLayer
{
    public class LoadSaveXml : ILoadSaveXml
    {
        public async Task<Config> LoadData(string filename)
        {
            await using var fs = new FileStream(filename, FileMode.Open);
            return await JsonSerializer.DeserializeAsync<Config>(fs);
        }

        public async void Savedata(Config config, string filename)
        {
            var options = new JsonSerializerOptions() { WriteIndented = true };
            await using var fs = new FileStream(filename, FileMode.Create);
            await JsonSerializer.SerializeAsync(fs, config, options);
        }
    }
}
