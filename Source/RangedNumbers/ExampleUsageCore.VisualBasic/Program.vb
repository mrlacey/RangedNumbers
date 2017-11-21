Imports RangedNumbers
Imports System

Module Program
    Sub Main(args As String())
        ' Define a variable that holds a number between zero and ten
        Dim foo = 7

        foo += 5

        Console.WriteLine(foo)  ' 12
        Dim dpp As RangedInt = foo

        Console.WriteLine(dpp.Value)

        Dim bar = New RangedInt(7, 0, 10)

        bar += 5

        Console.WriteLine(bar.Value)  ' 10

        Dim xyz As RangedInt = (7, 0, 10)

        xyz += 5

        Console.WriteLine(xyz.Value)  ' 10
        Dim a = xyz < 5
        Dim b = xyz > 5
        Dim c = xyz <= 5
        Dim d = xyz >= 5
        Dim e = xyz = 5
        Dim f = xyz <> 5

        Dim bb As SByte = 8

        Dim rib As RangedInt = bb

        Console.WriteLine(rib)

        Console.ReadKey(True)
    End Sub
End Module
