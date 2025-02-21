using SQLite4Unity3d;

public class HistoryMessage 
{
    [PrimaryKey, AutoIncrement] //设置主键 自动增长
    public int id { get; set; }//Id作为主键
    public string Charname { get; set; }
    public string Q_word { get; set; }

    public string A_word { get; set; }
    public string time { get; set; }
   

    /// <summary>
    /// 重写ToString函数，方便控制台打印
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format("[Person: Id={0}, Name={1},  Word={2}, A_word={3},time={4}]", id, Charname,Q_word,A_word,time);
    }
}
