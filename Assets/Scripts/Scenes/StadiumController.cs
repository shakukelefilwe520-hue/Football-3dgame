using UnityEngine;
using Football3D.Core;
using Football3D.Data;
using Football3D.AI;

namespace Football3D.Scenes
{
    /// <summary>
    /// Manages stadium scene setup including field, players, and ball
    /// </summary>
    public class StadiumController : MonoBehaviour
    {
        [SerializeField] private float fieldLength = 100f;
        [SerializeField] private float fieldWidth = 60f;
        [SerializeField] private Material fieldMaterial;
        [SerializeField] private Material goalPostMaterial;

        private GameManager gameManager;
        private AIManager aiManager;
        private Team homeTeam;
        private Team awayTeam;
        private Ball ball;

        private void Start()
        {
            gameManager = GameManager.Instance;
            aiManager = FindObjectOfType<AIManager>();

            if (gameManager == null)
            {
                Debug.LogError("[StadiumController] GameManager not found!");
                return;
            }

            homeTeam = gameManager.GetHomeTeam();
            awayTeam = gameManager.GetAwayTeam();

            if (homeTeam == null || awayTeam == null)
            {
                Debug.LogError("[StadiumController] Teams not initialized!");
                return;
            }

            Debug.Log("[StadiumController] Setting up stadium");
            
            SetupField();
            SetupPlayers();
            SetupBall();
            SetupLighting();
            SetupCamera();

            gameManager.SetGameState(GameManager.GameState.Playing);
        }

        /// <summary>
        /// Setup soccer field
        /// </summary>
        private void SetupField()
        {
            // Create field plane
            GameObject fieldObject = new GameObject("SoccerField");
            fieldObject.transform.position = Vector3.zero;

            MeshFilter meshFilter = fieldObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = fieldObject.AddComponent<MeshRenderer>();
            
            // Create simple plane mesh
            Mesh fieldMesh = new Mesh();
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(-fieldWidth / 2, 0, -fieldLength / 2),
                new Vector3(fieldWidth / 2, 0, -fieldLength / 2),
                new Vector3(fieldWidth / 2, 0, fieldLength / 2),
                new Vector3(-fieldWidth / 2, 0, fieldLength / 2)
            };
            
            int[] triangles = new int[] { 0, 2, 1, 0, 3, 2 };
            
            fieldMesh.vertices = vertices;
            fieldMesh.triangles = triangles;
            fieldMesh.RecalculateNormals();

            meshFilter.mesh = fieldMesh;

            // Add collider
            BoxCollider fieldCollider = fieldObject.AddComponent<BoxCollider>();
            fieldCollider.size = new Vector3(fieldWidth, 0.1f, fieldLength);

            // Add material
            if (fieldMaterial != null)
                meshRenderer.material = fieldMaterial;
            else
            {
                Material defaultMaterial = new Material(Shader.Find("Standard"));
                defaultMaterial.color = new Color(0f, 0.7f, 0f);
                meshRenderer.material = defaultMaterial;
            }

            Debug.Log("[StadiumController] Field created");
        }

        /// <summary>
        /// Setup goal posts and scoring triggers
        /// </summary>
        private void SetupGoals()
        {
            // Home goal (attacking direction)
            GameObject homeGoal = new GameObject("HomeGoal");
            homeGoal.transform.position = new Vector3(0, 2, fieldLength / 2);
            
            BoxCollider homeGoalCollider = homeGoal.AddComponent<BoxCollider>();
            homeGoalCollider.isTrigger = true;
            homeGoalCollider.size = new Vector3(fieldWidth * 0.15f, 2, 1);
            homeGoal.tag = "Goal";

            // Away goal
            GameObject awayGoal = new GameObject("AwayGoal");
            awayGoal.transform.position = new Vector3(0, 2, -fieldLength / 2);
            
            BoxCollider awayGoalCollider = awayGoal.AddComponent<BoxCollider>();
            awayGoalCollider.isTrigger = true;
            awayGoalCollider.size = new Vector3(fieldWidth * 0.15f, 2, 1);
            awayGoal.tag = "Goal";

            Debug.Log("[StadiumController] Goals created");
        }

        /// <summary>
        /// Spawn players on field
        /// </summary>
        private void SetupPlayers()
        {
            // Spawn home team
            SpawnTeam(homeTeam, -fieldLength / 3);
            
            // Spawn away team
            SpawnTeam(awayTeam, fieldLength / 3);

            Debug.Log("[StadiumController] Players spawned");
        }

        /// <summary>
        /// Spawn team players in formation
        /// </summary>
        private void SpawnTeam(Team team, float zPosition)
        {
            Vector3[] formationPositions = GetFormationPositions(team.CurrentFormation, zPosition);

            for (int i = 0; i < team.Players.Count && i < formationPositions.Length; i++)
            {
                GameObject playerObject = new GameObject(team.Players[i].PlayerName);
                playerObject.transform.position = formationPositions[i];

                // Add components
                Player player = playerObject.AddComponent<Player>();
                player.Stats = team.Players[i].Stats;
                player.Attributes = team.Players[i].Attributes;

                Rigidbody rb = playerObject.AddComponent<Rigidbody>();
                rb.mass = 70f;
                rb.drag = 0.1f;

                CapsuleCollider collider = playerObject.AddComponent<CapsuleCollider>();
                collider.height = 1.8f;
                collider.radius = 0.3f;

                // Add visual (simple sphere)
                GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                visual.name = "Visual";
                visual.transform.parent = playerObject.transform;
                visual.transform.localPosition = Vector3.zero;
                visual.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                
                if (visual.GetComponent<Collider>() != null)
                    DestroyImmediate(visual.GetComponent<Collider>());

                Renderer renderer = visual.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material mat = new Material(Shader.Find("Standard"));
                    mat.color = team.PrimaryColor;
                    renderer.material = mat;
                }

                if (team == homeTeam)
                {
                    playerObject.tag = "HomePlayer";
                }
                else
                {
                    playerObject.tag = "AwayPlayer";
                }
            }
        }

        /// <summary>
        /// Get player positions based on formation
        /// </summary>
        private Vector3[] GetFormationPositions(Formation formation, float zPos)
        {
            return formation switch
            {
                Formation.F442 => Get442Positions(zPos),
                Formation.F433 => Get433Positions(zPos),
                Formation.F352 => Get352Positions(zPos),
                Formation.F541 => Get541Positions(zPos),
                _ => Get442Positions(zPos)
            };
        }

        private Vector3[] Get442Positions(float zPos)
        {
            return new Vector3[]
            {
                new Vector3(0, 1, zPos - 10), // GK
                new Vector3(-12, 1, zPos - 5), // LB
                new Vector3(-6, 1, zPos - 5), // CB
                new Vector3(6, 1, zPos - 5), // CB
                new Vector3(12, 1, zPos - 5), // RB
                new Vector3(-15, 1, zPos + 5), // LM
                new Vector3(-5, 1, zPos + 5), // CM
                new Vector3(5, 1, zPos + 5), // CM
                new Vector3(15, 1, zPos + 5), // RM
                new Vector3(-8, 1, zPos + 15), // ST
                new Vector3(8, 1, zPos + 15) // ST
            };
        }

        private Vector3[] Get433Positions(float zPos)
        {
            return new Vector3[]
            {
                new Vector3(0, 1, zPos - 10), // GK
                new Vector3(-12, 1, zPos - 5), // LB
                new Vector3(-6, 1, zPos - 5), // CB
                new Vector3(6, 1, zPos - 5), // CB
                new Vector3(12, 1, zPos - 5), // RB
                new Vector3(-8, 1, zPos + 2), // LM
                new Vector3(0, 1, zPos + 2), // CM
                new Vector3(8, 1, zPos + 2), // RM
                new Vector3(-10, 1, zPos + 12), // LW
                new Vector3(0, 1, zPos + 15), // ST
                new Vector3(10, 1, zPos + 12) // RW
            };
        }

        private Vector3[] Get352Positions(float zPos)
        {
            return new Vector3[]
            {
                new Vector3(0, 1, zPos - 10), // GK
                new Vector3(-8, 1, zPos - 5), // CB
                new Vector3(0, 1, zPos - 5), // CB
                new Vector3(8, 1, zPos - 5), // CB
                new Vector3(-15, 1, zPos + 5), // LWB
                new Vector3(-5, 1, zPos + 5), // CM
                new Vector3(0, 1, zPos + 8), // CM
                new Vector3(5, 1, zPos + 5), // CM
                new Vector3(15, 1, zPos + 5), // RWB
                new Vector3(-6, 1, zPos + 15), // ST
                new Vector3(6, 1, zPos + 15) // ST
            };
        }

        private Vector3[] Get541Positions(float zPos)
        {
            return new Vector3[]
            {
                new Vector3(0, 1, zPos - 10), // GK
                new Vector3(-15, 1, zPos - 5), // LB
                new Vector3(-8, 1, zPos - 5), // CB
                new Vector3(0, 1, zPos - 5), // CB
                new Vector3(8, 1, zPos - 5), // CB
                new Vector3(15, 1, zPos - 5), // RB
                new Vector3(-10, 1, zPos + 5), // LM
                new Vector3(-4, 1, zPos + 5), // CM
                new Vector3(4, 1, zPos + 5), // CM
                new Vector3(10, 1, zPos + 5), // RM
                new Vector3(0, 1, zPos + 15) // ST
            };
        }

        /// <summary>
        /// Setup ball
        /// </summary>
        private void SetupBall()
        {
            GameObject ballObject = new GameObject("Ball");
            ballObject.transform.position = Vector3.zero + Vector3.up * 0.5f;

            Rigidbody rb = ballObject.AddComponent<Rigidbody>();
            rb.mass = 0.43f;
            rb.drag = 0.1f;

            SphereCollider collider = ballObject.AddComponent<SphereCollider>();
            collider.radius = 0.22f;

            ball = ballObject.AddComponent<Ball>();

            // Visual
            GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            visual.name = "Visual";
            visual.transform.parent = ballObject.transform;
            visual.transform.localPosition = Vector3.zero;
            visual.transform.localScale = Vector3.one * 0.44f;

            if (visual.GetComponent<Collider>() != null)
                DestroyImmediate(visual.GetComponent<Collider>());

            Renderer renderer = visual.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = new Material(Shader.Find("Standard"));
                mat.color = Color.white;
                renderer.material = mat;
            }

            Debug.Log("[StadiumController] Ball created");
        }

        /// <summary>
        /// Setup lighting
        /// </summary>
        private void SetupLighting()
        {
            // Remove default light
            Light[] lights = FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                    light.intensity = 1.5f;
            }

            if (lights.Length == 0)
            {
                GameObject lightObject = new GameObject("DirectionalLight");
                Light light = lightObject.AddComponent<Light>();
                light.type = LightType.Directional;
                light.intensity = 1.5f;
                lightObject.transform.rotation = Quaternion.Euler(45, 45, 0);
            }

            Debug.Log("[StadiumController] Lighting setup");
        }

        /// <summary>
        /// Setup camera
        /// </summary>
        private void SetupCamera()
        {
            Camera camera = Camera.main;
            if (camera != null)
            {
                camera.transform.position = new Vector3(0, 30, -20);
                camera.transform.rotation = Quaternion.Euler(45, 0, 0);
                camera.fieldOfView = 60f;
            }

            Debug.Log("[StadiumController] Camera setup");
        }
    }
}