
using System;
using UnityEngine;

using pacmac.random;

namespace pacmac
{
    public class Configuration
    { 
        /* Random Number Generator */
        public RandomGenerator _gen; 
        /* distributions for GridManager */
        public FiniteDistribution<int> _distDim; // Dimension of the grid
        /* distributions for TileGenerator */
        public FiniteDistribution<int> _distNumberPaths, _distPaths, _distCoods;

        public Configuration()
        {
            _gen = new Random();
        }

        public reset(int level)
        {
            /* todo: */
            _distDim = new BinomialDistribution(/* */);
            
            /* */
            _distNumberPaths = new UniformRangeIntDistribution(/* */);
            _distPaths = new UniformRangeIntDistribution(/* */);
            _distCoods = new UniformRangeIntDistribution(/* */);
        }

        public Vector2Int RandomDimensions()
        {
            int dimX = _randGen.Random<int>(_distDim);
            int dimY = _randGen.Random<int>(_distDim);
            return new Vector2Int(dimX, dimY);
        }

        private Vector2Int RandomCoords()
        {
                return new Vector2Int(RandomX(), RandomY());
        }

        private int RandomX()
        {
            return _gen.Random<int>(_distCoods[0]) / 4;
        }

        private int RandomY()
        {
            return _gen.Random<int>(_distCoods[1]) / 4;
        }

        private int RandomPathNumber()
        {
            return _gen.Random<int>(_distNumberPaths) / 4;
        }
    }

}