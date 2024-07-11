/*! \file */ 
/// <summary>
/// The enum that contains state types using at <b>GazeCallback</b>.
/// </summary>
/// <b>Note</b>
/// - <b>FIXATION</b> means that the gazes from the past and the gazes up to the present have made a fixation.
/// - <b>SACCADE</b> means that the gazes from the past and the gazes up to the present have formed a saccade.
/// - <b>UNKNOWN</b> means that Not fixation or saccade.
public enum EyeMovementState
{
    FIXATION = 0,
    SACCADE = 1,
    UNKNOWN = 2
}
