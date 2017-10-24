// Gear.cs (C)2013 by Alexander Schlottau, Hamburg, Germany
//   procedurally gernerates a gear mesh

using System;
using System.Collections;
using UnityEngine;
using System.Diagnostics;
[ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class ProceduralGear : MonoBehaviour {
	
	[Serializable]
	public class Point {
		public Vector3 offset = new Vector3(1f,1f,0f);
		public float uvOffX  = 0.0f, uvOffY  = 0.0f, uvScaleX = 1.0f, uvScaleY = 1.0f;
		public bool uvMapping = false;
		public int mat = 0;
	}
	
	[Serializable]
	public class Prefs {
		public float modul = 0.5f, thickness = 1.0f, dk = 0.0f, d = 0.0f,
					 ramp = 0.7f, slope = 0.0f, coneX = 0.0f, coneY = 0.0f;
		public int  teethCount = 20, teethParts = 1, bodyParts  = 1;
		public bool autoUV = true, tangens = false, flanks = true, inner = false, capT = true, capB = true;
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

	public void UpdateGear() { if (!create) StartCoroutine("CreateGear"); }
	
	private IEnumerator CreateGear () {
		create = true;
		System.Diagnostics.Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
		if(mesh == null){
			GetComponent<MeshFilter>().mesh = mesh = new Mesh();
			mesh.name = "Gear";
			mesh.hideFlags = HideFlags.HideAndDontSave;
		}
		while (prefs == null) yield return 0;
		
		if(points == null || points.Length == 0){
			points = new Point[]{ new Point(),new Point(),new Point(),new Point(),new Point()};
			points[0].offset.x = -1f;
			points[2].offset.x = prefs.modul*2f-0.02f;
			points[3].offset.Set(-1f,-1f,0f);
			points[3].uvMapping = true;
			transform.localScale = Vector3.one;
		}
		if (prefs.teethCount < 3) prefs.teethCount = 3;
		int bParts = prefs.bodyParts, tParts = prefs.teethParts, tCount = prefs.teethCount,
			partCount = tParts+bParts, backParts = points[bParts].mat, vp = 0;
		points[bParts].offset.Set(0, prefs.thickness , 0.0f);
		if (prefs.thickness < 0f) prefs.thickness = 0f;
		if (prefs.modul < 0f) prefs.modul = 0f;
		prefs.d = tCount * prefs.modul*0.5f;
		prefs.dk = (tCount + 2.0f) * prefs.modul*0.5f;
		df = (tCount - 2.5f) * prefs.modul*0.5f;
		float angle = 360.0f/tCount,
			 offset = prefs.modul*2.25f;
		mesh.subMeshCount = GetComponent<Renderer>().sharedMaterials.Length;
		pointsLenght = points.GetLength(0);

		int vLenght = tCount*tParts*24 +(backParts>0 ? bParts+backParts : bParts*2) *(tCount+1)*2;
		if (prefs.capT) vLenght+= tParts*tCount*4;
		if (prefs.capB) vLenght+= bParts*(tCount+1)*2;
	
		if (vLenght < 62000) {
			int[] tp = new int[mesh.subMeshCount];
			int[,] tr = new int[mesh.subMeshCount,vLenght*3];
			v = new Vector3[vLenght];
			Vector2[] uvs = new Vector2[vLenght];
			
			// body (corpus)
			int[] tris = new int[12];
			float inner = 1.0f, innOff = 0.0f, s = 0.0f;
			if (prefs.inner) {
				innOff = prefs.modul*2f;
				tris = new int[] {0,2,3, 0,3,1, 0,3,2, 0,1,3};
			} else
				tris = new int[] {0,3,2, 0,1,3, 0,2,3, 0,3,1};
				//front vertices, uv's and triangles
			for (int p = 0; p < bParts; p++) {
				if (p == bParts-1) s = prefs.slope;
				if (points[p].offset.z != 0f) points[p].offset.z = 0f;
				float x = df+innOff, y = prefs.thickness*inner;
				Vector3 p1 = new Vector3(points[p].offset.x   +x,  points[p].offset.y   *y*inner, 0f);
				Vector3 p3 = new Vector3(points[p+1].offset.x +x,  points[p+1].offset.y *y*inner,  s);
				Quaternion q = new Quaternion();
				if (prefs.autoUV) points[p].uvMapping = GetAutoUV(p, 1.0f);
				
				for (int i = 0; i < tCount+1; i ++) {
					q  = Quaternion.Euler(0f,angle*0.5f+angle*i, 0f);
					v[vp]   = q * p1;
					v[vp+1] = q * p3;
					if(points[p].uvMapping) {
						uvs[vp] = UV(v[vp],p,i,prefs.inner?5:4);
						uvs[vp+1] = UV(v[vp+1],p,i,prefs.inner?4:5);}
					else {
						uvs[vp] = UV(v[vp],p,i,0);
						uvs[vp+1] = UV(v[vp+1],p,i,1);}
					if (i != 0)
						for (int j=0; j<6; j++)
							tr[points[p].mat,j+tp[points[p].mat]] = tris[j]+vp-2;
					tp[points[p].mat]+=6;
					vp+=2;
				}
			}
				//back vertices, uv's and triangles
			float inv = 1.0f; s = 0;
			int svp = vp, lvp = vp, bvp = vp, c, sp = backParts < 1 ? 0 : partCount+1;
			if (backParts > 0) {
				points[sp+backParts].offset.y = prefs.thickness;
				points[sp+backParts].offset.x = 0.0f;
				inv = -1.0f; c = backParts;
			} else 
				c = bParts;
			for (int p = 0; p < c; p++) {
				if (p == backParts-1 || points[bParts].mat == -1) s = prefs.slope;
				if (points[p+sp].offset.z != 0f) points[p+sp].offset.z = 0f;
				if (prefs.inner) innOff = prefs.modul*2f;
				float x = df+innOff, y = prefs.thickness*inner;
				Vector3 p2 = new Vector3(points[p+sp].offset.x   +x, -points[p+sp].offset.y   *y*inv*inner, 0f);
				if(inv < 0) if (p==c-1) y = -y;
				Vector3 p4 = new Vector3(points[p+sp+1].offset.x +x, -points[p+1+sp].offset.y *y*inv*inner, -s);
				Quaternion q = new Quaternion();
				if (prefs.autoUV && (backParts > 0)) points[p+sp].uvMapping = GetAutoUV(p+sp, -1.0f);
				
				for (int i = 0; i < tCount+1; i ++) {
					q  = Quaternion.Euler(0f,angle*0.5f+angle*i, 0f);
					v[vp]   = q * p2;
					v[vp+1] = q * p4;
					if(points[p+sp].uvMapping) {
						uvs[vp] = UV(v[vp],p+sp,i,prefs.inner?4:5);
						uvs[vp+1] = UV(v[vp+1],p+sp,i,prefs.inner?5:4);}
					else {
						uvs[vp] = UV(v[vp],p+sp,i,5);
						uvs[vp+1] = UV(v[vp+1],p+sp,i,5);}
					if (i != 0)
						for (int j=0; j<6; j++)
							   tr[points[p+sp].mat,j+tp[points[p+sp].mat]] = tris[j+6]+vp-2;
					tp[points[p+sp].mat]+=6;
					vp+=2;
				}
			}
			// cap body
			int cvp = vp;
			if (prefs.capB && points[0].offset.y != 0.0f) {
				if (!prefs.inner)
					 tris = new int[] {0,2,1, 2,3,1}; 
				else tris = new int[] {0,1,2, 2,1,3};
				for (int i = 0; i < tCount+1; i ++) { 
					v[vp]   = v[i*2];
					v[vp+1] = v[i*2+lvp];
					int part = pointsLenght-2;
					if (points[part].uvMapping) {
						uvs[vp+1] = UV(v[vp+1],part,i,prefs.inner?5:4);
						uvs[vp] = UV(v[vp],part,i,prefs.inner?4:5); }
					else {
						uvs[vp+1] = UV(v[vp+1],part,i,2);
						uvs[vp] = UV(v[vp],part,i,3); }
					vp+=2;
					if (i!=0){
						for (int j=0; j<6; j++) 
							tr[points[part].mat,j+tp[points[part].mat]] = tris[j]+vp-4;
						tp[points[part].mat]+=6;
					}
				}
			}

			// teeth (cogs)
			if (!prefs.inner)
				tris = new int[] {0,1,2, 1,3,2, 4,6,5, 5,6,7, 8,10,12, 12,10,14, 11,9,13, 11,13,15};
			else
				tris = new int[] {0,2,1, 1,2,3, 4,5,6, 5,7,6, 8,12,10, 12,14,10, 11,13,9, 11,15,13};
			svp = vp;
			float cX = prefs.coneX, cY = prefs.coneY*2f, c2X = 0.0f, c2Y = 0.0f;
			
			for (int p = bParts; p < partCount; p++) {
				if (p > bParts) { c2X = prefs.coneX; c2Y = prefs.coneY*2f; }
				if (points[p].offset.z < 0f) points[p].offset.z = 0f;
				if (points[p+1].offset.z < 0f) points[p+1].offset.z = 0f;
				if (points[p].offset.y < 0f) points[p].offset.y = 0f;
				if (points[p+1].offset.y < 0f) points[p+1].offset.y = 0f;
				lvp = vp;
				float x1 = points[p].offset.x  * offset+df + innOff,
					  y1 = points[p].offset.y *prefs.thickness ,
				      z1 = -points[p].offset.z  * offset,
					  x2 = points[p+1].offset.x* offset+df + innOff,
				      y2 = points[p+1].offset.y*prefs.thickness,
				      z2 = -points[p+1].offset.z* offset;
				
				Vector3[] v3 = new Vector3[]{new Vector3(x1+c2Y, y1, (z1+s)*(1+c2X)),new Vector3(x1+c2Y, y1,(-z1+s)*(1+c2X)),
										new Vector3(x1-c2Y,-y1, (z1-s)*(1-c2X)),new Vector3(x1-c2Y,-y1,(-z1-s)*(1-c2X)), 
											 new Vector3(x2+cY, y2, (z2+s)*(1+cX)),new Vector3(x2+cY, y2,(-z2+s)*(1+cX)),
										new Vector3(x2-cY,-y2, (z2-s)*(1-cX)),new Vector3(x2-cY,-y2,(-z2-s)*(1-cX))}; 
				Quaternion q = new Quaternion();
				
				if (prefs.autoUV) points[p].uvMapping = GetAutoUV(p, 1.0f);
					
				for (int i = 0; i < tCount; i ++) {
					q  = Quaternion.Euler(0f,angle*i, 0f);
					if (p!=bParts) {	//top & bottom vertices of a tooth
						v[vp]   = q * v3[0];
						v[1+vp] = q * v3[1];
						v[2+vp] = q * v3[4];
						v[3+vp] = q * v3[5];
						v[4+vp] = q * v3[2]; 
						v[5+vp] = q * v3[3];
						v[6+vp] = q * v3[6];
						v[7+vp] = q * v3[7];
					} else {					//top & bottom at base line vertices
						v[2+vp] = q * v3[4];
						v[3+vp] = q * v3[5];
						v[6+vp] = q * v3[6];
						v[7+vp] = q * v3[7];
						q  = Quaternion.Euler(0f,angle*i + angle*0.5f, 0f);
						v[vp]   = q * v3[0];
						v[4+vp] = q * v3[2];
						q  = Quaternion.Euler(0f,angle*i - angle*0.5f, 0f);
						v[5+vp] = q * v3[3];
						v[1+vp] = q * v3[1];
					}
					v[8+vp]  = v[vp];			// left & right vertices of a tooth
					v[9+vp]  = v[1+vp];
					v[10+vp] = v[2+vp];
					v[11+vp] = v[3+vp];
					v[12+vp] = v[4+vp];
					v[13+vp] = v[5+vp];
					v[14+vp] = v[6+vp];
					v[15+vp] = v[7+vp];
												// all UVs
					if (points[p].uvMapping) {
						uvs[vp+2] = UV(uvs[vp+2],p,i+1,2);
						uvs[vp+3] = UV(uvs[vp+3],p,i,2);
						uvs[vp]   = UV(uvs[vp],p,i+1,3);
						uvs[vp+1] = UV(uvs[vp+1],p,i,3);
						uvs[vp+4] = uvs[vp+2]; uvs[vp+5] = uvs[vp+3];
						uvs[vp+6] = uvs[vp+0]; uvs[vp+7] = uvs[vp+1];
					} else 
						for (int j=0; j<4;j++) {
							uvs[vp+j]   = UV(v[vp+j],p,i,4);
							uvs[vp+j+4] = UV(v[vp+j+4],p,i,5);
						}
					for (int j=0; j<4;j++) {
						if (prefs.flanks) {
							uvs[vp+j+8] =  UV(v[svp+j+8],p,1,6);
							uvs[vp+j+12] = UV(v[svp+j+12],p,1,6); 
						} else {
							uvs[vp+j+8] = UV(v[vp+j+8],p,i,4);
							uvs[vp+j+12] = UV(v[vp+j+12],p,i,5);
						}
					}
												// all triangles of a tooth
					for (int j=0; j<24; j++)
						tr[points[p+1].mat,j+tp[points[p+1].mat]] = tris[j]+vp;
					vp+=16;
					tp[points[p+1].mat] += 24;
				}
			}
			// caps teeth
			if (prefs.capT && (points[partCount].offset.y != 0.0f && 
									points[partCount].offset.z != 0.0f)) {
					if (!prefs.inner) 
						 tris = new int[] {2,0,1, 3,2,1};
					else tris = new int[] {2,1,0, 3,1,2};
					svp = vp;
					for (int i = -1; i < tCount; i++) { 
						v[vp]   = v[lvp+(i*16)+3];
						v[vp+1] = v[lvp+(i*16)+7];
						v[vp+2] = v[lvp+(i*16)+2];
						v[vp+3] = v[lvp+(i*16)+6];
						int part = pointsLenght-1;
						if (points[part].uvMapping) {
							for (int j=0;j<4;j+=2) {
								uvs[vp+j] = UV(v[vp+j],part,i,prefs.inner?5:4);
								uvs[vp+1+j] = UV(v[vp+1+j],part,i,prefs.inner?4:5);
							}
						} else
							for (int j=0;j<4;j++) 
								uvs[vp+j] = UV(v[svp+j],part,i,6);
						vp+=4;
						for (int j=0; j<6; j++)
							tr[points[part].mat,j+tp[points[part].mat]] = tris[j]+vp;
						tp[points[part].mat]+=6;
					}
			}
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
			Vector3[] n = mesh.normals;
			c = (tCount+1)*2;
			sp = tCount*2;
			for (int i=0; i < (backParts < 0 ? bParts*2 : bParts); i++){
				n[sp+c*i] = n[c*i];
				n[sp+c*i+1] = n[c*i+1];
			}
			if (backParts > 0)
				for (int i=0; i < backParts; i++) {
				n[bvp+sp+c*i] = n[bvp+c*i];
				n[bvp+sp+c*i+1] = n[bvp+c*i+1];
			}
			n[cvp+sp] = n[cvp];
			n[cvp+sp+1] = n[cvp+1];
			mesh.normals = n;
			if (prefs.tangens)
		    	RecalculateTangens(mesh);
			;
		}
		vt = vp; 
		v = null;
		create = false;
		stopWatch.Stop();
		ms = stopWatch.ElapsedMilliseconds;
	}
	
	private Vector2 UV(Vector3 _point,int _p, int _i, int _j) {
		
		if (_j == 2)
			return new Vector2((float)(_i)/(prefs.teethCount)*points[_p].uvScaleX+points[_p].uvOffX,
												points[_p].uvScaleX+points[_p].uvOffY+0.5f);
		if (_j == 3)
			return new Vector2((float)(_i)/(prefs.teethCount)*points[_p].uvScaleX+points[_p].uvOffX,
												1f+points[_p].uvOffY*points[_p].uvScaleY);
		if (_j == 6)
			return new Vector2(-_point.x*points[_p].uvScaleX+points[_p].uvOffX,
							   _point.y*points[_p].uvScaleY+points[_p].uvOffY*points[_p].uvScaleY);
		if (_j == 7)
			return new Vector2(points[_p].uvScaleX+points[_p].uvOffX,
							   points[_p].uvScaleY+points[_p].uvOffY*points[_p].uvScaleY);
		
		if (points[_p].uvMapping) 
			return new Vector2((float)(_i+1)/(prefs.teethCount)*points[_p].uvScaleX+points[_p].uvOffX,
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
	
	public void SwitchInnerGearing() {
		
		prefs.inner = !prefs.inner;
		for (int i=0; i < pointsLenght; i++)
			points[i].offset.x = -points[i].offset.x;
		UpdateGear();
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
