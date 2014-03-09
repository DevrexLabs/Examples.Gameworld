using System;

namespace OrigoDB.Examples.Proxy
{

    [Serializable]
    public class Player
    {
        public readonly string Name;

        /// <summary>
        /// The last recorded position
        /// </summary>
        public Point Position { get; private set; }

        /// <summary>
        /// When the position was last updated
        /// </summary>
        public DateTime LastUpdated { get; private set; }        
        
        
        /// <summary>
        /// Constant speed per second in X and Y directions
        /// </summary>
        public Point Velocity { get; private set; }

        /// <summary>
        /// Current position calculated by projecting the velocity over the 
        /// period of time since LastUpdated
        /// </summary>
        public Point CalculatedPosition
        {
            get { return CalculatePosition(DateTime.Now); }
        }

        public Point CalculatePosition(DateTime pointInTime)
        {
            TimeSpan elapsed = pointInTime - LastUpdated;
            return Position.Translate(Velocity, elapsed);
        }

        public Player(string name, DateTime when)
        {
            Name = name;
            LastUpdated = when;
        }

        public void Update(DateTime when, Point newVelocity)
        {
            //Calculate current position based on prior movement
            Position = CalculatePosition(LastUpdated);
            LastUpdated = when;
            Velocity = newVelocity;
            
        }

        public double DistanceTo(Player otherPlayer)
        {
            return CalculatedPosition.DistanceTo(otherPlayer.CalculatedPosition); 
        }

    }
}
