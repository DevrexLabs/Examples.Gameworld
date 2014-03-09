using System;
using System.Collections.Generic;
using System.Linq;
using OrigoDB.Core;


namespace OrigoDB.Examples.Proxy
{

    /// <summary>
    /// Keep track of players, their position and velocity for a fictive game.
    /// </summary>
    [Serializable] 
    public class GameWorldModel : Model
    {

        /// <summary>
        /// the players
        /// </summary>
        private Dictionary<string, Player> _players;

        /// <summary>
        /// Initialize case insensitive keys
        /// </summary>
        public GameWorldModel()
        {
            _players = new Dictionary<string, Player>(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Add a player to the game at position 0,0 and with zero velocity
        /// </summary>
        /// 
        public void AddPlayer(DateTime when, string name)
        {
            if(_players.ContainsKey(name)) throw new ArgumentException("duplicate player name");
            var player = new Player(name, when);
            
            _players.Add(name, player);
        }

        public void RemovePlayer(string playerName)
        {
            _players.Remove(playerName);
        }

        public Player[] GetPlayersWithinRadius(string playerName, double radius)
        {
            var player = _players[playerName];
            return _players.Values.Where(p => WithinRadius(player,p,radius)).ToArray();
        }

        private bool WithinRadius(Player p1, Player p2, double radius)
        {
            return p1 != p2 && p1.DistanceTo(p2) <= radius;
        }

        public void Update(string playerName, Point newVelocity, DateTime when)
        {
            var player = _players[playerName];
            player.Update(when, newVelocity);
        }
    }
}
