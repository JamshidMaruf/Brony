using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brony.Helpers;

public class IdGeneration
{
    public static int IdGenerate(string path)
    {
        // check path and append new id
        int id = 0;
        if (File.Exists(path))
        {
            var text = File.ReadAllText(path);
            int.TryParse(text, out id);
        }

        int newID = id+1;
        File.WriteAllText(path, newID.ToString());
        return newID;
    }

}
