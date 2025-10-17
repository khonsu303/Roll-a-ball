using UnityEngine;
using UnityEditor; 

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))] // Asegura que se pueda recoger
public class FormaPersonalizada : MonoBehaviour
{
	// Tipos de forma disponibles
	public enum TipoForma { Cubo, Anillo }

	// --- EVENTO DE RECOGIDA ---
	public delegate void ColeccionableRecogido(TipoForma forma);
	public static event ColeccionableRecogido OnColeccionableRecogido;

	[Header("Tipo de forma del coleccionable")]
	public TipoForma tipoForma = TipoForma.Cubo;

	[Header("Parámetros del Anillo")]
	[Range(0.1f, 1f)] public float radioMayor = 0.5f;
	[Range(0.05f, 0.4f)] public float radioMenor = 0.15f;
	[Range(3, 64)] public int segmentosRadiales = 24;
	[Range(3, 32)] public int segmentosTubulares = 12;

	private MeshFilter meshFilter;
	private TipoForma formaAnterior;

	void OnValidate()
	{
		if (meshFilter == null)
			meshFilter = GetComponent<MeshFilter>();

		// Asegura que el collider sea Trigger para OnTriggerEnter
		Collider col = GetComponent<Collider>();
		if (col != null) col.isTrigger = true;

		if (formaAnterior != tipoForma || tipoForma == TipoForma.Anillo)
		{
			GenerarForma();
		}
		formaAnterior = tipoForma;
	}

	void Start()
	{
		GenerarForma();
		formaAnterior = tipoForma;
	}

	void OnTriggerEnter(Collider other)
	{
		// Verificar si el objeto que entró en el trigger tiene la etiqueta "Player"
		if (other.CompareTag("Player"))
		{
			// 1. Notificar al gestor de audio sobre el tipo de forma recogida
			if (OnColeccionableRecogido != null)
			{
				OnColeccionableRecogido(tipoForma);
			}

			// 2. Destruir el coleccionable
			Destroy(gameObject);
		}
	}

	private void OnDestroy()
	{
		if (meshFilter != null && meshFilter.sharedMesh != null)
		{
			if(Application.isEditor && !Application.isPlaying)
			{
				DestroyImmediate(meshFilter.sharedMesh);
			}
			else
			{
				Destroy(meshFilter.sharedMesh);
			}
		}
	}

	void GenerarForma()
	{
		if (meshFilter == null)
			meshFilter = GetComponent<MeshFilter>();

		if (meshFilter.sharedMesh != null)
		{
			if (Application.isEditor && !Application.isPlaying)
				DestroyImmediate(meshFilter.sharedMesh);
			else
				Destroy(meshFilter.sharedMesh);
		}

		Mesh mesh = null;

		switch (tipoForma)
		{
		case TipoForma.Cubo:
			mesh = CrearCubo();
			break;
		case TipoForma.Anillo:
			mesh = CrearTorus();
			break;
		}

		meshFilter.sharedMesh = mesh;
	}

	// -------------------------------------------------------------------------
	// ## CUBO: Implementación de Malla ##
	// -------------------------------------------------------------------------
	Mesh CrearCubo()
	{
		// ... (Implementación de CrearCubo, la cual ya tienes correcta) ...
		Mesh mesh = new Mesh();
		mesh.name = "CuboPersonalizado";

		Vector3[] vertices = new Vector3[24];
		Vector3[] normales = new Vector3[24];
		Vector2[] uvs = new Vector2[24];
		int[] triangles = new int[36];

		float s = 0.5f; 

		// 1. Cara Frontal (Z+)
		vertices[0] = new Vector3(-s, -s, s); normales[0] = Vector3.forward; uvs[0] = new Vector2(0, 0);
		vertices[1] = new Vector3(s, -s, s);  normales[1] = Vector3.forward; uvs[1] = new Vector2(1, 0);
		vertices[2] = new Vector3(s, s, s);   normales[2] = Vector3.forward; uvs[2] = new Vector2(1, 1);
		vertices[3] = new Vector3(-s, s, s);  normales[3] = Vector3.forward; uvs[3] = new Vector2(0, 1);

		// 2. Cara Trasera (Z-)
		vertices[4] = new Vector3(s, -s, -s);  normales[4] = Vector3.back; uvs[4] = new Vector2(0, 0);
		vertices[5] = new Vector3(-s, -s, -s); normales[5] = Vector3.back; uvs[5] = new Vector2(1, 0);
		vertices[6] = new Vector3(-s, s, -s);  normales[6] = Vector3.back; uvs[6] = new Vector2(1, 1);
		vertices[7] = new Vector3(s, s, -s);   normales[7] = Vector3.back; uvs[7] = new Vector2(0, 1);

		// 3. Cara Superior (Y+)
		vertices[8] = new Vector3(-s, s, s);   normales[8] = Vector3.up; uvs[8] = new Vector2(0, 0);
		vertices[9] = new Vector3(s, s, s);    normales[9] = Vector3.up; uvs[9] = new Vector2(1, 0);
		vertices[10] = new Vector3(s, s, -s);  normales[10] = Vector3.up; uvs[10] = new Vector2(1, 1);
		vertices[11] = new Vector3(-s, s, -s); normales[11] = Vector3.up; uvs[11] = new Vector2(0, 1);

		// 4. Cara Inferior (Y-)
		vertices[12] = new Vector3(-s, -s, -s); normales[12] = Vector3.down; uvs[12] = new Vector2(0, 0);
		vertices[13] = new Vector3(s, -s, -s);  normales[13] = Vector3.down; uvs[13] = new Vector2(1, 0);
		vertices[14] = new Vector3(s, -s, s);   normales[14] = Vector3.down; uvs[14] = new Vector2(1, 1);
		vertices[15] = new Vector3(-s, -s, s);  normales[15] = Vector3.down; uvs[15] = new Vector2(0, 1);

		// 5. Cara Derecha (X+)
		vertices[16] = new Vector3(s, -s, s);   normales[16] = Vector3.right; uvs[16] = new Vector2(0, 0);
		vertices[17] = new Vector3(s, -s, -s);  normales[17] = Vector3.right; uvs[17] = new Vector2(1, 0);
		vertices[18] = new Vector3(s, s, -s);   normales[18] = Vector3.right; uvs[18] = new Vector2(1, 1);
		vertices[19] = new Vector3(s, s, s);    normales[19] = Vector3.right; uvs[19] = new Vector2(0, 1);

		// 6. Cara Izquierda (X-)
		vertices[20] = new Vector3(-s, -s, -s); normales[20] = Vector3.left; uvs[20] = new Vector2(0, 0);
		vertices[21] = new Vector3(-s, -s, s);  normales[21] = Vector3.left; uvs[21] = new Vector2(1, 0);
		vertices[22] = new Vector3(-s, s, s);   normales[22] = Vector3.left; uvs[22] = new Vector2(1, 1);
		vertices[23] = new Vector3(-s, s, -s); normales[23] = Vector3.left; uvs[23] = new Vector2(0, 1);

		// Definición de triángulos
		for (int i = 0; i < 6; i++)
		{
			int v = i * 4;
			int t = i * 6;

			// Primer triángulo: 0, 2, 1
			triangles[t + 0] = v + 0;
			triangles[t + 1] = v + 2;
			triangles[t + 2] = v + 1;

			// Segundo triángulo: 0, 3, 2
			triangles[t + 3] = v + 0;
			triangles[t + 4] = v + 3;
			triangles[t + 5] = v + 2;
		}

		mesh.vertices = vertices;
		mesh.normals = normales;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
		return mesh;
	}

	// -----------------------------------------------------------
	// ## ANILLO (Torus): Implementación de Malla ##
	// -----------------------------------------------------------
	Mesh CrearTorus()
	{
		// ... (Implementación de CrearTorus, la cual ya tienes correcta) ...
		Mesh mesh = new Mesh();
		mesh.name = "AnilloPersonalizado";

		int numVertsTubular = segmentosTubulares + 1;
		int numVertsRadial = segmentosRadiales + 1;
		int numVerts = numVertsRadial * numVertsTubular;

		Vector3[] vertices = new Vector3[numVerts];
		Vector3[] normales = new Vector3[numVerts];
		Vector2[] uvs = new Vector2[numVerts];
		int[] triangles = new int[segmentosRadiales * segmentosTubulares * 6];

		int index = 0;
		// Generación de Vértices, Normales y UVs
		for (int i = 0; i < numVertsRadial; i++)
		{
			float anguloPrincipal = (float)i / segmentosRadiales * Mathf.PI * 2f;
			float cosPrincipal = Mathf.Cos(anguloPrincipal);
			float sinPrincipal = Mathf.Sin(anguloPrincipal);

			Vector3 centro = new Vector3(cosPrincipal * radioMayor, 0, sinPrincipal * radioMayor);

			for (int j = 0; j < numVertsTubular; j++)
			{
				float anguloSecundario = (float)j / segmentosTubulares * Mathf.PI * 2f;
				float cosSecundario = Mathf.Cos(anguloSecundario);
				float sinSecundario = Mathf.Sin(anguloSecundario);

				Vector3 offset = new Vector3(
					cosPrincipal * cosSecundario * radioMenor,
					sinSecundario * radioMenor,
					sinPrincipal * cosSecundario * radioMenor
				);

				vertices[index] = centro + offset;
				normales[index] = offset.normalized;

				uvs[index] = new Vector2(
					(float)i / segmentosRadiales,
					(float)j / segmentosTubulares
				);

				index++;
			}
		}

		// Generación de Triángulos
		int tri = 0;
		for (int i = 0; i < segmentosRadiales; i++)
		{
			for (int j = 0; j < segmentosTubulares; j++)
			{
				int a = (i * numVertsTubular) + j;
				int b = ((i + 1) * numVertsTubular) + j;
				int d = (i * numVertsTubular) + j + 1;
				int c = ((i + 1) * numVertsTubular) + j + 1;

				// Triángulo 1: a, d, b
				triangles[tri++] = a;
				triangles[tri++] = d;
				triangles[tri++] = b;

				// Triángulo 2: b, d, c
				triangles[tri++] = b;
				triangles[tri++] = d;
				triangles[tri++] = c;
			}
		}

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normales;
		mesh.uv = uvs;
		mesh.RecalculateBounds();

		return mesh;
	}
}