''AUGUSTO CAVALER 17/09/2005
''SIMPLE CLASS FOR CONTROLS RESIZING

Public Class Control_Resizer
    Public alCtrl As New SortedList
    Public alctrlWidth As New SortedList
    Public alctrlHeigth As New SortedList
    Public alctrlPosX As New SortedList
    Public alctrlPosY As New SortedList
    Public alctrlFontSiz As New SortedList

    Public Event resize(ByVal ctrl As Control, ByVal ctrlW As Single, ByVal ctrlH As Single, ByVal ctrlPosX As Single, ByVal ctrlPosy As Single, ByVal fntSize As Integer)

    Sub createConst(ByVal ctrl As Control, ByVal ctrlWidth As Single, ByVal ctrlHeigth As Single, ByVal ctrlPosX As Single, ByVal ctrlPosY As Single, ByVal ctrlFontSiz As Integer, ByVal i As Integer)
        If Not alCtrl(i) Is Nothing Then Exit Sub

        alCtrl.Add(i, ctrl)
        alctrlWidth.Add(i, ctrlWidth)
        alctrlHeigth.Add(i, ctrlHeigth)
        alctrlPosX.Add(i, ctrlPosX)
        alctrlPosY.Add(i, ctrlPosY)
        alctrlFontSiz.Add(i, ctrlFontSiz)
    End Sub

    Sub resizeCtrl(ByVal origformX As Single, ByVal origformY As Single, ByVal formX As Single, ByVal formY As Single, ByVal i As Integer)

        Dim ctrl As Control
        ctrl = CType(alCtrl.Item(i), Control)

        Dim ActCtrlW As Single
        ActCtrlW = (formX * alctrlWidth.Item(i)) / origformX

        Dim ActCtrlH As Single
        ActCtrlH = (formY * alctrlHeigth.Item(i)) / origformY

        Dim ActCtrlPosX As Single
        ActCtrlPosX = (formX * alctrlPosX(i)) / origformX

        Dim ActCtrlPosY As Single
        ActCtrlPosY = (formY * alctrlPosY(i)) / origformY

        Dim ActCtrlFntSize As Integer
        ActCtrlFntSize = (formY * alctrlFontSiz.Item(i)) \ origformY


        If ctrl Is Nothing Then Exit Sub

        RaiseEvent resize(ctrl, ActCtrlW, ActCtrlH, ActCtrlPosX, ActCtrlPosY, ActCtrlFntSize)
    End Sub

End Class
