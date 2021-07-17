using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Config
    {

        private string madmaxDir;
        private string contractKey;
        private string farmerKey;
        private string tempDir;
        private string tempDir2;
        private string finalDir;

        public string MadmaxDir { get => madmaxDir; set => madmaxDir = value; }
        public string ContractKey { get => contractKey; set => contractKey = value; }
        public string FarmerKey { get => farmerKey; set => farmerKey = value; }
        public string TempDir { get => tempDir; set => tempDir = value; }
        public string TempDir2 { get => tempDir2; set => tempDir2 = value; }
        public string FinalDir { get => finalDir; set => finalDir = value; }
    }
}
