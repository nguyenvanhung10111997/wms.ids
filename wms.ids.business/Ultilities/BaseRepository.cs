using wms.infrastructure;
using wms.infrastructure.Enums;
using wms.infrastructure.Models;

namespace wms.business
{
    internal class BaseRepository
    {
        private readonly Lazy<IRepository> _repository;
        protected IRepository Repository => _repository.Value;

        private readonly Lazy<IReadOnlyRepository> _readOnlyRepository;
        protected IReadOnlyRepository ReadRepository => _readOnlyRepository.Value;

        #region Constructor
        public BaseRepository(Lazy<IRepository> repository, Lazy<IReadOnlyRepository> readOnlyRepository)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
        }
        #endregion

        #region Function
        protected CRUDResult<T> Success<T>(T data)
        {
            var result = new CRUDResult<T>()
            {
                Data = data,
                StatusCode = CRUDStatusCodeRes.Success,
                ErrorMessage = string.Empty
            };

            return result;
        }

        protected CRUDResult<T> Error<T>(T data = default(T), CRUDStatusCodeRes statusCode = CRUDStatusCodeRes.ResourceNotFound, string errorMessage = "")
        {
            var result = new CRUDResult<T>()
            {
                Data = data,
                StatusCode = statusCode,
                ErrorMessage = string.IsNullOrEmpty(errorMessage) ? "Resource not found" : errorMessage
            };

            return result;
        }
        #endregion

        #region Destructor
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_repository != null && _repository.IsValueCreated)
                    {
                        _repository.Value.Dispose();
                    }

                    if (_readOnlyRepository != null && _readOnlyRepository.IsValueCreated)
                    {
                        _readOnlyRepository.Value.Dispose();
                    }
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BaseRepository()
        {
            Dispose(false);
        }
        #endregion
    }
}
