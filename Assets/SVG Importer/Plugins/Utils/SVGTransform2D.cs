// Copyright (C) 2019 Jaroslav Stehlik
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace SVGImporter.Rendering
{        
    using Utils;

    [System.Serializable]
    public class SVGTransform2D : System.Object, ICloneable {
        
        [SerializeField]
        protected Vector2 _position;
        [HideInInspector]
        public Vector2 position
        {
            get {
                return _position;
            }
            set {
                if(_position == value)
                    return;
                
                _position = value;
            }
        }
        
        [SerializeField]
        protected float _rotation;
        public float rotation
        {
            get {
                return _rotation;
            }
            set {
                if(_rotation == value)
                    return;
                
                _rotation = value;
            }
        }
        
        [SerializeField]
        protected Vector2 _scale = Vector2.one;
        public Vector2 scale
        {
            get {
                return _scale;
            }
            set {
                if(_scale == value)
                    return;
                
                _scale = value;
            }
        }

        public Matrix4x4 matrix4x4
        {
            get {
                return Matrix4x4.TRS(new Vector3(_position.x, _position.y, 0f), Quaternion.Euler(0f, 0f, _rotation), new Vector3(_scale.x, _scale.y, 1f));
            }
        }

        public SVGMatrix matrix
        {
            get {
                return SVGMatrix.TRS(new Vector3(_position.x, _position.y, 0f), _rotation, new Vector2(_scale.x, _scale.y));
            }
        }
        
        public SVGTransform2D()
        {
            this._position = Vector2.zero;
            this._rotation = 0f;
            this._scale = Vector2.one;
        }
        
        public SVGTransform2D(Vector2 position, float rotation, Vector2 scale)
        {
            this._position = position;
            this._rotation = rotation;
            this._scale = scale;
        }
        
        public SVGTransform2D(SVGTransform2D transform)
        {
            SetTransform(transform);
        }

        public System.Object Clone()
        {
            return new SVGTransform2D(this._position, this._rotation, this._scale);
        }
        
        public void SetTransform(SVGTransform2D transform)
        {
            if(transform == null)
                return;

            this._position = transform._position;
            this._rotation = transform._rotation;
            this._scale = transform._scale;
        }
        
        public void Reset()
        {
            this._position = Vector2.zero;
            this._rotation = 0f;
            this._scale = Vector2.one;
        }
        
        public void TRS(Vector2 position, float rotation, Vector2 scale)
        {
            this._position = position;
            this._rotation = rotation;
            this._scale = scale;
        }
        
        public bool Compare(SVGTransform2D transform)
        {
            if(transform == null) return false;
            return (this._position == transform._position &&
                    this._rotation == transform._rotation &&
                    this._scale == transform._scale);
        }
        
        public static SVGTransform2D DecomposeMatrix(Matrix4x4 matrix)
        {
            return new SVGTransform2D(//matrix.GetColumn(3), 
                                      new Vector2(matrix[0, 3], matrix[1, 3]),
                                      Quaternion.LookRotation(
                new Vector3(matrix[0, 2], matrix[1, 2], matrix[2, 2]),
                new Vector3(matrix[0, 1], matrix[1, 1], matrix[2, 1])
                ).eulerAngles.z,
                                      new Vector2(
                new Vector2(matrix[0, 0], matrix[1, 0]).magnitude,
                new Vector2(matrix[0, 1], matrix[1, 1]).magnitude
                ));
        }
    }
}
