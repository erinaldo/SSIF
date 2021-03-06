﻿Imports Sadara.Models.V1.Database
Imports Sadara.Models.V1.POCO
'Imports System.Data.Entity

Public Class frmLogin

    'Tipo de inicio de sesión
    Public Property Type As Boolean

    'Rol o Permiso
    Public Property Rol As String

    'Tipo de Cerrado del Formulario
    Dim salir As Boolean = False

    Private Async Sub frmIniciarSesion_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Log.Instance.RegisterActivity(
            If(Config.currentBusiness IsNot Nothing, Config.currentBusiness.IdEmpresa, Guid.Empty),
            "Login",
            "Load",
            "Load Login"
        )

        txtUsuario.Focus()

    End Sub

    Private Sub txtUsuario_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtUsuario.KeyPress

        If e.KeyChar = ChrW(13) Then

            If Not txtUsuario.Text.Trim = "" Then
                txtContraseña.Focus()
            Else
                MessageBox.Show("Ingresar el nombre de usuario")
            End If

        End If

    End Sub

    Private Sub txtContraseña_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtContraseña.KeyPress
        If e.KeyChar = ChrW(13) Then
            If Not txtContraseña.Text.Trim = "" Then
                btEntrar.Focus()
            Else
                MessageBox.Show("Ingresar la contraseña")
            End If
        End If
    End Sub

    Private Async Sub btEntrar_Click(sender As Object, e As EventArgs) Handles btEntrar.Click

        Dim catchMessage As String = String.Empty

        Try
            If Not txtUsuario.Text = "" And Not txtContraseña.Text = "" Then
                Using db As New CodeFirst
                    Dim User = db.Usuarios.Where(Function(f) f.NombreCuenta = txtUsuario.Text And f.Activo = "S").FirstOrDefault
                    If Not User Is Nothing Then

                        Dim password = CryptoSecurity.Decoding(User.Contraseña)

                        If password = txtContraseña.Text Then

                            Log.Instance.RegisterActivity(
                                If(Config.currentBusiness IsNot Nothing, Config.currentBusiness.IdEmpresa, Guid.Empty),
                                "Login",
                                "Enter",
                                "Enter Login",
                                "User Password - Success",
                                Guid.Parse(User.IDUsuario)
                            )

                            'Evaluar que tipo de inicio de sesión (-Ingreso  -Autorización)
                            If Me.Type Then

                                'Aquí se realiza la autorización
                                Config.activatePrivileges = User
                                Me.salir = True
                                Me.Close()

                            Else

                                'Aquí se agrega el proceso de entrada al sistema
                                Config.currentUser = User
                                frmSeleccionarBodega.frm_return = 1
                                frmSeleccionarBodega.Show()
                                Me.salir = True
                                Me.Close()

                            End If

                        Else

                            Log.Instance.RegisterActivity(
                                If(Config.currentBusiness IsNot Nothing, Config.currentBusiness.IdEmpresa, Guid.Empty),
                                "Login",
                                "Enter",
                                "Enter Login",
                                "Password - Failed",
                                Guid.Parse(User.IDUsuario)
                            )

                            MessageBox.Show("Contraseña incorrecta")

                            txtContraseña.Focus()

                        End If

                    Else

                        Log.Instance.RegisterActivity(
                            If(Config.currentBusiness IsNot Nothing, Config.currentBusiness.IdEmpresa, Guid.Empty),
                            "Login",
                            "Enter",
                            "Enter Login",
                            "User - Failed"
                        )

                        MessageBox.Show("No se encuentra ningún usuario con este nombre")

                        txtUsuario.Focus()

                    End If

                End Using

            Else

                Log.Instance.RegisterActivity(
                    If(Config.currentBusiness IsNot Nothing, Config.currentBusiness.IdEmpresa, Guid.Empty),
                    "Login",
                    "Enter",
                    "Enter Login",
                    "User Or Password - Nothing"
                )

                MessageBox.Show("Ingresar usuario y contraseña")

                txtUsuario.Focus()

            End If

        Catch ex As Exception

            MessageBox.Show("Error, " & ex.ToString())

        End Try

        If Not String.IsNullOrEmpty(catchMessage) Then

            Log.Instance.RegisterActivity(
                If(Config.currentBusiness IsNot Nothing, Config.currentBusiness.IdEmpresa, Guid.Empty),
                "Login",
                "Enter",
                "Enter Login",
                "Login Catch -(" & catchMessage & ")"
            )

        End If

    End Sub

    Private Sub frmIniciarSesion_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If salir Or Me.Type Then
            Me.Dispose()
        Else
            Application.ExitThread()
        End If
    End Sub

    Private Sub btSalir_Click(sender As Object, e As EventArgs) Handles btSalir.Click
        Me.Close()
    End Sub

    Private Sub frmIniciarSesion_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        gpSesion.Left = (Me.Width / 2) - (gpSesion.Width / 2)
        gpSesion.Top = (Me.Height / 2) - (gpSesion.Height / 2) - 10
    End Sub
End Class