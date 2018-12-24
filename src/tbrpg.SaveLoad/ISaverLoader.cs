using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tbrpg.SaveLoad
{
    public  interface ISaverLoader<T>
    {
        bool Save(T entity);

        bool Load();

        List<T> LoadedItems { get; }
    }
}
