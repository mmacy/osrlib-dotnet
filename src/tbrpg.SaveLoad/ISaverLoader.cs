using System.Collections.Generic;

namespace tbrpg.SaveLoad
{
    public  interface ISaverLoader<T>
    {
        bool Save(T entity);

        bool Load();

        List<T> LoadedItems { get; }
    }
}
