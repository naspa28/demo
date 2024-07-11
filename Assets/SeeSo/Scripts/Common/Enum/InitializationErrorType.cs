/*! \file */ 
/// <summary>
/// The Enum that contains error types of <b>InitializationDelegate</b>
/// </summary>
/// <remarks>
/// Please read : <a href="https://docs.seeso.io/docs/document/authentication">Authentication</a> for more details.
/// </remarks>
public enum InitializationErrorType
{
    ERROR_NONE = 0,
    ERROR_INIT = 1,
    ERROR_CAMERA_PERMISSION = 2,
    AUTH_INVALID_KEY = 3,
    AUTH_INVALID_ENV_USED_DEV_IN_PROD = 4,
    AUTH_INVALID_ENV_USED_PROD_IN_DEV = 5,
    AUTH_INVALID_PACKAGE_NAME = 6,
    AUTH_INVALID_APP_SIGNATURE = 7,
    AUTH_EXCEEDED_FREE_TIER = 8,
    AUTH_DEACTIVATED_KEY = 9,
    AUTH_INVALID_ACCESS = 10,
    AUTH_UNKNOWN_ERROR = 11,
    AUTH_SERVER_ERROR = 12,
    AUTH_CANNOT_FIND_HOST = 13,
    AUTH_WRONG_LOCAL_TIME = 14,
    AUTH_INVALID_KEY_FORMAT = 15,
    AUTH_EXPIRED_KEY = 16,
    ERROR_NOT_ADVANCED_TIER = 17
}