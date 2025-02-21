using SQLite4Unity3d;

public class Initinfo
{
    [PrimaryKey, AutoIncrement] //设置主键 自动增长
   
    public int Id { get; set; }
    public int Char { get; set; }
    public string time { get; set; }


    /// <summary>
    /// 重写ToString函数，方便控制台打印
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format("[Person: Id={0}", Char);
    }
}
