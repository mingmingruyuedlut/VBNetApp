Imports System.Data.OleDb

Namespace Common
    Public Class SqlHelper
        Public Property DbConnectionString As String

        Public Sub New(ByVal dbConnStr As String)
            Me.DbConnectionString = dbConnStr
        End Sub

        ''' <summary>
        ''' Prepare the db command
        ''' </summary>
        ''' <param name="conn">db connection</param>
        ''' <param name="cmd">db command</param>
        ''' <param name="ct">command type</param>
        ''' <param name="commandString">command string</param>
        ''' <param name="param">db parameters</param>
        Private Sub PrepareCommand(ByRef conn As OleDbConnection, ByRef cmd As OleDbCommand, ByVal ct As CommandType, ByVal commandString As String, ByVal param As List(Of OleDbParameter))
            'If closed, then open
            If conn.State <> ConnectionState.Open Then
                conn.Open()
            End If

            'Bind command
            cmd.Connection = conn
            cmd.CommandText = commandString
            cmd.CommandType = ct

            'If don't have parameter
            If param Is Nothing Then
                Return
            End If

            'If have parameter
            For Each pa As OleDbParameter In param
                cmd.Parameters.Add(pa)
            Next
        End Sub

        ''' <summary>
        ''' Excute the scalar/query, and returns the first column of the first row in the result
        ''' </summary>
        ''' <param name="ct">command type</param>
        ''' <param name="commandString">command string</param>
        ''' <param name="param">db parameters</param>
        ''' <returns></returns>
        Public Function ExcuteScalar(ByVal ct As CommandType, ByVal commandString As String, ByVal param As List(Of OleDbParameter)) As Object
            Try
                Dim cmd As New OleDbCommand
                Using cnn As New OleDbConnection(DbConnectionString)
                    PrepareCommand(cnn, cmd, ct, commandString, param)
                    Dim result = cmd.ExecuteScalar()
                    cmd.Parameters.Clear()
                    Return result
                End Using
            Catch ex As Exception
                Log_Anything("ExcuteScalar - " & GetExceptionInfo(ex))
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Excute non query by command string and parameters, such as: add, delete and update
        ''' It will prevent the database injection attacks.
        ''' </summary>
        ''' <param name="ct">command type</param>
        ''' <param name="commandString">command string</param>
        ''' <param name="param">db parameters</param>
        ''' <returns></returns>
        Public Function ExcuteNonQuery(ByVal ct As CommandType, ByVal commandString As String, ByVal param As List(Of OleDbParameter)) As Integer
            Try
                Dim cmd As New OleDbCommand
                Using cnn As New OleDbConnection(DbConnectionString)
                    PrepareCommand(cnn, cmd, ct, commandString, param)
                    Dim result = cmd.ExecuteNonQuery()
                    cmd.Parameters.Clear()
                    Return result
                End Using
            Catch ex As Exception
                Log_Anything("ExcuteNonQuery - " & GetExceptionInfo(ex))
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Excute non query by command string, such as: add, delete and update
        ''' </summary>
        ''' <param name="commandString">command string</param>
        ''' <returns></returns>
        Public Function ExcuteNonQuery(ByVal commandString As String) As Integer
            Try
                Using cnn As New OleDbConnection(DbConnectionString)
                    cnn.Open()
                    Dim cmd As New OleDbCommand(commandString, cnn)
                    Dim result = cmd.ExecuteNonQuery()
                    Return result
                End Using
            Catch ex As Exception
                Log_Anything("ExcuteNonQuery - " & GetExceptionInfo(ex))
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Get data reader by db command string and parameters
        ''' </summary>
        ''' <param name="ct">command type</param>
        ''' <param name="commandString">command string</param>
        ''' <param name="param">db parameters</param>
        ''' <returns></returns>
        Public Function ExcuteReader(ByVal ct As CommandType, ByVal commandString As String, ByVal param As List(Of OleDbParameter)) As OleDbDataReader
            Try
                Dim cmd As New OleDbCommand
                Using cnn As New OleDbConnection(DbConnectionString)
                    PrepareCommand(cnn, cmd, ct, commandString, param)
                    Dim result = cmd.ExecuteReader()
                    cmd.Parameters.Clear()
                    Return result
                End Using
            Catch ex As Exception
                Log_Anything("ExcuteReader - " & GetExceptionInfo(ex))
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Get data set by db query string.
        ''' </summary>
        ''' <param name="query">db query</param>
        ''' <returns></returns>
        Public Function ExcuteDataSet(ByVal query As String) As DataSet
            Try
                Using cnn As New OleDbConnection(DbConnectionString)
                    cnn.Open()
                    Dim cmd As New OleDbCommand(query, cnn)
                    Dim da As New OleDbDataAdapter(cmd)
                    Dim ds As New DataSet
                    da.Fill(ds)
                    Return ds
                End Using
            Catch ex As Exception
                Log_Anything("ExcuteDataSet - " & GetExceptionInfo(ex))
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Get data set by db query string and parameters.
        ''' It will prevent the database injection attacks.
        ''' </summary>
        ''' <param name="ct">command type</param>
        ''' <param name="query">db query</param>
        ''' <param name="param">db parameters</param>
        ''' <returns></returns>
        Public Function ExcuteDataSet(ByVal ct As CommandType, ByVal query As String, ByVal param As List(Of OleDbParameter)) As DataSet
            Try
                Dim cmd As New OleDbCommand
                Using cnn As New OleDbConnection(DbConnectionString)
                    PrepareCommand(cnn, cmd, ct, query, param)
                    Dim da As New OleDbDataAdapter(cmd)
                    Dim ds As New DataSet
                    da.Fill(ds)
                    Return ds
                End Using
            Catch ex As Exception
                Log_Anything("ExcuteDataSet - " & GetExceptionInfo(ex))
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Get data set by db query string.
        ''' </summary>
        ''' <param name="query">db query</param>
        ''' <param name="dbTable">customize the cached db table name, can get data table from data set by this name</param>
        ''' <returns></returns>
        Public Function ExcuteDataSet(ByVal query As String, ByVal dbTable As String) As DataSet
            Try
                Using cnn As New OleDbConnection(DbConnectionString)
                    cnn.Open()
                    Dim cmd As New OleDbCommand(query, cnn)
                    Dim da As New OleDbDataAdapter(cmd)
                    Dim ds As New DataSet
                    da.Fill(ds, dbTable)
                    Return ds
                End Using
            Catch ex As Exception
                Log_Anything("ExcuteDataSet - " & GetExceptionInfo(ex))
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Get data table by db query string and parameters
        ''' It will prevent the database injection attacks.
        ''' </summary>
        ''' <param name="ct">command type</param>
        ''' <param name="query">db query</param>
        ''' <param name="param">db parameters</param>
        ''' <returns></returns>
        Public Function ExcuteDataTable(ByVal ct As CommandType, ByVal query As String, ByVal param As List(Of OleDbParameter)) As DataTable
            Try
                Dim cmd As New OleDbCommand
                Using cnn As New OleDbConnection(DbConnectionString)
                    PrepareCommand(cnn, cmd, ct, query, param)
                    Dim da As New OleDbDataAdapter(cmd)
                    Dim ds As New DataSet
                    da.Fill(ds)
                    Dim dt = ds.Tables.Item(0)
                    Return dt
                End Using
            Catch ex As Exception
                Log_Anything("ExcuteDataTable - " & GetExceptionInfo(ex))
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Get data table by db query string.
        ''' </summary>
        ''' <param name="query">db query</param>
        ''' <returns></returns>
        Public Function ExcuteDataTable(ByVal query As String) As DataTable
            Try
                Using cnn As New OleDbConnection(DbConnectionString)
                    cnn.Open()
                    Dim cmd As New OleDbCommand(query, cnn)
                    Dim da As New OleDbDataAdapter(cmd)
                    Dim ds As New DataSet
                    da.Fill(ds)
                    Dim dt = ds.Tables.Item(0)
                    Return dt
                End Using
            Catch ex As Exception
                Log_Anything("ExcuteDataTable - " & GetExceptionInfo(ex))
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Get data table by db query string.
        ''' </summary>
        ''' <param name="query">db query</param>
        ''' <param name="dbTable">customize the cached db table name</param>
        ''' <returns></returns>
        Public Function ExcuteDataTable(ByVal query As String, ByVal dbTable As String) As DataTable
            Try
                Using cnn As New OleDbConnection(DbConnectionString)
                    cnn.Open()
                    Dim cmd As New OleDbCommand(query, cnn)
                    Dim da As New OleDbDataAdapter(cmd)
                    Dim ds As New DataSet
                    da.Fill(ds, dbTable)
                    Dim dt = ds.Tables(dbTable)
                    Return dt
                End Using
            Catch ex As Exception
                Log_Anything("ExcuteDataTable - " & GetExceptionInfo(ex))
                Return Nothing
            End Try
        End Function

    End Class
End Namespace
