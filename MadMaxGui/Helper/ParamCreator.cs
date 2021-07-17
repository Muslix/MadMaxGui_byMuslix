using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadMaxGui.Helper
{
    public class ParamCreator
    {
        
        private string param;
        public ParamCreator()
        {
        }

        public string Create(Config config)
        {
            if (config is null)
                return null;
            param += config.NumberOfPLots is not null ? "-n " + config.NumberOfPLots : "-n 1";
            param += config.Threads is not null ? " -r " + config.Threads : " -r 4";
            param += config.Buckets is not null ? " -u " + config.Buckets : " -u 256";
            param += config.BucketsPhaseThreeAndFour is not null ? " -v " + config.BucketsPhaseThreeAndFour : "";
            param += config.TempDir is not null ? " -t " + config.TempDir : " -t ";
            param += config.TempDir2 is not null ? " -2 " + config.TempDir2 : "";
            param += config.FinalDir is not null ? " -d " + config.FinalDir : " -d ";
            param += config.ContractKey is not null ? " -c " + config.ContractKey : " -c ";
            param += config.FarmerKey is not null ? " -f " + config.FarmerKey : " -f ";

            return param;
        }
    }
}
