﻿''' <summary>
''' General Utility functions we haven't found a home for yet
''' </summary>
<CLSCompliant(True)>
Public NotInheritable Class Utilities

#Region "Public Methods"

    Public Shared Function CountOccurrences(ByRef StToSerach As String, ByRef StToLookFor As String) As Int32
        Dim iPos As Integer = -1
        Dim iFound As Integer = 0
        Do
            iPos = StToSerach.IndexOf(StToLookFor, iPos + 1)
            If iPos <> -1 Then
                iFound += 1
            End If
        Loop Until iPos = -1
        Return iFound
    End Function

    Public Shared Function DateTimeToUnixTimestamp(dTime As DateTime) As Double
        Return (dTime - New DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds
    End Function

    ''' <summary>
    ''' Letter increment for walking PS tables
    ''' </summary>
    ''' <param name="Input"></param>
    ''' <returns></returns>
    Public Shared Function IncrementLetter(ByRef Input As String) As Char
        Input = Input.Substring(0, 1)
        Dim i As Integer = AscW(Input)
        Dim test As Char
        Select Case Input
            Case "A"c To "Z"c
                test = ChrW(i + 1)
            Case "a"c To "z"c
                test = ChrW(i + 1)
            Case "0"c To "8"c
                test = ChrW(i + 1)
            Case "9"c
                test = "a"c
            Case Else
                test = "{"c
        End Select
        Input = test
        Return test
    End Function

    ''' <summary>
    ''' Converts a number representing a Unix Time stamp and converts it to a usable DateTime format
    ''' </summary>
    ''' <param name="unixTimeStamp"></param>
    ''' <returns>DataTime</returns>
    Public Shared Function UnixTimeStampToDateTime(ByRef unixTimeStamp As Double) As DateTime
        ' Unix timestamp is seconds past epoch
        Dim dtDateTime As System.DateTime = New DateTime(1970, 1, 1, 0, 0, 0, 0)
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime
        Return dtDateTime
    End Function

#End Region

End Class