// Gear.cs (C)2015 by Alexander Schlottau, Hamburg, Germany
//   procedurally gernerates a gear mesh

using System;
using System.Collections;
using UnityEngine;
using System.Diagnostics;
[ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class ProceduralWormGear : MonoBehaviour {
	
	[Serializable]
	public class Point {
		public Vector3 offset = new Vector3(1f,0f,0f);
		public float uvOffX  = 0.0f, uvOffY  = 0.0f, uvScaleX = 1.0f, uvScaleY = 1.0f;
		public bool uvMapping = false;
		public int mat = 0;
	}
	
	[Serializable]
	public class Prefs {
		public float modul = 0.5f, toothWidth = 0.88f, dk = 0.0f, d = 0.0f,
		ramp = 0.72f, radius = 5.0f, lenght = 10.5f;
		public int  divisions = 20, teethParts = 1, bodyParts  = 1;
		public bool autoUV = true, tangens = false, flanks = true, capT = true, capB = true, lr = false;
	}
	
	public Point[] points;
	public Prefs prefs;
	public long ms;
	public float df = 0.0f;
	public int vt;
	private int pointsLenght = 0;
	private Mesh mesh;
	private Vector3[] v;
	private bool create = false;
	private int[] tp = new int[0];
	private int[,] tr = new int[0,0];
	
	public void UpdateGear() { if (!create) StartCoroutine("CreateGear"); }
	
	private IEnumerator CreateGear () {
		create = true;
		System.Diagnostics.Stopwatch stopWatch = new Stopwatch();
		stopWatch.Start();
		if(mesh == null || GetComponent<MeshFilter>().sharedMesh == null){
			GetComponent<MeshFilter>().mesh = mesh = new Mesh();
			mesh.name = "WormGear";
			mesh.hideFlags = HideFlags.HideAndDontSave;
		}
		while (prefs == null) yield return 0;
		
		if(points == null || points.Length == 0){
			points = new Point[]{ new Point(),new Point(),new Point(),new Point(),new Point()};
			points[0].offset.x = -1f;
			points[2].offset.x = prefs.modul*2f-0.02f;
			points[3].offset.Set(0f,-prefs.toothWidth,0f);
			points[3].uvMapping = true;
			transform.localScale = Vector3.one;
		}

		points[prefs.bodyParts-1].offset.y = prefs.lenght;
		points[prefs.bodyParts-1].offset.x = 0f;
		if (prefs.divisions < 3) prefs.divisions = 3;
		int bParts = prefs.bodyParts, tParts = prefs.teethParts, divisions = prefs.divisions,
		partCount = tParts+bParts, backParts = points[bParts].mat, vp = 0;
		points[bParts].offset.Set(0, prefs.toothWidth , 0.0f);
		if (prefs.toothWidth < 0f) prefs.toothWidth = 0f;
		if (prefs.modul < 0f) prefs.modul = 0f;
		prefs.d = divisions * prefs.modul*0.5f;
		prefs.dk = (divisions + 2.0f) * prefs.modul*0.5f;
		df = prefs.radius;
		float angle = 360.0f/divisions,
		offset = prefs.modul*2.25f;
		if (prefs.lr)
			angle=-angle;
		mesh.subMeshCount = GetComponent<Renderer>().sharedMaterials.Length;
		pointsLenght = points.GetLength(0);
		
		int vLenght = 60000;
		tp = new int[mesh.subMeshCount];
		tr = new int[mesh.subMeshCount,vLenght*6];
		v = new Vector3[vLenght+10];
		Vector2[] uvs = new Vector2[vLenght+10];
		
		// body (corpus)
		int[] tris = new int[12];
		if (!prefs.lr)
			tris = new int[] {0,3,2, 0,1,3, 0,2,3, 0,3,1};
		else
			tris = new int[] {0,2,3, 0,3,1, 0,3,2, 0,1,3};
		//front vertices, uv's and triangles
		for (int p = 0; p < bParts; p++) {

			if (vp > vLenght)
				break;

			points[p].offset.z = 0f;
			float x = df;
			float yoff = (p!=prefs.bodyParts-1) ? 0f : prefs.toothWidth;
			Vector3 p1 = new Vector3(points[p].offset.x   +x,  points[p].offset.y , 0f);
			Vector3 p3 = new Vector3(points[p+1].offset.x +x,  points[p+1].offset.y-yoff , 0f);
			Quaternion q = new Quaternion();
			if (prefs.autoUV) points[p].uvMapping = GetAutoUV(p, 1.0f);
			
			for (int i = 0; i < divisions+1; i ++) {

				if (vp > vLenght)
					break;
				
				q  = Quaternion.Euler(0f,angle*0.5f+angle*i, 0f);
				v[vp]   = q * p1;
				v[vp+1] = q * p3;
				SetUVs(uvs, p, i, vp, 5, 4, 0, 1);
				if (i != 0)
					SetTriangles(tris, p, 0, vp-2);
				vp+=2;
			}
		}
		//back vertices, uv's and triangles
		int lvp = vp, c, sp = backParts < 1 ? 0 : partCount+1;
		if (backParts > 0) {
			points[sp+backParts].offset.y = 0.0f;
			points[sp+backParts].offset.x = 0.0f;
			c = backParts;
		} else 
			c = bParts;
		for (int p = 0; p < c; p++) {

			if (vp > vLenght) 
				break;
		
		    points[p+sp].offset.z = 0f;
			float x = df;
			Vector3 p2 = new Vector3(points[p+sp].offset.x   +x, points[p+sp].offset.y , 0f);
			Vector3 p4 = new Vector3(points[p+sp+1].offset.x +x, points[p+1+sp].offset.y , 0f);
			Quaternion q = new Quaternion();
			if (prefs.autoUV && (backParts > 0)) points[p+sp].uvMapping = GetAutoUV(p+sp, -1.0f);
			
			for (int i = 0; i < divisions+1; i ++) {

				if (vp > vLenght)
					break;
				
				q  = Quaternion.Euler(0f,angle*0.5f+angle*i, 0f);
				v[vp]   = q * p2;
				v[vp+1] = q * p4;
				SetUVs(uvs, p, i, vp, 4, 5, 5, 5);
				if (i != 0)
					for (int j=0; j<6; j++)
						tr[points[p+sp].mat,j+tp[points[p+sp].mat]] = tris[j+6]+vp-2;
				tp[points[p+sp].mat]+=6;
				vp+=2;
			}
		}
		// cap body

		if (prefs.capB && points[0].offset.y != 0.0f) {
		if (!prefs.lr)
			tris = new int[] {0,2,1, 2,3,1}; 
		else
			tris = new int[] {0,1,2, 2,1,3};
			for (int i = 0; i < divisions+1; i ++) { 

				if (vp > vLenght)
					break;
				
				v[vp]   = v[i*2];
				v[vp+1] = v[i*2+lvp];
				int part = pointsLenght-2;
				SetUVs(uvs, part, i, vp, 4, 5, 3, 2);
				vp+=2;
				if (i!=0){
					SetTriangles(tris, part, 0, vp-4);
				}
			}
		}


		// tooth (cog)

		Vector3[] capVectors = new Vector3[8];
		if (!prefs.lr)
			tris = new int[] {0,1,2, 1,3,2 , 0,2,1, 1,2,3};
		else
			tris = new int[] {0,2,1, 1,2,3 , 0,1,2, 1,3,2};

		for (int p = bParts; p < partCount; p++) {

			if (vp > vLenght)
				break;

			lvp = vp;
			float x1 = points[p].offset.x  * offset+df,
			y1 = points[p].offset.y * prefs.toothWidth ,
			z1 = -points[p].offset.z * offset,
			x2 = points[p+1].offset.x* offset+df,
			y2 = points[p+1].offset.y* prefs.toothWidth,
			z2 = -points[p+1].offset.z* offset;
			
			Vector3[] v3 = new Vector3[]{
				new Vector3(x1, y1, z1), new Vector3(x1, -y1, z1), 
				new Vector3(x2, y2, z2), new Vector3(x2,-y2, z2)}; 
			Quaternion q = new Quaternion();
			Vector3 offs = Vector3.zero;
			//prefs.thickness = prefs.modul;
			float threadLead = prefs.modul * Mathf.PI;
			float rounds = points[bParts-1].offset.y / threadLead - 0.5f;

			if (prefs.autoUV) points[p].uvMapping = GetAutoUV(p, 1.0f);


			for (int i = (int)(divisions*0.5f); i < divisions * (rounds); i ++) {

				if (vp > vLenght)
					break;
				
				q  = Quaternion.Euler(0f,angle*i+(angle*0.5f), 0f);
				offs = new Vector3(0f,threadLead/divisions*i,0f);
				v[vp]   = q * v3[0]+offs;
				v[1+vp] = q * v3[2]+offs;

				// all UVs
				SetUVs(uvs, p, i, vp, 5, 4, 0, 1);

				// triangles
				if (i!=(int)(divisions*0.5f)) {
					SetTriangles(tris, p+1, 0, vp-2);
				} else {
					capVectors[0] = v[vp];
					capVectors[1] = v[vp+1];
				}
				vp+=2;
			}
			capVectors[4] = v[vp-2];
			capVectors[5] = v[vp-1];
		
			for (int i = (int)(divisions*0.5f); i < divisions * (rounds); i ++) {

				if (vp > vLenght)
					break;
				
				q  = Quaternion.Euler(0f,angle*i+(angle*0.5f), 0f);
				offs = new Vector3(0f,threadLead/divisions*i,0f);
				v[vp]   = q * v3[1]+offs;
				v[1+vp] = q * v3[3]+offs;
				
				// all UVs
				SetUVs(uvs, p, i, vp, 5, 4, 0, 1);

				// triangles
				if (i!=(int)(divisions*0.5f)) {
					SetTriangles(tris, p+1, 6, vp-2);
				} else {
					capVectors[2] = v[vp];
					capVectors[3] = v[vp+1];
				}
				SetTriangles(tris, p+1, 6, vp);
				vp+=2;
			}
			capVectors[6] = v[vp-2];
			capVectors[7] = v[vp-1];


			if (vp < vLenght) {
				// end cap
				v [vp] = capVectors [4];
				v [1 + vp] = capVectors [5];
				v [2 + vp] = capVectors [6];
				v [3 + vp] = capVectors [7];
				if (prefs.flanks) {
					SetUVs (uvs, points.Length - 1, 1, vp, 6, 6, 6, 6);
					SetUVs (uvs, points.Length - 1, 1, vp + 2, 6, 6, 6, 6);
				} else {
					SetUVs (uvs, points.Length - 1, 1, vp, 5, 4, 6, 6);
					SetUVs (uvs, points.Length - 1, 1, vp + 2, 5, 4, 6, 6);
				}
				SetTriangles (tris, p + 1, 0, vp);
				SetTriangles (tris, p + 1, 6, vp + 2);

				vp += 4;

				// beginn cap
				v [vp] = capVectors [0];
				v [1 + vp] = capVectors [2];
				v [2 + vp] = capVectors [1];
				v [3 + vp] = capVectors [3];
				if (prefs.flanks) {
					SetUVs (uvs, points.Length - 1, 1, vp, 6, 6, 6, 6);
					SetUVs (uvs, points.Length - 1, 1, vp + 2, 6, 6, 6, 6);
				} else {
					SetUVs (uvs, points.Length - 1, 1, vp, 5, 4, 6, 6);
					SetUVs (uvs, points.Length - 1, 1, vp + 2, 5, 4, 6, 6);
				}
				SetTriangles (tris, p + 1, 0, vp);
				SetTriangles (tris, p + 1, 6, vp + 2);

				vp += 4;
			}
			// caps tooth
			if (prefs.capT && (points[partCount].offset.y != 0.0f && p == partCount-1)) {
				for (int i = 0; i < divisions * (rounds) - (int)(divisions*0.5f); i ++) {

					if (vp > vLenght)
						break;
					
					q  = Quaternion.Euler(0f,angle*i, 0f);
					offs = new Vector3(0f,threadLead/divisions*i,0f);
					v[vp]   = q * capVectors[1]+offs;
					v[1+vp] = q * capVectors[3]+offs;
					// all UVs
					if (!points[p].uvMapping)
						SetUVs(uvs, points.Length-1, i, vp, 5, 4, 6, 6);
					else
						SetUVs(uvs, points.Length-1, 0, vp, 5, 4, 6, 6);
					// triangles
					if (i!=0) {
						SetTriangles(tris, p+1, 0, vp-2);
					} 
					vp+=2;
				}
			}

		}

		Vector3[] vert = new Vector3[vp];
		for (int i = 0; i < vp; i++) {
			vert[i] = v[i];
		}
		v = vert;
		Vector2[] u = new Vector2[vp];
		for (int i = 0; i < vp; i++) {
			u[i] = uvs[i];
		}
		uvs = u;

		// update mesh
		if (mesh.vertices.Length != vp)
			mesh.Clear(); 
		mesh.subMeshCount = GetComponent<Renderer>().sharedMaterials.Length;
		mesh.vertices = new Vector3[v.Length];
		mesh.vertices = v;
		for (int i=0; i<mesh.subMeshCount; i++){
			int[] tmpTr = new int[tp[i]];
			for (int j=0; j<tp[i]; j++)
				tmpTr[j] = tr[i,j];
			mesh.SetTriangles(tmpTr, i);
		}
		mesh.colors = new Color[mesh.vertices.Length];	
		mesh.uv = uvs;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		if (prefs.tangens)
			RecalculateTangens(mesh);
		;
		
		vt = vp; 
		v = null; tr = null; tp = null;
		create = false;
		stopWatch.Stop();
		ms = stopWatch.ElapsedMilliseconds;
	}
	

	private	void SetTriangles (int[] _triangles, int _part,int _starIndex, int _vertexIndex)  {

		for (int j=_starIndex; j<_starIndex+6; j++)
			tr[points[_part].mat,j+tp[points[_part].mat]] = _triangles[j]+_vertexIndex;
		tp [points [_part].mat] += 6;
	}

	private void SetUVs(Vector2[] _UVs, int _part, int _index, int _vertexIndex, int _a, int _b, int _c, int _d) {
		
		if(points[_part].uvMapping) {
			_UVs[_vertexIndex] = UV(v[_vertexIndex], _part,_index, _b );
			_UVs[_vertexIndex+1] = UV(v[_vertexIndex+1], _part, _index, _a);}
		else {
			_UVs[_vertexIndex] = UV(v[_vertexIndex], _part, _index, _c);
			_UVs[_vertexIndex+1] = UV(v[_vertexIndex+1], _part, _index, _d);}
		
	}

	private Vector2 UV(Vector3 _point,int _p, int _i, int _j) {
		
		if (_j == 2)
			return new Vector2((float)(_i)/(prefs.divisions)*points[_p].uvScaleX+points[_p].uvOffX,
			                   points[_p].uvScaleX+points[_p].uvOffY+0.5f);
		if (_j == 3)
			return new Vector2((float)(_i)/(prefs.divisions)*points[_p].uvScaleX+points[_p].uvOffX,
			                   1f+points[_p].uvOffY*points[_p].uvScaleY);
		if (_j == 6)
			return new Vector2(-_point.x*points[_p].uvScaleX+points[_p].uvOffX,
			                   _point.y*points[_p].uvScaleY+points[_p].uvOffY*points[_p].uvScaleY);
		if (_j == 7)
			return new Vector2(points[_p].uvScaleX+points[_p].uvOffX,
			                   points[_p].uvScaleY+points[_p].uvOffY*points[_p].uvScaleY);
		
		if (points[_p].uvMapping) 
			return new Vector2((float)(_i+1)/(prefs.divisions)*points[_p].uvScaleX+points[_p].uvOffX,
			                   (float)_j*points[_p].uvScaleY+points[_p].uvOffY*points[_p].uvScaleY);
		if (_j!=5)
			return new Vector2(_point.x*points[_p].uvScaleX+points[_p].uvOffX,
			                   _point.z*points[_p].uvScaleY+points[_p].uvOffY*points[_p].uvScaleY);
		else
			return new Vector2(-(_point.x*points[_p].uvScaleX+points[_p].uvOffX),
			                   _point.z*points[_p].uvScaleY+points[_p].uvOffY*points[_p].uvScaleY);
	}
	
	private bool GetAutoUV(int p, float side) {
		if (side*(points[p+1].offset.y-points[p].offset.y)*Mathf.Asin(prefs.ramp) < 
		    -(points[p+1].offset.x-points[p].offset.x)*Mathf.Acos(prefs.ramp))
			return true;
		else
			return false;
	}
	
	private void RecalculateTangens(Mesh _mesh) {
		
		int[] triangles = _mesh.triangles;
		Vector3[] vertices = _mesh.vertices;
		Vector2[] uv = _mesh.uv;
		Vector3[] normals = _mesh.normals;
		int triangleCount = triangles.Length;
		int vertexCount = vertices.Length;
		
		Vector3[] tan1 = new Vector3[vertexCount];
		Vector3[] tan2 = new Vector3[vertexCount];
		Vector4[] tangents = new Vector4[vertexCount];
		
		for (long a = 0; a < triangleCount; a += 3) {
			
			long i1 = triangles[a + 0], i2 = triangles[a + 1], i3 = triangles[a + 2];
			Vector3 v1 = vertices[i1], v2 = vertices[i2], v3 = vertices[i3];
			Vector2 w1 = uv[i1], w2 = uv[i2], w3 = uv[i3];
			
			float 	x1 = v2.x - v1.x, x2 = v3.x - v1.x,
			y1 = v2.y - v1.y, y2 = v3.y - v1.y,
			z1 = v2.z - v1.z, z2 = v3.z - v1.z,
			s1 = w2.x - w1.x, s2 = w3.x - w1.x,
			t1 = w2.y - w1.y, t2 = w3.y - w1.y,
			dz = s1 * t2 - s2 * t1,
			r  = dz == 0.0f ? 0.0f : 1.0f / dz;
			Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r),
			tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);
			tan1[i1] += sdir; tan1[i2] += sdir; tan1[i3] += sdir;
			tan2[i1] += tdir; tan2[i2] += tdir; tan2[i3] += tdir;
		}
		
		for (long a = 0; a < vertexCount; ++a) {
			Vector3 n = normals[a], t = tan1[a];
			Vector3.OrthoNormalize(ref n, ref t);
			tangents[a].x = t.x; tangents[a].y = t.y; tangents[a].z = t.z;
			tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
		}
		mesh.tangents = tangents;
	}

	
	public void RemoveGear() {
		if(Application.isEditor){
			GetComponent<MeshFilter>().mesh = null;
			DestroyImmediate(mesh, true);
		}
	}
	
	void OnEnable () 	{
		UpdateGear();
		if (GetComponent<Renderer>().sharedMaterial == null) {
			GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find("Diffuse"));
			if (GetComponent<Renderer>().sharedMaterial != null)
				GetComponent<Renderer>().sharedMaterial.color = Color.grey;
		}
	}
	
	void OnDisable () 	{StopCoroutine("CreateGear");}
	
	void OnDestroy () 	{StopCoroutine("CreateGear"); RemoveGear();}
	
	void Reset () 		{UpdateGear();}	
}

