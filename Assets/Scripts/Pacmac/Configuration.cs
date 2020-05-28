
using System;
using UnityEngine;

using pacmac.random;

namespace pacmac
{
    public class Configuration
    { 
        /* Random Number Generator */
        private RandomGenerator _gen; 
        /* distributions for .... */
        private FiniteDistribution<int> _distDim; // Dimension of the grid
        private FiniteDistribution<PelletType> _distPelletTypes;
        /* distributions for TileGenerator */
        private FiniteDistribution<int> _distNumberPaths, _distPathIndex;
        private FiniteDistribution<int>[] _distCoods;

        private Distribution<double> _distGhostSpeed;
        /* STORED VALUES */
        private PelletType[] _valuesPelletTypes = PelletExtension.PelletList();
        private Vector2Int _dim;
        private int _level;
        private int _numberPaths;
        /* CONFIG VALUES */
        private double _pDim = 0.4;
        private double _pPath = 0.1;
        private Probability[] _psPellet = new Probability[] { 0.99, 0.008, 0.002 };

        public Configuration()
        {
            _gen = new RandomGenerator();
        }

        public void Reset(int level)
        {
            /* todo: */
            _level = level;
            int maxDim = (int)(Math.Log(level + 1)) + 20;
            int minDim = 10;
            _distDim = new BinomialDistribution(_pDim, minDim, maxDim);
            _dim = RandomDimensions();
            /* */
            _distPelletTypes =  new CustomFiniteDistribution<PelletType>(
              _valuesPelletTypes,
              _psPellet
              );
            /* */
            int minPath = Math.Min(_dim[0], _dim[1]) / 4 + 1;
            int maxPath = (_dim[0] + _dim[1]) / 4;
            int numberTiles = _dim.x * _dim.y;
            _distNumberPaths = new HypergeometricDistribution(_pPath, minPath, maxPath, numberTiles);
            _numberPaths = RandomNumberPaths();
            _distPathIndex = new UniformRangeIntDistribution(0, _numberPaths - 1);
            /* */
            _distCoods = new FiniteDistribution<int>[2];
            _distCoods[0] = new UniformRangeIntDistribution(0, _dim[0]-1);
            _distCoods[1] = new UniformRangeIntDistribution(0, _dim[1]-1);

            /* */
            double mu = (1 - 1 / Math.Log(20 + _level)) / 2;
            double sigma = Math.Cos(_level) / 4;
            _distGhostSpeed = new GaussianDistribution(mu, sigma);
        }

        public Vector2Int GetLevelDimensions() { return _dim; }
        public int GetNumberPaths() { return _numberPaths; }

        private Vector2Int RandomDimensions()
        {
            int dimX = _gen.Random<int>(_distDim);
            int dimY = _gen.Random<int>(_distDim);
            return new Vector2Int(dimX, dimY);
        }

        public Vector2Int RandomCoords()
        {
                return new Vector2Int(RandomX(), RandomY());
        }

        public int RandomX()
        {
            return _gen.Random<int>(_distCoods[0]);
        }

        public int RandomY()
        {
            return _gen.Random<int>(_distCoods[1]);
        }

        private int RandomNumberPaths()
        {
            return _gen.Random<int>(_distNumberPaths);
        }

        public int RandomPathIndex()
        {
            return _gen.Random<int>(_distPathIndex); 
        }

        public PelletType RandomPelletType()
        {
            return _gen.Random<PelletType>(_distPelletTypes);
        }

        public float RandomGhostSpeed()
        {
            return (float) Math.Abs(_gen.Random<double>(_distGhostSpeed));
        }

        public int RandomUniformInt(int min, int max)
        {
            /* bad bad bad */
            var distInt = new UniformRangeIntDistribution(min, max);
            return _gen.Random<int>(distInt);
        }
    }

}