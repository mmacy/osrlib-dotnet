using System;
using tbrpg.CoreRules;
using tbrpg.SaveLoad;

namespace tbrpg.Controllers
{
    /// <summary>
    /// The GameManager singleton handles game execution lifecycle.
    /// </summary>
    /// <remarks>The GameManager singleton should be accessed only via the <see cref="GameManager.Instance"/> property.</remarks>
    public class GameManager
    {
        /// <summary>
        /// Gets the singleton instance of the <see cref="GameManager"/>.
        /// </summary>
        public static GameManager Instance { get; } = new GameManager();

        public Adventure ActiveAdventure { get; private set; }

        /// <summary>
        /// Event raised when the <see cref="ActiveAdventure"/> is loaded.
        /// </summary>
        public event EventHandler AdventureLoaded;

        /// <summary>
        /// Event raised when the <see cref="ActiveAdventure"/> is saved.
        /// </summary>
        public event EventHandler AdventureSaved;

        /// <summary>
        /// Creates a new instance of the GameManager class.
        /// </summary>
        /// <remarks>Private constructor to enforce singleton usage via the <see cref="GameManager.Instance"/> property.</remarks>
        private GameManager() { }

        /// <summary>
        /// Sets the active <see cref="Adventure"/> for the game session.
        /// </summary>
        /// <param name="adventure">The <see cref="Adventure"/> to set for the current game session.</param>
        public void SetActiveAdventure(Adventure adventure)
        {
            this.ActiveAdventure = adventure;
        }

        /// <summary>
        /// Loads the specified <see cref="Adventure"/> into the GameManager, and sets it as the <see cref="ActiveAdventure"/>.
        /// </summary>
        /// <param name="saveType">The type of save/load operation.</param>
        /// <param name="adventureId">The path or URL to the <see cref="Adventure"/> to load.</param>
        /// <returns>Value indicating whether the operation was successful.</returns>
        public bool LoadAdventure(SaveType saveType, string path)
        {
            bool loaded = true;

            switch (saveType)
            {
                case SaveType.Local:
                    try
                    {
                        SetActiveAdventure(SaveLoadLocal.Load(path));
                    }
                    catch (Exception)
                    {
                        loaded = false;

                        //TODO: Throwing load failure for now--make LoadFailed event?
                        throw;
                    }
                    break;
                case SaveType.Cloud:
                    throw new NotImplementedException("Cloud save/load is not yet implemented.");
                default:
                    break;
            }

            if (loaded)
            {
                OnAdventureLoaded();
            }

            return loaded;
        }

        /// <summary>
        /// Raises the <see cref="AdventureLoaded"/> event.
        /// </summary>
        private void OnAdventureLoaded()
        {
            this.AdventureLoaded?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Saves the <see cref="ActiveAdventure"/> to the specified storage location.
        /// </summary>
        /// <param name="saveType">The type of save/load operation.</param>
        /// <param name="adventureId">The path or URL to which to save the <see cref="Adventure"/>.</param>
        /// <returns>Value indicating whether the operation was successful.</returns>
        public bool SaveActiveAdventure(SaveType saveType, string path)
        {
            bool saved = true;

            switch (saveType)
            {
                case SaveType.Local:
                    try
                    {
                        SaveLoadLocal.Save(this.ActiveAdventure, path);
                    }
                    catch (Exception)
                    {
                        saved = false;

                        //TODO: Throwing save failure for now--make SaveFailed event?
                        throw;
                    }
                    break;
                case SaveType.Cloud:
                    throw new NotImplementedException("Cloud save/load is not yet implemented.");
                default:
                    break;
            }

            if (saved)
            {
                OnAdventureSaved();
            }

            return saved;
        }

        /// <summary>
        /// Raises the <see cref="AdventureSaved"/> event.
        /// </summary>
        private void OnAdventureSaved()
        {
            this.AdventureSaved?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Starts the active <see cref="Adventure"/>.
        /// </summary>
        public void StartAdventure()
        {
            if (this.ActiveAdventure != null)
            {
                Console.WriteLine("Adventure started.");
            }
            else
            {
                throw new InvalidOperationException("There is no active Adventure. Did you call LoadAdventure or SetActiveAdventure first?");
            }
        }
    }
}
