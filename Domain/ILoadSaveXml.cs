

using System.Threading.Tasks;

namespace Domain
{
    public interface ILoadSaveXml
    {
        void Savedata(Config config, string filename);
        Task<Config> LoadData(string filename);
    }
}
