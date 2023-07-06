using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.CynuSoftTeam
{
    public class PlayerThree : TeamPlayer
    {
        public int id = 3;
        public override void OnUpdate()
        {
            GoToBall();
        }

        public override void OnReachBall()
        {
            //Saque();
            // Debug.DrawLine(GetRivalGoalPosition(), GetPosition(), Color.magenta, 0.2f);
            MoveBy(GetDirectionTo(GetBallPosition()));
            Shoot();
            var ballPosition = GetBallPosition();
        }

        public void GoToBall()
        {
            Vector3 ballPosition = GetBallPosition();
            GoTo(ballPosition);
        }

        public void Shoot() {
            var arqueroPostition = getArquero();
            var goalPosition = GetRivalGoalPosition();

            if (Libre(goalPosition) || Vector3.Distance(GetBallPosition(), goalPosition) < 8.0f)
            {
                Vector3 shootTo;
                if (arqueroPostition.Position.z >= 0)
                    shootTo = goalPosition + new Vector3(0, 0, 1.7f);
                else
                    shootTo = goalPosition + new Vector3(0, 0, -1.7f);

                if (Libre(shootTo))
                {
                    Debug.DrawLine(shootTo, GetPosition(), Color.green, 0.2f);
                    Debug.Log("Messi tira.");
                    ShootBall(GetDirectionTo(shootTo), ShootForce.High);
                    return;
                }

                Debug.DrawLine(shootTo, GetPosition(), Color.red, 0.2f);
            } else
            {
                Pase();
            }
        }

        public void Pase()
        {
            var team = GetTeamMatesInformation();

            int defensa = 0;
            for (int i = 0; i < team.Count; i++)
            {
                if (
                    Vector3.Distance(
                        GetBallPosition(), 
                        team[i].Position
                    ) 
                    < 
                    Vector3.Distance(
                        GetBallPosition(), 
                        team[defensa].Position
                        )
                    )
                {
                    defensa = i;
                }
            }

            ShootBall(team[defensa].Position, ShootForce.Medium);
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

        public PlayerData getArquero()
        {
            var rivals = GetRivalsInformation();

            int arquero = 0;
            for (int i = 0; i < rivals.Count; i++)
            {
                if (DistanceToGoal(rivals[i].Position) < DistanceToGoal(rivals[arquero].Position))
                {
                    arquero = i;
                }
            }

            return rivals[arquero];
        }

        /// <summary>
        /// Get distance of player from rival Goal.
        /// Param: <param>rivalPost</param> rival position
        /// Returns: <returns>Distance to goal</returns>
        /// </summary>
        public float DistanceToGoal(Vector3 rivalPos)
        {
            Vector3 A = GetRivalGoalPosition();
            Vector3 C = new Vector3(A.x, A.y, rivalPos.z);

            var h = rivalPos.z - C.z;
            var b = A.x - C.x;

            return Mathf.Sqrt(h * h + b * b);
        }

        public void Saque()
        {
            TeamType pos = GetTeamType();

            if (pos == 0)
            {
                if (Random.Range(0, 1) == 1) {
                    GoTo(FieldPosition.D3);
                    return;
                }
                GoTo(FieldPosition.D1);
                return;
            }

            if (Random.Range(0, 1) == 1)
            {
                GoTo(FieldPosition.B3);
                return;
            }
            GoTo(FieldPosition.B1);
            return;
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {

        }

        public override FieldPosition GetInitialPosition() => FieldPosition.C2;

        public override string GetPlayerDisplayName() => "Messi";
    }
}