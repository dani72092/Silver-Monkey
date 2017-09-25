﻿Public Class Logger

#Region "Private Fields"

    Dim Stack As New ArrayList
    Dim strErrorFilePath As String

#End Region

#Region "Public Constructors"
    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="Name"></param>
    ''' <param name="message"></param>
    Public Sub New(Name As String, message As String)
        'Call Log Error
        strErrorFilePath = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) & "\Silver Monkey\Log\" & Name & Date.Now().ToString("MM_dd_yyyy_H-mm-ss") & ".txt"
        System.IO.Directory.CreateDirectory(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) & "\Silver Monkey\Log\")
        LogMessage(message)
    End Sub

#End Region

#Region "Public Methods"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="filePath"></param>
    ''' <returns></returns>
    Public Function IsFileInUse(ByVal filePath As String) As Boolean
        Try
            Dim contents() As String = IO.File.ReadAllLines(filePath)
        Catch ex As IO.IOException
            Return (ex.Message.StartsWith("The process cannot access the file") AndAlso
                    ex.Message.EndsWith("because it is being used by another process."))
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Message"></param>
    Public Sub LogMessage(Message As String)

        Dim ioFile As System.IO.StreamWriter = Nothing
        Try

            ioFile = New System.IO.StreamWriter(strErrorFilePath, True)
            For Each line In Stack.ToArray
                ioFile.WriteLine(line)
            Next
            Stack.Clear()
            ioFile.WriteLine(Message)

            ioFile.Close()
        Catch ex As IO.IOException
            If (ex.Message.StartsWith("The process cannot access the file") AndAlso
                    ex.Message.EndsWith("because it is being used by another process.")) Then
                Stack.Add(Message)
            End If
        Catch exLog As Exception
            If Not IsNothing(ioFile) Then
                ioFile.Close()
            End If
        End Try
    End Sub

#End Region

End Class