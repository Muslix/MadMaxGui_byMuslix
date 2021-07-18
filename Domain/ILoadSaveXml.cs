

namespace Domain
{
    public interface ILoadSaveXml
    {
        void savedata(object obj, string filename);
        Config loadData(string filename);
    }
}
