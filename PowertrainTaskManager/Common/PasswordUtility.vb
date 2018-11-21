Imports System.Collections.Generic
Imports System.Linq
Imports System.Security.Cryptography
Imports System.Text
Imports System.Web

Namespace Common
    Public Class PasswordUtility

        Private Const SALT_BYTE_SIZE = 24
        Private Const HASH_BYTE_SIZE = 24
        Private Const PBKDF2_ITERATIONS = 1000
        Private Const ITERATION_INDEX = 0
        Private Const SALT_INDEX = 1
        Private Const PBKDF2_INDEX = 2

        Public Shared Function Encrypt(ByVal password As String) As String
            Dim csprng = New RNGCryptoServiceProvider()
            Dim salt = New Byte(SALT_BYTE_SIZE) {}
            csprng.GetBytes(salt)
            Dim hash = Pbkdf2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE)
            Return PBKDF2_ITERATIONS & ":" &
                   Convert.ToBase64String(salt) & ":" &
                   Convert.ToBase64String(hash)
        End Function

        Public Shared Function ValidatePassword(ByVal password As String, ByVal encryptPassword As String) As Boolean
            Dim delimiter = New Char() {":"}
            Dim split = encryptPassword.Split(delimiter)
            Dim iterations = Integer.Parse(split(ITERATION_INDEX))
            Dim salt = Convert.FromBase64String(split(SALT_INDEX))
            Dim hash = Convert.FromBase64String(split(PBKDF2_INDEX))
            Dim testHash = Pbkdf2(password, salt, iterations, hash.Length)
            Dim result = SlowEquals(hash, testHash)
            Return result
        End Function

        Private Shared Function SlowEquals(ByVal a As Byte(), ByVal b As Byte()) As Boolean
            'to-do'
            If (a.Length <> b.Length) Then
                Return False
            End If

            Dim diff = a.Length Xor b.Length
            For i = 0 To a.Length - 1
                diff = diff Or (a(i) Xor b(i))
            Next

            Return diff = 0
        End Function

        Private Shared Function Pbkdf2(ByVal password As String, ByVal salt As Byte(), ByVal iterations As Integer, ByVal outputBytes As Integer) As Byte()
            Dim rfc = New Rfc2898DeriveBytes(password, salt, iterations)
            Return rfc.GetBytes(16)
        End Function

    End Class
End Namespace
