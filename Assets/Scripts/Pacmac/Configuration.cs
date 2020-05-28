
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
        private MemoryDistribution<int> _distDim; // Dimension of the grid
        private MemoryDistribution<PelletType> _distPelletTypes;
        /* distributions for TileGenerator */
        private MemoryDistribution<int> _distNumberPaths, _distPathIndex;
        private MemoryDistribution<int>[] _distCoods;

        private MemoryDistribution<double> _distGhostSpeed;
        /* STORED VALUES */
        private PelletType[] _valuesPelletTypes = PelletExtension.PelletList();
        private Vector2Int _dim;
        private int _level;
        private int _numberPaths;
        /* CONFIG VALUES */
        private double _pDim = 0.4;
        private double _pPath = 0.1;
        private double _facGhostSpeed = 0.1;
        private double[] _psPellet = new double[] { 0.99, 0.008, 0.002 };

        public double GetPDim() { return _pDim; }
        public double GetPPath() { return _pPath; }
        public double GetFacGhostSpeed() { return _facGhostSpeed; }
        public double[] GetPsPellet() { return _psPellet; }

        public void SetPDim(double pDim) { _pDim = pDim; }
        public void SetPPath(double pPath) { _pPath = pPath; }
        public void SetFacGhostSpeed(double facGhostSpeed) { _facGhostSpeed = facGhostSpeed; }
        public void SetPsPellet(double[] psPellet) { _psPellet = psPellet; }

        public RandomMemoryResult<int> GetDimResults() { return _distDim.GetMemory(); }
        public RandomMemoryResult<PelletType> GetPelletTypesResults() { return _distPelletTypes.GetMemory(); }
        public RandomMemoryResult<int> GetNumberPathsResults() { return _distNumberPaths.GetMemory(); }
        public RandomMemoryResult<double> GetGhostSpeedResults() { return _distGhostSpeed.GetMemory(); }

        public Configuration()
        {
            _gen = new RandomGenerator();
        }

        public void Reset(int level)
        {
            /* todo: */
            _level = level;
            int maxDim = (int)(Math.Sqrt(level + 1)) + 20;
            int minDim = 10;
            _distDim = new MemoryDistribution<int>(new BinomialDistribution(_pDim, minDim, maxDim));
            _dim = RandomDimensions();
            Probability[] psPellet = Array.ConvertAll(_psPellet, p => (Probability)p);
            /* */
            _distPelletTypes =  new MemoryDistribution<PelletType>(new CustomFiniteDistribution<PelletType>(
              _valuesPelletTypes,
              psPellet
              ));
            /* */
            int minPath = Math.Min(_dim[0], _dim[1]) / 4 + 1;
            int maxPath = (_dim[0] + _dim[1]) / 4;
            int numberTiles = _dim.x * _dim.y;
            _distNumberPaths = new MemoryDistribution<int>(new HypergeometricDistribution(_pPath, minPath, maxPath, numberTiles));
            _numberPaths = RandomNumberPaths();
            _distPathIndex = new MemoryDistribution<int>(new UniformRangeIntDistribution(0, _numberPaths - 1));
            /* */
            _distCoods = new MemoryDistribution<int>[2];
            for(int i=0; i<_distCoods.GetLength(0); ++i)
            {
                _distCoods[i] = new MemoryDistribution<int>(new UniformRangeIntDistribution(0, _dim[0] - 1));
            }

            /* */
            double mu = _facGhostSpeed;
            double sigma = Math.Cos(mu) / 4;
            _distGhostSpeed = new MemoryDistribution<double>(new GaussianDistribution(mu, sigma));
            
            /*
             * _pDim
             * 
             *
             */
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