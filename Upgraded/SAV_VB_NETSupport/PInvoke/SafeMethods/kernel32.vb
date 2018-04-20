Imports System
Imports System.Runtime.InteropServices
Namespace SafeNative
	Public Module kernel32
		Public Function GetPrivateProfileString(ByRef lpApplicationName As String, ByRef lpKeyName As String, ByRef lpDefault As String, ByRef lpReturnedString As String, ByVal nSize As Integer, ByRef lpFileName As String) As Integer
			Dim result As Integer = 0
			Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi(lpKeyName)
			Try
				result = SAV_VB_NETSupport.UnsafeNative.kernel32.GetPrivateProfileString(lpApplicationName, tmpPtr, lpDefault, lpReturnedString, nSize, lpFileName)
				lpKeyName = Marshal.PtrToStringAnsi(tmpPtr)
			Finally 
				Marshal.FreeHGlobal(tmpPtr)
			End Try
			Return result
		End Function
	End Module
End Namespace