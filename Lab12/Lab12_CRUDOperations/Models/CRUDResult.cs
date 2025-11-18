namespace Lab12_CRUDOperations.Models
{
    public class CRUDResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int RowsAffected { get; set; } = 0;
        public int? GeneratedId { get; set; }
        public string? ErrorNumber { get; set; }
        public string? ErrorMessage { get; set; }

        // Static factory methods
        public static CRUDResult SuccessResult(string message, int rowsAffected = 1, int? generatedId = null)
        {
            return new CRUDResult
            {
                Success = true,
                Message = message,
                RowsAffected = rowsAffected,
                GeneratedId = generatedId
            };
        }

        public static CRUDResult ErrorResult(string errorMessage, string? errorNumber = null)
        {
            return new CRUDResult
            {
                Success = false,
                Message = errorMessage,
                ErrorMessage = errorMessage,
                ErrorNumber = errorNumber
            };
        }

        public static CRUDResult BusinessRuleErrorResult(string message)
        {
            return new CRUDResult
            {
                Success = false,
                Message = message,
                RowsAffected = 0
            };
        }
    }
}