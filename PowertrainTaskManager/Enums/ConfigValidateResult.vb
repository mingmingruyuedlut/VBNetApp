
Namespace Enums
    Public Enum TaskConfigValidateResult
        Valid = 0 'Task is valid
        NotFound = 1 'No Records Found for Search Criteria - RED X
        Invalid = 2 'Records found for Specific Instance of Task but Task_Number not recorded. YELLOW X
        NotFoundValidationField = 3 'Don't contains the field to validate, such as Task_Number or others
    End Enum

    Public Enum StationConfigValidateResult
        NotFound = 1 'No Records Found for Search Criteria or No Validation Field Found for Search Criteria- RED X
        Invalid = 2 'Records found for Specific Instance of Task but Task_Number not recorded. YELLOW X
        Valid = 3 'Task is valid
    End Enum
End Namespace
