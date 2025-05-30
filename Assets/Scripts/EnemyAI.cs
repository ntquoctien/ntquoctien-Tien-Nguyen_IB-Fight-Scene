using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    //public Transform player;             
    //public float moveSpeed = 3f;
    //public float detectionRange = 10f;

    //private Rigidbody rb;

    //void Start()
    //{
    //    rb = GetComponent<Rigidbody>();

    //    if (player == null)
    //    {
    //        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
    //        if (playerObj != null)
    //            player = playerObj.transform;
    //    }
    //}

    //void FixedUpdate()
    //{
    //    if (player == null) return;

    //    float distance = Vector3.Distance(transform.position, player.position);
    //    if (distance <= detectionRange)
    //    {
    //        MoveTowardsPlayer();
    //    }
    //}

    //void MoveTowardsPlayer()
    //{
    //    Vector3 direction = (player.position - transform.position).normalized;
    //    direction.y = 0f;


    //    rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);

    //    // Xoay enemy nhìn về hướng Player
    //    if (direction != Vector3.zero)
    //    {
    //        Quaternion lookRotation = Quaternion.LookRotation(direction);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
    //    }
    //}
    public NavMeshAgent agent;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 10;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_playerPosition;

    float m_WaitTime;
    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;

    private void Start()
    {
        m_playerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

        m_CurrentWaypointIndex = 0;
        agent = GetComponent<NavMeshAgent>();

        agent.isStopped = false;
        agent.speed = speedWalk;
        agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);

    }
    private void Update()
    {
        EnvironmentView();

        if (!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }

    private void Chasing()
    {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;
        if (!m_CaughtPlayer)
        {
            Move(speedRun);
            agent.SetDestination(m_playerPosition);
        }
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            if (m_WaitTime <= 0 &&  !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
            else
            {
               if(Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 6f)
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }       
            }
        }
    }
    private void Patroling()
    {
        if (m_PlayerNear)
        {
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;
            agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }
        void Move(float speed)
        {
            agent.isStopped = false;
            agent.speed = speed;
        }
        void Stop()
        {
            agent.isStopped = true;
            agent.speed = 0;
        }
        void CaughtPlayer()
        {
            m_CaughtPlayer = true;
        }
        public void NextPoint()
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
        void LookingPlayer(Vector3 player)
        {
            agent.SetDestination(player);
            if (Vector3.Distance(transform.position, player) <= 0.3)
            {
                if (m_WaitTime <= 0)
                {
                    m_PlayerNear = false;
                    Move(speedWalk);
                    agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                    m_WaitTime = startWaitTime;
                    m_TimeToRotate = timeToRotate;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }

            }
        }
        void EnvironmentView()
        {
            Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            for (int i = 0; i < playerInRange.Length; i++)
            {
                Transform player = playerInRange[i].transform;
                Vector3 dirToPlayer = (player.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                    if (!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask))
                    {

                        m_PlayerInRange = true;
                        m_IsPatrol = false;
                    }
                    else
                    {
                        m_PlayerInRange = false;
                    }
                }
                if (Vector3.Distance(transform.position, player.position) > viewRadius)
                {
                    m_PlayerInRange = false;
                }
                if (m_PlayerInRange)
                {
                    m_playerPosition = player.transform.position;
                }
            }

        }


    }
