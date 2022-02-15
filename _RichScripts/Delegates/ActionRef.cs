namespace RichPackage
{
    public delegate void ActionRef<T>(ref T arg);
    public delegate void ActionRef<T, U>(ref T arg1, ref U arg2);
    public delegate void ActionRef<T, U, V>(ref T arg1, ref U arg2, ref V arg3);
}
