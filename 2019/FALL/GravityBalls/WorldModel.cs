using System;

namespace GravityBalls
{
	public class WorldModel
	{
        const double g = 9.8;
        const double m = 0.99;
        const double k = 2;
        const double maxVelocity = 1000;

		public double BallX;
		public double BallY;
		public double BallRadius;
		public double WorldWidth;
		public double WorldHeight;

        public double velocityX = 5;
        public double velocityY;
        public double accelerationX;
        public double accelerationY = g;

        public double cursorX;
        public double cursorY;

        public void SimulateTimeframe(double dt)
		{
            if ((BallX == WorldWidth - BallRadius) || (BallX == BallRadius))
                velocityX *= -1;
            if ((BallY == WorldHeight - BallRadius) || (BallY == BallRadius))
                velocityY *= -1;
            ChangeVelocity(dt);
            if (BallX > WorldWidth/2)
                BallX = Math.Min(BallX + velocityX, WorldWidth - BallRadius);
            else
                BallX = Math.Max(BallX + velocityX, BallRadius);
            if (BallY > WorldHeight / 2)
                BallY = Math.Min(BallY + velocityY, WorldHeight - BallRadius);
            else
                BallY = Math.Max(BallY + velocityY, BallRadius);
		}

        public void ChangeVelocity(double dt)
        {
            accelerationX += CursorAccelerationX();
            accelerationY += CursorAccelerationY();

            velocityX += accelerationX * dt;
            velocityY += accelerationY * dt;

            velocityX *= m;
            velocityY *= m;
        }

        public double CursorAccelerationX()
        {
            if (BallX - cursorX == 0)
                return k / maxVelocity;
            return k / (BallX - cursorX);
        }

        public double CursorAccelerationY()
        {
            if (BallY - cursorY == 0)
                return k / maxVelocity;
            return k / (BallY - cursorY);
        }
    }
}