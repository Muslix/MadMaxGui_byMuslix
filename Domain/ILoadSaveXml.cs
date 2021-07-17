using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface ILoadSaveXml
    {
        void savedata(object obj, string filename);
        Config loadData(string filename);
    }
}
