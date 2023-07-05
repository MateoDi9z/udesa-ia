using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.CynuSoftTeam
{
    public class PlayerOne : TeamPlayer
    {
        public int id = 1;
        public override void OnUpdate()
        {
            GoToBase();

            Vector3 ballPosition = GetBallPosition();
            if (Vector3.Distance(ballPosition, GetMyGoalPosition()) < 5)
            {
                GoToBall();
            }
        }

        public override void OnReachBall()
        {
            //Shoot();
            PaseAMessi();
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {

        }

        public void PaseAMessi()
        {
            var messi = GetTeamMatesInformation()[1].Position;

            if (Libre(messi))
            {
                Vector3 directionToMessi = GetDirectionTo(messi);
                Debug.DrawLine(messi, GetPosition(), Color.green, 0.3f);
                ShootBall(directionToMessi, ShootForce.Medium);
                return;
            }

            Vector3 directionToGoal = GetDirectionTo(GetRivalGoalPosition());
            Debug.DrawLine(GetRivalGoalPosition(), GetPosition(), Color.red, 0.3f);
            ShootBall(directionToGoal, ShootForce.High);
        }

        public bool Libre(Vector3 destino)
        {
            bool libre = true;
            var rivales = GetRivalsInformation();
            var m = GetPendiente(GetPosition(), destino);

            rivales.ForEach((rival) =>
            {
                if (Mathf.RoundToInt(m * rival.Position.x) == Mathf.RoundToInt(rival.Position.z))
                {
                    libre = false;
                }
            });

            return libre;
        }

        public float GetPendiente(Vector3 A, Vector3 B)
        {
            return (A.z - B.z) / (A.x - B.x);
        }

        public void Shoot()
        {
            MoveBy(GetDirectionTo(GetBallPosition()));
            var rivalGoalPosition = GetRivalGoalPosition();
            var directionToRivaGoal = GetDirectionTo(rivalGoalPosition);
            ShootBall(directionToRivaGoal, ShootForce.High);
        }

        public void GoToBall()
        {
            Vector3 ballPosition = GetBallPosition();
            GoTo(ballPosition);
        }

        public void GoToBase()
        {
            TeamType pos = GetTeamType();

            if (pos == 0)
                GoTo(new Vector3(-8.85f, -0.3f, 1.2f));
            else
                GoTo(new Vector3(8.85f, -0.3f, 1.2f));
        }

        public override FieldPosition GetInitialPosition() => FieldPosition.A2;

        public override string GetPlayerDisplayName() => "Mordekaiser";
    }
}